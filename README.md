# DCOM-API

.NET 10 ile geliştirilmiş bir DICOM Web API projesi.

## Özellikler
- DICOM dosyası yükleme
- DICOM içinden Patient, Study ve Series bilgilerini okuma
- PostgreSQL'e ilişkisel kayıt
- Yüklenen dosyayı `wwwroot/dicoms` içine kaydetme
- Study listeleme endpointi

## Teknoloji
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- fo-dicom

## Endpointler
### POST /api/Dicom/upload
DICOM dosyası yükler ve bilgileri veritabanına kaydeder.

### GET /api/Studies
Veritabanındaki study kayıtlarını listeler.

## Not
Bu projede aynı PatientID, StudyInstanceUID ve SeriesInstanceUID tekrar oluşuyorsa yeni kayıt açılmadan mevcut ilişkiler kullanılır.
