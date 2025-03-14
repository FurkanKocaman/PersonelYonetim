# Personel Yönetim Sistemi

## Proje Genel Bakış

Bu proje, Vue.js ve TypeScript kullanılarak oluşturulmuş bir Personel Yönetim Sistemi'nin frontend kısmıdır. Backend kısmının ayrıca geliştirilmesi beklenmektedir.

## Frontend Yapısı

Frontend aşağıdaki şekilde organize edilmiştir:

- `src/components/`: Tüm Vue bileşenlerini içerir
  - `src/components/dashboard/`: Gösterge paneline özel bileşenler
  - `src/components/izin/`: İzin yönetimine özel bileşenler
- `src/views/`: Sayfa görünümlerini içerir
  - `src/views/DashboardView.vue`: Ana gösterge paneli
  - `src/views/PersonelView.vue`: Personel yönetimi
  - `src/views/MaasView.vue`: Maaş yönetimi
  - `src/views/IzinView.vue`: İzin listesi yönetimi
  - `src/views/IzinTalepView.vue`: İzin talep formu
  - `src/views/AyarlarView.vue`: Sistem ayarları
- `src/services/`: API servis katmanları
  - `src/services/AuthService.ts`: Kimlik doğrulama servisi
  - `src/services/DashboardService.ts`: Gösterge paneli servisi
  - `src/services/PersonelService.ts`: Personel yönetimi servisi
  - `src/services/MaasService.ts`: Maaş yönetimi servisi
  - `src/services/IzinService.ts`: İzin yönetimi servisi
- `src/models/`: Veri modelleri ve arayüzler
  - `src/models/LoginRequest.ts`: Giriş isteği modeli
  - `src/models/LoginResponse.ts`: Giriş yanıtı modeli
  - `src/models/PersonelModels.ts`: Personel veri modelleri
  - `src/models/MaasModels.ts`: Maaş veri modelleri
  - `src/models/IzinModels.ts`: İzin veri modelleri
- `src/router/`: Vue Router yapılandırması
- `src/layouts/`: Sayfa düzenleri
  - `src/layouts/DashBoardLayout.vue`: Gösterge paneli düzeni

## Son Geliştirmeler ve Güncellemeler

Frontend ekibi aşağıdaki güncellemeleri tamamlamıştır:

1. **TypeScript Entegrasyonu**: Tüm bileşenler ve servisler TypeScript ile güçlendirilmiş ve tip güvenliği sağlanmıştır.
2. **İzin Talep Sayfası**: Personel izin talep formu geliştirilmiş ve dosya yükleme desteği eklenmiştir.
3. **Maaş Yönetim Sayfası**: Maaş kayıtları için CRUD işlemleri ve filtreleme özellikleri eklenmiştir.
4. **Ayarlar Sayfası**: Tema değiştirme ve kullanıcı tercihleri yönetimi eklenmiştir.

## Backend Geliştirici için API Gereksinimleri

Frontend aşağıdaki API uç noktalarını beklemektedir:

### Kimlik Doğrulama

- `POST /api/auth/login`: Kullanıcı girişi
  - İstek: `{ email: string, password: string }`
  - Yanıt: `{ token: string, user: { id: number, name: string, role: string, avatar: string } }`

### Gösterge Paneli Verileri

- `GET /api/dashboard/stats`: Gösterge paneli istatistiklerini alır
  - Yanıt: `{ title: string, value: number, icon: string, color: string }` dizisi

- `GET /api/payroll`: Maaş öğelerini alır
  - Yanıt: `{ id: number, personelAdi: string, departman: string, durum: string, odenmeTarihi: string, maas: number }` dizisi

- `GET /api/announcements`: Duyuruları alır
  - Yanıt: `{ id: number, title: string, date: string, content: string }` dizisi

- `GET /api/quick-access`: Hızlı erişim butonlarını alır
  - Yanıt: `{ id: number, title: string, icon: string, color: string, actionUrl: string }` dizisi

### Personel Yönetimi

- `GET /api/personel`: Personel listesini alır
  - Parametreler: `sayfa`, `sayfaBoyutu`, `siralamaAlani`, `siralama`
  - Yanıt: `{ items: PersonelItem[], toplamSayfa: number, mevcutSayfa: number, toplamKayit: number }`

- `GET /api/personel/:id`: Belirli bir personelin detaylarını alır
  - Yanıt: `PersonelItem`

- `POST /api/personel`: Yeni personel kaydı oluşturur
  - İstek: `PersonelRequest`
  - Yanıt: `PersonelItem`

- `PUT /api/personel/:id`: Personel kaydını günceller
  - İstek: `PersonelRequest`
  - Yanıt: `PersonelItem`

- `DELETE /api/personel/:id`: Personel kaydını siler
  - Yanıt: `{ success: boolean }`

### Maaş Yönetimi

- `GET /api/payroll`: Maaş listesini alır
  - Parametreler: `sayfa`, `sayfaBoyutu`, `siralamaAlani`, `siralama`
  - Yanıt: `{ items: MaasItem[], toplamSayfa: number, mevcutSayfa: number, toplamKayit: number }`

- `POST /api/payroll`: Yeni maaş kaydı oluşturur
  - İstek: `MaasRequest`
  - Yanıt: `MaasItem`

- `PUT /api/payroll/:id`: Maaş kaydını günceller
  - İstek: `MaasRequest`
  - Yanıt: `MaasItem`

