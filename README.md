# ASP.NET Core ile Üniversite BYS Sistemi

Bu proje, ASP.NET Core kullanarak bir üniversite bilgi yönetim sistemi (BYS) geliştirmek için hazırlanmıştır. Sistem, öğrenci ders seçiminden öğretmen onayına kadar çeşitli özellikler sunar.

## Proje Özellikleri

- **Ders Yönetimi**: Zorunlu ve seçmeli derslerin düzenlenmesi.
- **Ders Seçimi**: Öğrenciler için ders seçim ekranı.
  - Zorunlu dersler her öğrencide otomatik olarak seçilir.
  - Seçmeli dersler, gruplar halinde bloklara ayrılır ve her gruptan bir ders seçilebilir.
- **Kontenjan Takibi**: Derslerin kontenjanları takip edilir ve seçimlere göre güncellenir.
- **Onay Süreci**: Öğrencilerin ders seçimleri, öğretmenler tarafından onaylanır.
  - Onaylanmamış seçimler `NonConfirmedSelections` tablosunda tutulur.
  - Onay sonrası, seçimler `StudentCourseSelections` tablosuna taşınır.
- **MSSQL Veritabanı Desteği**.
- **Tailwind CSS ile Modern Arayüz**.

## Kullanılan Teknolojiler

- **Backend**: ASP.NET Core
- **Frontend**: Razor Pages & Tailwind CSS
- **Veritabanı**: MSSQL

## Kurulum

### Gerekli Araçlar

- [.NET 7 SDK](https://dotnet.microsoft.com/download)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Adım 1: Projeyi Klonlayın

```bash
git clone https://github.com/emreucbudak/ASPNETCORE-ILE-UNIVERSITE-BYS-SISTEMI.git
cd ASPNETCORE-ILE-UNIVERSITE-BYS-SISTEMI
```

### Adım 2: Veritabanını Oluşturun

1. MSSQL Server'ı çalıştırın.
2. `appsettings.json` dosyasındaki `ConnectionStrings` bölümünü kendi MSSQL bağlantı bilgilerinizle güncelleyin.
3. Aşağıdaki komutla veritabanını oluşturun:

```bash
dotnet ef database update
```

### Adım 3: Uygulamayı Çalıştırın

```bash
dotnet run
```

Tarayıcınızda [http://localhost:5000](http://localhost:5000) adresini ziyaret edin.

## Proje Yapısı

```
ASPNETCORE-ILE-UNIVERSITE-BYS-SISTEMI/
├── EntityLayer/          # Veritabanı modelleri
├── DataAccessLayer/      # Veritabanı bağlantı katmanı
├── BusinessLayer/        # İş mantığı
├── PresentationLayer/    # Uygulamanın frontend ve backend katmanı
└── README.md             # Proje açıklamaları
```

## Ekran Görüntüleri

Projenin ekran görüntülerine PDF dosyası üzerinden ulaşabilirsiniz.

## Katkıda Bulunma

1. Projeyi forklayın.
2. Yeni bir dal oluşturun: `git checkout -b yeni-ozellik`
3. Değişikliklerinizi yapıp commitleyin: `git commit -m 'Yeni özellik ekle'`
4. Dalınızı pushlayın: `git push origin yeni-ozellik`
5. Bir Pull Request oluşturun.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır. Daha fazla bilgi için [LICENSE](LICENSE) dosyasını inceleyin.
