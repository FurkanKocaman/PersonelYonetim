# Personel Yönetim Sistemi

## Proje Genel Bakış

Bu proje, Vue.js ve TypeScript kullanılarak oluşturulmuş bir Personel Yönetim Sistemi'nin frontend kısmıdır. Backend kısmının ayrıca geliştirilmesi beklenmektedir.

## Frontend Yapısı

Frontend aşağıdaki şekilde organize edilmiştir:

- `src/components/`: Tüm Vue bileşenlerini içerir
  - `src/components/dashboard/`: Gösterge paneline özel bileşenler
- `src/views/`: Sayfa görünümlerini içerir
- `src/services/`: API servis katmanları
- `src/models/`: Veri modelleri ve arayüzler
- `src/router/`: Vue Router yapılandırması

## Backend Geliştirici için API Gereksinimleri

Frontend aşağıdaki API uç noktalarını beklemektedir:

### Kimlik Doğrulama

- `POST /auth/login`: Kullanıcı girişi
  - İstek: `{ email: string, password: string }`
  - Yanıt: `{ accessToken: string, refreshToken: string }`

### Gösterge Paneli Verileri

- `GET /api/dashboard/statistics`: Gösterge paneli istatistiklerini alır
  - Yanıt: `{ title: string, value: number, icon: string, color: string }` dizisi

- `GET /api/dashboard/payroll`: Bordro öğelerini alır
  - Yanıt: `{ id: number, name: string, department: string, status: string, date: string }` dizisi

- `GET /api/dashboard/announcements`: Duyuruları alır
  - Yanıt: `{ id: number, title: string, date: string, content: string }` dizisi

- `GET /api/dashboard/quick-access`: Hızlı erişim butonlarını alır
  - Yanıt: `{ id: number, title: string, icon: string, color: string, action?: string }` dizisi

## Geliştirme Ortamı Kurulumu

1. Bağımlılıkları yükleyin:
   ```
   npm install
   ```

2. Geliştirme sunucusunu çalıştırın:
   ```
   npm run dev
   ```
