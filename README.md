# IIS-Publisher (Minimal API)

Bu proje, GitHub repository'sinde `master` branch'ine yapılan her birleştirmeden (merge) sonra, webhook aracılığıyla tetiklenen bir Minimal API'dir. API, sunucuya gelen isteği alır ve hedef projeyi IIS üzerinde otomatik olarak publish eder.

## 🚀 Özellikler

- GitHub Webhook desteği
- Minimal API (.NET 8)
- IIS üzerinde otomatik publish işlemi
- Güvenlik için WebhookSecret kontrolü
- Loglama

## ⚙️ Nasıl Çalışır?

1. GitHub üzerinde bir webhook tanımlanır.
2. `master` branch'ine merge yapıldığında GitHub, belirlenen URL'ye HTTP POST isteği gönderir.
3. Bu API isteği alır, gerekli güvenlik kontrolünden geçirir.
4. Sunucuda ilgili dizindeki proje publish edilir ve IIS üzerinde güncellenir.
