using IIS_Publisher.Executors;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day) //This line writes logs to the logs/myapp.txt file and creates a new log file every day.
    .CreateLogger();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/publish", (HttpRequest request) =>
{
    try
    {
        // GitHub signature
        var signature = request.Headers["X-Hub-Signature-256"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(signature))
        {
            Log.Information("GitHub signature header not found");
            return Results.BadRequest("Signature header is missing");
        }

        var config = new Configuration();

        if (signature != config.WebhookSecret)
        {
            Log.Information("Invalid signature");
            return Results.BadRequest("Invalid signature");
        }
        
        using (var con = new ComandProcessContext())
        {
            GitProcess gitProc = new(con);
            gitProc.ExcecGitCommands(config.WorkingDir, config.CloneURL);
            Log.Information("Git Cmmands Excected");
            MSBuildProcess buildProc = new(con);
            Log.Information("Build Cmmands Excected");
            buildProc.ExcecBuildCommand(config.WorkingDir, config.RootPath, config.ProjectFileName);
        }

        return Results.Ok();

    }
    catch (Exception e)
    {
        Log.Error(e, "Error");
        throw;
    }
})
.WithName("Publish")
.WithOpenApi();



app.Run();
