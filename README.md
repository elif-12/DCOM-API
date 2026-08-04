# DCOM-API

.NET 10 ile geliştirilmiş, Clean Architecture prensiplerine uygun katmanlı yapıda ve xUnit ile Moq kullanılarak birim testleri yazılmış, tıbbi görüntü (DICOM) dosyalarını yönetmek için hazırlanmış bir Web API projesi.

## Özellikler

- DICOM dosyası yükleme ve dosya içinden Patient, Study, Series bilgilerini otomatik okuma
- JWT tabanlı kimlik doğrulama
- Rol bazlı yetkilendirme (SuperAdmin ve Doctor)
- Uygulama ilk açıldığında süper admin hesabının otomatik oluşturulması
- Çok kiracılı veri izolasyonu, her kullanıcı yalnızca kendi verisini görür
- Oturum zaman aşımı, 15 dakika hareketsizlikte oturum düşer, her istekte süre yenilenir, en fazla 1 saat
- Oturum bilgisini tutmak için değiştirilebilir store yapısı (Redis veya bellek içi)
- Merkezi hata yönetimi, tüm hatalar tek bir middleware üzerinden yakalanır ve loglanır
- Liste endpointlerinde sayfalama
- Soft delete ve audit log alanları (oluşturan, oluşturma tarihi, güncelleme bilgileri)

## Mimari

Proje Clean Architecture yaklaşımıyla dört katmana ayrılmıştır.

**Domain**
Sistemin temel varlıkları. BaseEntity, User, Patient, Study, Series, DicomFile. Hiçbir katmanı referans almaz, hiçbir paket bağımlılığı yoktur.

**Application**
İş kuralları ve use-case servisleri. DicomService, StudyService, UserService, TokenService. Repository arayüzleri, DTO'lar ve ortak model tipleri de bu katmandadır. Yalnızca Domain'i referans alır.

**Infrastructure**
Teknik gerçekleştirmeler. AppDbContext, EF Core repository'leri, RedisStore, InMemoryStore ve migration'lar. Kendi içinde teknolojiye göre bölünmüştür: EntityFramework, Redis, Caching. Application ve Domain'i referans alır.

**Api**
Dış dünyayla iletişim. Controller'lar, middleware'ler ve uygulama başlangıç yapılandırması. Diğer katmanları referans alır.

Bağımlılık yönü tek taraflıdır: Domain hiçbir şeyi bilmez, Application Domain'i bilir, Infrastructure Application ve Domain'i bilir, Api hepsini bilir. Application katmanı Infrastructure'ı referans almadığı için servisler veritabanına doğrudan erişemez, bu kural derleyici tarafından zorlanır.

## Teknolojiler

- .NET 10 ve ASP.NET Core Web API
- Entity Framework Core ve PostgreSQL
- Redis (oturum store'u için)
- fo-dicom (DICOM dosya okuma)
- BCrypt (şifre hashleme)
- JWT Bearer Authentication
- Swagger / OpenAPI
- Docker ve Docker Compose

## Kurulum

### Docker ile

Repo kökünde:

    docker compose up --build

Uygulama, PostgreSQL ve Redis birlikte ayağa kalkar.

Swagger arayüzü: http://localhost:8080/swagger

### Yerel çalıştırma

PostgreSQL'in çalışıyor olması ve appsettings.json içindeki bağlantı bilgilerinin doğru olması gerekir.

    dotnet run --project DCOM-API

## Yapılandırma

appsettings.json içindeki başlıca ayarlar:

- ConnectionStrings:DefaultConnection — PostgreSQL bağlantısı
- Jwt:Key, Jwt:Issuer, Jwt:Audience — token ayarları
- Jwt:ExpiryMinutes — token mutlak ömrü (varsayılan 60 dakika)
- Jwt:IdleMinutes — hareketsizlik süresi (varsayılan 15 dakika)
- TokenStore:Provider — oturum store seçimi, InMemory veya Redis
- Redis:ConnectionString — Redis bağlantısı (provider Redis ise kullanılır)
- SuperAdmin:Username, SuperAdmin:Password — ilk açılışta oluşturulacak süper admin

## Varsayılan kullanıcı

Uygulama ilk açıldığında süper admin hesabı otomatik oluşturulur.

Kullanıcı adı: admin
Şifre: Admin123!

## Endpointler

### Auth
- POST /api/Auth/login — giriş yapar ve JWT token döner

### Users (yalnızca SuperAdmin)
- GET /api/Users — kullanıcıları sayfalı listeler
- POST /api/Users — yeni doktor kullanıcısı oluşturur

### Dicom
- POST /api/Dicom/upload — DICOM dosyası yükler, içindeki bilgileri okuyup kaydeder

### Studies
- GET /api/Studies — çalışmaları sayfalı listeler
- PUT /api/Studies/{id} — çalışma bilgilerini günceller
- DELETE /api/Studies/{id} — çalışmayı siler (soft delete)

Auth dışındaki tüm endpointler token gerektirir. Swagger üzerinde Authorize butonuna token girilerek test edilebilir.

## Sayfalama

Liste endpointleri PageNumber ve PageSize parametrelerini alır. Cevap, kayıtların yanında toplam kayıt sayısı ve toplam sayfa sayısını da içerir.

Örnek: GET /api/Studies?PageNumber=1&PageSize=10
