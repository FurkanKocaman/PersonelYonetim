export interface Etkinlik {
    id: number;
    baslik: string;
    tarih: string; 
  }
  
  export const takvimService = {
    etkinlikler: <Etkinlik[]>[
      { id: 1, baslik: "Toplantı", tarih: "2025-03-05" },
      { id: 2, baslik: "Doğum Günü", tarih: "2025-04-23" },
    ],
  
    tatilGunleri: {
      "1-1": "Yılbaşı",
      "4-23": "23 Nisan",
      "5-19": "19 Mayıs",
      "8-30": "30 Ağustos",
      "10-29": "Cumhuriyet Bayramı",
    },
  
    getEtkinlikler() {
      return Promise.resolve(this.etkinlikler);
    },
  
    getTatilGunleri() {
      return Promise.resolve(this.tatilGunleri);
    },
  
    ekleEtkinlik(etkinlik: Etkinlik) {
      this.etkinlikler.push(etkinlik);
    },
  
    silEtkinlik(id: number) {
      this.etkinlikler = this.etkinlikler.filter(e => e.id !== id);
    },
  };