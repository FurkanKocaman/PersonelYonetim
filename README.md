# PersonelYonetim

PersonelYonetim, **.NET 9** ve **C# 13.0** kullanılarak geliştirilen bir **personel yönetim sistemi**dir. Bu proje, personel, departmanlar, pozisyonlar ve izin taleplerini yönetmek için çeşitli işlevler içerir.

## 📁 Proje Yapısı

Proje birkaç katmana ayrılmıştır:

- **Domain:** Temel iş mantığını ve alan varlıklarını içerir.
- **Application:** Komutlar, sorgular ve işleyiciler gibi uygulama mantığını içerir.
- **Infrastructure:** Depoların uygulanması ve diğer altyapı ile ilgili kodları içerir.
- **WebAPI:** API denetleyicilerini ve uç noktaların tanımlarını içerir.

## 🏗 Alan (Domain) Katmanı

### 📌 Varlıklar

- **Personel:** Ad, Soyad, İletişim, Adres, UserId gibi özelliklere sahip personel varlığını temsil eder.
- **İzinTalep:** PersonelId, BaşlangıçTarihi, BitişTarihi, İzinTürId gibi özelliklere sahip izin talebi varlığını temsil eder.
- **PersonelAtama:** Personel atama detaylarını temsil eder.
- **İzinTür:** İzin türlerini temsil eder.
- **DeğerlendirmeDurumEnum:** İzin talebinin değerlendirme durumunu temsil eder.

### 📂 Depolar (Repositories)

- `IPersonelRepository`: Personel deposu arayüzü.
- `IIzinTalepRepository`: İzin talep deposu arayüzü.
- `ISirketRepository`: Şirket deposu arayüzü.
- `ISubeRepository`: Şube deposu arayüzü.
- `IDepartmanRepository`: Departman deposu arayüzü.
- `IPozisyonRepository`: Pozisyon deposu arayüzü.

## ⚙ Uygulama (Application) Katmanı

### 📝 Komutlar (Commands)

- `PersonelCreateCommand`: Yeni personel oluşturma komutu.
- `PersonelUpdateCommand`: Personel bilgilerini güncelleme komutu.
- `PersonelDeleteCommand`: Personel silme komutu.
- `IzinTalepCreateCommand`: Yeni izin talebi oluşturma komutu.
- `IzinTalepUpdateCommand`: İzin talebi güncelleme komutu.
- `IzinTalepDeleteCommand`: İzin talebi silme komutu.

### 🔍 Sorgular (Queries)

- `IzinTalepGetAllQuery`: Tüm izin taleplerini getiren sorgu.

## 🌐 WebAPI Katmanı

### 🎮 Denetleyiciler (Controllers)

- **PersonelController**: Personel yönetimi için API denetleyicisi.
- **İzinTalepController**: İzin taleplerini yönetmek için API denetleyicisi.
- **AppODataController**: OData uç noktaları için API denetleyicisi.

### 📌 Uç Noktalar (Endpoints)

- `POST /personeller/create` - Yeni personel oluştur.
- `PUT /personeller/update` - Personel bilgilerini güncelle.
- `DELETE /personeller/delete` - Personel sil.
- `POST /izintalepler/create` - Yeni izin talebi oluştur.
- `PUT /izintalepler/update` - İzin talebini güncelle.
- `DELETE /izintalepler/delete` - İzin talebini sil.
- `GET /izintalepler/getall` - Tüm izin taleplerini getir.

## 🔒 Yetkilendirme ve Kimlik Doğrulama

Uygulama, **JWT tabanlı kimlik doğrulama** ve **kullanıcı rolleri** ile yetkilendirme kullanır.

### 🔑 JWT Kimlik Doğrulama

Kullanıcılar **JSON Web Token (JWT)** kullanarak kimlik doğrulama yapar.

### 👥 Kullanıcı Rolleri

- **Admin**: Tüm kaynaklara tam erişim.
- **Yönetici**: Personel ve izin taleplerini yönetme erişimi.
- **Çalışan**: Sadece kendi izin taleplerini görüntüleme ve yönetme erişimi.

## 🗄 Veritabanı

Uygulama **Microsoft SQL Server (MSSQL)** veritabanını kullanır.

### 🔄 Veritabanı Göçlerini Çalıştırma (Migrations)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
