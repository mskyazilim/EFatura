using E_Fatura;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;


namespace EFatura
{
    public partial class frmFatura : Form
    {
        public frmFatura()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var FaturaBilg = new Fatura
            {

                CariKod = "0001",
                CariUnv = "MUART SARIKÜLÇE",
                FaturaNo = "SDF2020000000023",
                SubeAdi = "MM",
                SubeKodu = "MM",
                Tarih = DateTime.Now,
                Tutar = 1000,
                NetTutar = 200,
                KdvTutar = 180,
                Iskonto = 0,
                ToplamTutar = 1180
            };

            FaturaOlustur(FaturaBilg);
            


            var xml = $@"{Application.StartupPath}\EFatura\{FaturaBilg.FaturaNo}.xml";
            var xslt = $@"{Application.StartupPath}\EFatura.xslt";

         //   string xmlFile = Application.StartupPath + "\\EFATURA\\" + FaturaBilg.FaturaNo + ".XML";
         //   string Xslt = Application.StartupPath + "\\general.xslt";

            webBrowser1.DocumentText = EFatGetir(xml, xslt);
        }

        string EFatGetir(string xmlFilePath, string xsltFilePath)
        {
            var XslTrans = new XslCompiledTransform();
            var StrWrite = new StringWriter();
        
            var reader = XmlReader.Create(xsltFilePath, new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse });
            XslTrans.Load(reader);
            XslTrans.Transform(xmlFilePath, null, StrWrite);

