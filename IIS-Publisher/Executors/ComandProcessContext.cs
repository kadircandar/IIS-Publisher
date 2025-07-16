using Serilog;
using System.Diagnostics;

namespace IIS_Publisher.Executors
{
    public class ComandProcessContext : IDisposable
    {
        readonly Process _p;
        public ComandProcessContext()
        {
            Process p = new();
            p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();
            _p = p;
        }

    
        public void DoCommands(params string[] commands)
        {
            System.IO.StreamWriter sw = _p.StandardInput;
            foreach (var command in commands)
            {
                sw.WriteLine(command);
            }
        }
        public void Dispose()
        {
            _p.StandardInput.Close();
            _p.WaitForExit();
            var pm = _p.StandardOutput.ReadToEnd();
            var er = _p.StandardError.ReadToEnd();
            Log.Error(pm, "Something went wrong");
            Log.Information("=========PROCESS ERRORs========");
            Log.Error(er, "Something went wrong");
            _p.Close();
            _p.Dispose();
        }
    }
}
