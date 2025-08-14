# ZeylProcessor

## Ne İşe Yarar?
ZeylProcessor, sigorta şirketlerinin zeyil (poliçe eki) kayıtlarında alt grup numaralandırması yapan akıllı bir sistemdir. Excel dosyalarındaki zeyil kayıtlarını analiz ederek, sigortalı desenlerine göre otomatik numaralandırma yapar.

[![.NET](https://img.shields.io/badge/.NET-6.0+-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![EPPlus](https://img.shields.io/badge/EPPlus-6.0+-orange)](https://www.epplussoftware.com/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.1+-7952B3)](https://getbootstrap.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## Video

<p align="center">
  <img src="gif/gifvid1.gif" alt="ZeylProcessor Demo" width="800" style="border-radius: 10px; box-shadow: 0 4px 20px rgba(0,0,0,0.1);">
</p>

<p align="center"><em>Drag & drop ile Excel yükleme, işleme ve indirme süreci</em></p>

## Ekran Görüntüleri

<p align="center">
  <img src="gif/img11.png" alt="Ana Sayfa" width="650" style="margin: 10px; border-radius: 8px;">
  <img src="gif/img22.png" alt="Dosya Yükleme" width="650" style="margin: 10px; border-radius: 8px;">
  <img src="gif/img33.png" alt="Sonuç Ekranı" width="650" style="margin: 10px; border-radius: 8px;">
</p>


## Algoritma Mantığı

ZeylProcessor, sigortalı isimlerindeki tekrar eden örüntüleri analiz ederek numaralandırma yapar:

### 📏 Temel Kurallar

| Durum | Kural | Örnek |
|-------|--------|-------|
| **Tek Kayıt** | Alt numara verilmez | `140` → `140` |
| **Çoklu Kayıt (Desen Yok)** | Sıralı numaralama | `231-1`, `231-2`, `231-3` |
| **Tekrar Eden Desen** | Sabit pozisyon numarası | Aşağıda detay ↓ |

### Desen Tanıma Örneği

**Giriş Verisi (Excel A-D sütunları):**
```
Zeyil No | Ana Grup | [Boş] | Sigortalı
231      | GRUP-A   |       | PELTIM MAKINA
231      | GRUP-A   |       | SINCAN KAĞIT  
231      | GRUP-A   |       | HAYAT KIMYA
231      | GRUP-A   |       | PELTIM MAKINA  ← Desen tekrarı
231      | GRUP-A   |       | SINCAN KAĞIT
231      | GRUP-A   |       | HAYAT KIMYA
```

**Algoritma Çıktısı:**
```
Zeyil No | Ana Grup | ALT GRUP ZEYİL NO | Sigortalı
231      | GRUP-A   | 231-1            | PELTIM MAKINA
231      | GRUP-A   | 231-2            | SINCAN KAĞIT
231      | GRUP-A   | 231-3            | HAYAT KIMYA
231      | GRUP-A   | 231-1            | PELTIM MAKINA ← Aynı sigortalı, aynı numara
231      | GRUP-A   | 231-2            | SINCAN KAĞIT
231      | GRUP-A   | 231-3            | HAYAT KIMYA
```

## Özellikler

## 📁 Proje Yapısı

```
ZeylAPI/
├── 📁 Controllers/
│   └── ZeylController.cs          # API endpoint'leri
├── 📁 Services/
│   ├── ExcelService.cs            # Excel okuma/yazma (EPPlus)
│   ├── ZeylService.cs             # Ana algoritma (desen tespiti)
│   ├── FileStorageService.cs      # Geçici dosya saklama
│   └── 📁 Interfaces/             # Servis interface'leri
├── 📁 Models/Entities/
│   └── ZeylRecord.cs              # Zeyil kayıt modeli
├── 📁 wwwroot/
│   ├── 📁 css/zeyil.css          # Custom gradient stilleri
│   ├── 📁 js/zeyil.js            # Frontend JavaScript
│   ├── 📁 images/                # Logo ve görseller
│   └── index.html                 # Ana sayfa
└── Program.cs                     # Uygulama yapılandırması
```

## Kurulum

1. **Projeyi klonlayın:**
```bash
git clone https://github.com/username/ZeylProcessor.git
cd ZeylProcessor
```

2. **Bağımlılıkları yükleyin:**
```bash
dotnet restore
```

3. **Projeyi çalıştırın:**
```bash
dotnet run
```

⭐ **Proje faydalıysa yıldız vermeyi unutmayın!** ⭐