- `DELETE /api/payroll/:id`: Maaş kaydını siler
  - Yanıt: `{ success: boolean }`

### İzin Yönetimi

- `GET /api/izin`: İzin listesini alır
  - Parametreler: `sayfa`, `sayfaBoyutu`, `siralamaAlani`, `siralama`
  - Yanıt: `{ items: IzinItem[], toplamSayfa: number, mevcutSayfa: number, toplamKayit: number }`

- `GET /api/izin/:id`: Belirli bir izin kaydının detaylarını alır
  - Yanıt: `IzinItem`

- `POST /api/izin`: Yeni izin talebi oluşturur
  - İstek: `IzinRequest` (multipart/form-data formatında, dosya yükleme desteği ile)
  - Yanıt: `IzinItem`

- `PUT /api/izin/:id`: İzin talebini günceller
  - İstek: `IzinRequest` (multipart/form-data formatında, dosya yükleme desteği ile)
  - Yanıt: `IzinItem`

- `DELETE /api/izin/:id`: İzin talebini siler
  - Yanıt: `{ success: boolean }`

## Yeni Eklenen Özelliklerin Backend Gereksinimleri

### 1. İzin Talep Formu (IzinTalepView.vue)

- **Dosya Yükleme Desteği**: İzin talep formunda dosya yükleme özelliği eklenmiştir. Backend, multipart/form-data formatında gelen istekleri işleyebilmeli ve dosyaları saklayabilmelidir.
  - Dosya yükleme API'si: `POST /api/izin/upload`
  - Desteklenen dosya tipleri: PDF, DOC, DOCX, JPG, PNG
  - Maksimum dosya boyutu: 5MB

- **Personel Listesi Entegrasyonu**: İzin talep formunda personel seçimi için PersonelService kullanılmaktadır. Backend, personel listesini aşağıdaki formatta döndürmelidir:
  ```json
  {
    "items": [
      {
        "id": 1,
        "ad": "Ahmet",
        "soyad": "Yılmaz",
        "departman": "Bilgi İşlem",
        "pozisyon": "Yazılım Geliştirici",
        "iseGirisTarihi": "2023-01-15",
        "email": "ahmet.yilmaz@sirket.com",
        "telefon": "555-123-4567",
        "adres": "İstanbul",
        "durum": "Aktif"
      }
    ],
    "toplamSayfa": 1,
    "mevcutSayfa": 1,
    "toplamKayit": 1
  }
  ```

- **İzin Hesaplama**: İzin gün sayısı hesaplama mantığı frontend'de uygulanmıştır, ancak backend'de de doğrulanmalıdır. Başlangıç ve bitiş tarihleri arasındaki iş günü sayısı hesaplanmalıdır.

### 2. Maaş Yönetim Sayfası (MaasView.vue)

- **Maaş Hesaplama**: Maaş hesaplama mantığı için backend desteği gereklidir. Aşağıdaki parametreler dikkate alınmalıdır:
  - Taban maaş
  - Prim
  - Kesintiler
  - Vergiler

- **Maaş Raporu Oluşturma**: Maaş raporları oluşturma ve PDF formatında indirme özelliği için backend desteği gereklidir.
  - API: `GET /api/payroll/:id/report`
  - Yanıt formatı: PDF dosyası

- **Toplu Maaş İşlemleri**: Toplu maaş işlemleri için API desteği:
  - API: `POST /api/payroll/bulk`
  - İstek: `{ personelIds: number[], islemTipi: string, tutar: number }`

### 3. Ayarlar Sayfası (AyarlarView.vue)

- **Kullanıcı Tercihleri**: Kullanıcı tercihlerini kaydetme ve getirme için API desteği:
  - Tercihleri kaydet: `POST /api/settings/preferences`
  - Tercihleri getir: `GET /api/settings/preferences`
  - Tercih modeli:
  ```json
  {
    "tema": "acik" | "koyu",
    "dil": "tr" | "en",
    "bildirimler": boolean,
    "sayfaBoyutu": number
  }
  ```

- **Sistem Ayarları**: Sistem genelinde ayarlar için API desteği:
  - API: `GET /api/settings/system`
  - Yanıt:
  ```json
  {
    "sirketAdi": "Şirket Adı",
    "logo": "logo_url",
    "adres": "Şirket Adresi",
    "telefon": "Şirket Telefonu",
    "email": "info@sirket.com",
    "vergiNo": "1234567890",
    "vergiDairesi": "Vergi Dairesi"
  }
  ```

## Veri Modelleri

### PersonelItem
```typescript
interface PersonelItem {
  id: number;
  ad: string;
  soyad: string;
  departman: string;
  pozisyon: string;
  iseGirisTarihi: string;
  email: string;
  telefon: string;
  adres: string;
  durum: string;
}
```

### IzinRequest
```typescript
interface IzinRequest {
  personelId: number;
  izinTipi: string;
  baslangicTarihi: string;
  bitisTarihi: string;
  gun: number;
  aciklama: string;
  dosya?: File;
}
```

### MaasItem
```typescript
interface MaasItem {
  id: number;
  personelId: number;
  personelAdi: string;
  departman: string;
  pozisyon: string;
  maas: number;
  prim: number;
  kesinti: number;
  netMaas: number;
  odenmeTarihi: string;
  durum: string;
}
```

