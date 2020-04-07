namespace E_Fatura
{
    public class FaturaHareket
    {
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public int Miktar { get; set; }
        public string Birim { get; set; }
        public decimal Fiyat { get; set; }
        public decimal Kdv { get; set; }
        public decimal Tutar { get; set; }
        public double IskontoOrani { get; set; }
        public decimal Iskonto { get; set; }
        public decimal NetTutar { get; set; }
        public decimal ToplamTutar { get; set; }
    }
}
