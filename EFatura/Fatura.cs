using System;


namespace E_Fatura
{
    public class Fatura
    {
        public string FaturaNo { get; set; }
        public string CariKod { get; set; }
        public string CariUnv { get; set; }
        public DateTime Tarih { get; set; }
        public string SubeKodu { get; set; }
        public string SubeAdi { get; set; }
        public decimal Tutar { get; set; }
        public decimal Iskonto { get; set; }
        public decimal NetTutar { get; set; }
        public decimal KdvTutar { get; set; }
        public decimal ToplamTutar { get; set; }
    }
}