            return StrWrite.ToString();
        }

        private void FaturaOlustur(E_Fatura.Fatura FaturaBilgileri)//, List<E_Fatura.FaturaHareket> faturaHareketleri
        {

            

            InvoiceLineType[] FaturaHareketleri()
            {
                var x = 1;

                var lines = new List<InvoiceLineType>();
                // var source = dataGridView1.Columns.Cast < FaturaHareketleri() >;
              //  while (x >=30)
               // {
                    var line = new InvoiceLineType
                    {
                        ID = new IDType { Value = x.ToString() },
                        Note = new[] { new NoteType { Value = "00001 - kkkkk" } },
                        InvoicedQuantity = new InvoicedQuantityType { unitCode = "C62", Value = 1 },
                        LineExtensionAmount = new LineExtensionAmountType { currencyID = "TRY", Value = 1000 },
                        AllowanceCharge = new[]{new AllowanceChargeType
                        {
                            ChargeIndicator = new ChargeIndicatorType{ Value = false },
                            MultiplierFactorNumeric = new MultiplierFactorNumericType{ Value = 10 },
                            Amount = new AmountType2{ currencyID = "TRY", Value = 100 },
                            BaseAmount = new BaseAmountType{ currencyID = "TRY", Value = 1000},
                        }},
                        TaxTotal = new TaxTotalType
                        {
                            TaxAmount = new TaxAmountType { currencyID = "TRY", Value = 180 },
                            TaxSubtotal = new[]
                            {
                                new TaxSubtotalType
                                {
                                    TaxableAmount = new TaxableAmountType{ currencyID = "TRY", Value = 1000 },
                                    TaxAmount = new TaxAmountType{ currencyID = "TRY", Value = 180 },
                                    CalculationSequenceNumeric = new CalculationSequenceNumericType{ Value = 1 },
                                    Percent = new PercentType1{ Value = 18 },
                                    TaxCategory = new TaxCategoryType
                                    {
                                        TaxScheme = new TaxSchemeType
                                        {
                                            TaxTypeCode = new TaxTypeCodeType{ Value = "0015", name = "KDV" },
                                            Name = new NameType1{ Value = "KDV" }
                                        },
                                    }
                                }
                            }
                        },
                        Item = new ItemType { Name = new NameType1 { Value = "Açıklama gir Stok Kod Adı neyin" } },
                        Price = new PriceType { PriceAmount = new PriceAmountType { currencyID = "TRY", Value = 200 } }



                    };
                  //  x++;
                    lines.Add(line);
               // };

                return lines.ToArray();
            };




            var fatura = new InvoiceType
            {



                UBLVersionID = new UBLVersionIDType { Value = "2.1" },
                CustomizationID = new CustomizationIDType { Value = "TR1.2" },
                ProfileID = new ProfileIDType { Value = "TEMELFATURA" },
                ID = new IDType { Value = FaturaBilgileri.FaturaNo },
                CopyIndicator = new CopyIndicatorType { Value = false },
                UUID = new UUIDType { Value = Guid.NewGuid().ToString() },
                IssueDate = new IssueDateType { Value = FaturaBilgileri.Tarih },
                IssueTime = new IssueTimeType { Value = FaturaBilgileri.Tarih },
                InvoiceTypeCode = new InvoiceTypeCodeType { Value = "SATIS" },  // SATIS,IADE,
                Note = new[] { new NoteType { Value = "Açıklamalar" },
                    new NoteType { Value = "İş bu fatura muhteviyatına 7 gün içerisinde itiraz edilmediği taktirde aynen kabul edilmiş sayılır." },
                    new NoteType { Value = FaturaBilgileri.CariKod + " " + FaturaBilgileri.CariUnv }
                },
                DocumentCurrencyCode = new DocumentCurrencyCodeType { Value = "TRY" },
                LineCountNumeric = new LineCountNumericType { Value = 3 },
                AdditionalDocumentReference = new[] { new DocumentReferenceType
                {
                    ID = new IDType { Value = Guid.NewGuid().ToString() },
                    IssueDate = new IssueDateType { Value = FaturaBilgileri.Tarih },
                    DocumentType = new DocumentTypeType { Value = "XSLT" },
                    Attachment = new AttachmentType
                    {
                        EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType
                        {
                            characterSetCode = "UTF-8",

                            encodingCode = "Base64",
                            mimeCode = "application/xml",
                            filename = "EArchiveInvoice.xslt",
                            Value = Encoding.UTF8.GetBytes(new StreamReader(new FileStream(path: Application.StartupPath + "\\" + "EFatura.xslt", FileMode.Open, FileAccess.Read), Encoding.UTF8).ReadToEnd())

                        }
                    }
                },

                    new DocumentReferenceType
                    {
                        ID = new IDType { Value = Guid.NewGuid().ToString() },
                        IssueDate = new IssueDateType { Value = FaturaBilgileri.Tarih },
                        DocumentTypeCode = new DocumentTypeCodeType { Value = "SendingType" },
                        DocumentType = new DocumentTypeType { Value = " ELEKTRONIK" }
                    }
                },
                Signature = new[] { new SignatureType
                {
                    ID = new IDType { schemeID = "VKN_TCKN", Value = "7500263381" },
                    SignatoryParty = new PartyType
                    {
                        PartyIdentification = new[] { new PartyIdentificationType { ID = new IDType { schemeID = "VKN", Value = "7500263381" } } },
                        PostalAddress = new AddressType
                        {
                            Room = new RoomType { Value = "" },
                            BlockName = new BlockNameType { Value = "B Blok" },
                            BuildingName = new BuildingNameType { Value = "dd" },
                            BuildingNumber = new BuildingNumberType { Value = "35" },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = "Çanka" },
                            CityName = new CityNameType { Value = "ANAKATA" },
                            PostalZone = new PostalZoneType { Value = "78200" },
                            Country = new CountryType { Name = new NameType1 { Value = "Türkiye" } }

                        }

                    },
                    DigitalSignatureAttachment = new AttachmentType { ExternalReference = new ExternalReferenceType { URI = new URIType { Value = "#Signature_" + FaturaBilgileri.FaturaNo } } }

                },


                },

                AccountingSupplierParty = new SupplierPartyType
                {
                    Party = new PartyType
                    {
                        PartyIdentification = new[]
                        {

                            new PartyIdentificationType { ID = new IDType { schemeID = "VKN", Value = "7500263381" } },
                            new PartyIdentificationType { ID = new IDType { schemeID = "MERSISNO", Value = "368955457878" } },
                        },
                        PartyName = new PartyNameType { Name = new NameType1 { Value = "MSK YAZILIM" } },

                        PostalAddress = new AddressType
                        {
                            Room = new RoomType { Value = "" },
                            BlockName = new BlockNameType { Value = "B Blok" },
                            BuildingName = new BuildingNameType { Value = "dd" },
                            BuildingNumber = new BuildingNumberType { Value = "35" },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = "Çanka" },
                            CityName = new CityNameType { Value = "ANAKATA" },
                            PostalZone = new PostalZoneType { Value = "78200" },
                            Country = new CountryType { Name = new NameType1 { Value = "Türkiye" } }

                        },
                        WebsiteURI = new WebsiteURIType { Value = "www.mskyazilim.com" },
                        Contact = new ContactType { ElectronicMail = new ElectronicMailType { Value = "info@gencbilgi.net" }, Telephone = new TelephoneType { Value = "05559963526" } },
                        PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = "KARABÜK VERGİ DAİRESİ" }, TaxTypeCode = new TaxTypeCodeType { Value = "078220" } } }

                    },


                },

                AccountingCustomerParty = new CustomerPartyType
                {
                    Party = new PartyType
                    {
                        PartyIdentification = new[]
                        {
                            new PartyIdentificationType { ID = new IDType { schemeID = "TCK", Value = "3498788" } },
                        },

                        PartyName = new PartyNameType { Name = new NameType1 { Value = "UZUNER PAZARLAMA" } },


                        PostalAddress = new AddressType
                        {
                            Room = new RoomType { Value = "" },
                            BlockName = new BlockNameType { Value = "C BLOK" },
                            BuildingName = new BuildingNameType { Value = "KEN" },
                            BuildingNumber = new BuildingNumberType { Value = "78" },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = "ÜNİVERSİTE MAHALLESİ" },
                            CityName = new CityNameType { Value = "KARABÜK" },
                            PostalZone = new PostalZoneType { Value = "78200" },
                            Country = new CountryType { Name = new NameType1 { Value = "Türkiye" } }

                        },
                        Contact = new ContactType { ElectronicMail = new ElectronicMailType { Value = "info@UZUNER.COM" }, Telephone = new TelephoneType { Value = "037063526" } },
                        Person = new PersonType { FirstName = new FirstNameType { Value = "ISIM" }, FamilyName = new FamilyNameType { Value = "SOYISIM" } } // şahıslarda
                                                                                                                                                            // PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = "KARABÜK VERGİ DAİRESİ" }, TaxTypeCode = new TaxTypeCodeType { Value = "078220" } } }// Firmalarda


                    },
                },

                TaxTotal = new[]
                { new TaxTotalType
                { TaxAmount = new TaxAmountType { Value = FaturaBilgileri.KdvTutar },
                    TaxSubtotal = new[]
                      {

                        new TaxSubtotalType
                        {
                            TaxableAmount = new TaxableAmountType { currencyID = "TRY", Value = FaturaBilgileri.NetTutar },
                            TaxAmount = new TaxAmountType { currencyID = "TRY", Value = FaturaBilgileri.KdvTutar },
                            CalculationSequenceNumeric = new CalculationSequenceNumericType { Value = 1 },
                            TransactionCurrencyTaxAmount = new TransactionCurrencyTaxAmountType { currencyID = "TRY", Value = FaturaBilgileri.KdvTutar },
                            Percent = new PercentType1 { Value = 18 }, // zorunlu değil
                            TaxCategory = new TaxCategoryType { Name = new NameType1 { Value = "KDV" }, TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = "KDV GERCEK" }, TaxTypeCode = new TaxTypeCodeType { Value = "0015" } } }



                        }
                    }
                }
                },
                LegalMonetaryTotal = new MonetaryTotalType
                {

                    LineExtensionAmount = new LineExtensionAmountType { currencyID = "TRY", Value = FaturaBilgileri.Tutar },
                    TaxExclusiveAmount = new TaxExclusiveAmountType { Value = FaturaBilgileri.NetTutar },
                    TaxInclusiveAmount = new TaxInclusiveAmountType { Value = FaturaBilgileri.ToplamTutar },
                    AllowanceTotalAmount = new AllowanceTotalAmountType { Value = FaturaBilgileri.Iskonto },
                    PayableAmount = new PayableAmountType { Value = FaturaBilgileri.ToplamTutar },

                },



               InvoiceLine = FaturaHareketleri()

               // InvoiceLine = FaturaHareketleri()


        };


           

            var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
            var ms = new MemoryStream();
            var write = XmlWriter.Create(ms, settings);
            var srl = new XmlSerializer(fatura.GetType());
            srl.Serialize(write, fatura, XmlNameSpace());
            ms.Flush();
            ms.Seek(offset: 0, loc: SeekOrigin.Begin);
            var srRead = new StreamReader(ms);
            var readxml = srRead.ReadToEnd();
            var path = Path.Combine(Application.StartupPath + "\\EFATURA\\" + fatura.ID.Value.ToString() + ".XML");


            void FaturaOlus()
            {
                using (var sWrt = new StreamWriter(path, append: false, Encoding.UTF8))

                {
                    sWrt.Write(readxml);
                    sWrt.Close();




                }


            }
            if (!Directory.Exists(Application.StartupPath + "\\EFATURA"))

                Directory.CreateDirectory(Application.StartupPath + "\\EFATURA");
            if (!File.Exists(Application.StartupPath + "\\EFATURA\\" + fatura.ID.Value.ToString() + ".XML"))
                FaturaOlus();
            else
                if (MessageBox.Show("Dosya Daha önce var", "Dikka", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                FaturaOlus();

           


                XmlSerializerNamespaces XmlNameSpace()
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                    ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
                    ns.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                    ns.Add("ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
                    ns.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                    ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                    ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                    ns.Add("ccts", "urn:un:unece:uncefact:documentation:2");
                    ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
                    return ns;
                }

            


        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();

        }



    }

}
