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



Proje Görselleri 

<img width="1481" height="853" alt="1" src="https://github.com/user-attachments/assets/4687a6c1-8e4f-4214-87e8-5cef2a9121f9" />

<img width="1478" height="853" alt="2" src="https://github.com/user-attachments/assets/61133f45-b760-49d1-bf78-bd90774949fa" />

<img width="1456" height="874" alt="3" src="https://github.com/user-attachments/assets/f5c65b6d-567a-4247-948f-165978ea5a7b" />

<img width="1438" height="883" alt="4" src="https://github.com/user-attachments/assets/39d20fcc-4c81-4acf-b8d3-6500e714a29c" />

<img width="970" height="909" alt="5" src="https://github.com/user-attachments/assets/1d7f3f08-8146-45b5-b09c-309c8517846f" />

<img width="1310" height="910" alt="6" src="https://github.com/user-attachments/assets/f17a0bd0-0a27-47d0-b983-18a7c93de2a4" />

<img width="1353" height="701" alt="7" src="https://github.com/user-attachments/assets/7c6ad9f8-24e6-4965-ae6a-7e94f685e364" />

<img width="1230" height="889" alt="8" src="https://github.com/user-attachments/assets/7627fb4e-3f15-476f-ae8b-cb13ac7a51d6" />

<img width="1305" height="889" alt="9" src="https://github.com/user-attachments/assets/2a78be55-6606-42a3-9c05-595d86236209" />

<img width="1611" height="588" alt="10" src="https://github.com/user-attachments/assets/955cd775-46b6-4f19-9f43-b25dd7ae199e" />

<img width="1891" height="726" alt="11" src="https://github.com/user-attachments/assets/02723fc1-0901-445e-accb-f06ea6aba8e3" />

<img width="1900" height="906" alt="Ekran görüntüsü 2026-07-16 211704" src="https://github.com/user-attachments/assets/2e97b660-896a-4001-8fab-d88357cdd22d" />

<img width="1892" height="870" alt="Ekran görüntüsü 2026-07-16 211724" src="https://github.com/user-attachments/assets/254ba1fa-d7d5-4dc8-8992-b1738cefc2c5" />

<img width="1901" height="764" alt="Ekran görüntüsü 2026-07-16 211746" src="https://github.com/user-attachments/assets/bd127483-5d04-4415-91e7-9ebec76eb71f" />

<img width="1871" height="897" alt="Ekran görüntüsü 2026-07-16 211812" src="https://github.com/user-attachments/assets/17bd68d6-1ccf-4d1c-a436-65ed6fadd76c" />

<img width="1904" height="695" alt="Ekran görüntüsü 2026-07-16 211825" src="https://github.com/user-attachments/assets/b51487f1-6cf9-4069-8e04-be506911fdcb" />

<img width="1908" height="910" alt="Ekran görüntüsü 2026-07-16 211836" src="https://github.com/user-attachments/assets/7bbbe1f2-b7e1-4dc3-b342-36bfc0f6c028" />
