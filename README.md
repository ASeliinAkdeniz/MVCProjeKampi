# 🍧 Tatlı Sözlük — Kolektif Bilgi Platformu

Tatlı Sözlük, kullanıcıların diledikleri konularda başlıklar açıp bu başlıklar altında kendi görüşlerini (entry) paylaşabildiği, kurumsal standartlarda geliştirilmiş modern bir sosyal etkileşim platformudur. ASP.NET MVC 5 üzerinde 4 katmanlı yazılım mimarisiyle (N-Tier) inşa edilen proje; veri tutarlılığı, güvenlik ve yüksek performans hedeflenerek tasarlanmıştır. Hazır hiçbir şablona bağımlı kalınmadan sıfırdan geliştirilen "Nude" temalı özgün arayüzü sayesinde hem ziyaretçilere hem de yöneticilere kusursuz bir kullanıcı deneyimi sunar.

---

## 💻 Modüller ve Panel Özellikleri

### 1. Vitrin (Ziyaretçi Arayüzü)
*   **Gündem & Akış:** En çok etkileşim alan dinamik başlık listesi ve son entry önizlemelerini içeren temiz ana sayfa akışı.
*   **Kategori Filtreleme:** Başlıkları kategorilerine göre süzme ve her kategoriye özel renk kodlamasıyla kolay takip imkanı.
*   **Arama Motoru:** Başlık adları ile entry içeriklerinde büyük/küçük harf duyarsız, akıllı ve hızlı arama sistemi.
*   **Hakkımızda & İletişim:** Dinamik galeri slider'ı, site istatistikleri, Google Maps entegrasyonu ve ziyaretçi iletişim formu.

### 2. Yazar Paneli (Kullanıcı Arayüzü)
*   **Başlık & İçerik Yönetimi:** Yazarların kendi açtıkları başlıkları yönetebilmesi ve yazdıkları entry'leri sayfa yenilenmeden (Modal ile) düzenleyip silebilmesi.
*   **Sahiplik Güvenliği:** URL manipülasyonu ile başkalarının içeriklerine müdahaleyi engelleyen sıkı sahiplik denetimleri.
*   **Site İçi Mesajlaşma:** Okundu/okunmadı takibi sunan, profil fotoğraflı (yoksa baş harf avatarı) gelişmiş gelen/giden/çöp kutusu mesajlaşma sistemi.
*   **Güvenli Profil:** Şifrenin bozulmasını veya istemciye sızmasını önleyen akıllı "şifre koruma" stratejili profil güncelleme ekranı.

### 3. Yönetim Paneli (Admin)
*   **Dashboard:** Sistem genelindeki başlık, entry, yazar ve kategori sayılarını tek bakışta gösteren özet paneli.
*   **Rol Bazlı Yetkilendirme (RBAC):** Veritabanı tabanlı 7 farklı yönetici rolü (`BasAdmin`, `Moderator`, `KategoriYoneticisi` vb.) ve rol bazlı menü gizleme/erişim engelleme altyapısı.
*   **Moderasyon Sistemi:** Uygunsuz entry'leri sistemden kalıcı olarak silmeden, veri bütünlüğünü koruyarak pasifleştirme (Soft Delete) imkanı.

---

## 📊 Listeleme, İstatistikler ve Ek Özellikler

*   **Akıllı Sayfalama (Paging):** Sunucu taraflı 10'ar kayıtlı sayfalama ile veritabanını yormayan, hızlı yüklenen listeler.
*   **Dinamik Google Charts:** Kategorilere göre başlık dağılımı (Halka Pasta), kategori bazlı entry sayısı (Sütun) ve en aktif ilk 6 yazarı (Yatay Bar) gösteren canlı grafikler.
*   **Renk Rotasyon Algoritması:** Her kategorinin ve yazarın benzersiz ID'si üzerinden dinamik atanan sabit renklerle görsel tutarlılık.
*   **Eklentisiz Saf JS Çözümleri:** Modal pencereler, lightbox galeri ve slider bileşenlerinin harici ağır kütüphaneler (jQuery, Bootstrap vb.) olmadan tamamen Vanilla JS ile yazılması.

---

## 🛠️ Kullanılan Teknolojiler

### Backend
*   **Çatı (Framework):** ASP.NET MVC 5 (.NET Framework 4.7.2)
*   **ORM & Veritabanı:** Entity Framework 6 (Code First) & MS SQL Server
*   **Doğrulama (Validation):** FluentValidation (Merkezi iş kuralları denetimi)
*   **Güvenlik:** SHA256 Şifre Hash'leme, Cross-Site Request Forgery (CSRF) Koruması (`[ValidateAntiForgeryToken]`)

### Frontend
*   **Tasarım & CSS:** Standalone Tailwind CLI v3 (Özel "Nude" renk paleti entegreli)
*   **Etkileşim:** Pure JavaScript (Vanilla JS)
*   **Görseller:** Google Charts API, Material Symbols & Google Fonts
