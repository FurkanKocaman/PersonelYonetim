namespace PersonelYonetim.Server.Domain.Izinler;
public static class DefaultIzinTurler
{
    public static List<IzinTur> GetDefaultIzinTurler(Guid sirketId){
        var  defaultIzinTurleri = new List<IzinTur>
        {
            new IzinTur
            {
                Ad = "Yıllık İzin",
                Aciklama = "Çalışanın yıllık olarak hak edebileceği ücretli izin.",
                UcretliMi = true,
                LimitTipi = LimitTipiEnum.YılLimit,
                LimitGunSayisi = 14,
                EksiBakiyeHakkı = EksiBakiyeHakkıEnum.HakYok,
                HakEdis = HakEdisEnum.Yillik,
                HakEdisDonem = true,
                HakEdisBaslangic = false,
                DevretmeTipi = DevretmeTipiEnum.Limitsiz,
                DevretmeGunLimit = 0,
                KidemYil = 0,
                KidemArtıGun = 0,
                EnAzTalep = 0,
                EnCokTalep = 14,
                HesapSekli = true,
                AciklamaZorunlu = false,
                YerineBakacakZorunlu = false,
                SirketId = sirketId
            },
             new IzinTur
            {
                Ad = "Mazeret İzni",
                Aciklama = "Kişisel ihtiyaçlar için ücretli izin",
                UcretliMi = true,
                LimitTipi = LimitTipiEnum.YılLimit,
                LimitGunSayisi = 5,
                HakEdis = HakEdisEnum.Yillik,
                HakEdisDonem = true,
                HakEdisBaslangic = false,
                HesapSekli = true,
                AciklamaZorunlu = false,
                SirketId = sirketId
            },
            new IzinTur
            {
                Ad = "Hastalık İzni",
                Aciklama = "Çalışanın sağlık problemi nedeniyle alabileceği ücretli izin.",
                UcretliMi = true,
                LimitTipi = LimitTipiEnum.Limitsiz,
                HakEdis = HakEdisEnum.Gunluk,
                DevretmeTipi = DevretmeTipiEnum.Sifirlama,
                HesapSekli = true,
                AciklamaZorunlu = true,
                SirketId = sirketId
            },
            new IzinTur
            {
                Ad = "Doğum İzni",
                Aciklama = "Kadın çalışanlar için doğum öncesi ve sonrası kullanabilecekleri ücretli izin.",
                UcretliMi = true,
                LimitTipi = LimitTipiEnum.YılLimit,
                LimitGunSayisi = 112,
                HakEdis = HakEdisEnum.Gunluk,
                HesapSekli = true,
                AciklamaZorunlu = false,
                SirketId = sirketId
            },
            new IzinTur
            {
                Ad = "Babalık İzni",
                Aciklama = "Baba olan çalışanlara verilen kısa süreli izin.",
                UcretliMi = true,
                LimitTipi = LimitTipiEnum.TalepLimit,
                LimitGunSayisi = 5,
                HakEdis = HakEdisEnum.Gunluk,
                HesapSekli = true,
                AciklamaZorunlu = false,
                SirketId = sirketId
            },
            new IzinTur
            {
                Ad = "Evlilik İzni",
                Aciklama = "Çalışanın evlenmesi durumunda verilen ücretli izin.",
                UcretliMi = true,
                LimitTipi = LimitTipiEnum.TalepLimit,
                LimitGunSayisi = 3,
                HakEdis = HakEdisEnum.Gunluk,
                HesapSekli = true,
                AciklamaZorunlu = false,
                SirketId = sirketId
            },
            new IzinTur
            {
                Ad = "Ölüm İzni",
                Aciklama = "Çalışanın birinci derece yakınını kaybetmesi durumunda verilen ücretli izin.",
                UcretliMi = true,
                LimitTipi = LimitTipiEnum.TalepLimit,
                LimitGunSayisi = 3,
                HakEdis = HakEdisEnum.Gunluk,
                HesapSekli = true,
                AciklamaZorunlu = false,
                SirketId = sirketId
            },
            new IzinTur
            {
                Ad = "Özel İzin",
                Aciklama = "Çalışanın kişisel sebeplerle kullanabileceği izin (Ücretli veya ücretsiz olabilir).",
                UcretliMi = false,
                LimitTipi = LimitTipiEnum.Limitsiz,
                HakEdis = HakEdisEnum.Gunluk,
                HesapSekli = true,
                AciklamaZorunlu = true,
                SirketId = sirketId
            }
        };
        return defaultIzinTurleri;
}
}
