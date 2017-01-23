using Model.Helper;
using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model.PengadaanRepository;

namespace Reston.Pinata.Model.Migrations
{
    internal static class Seeder
    {
        internal static void Seed(JimbisContext context)
        {
            //SeedReferenceDatas(context);
            SeedMenu(context);
            SeedRoleMenu(context);
        }

        private static void SeedReferenceDatas(JimbisContext context)
        {
            //menyimpan data CURRENCY
            ReferenceData[] masterData = new ReferenceData[]{
                //CURRENCY
                new ReferenceData() { Qualifier = RefDataQualifier.CURRENCY, Code = "IDR", LocalizedName = "IDR", LocalizedDesc = "Indonesian Rupiah" }
                ,new ReferenceData() { Qualifier = RefDataQualifier.CURRENCY, Code = "USD", LocalizedName = "USD", LocalizedDesc = "United States Dollar" }
                ,new ReferenceData() { Qualifier = RefDataQualifier.CURRENCY, Code = "SGD", LocalizedName = "SGD", LocalizedDesc = "Singapore Dollar" }
                ,new ReferenceData() { Qualifier = RefDataQualifier.CURRENCY, Code = "MYR", LocalizedName = "SGD", LocalizedDesc = "Malaysian Ringgit" }
                ,new ReferenceData() { Qualifier = RefDataQualifier.CURRENCY, Code = "AUD", LocalizedName = "SGD", LocalizedDesc = "Australian Dollar" }
                //PROVINCE
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "NONE", LocalizedName = "Nihil", LocalizedDesc = "Nihil" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-AC", LocalizedName = "Aceh", LocalizedDesc = "Daerah Khusus Aceh" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-BA", LocalizedName = "Bali", LocalizedDesc = "Bali" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-BB", LocalizedName = "Bangka Belitung", LocalizedDesc = "Kepulauan Bangka Belitung" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-BT", LocalizedName = "Banten", LocalizedDesc = "Banten" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-BE", LocalizedName = "Bengkulu", LocalizedDesc = "Bengkulu" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-GO", LocalizedName = "Gorontalo", LocalizedDesc = "Gorontalo" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-JA", LocalizedName = "Jambi", LocalizedDesc = "Jambi" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-JB", LocalizedName = "Jawa Barat", LocalizedDesc = "Jawa Barat" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-JT", LocalizedName = "Jawa Tengah", LocalizedDesc = "Jawa Tengah" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-JI", LocalizedName = "Jawa Timur", LocalizedDesc = "Jawa Timur" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-KB", LocalizedName = "Kalimantan Barat", LocalizedDesc = "Kalimantan Barat" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-KS", LocalizedName = "Kalimantan Selatan", LocalizedDesc = "Kalimnatan Selatan" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-KT", LocalizedName = "Kalimantan Tengah", LocalizedDesc = "Kalimantan Tengah" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-KI", LocalizedName = "Kalimantan Timur", LocalizedDesc = "Kalimantan Utara" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-KU", LocalizedName = "Kalimantan Utara", LocalizedDesc = "Kalimantan Utara" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-KR", LocalizedName = "Kepulauan Riau", LocalizedDesc = "Kepulauan Riau" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-LA", LocalizedName = "Lampung", LocalizedDesc = "Lampung" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-MA", LocalizedName = "Maluku", LocalizedDesc = "Maluku" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-MU", LocalizedName = "Maluku Utara", LocalizedDesc = "Maluku Utara" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-NB", LocalizedName = "Nusa Tenggara Barat", LocalizedDesc = "Nusa Tenggara Barat" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-NT", LocalizedName = "Nusa Tenggara Timur", LocalizedDesc = "Nusa Tenggara Timur" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-PA", LocalizedName = "Papua", LocalizedDesc = "Daerah khusus Papua" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-PB", LocalizedName = "Papua Barat", LocalizedDesc = "Daerah khusus Papua Barat" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-RI", LocalizedName = "Riau", LocalizedDesc = "Riau" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-SR", LocalizedName = "Sulawesi Barat", LocalizedDesc = "Sulawesi Barat" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-SN", LocalizedName = "Sulawesi Selatan", LocalizedDesc = "Sulawesi Selatan" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-ST", LocalizedName = "Sulawesi Tengah", LocalizedDesc = "Sulawesi Tengah" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-SG", LocalizedName = "Sulawesi Tenggara", LocalizedDesc = "Sulawesi Tenggara" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-SA", LocalizedName = "Sulawesi Utara", LocalizedDesc = "Sulawesi Utara" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-SB", LocalizedName = "Sumatera Barat", LocalizedDesc = "Sumatera Barat" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-SS", LocalizedName = "Sumatera Selatan", LocalizedDesc = "Sumatera Selatan" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-SU", LocalizedName = "Sumatera Utara", LocalizedDesc = "Sumatera Utara" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-JK", LocalizedName = "Jakarta Raya", LocalizedDesc = "Daerah khusus ibukota Jakarta" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "ID-YO", LocalizedName = "Yogyakarta", LocalizedDesc = "Daerah istimewa Yogyakarta" }
                , new ReferenceData { Qualifier = RefDataQualifier.PROVINCE, Code = "OTHER", LocalizedName = "Lain-lain", LocalizedDesc = "Lain-lain", StringAttr1 = "" }
                //SATUAN
                , new ReferenceData { Qualifier = RefDataQualifier.SATUAN, Code = "UNIT", LocalizedName = "Unit", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.SATUAN, Code = "KG", LocalizedName = "Kg", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.SATUAN, Code = "ML", LocalizedName = "Ml", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.SATUAN, Code = "M2", LocalizedName = "m2", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.SATUAN, Code = "LUSIN", LocalizedName = "Lusin", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.SATUAN, Code = "SAK", LocalizedName = "Sak", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.SATUAN, Code = "OTHER", LocalizedName = "Lainnya", LocalizedDesc = "", StringAttr1 = "" }
                //SATUAN
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALI", LocalizedName = "REGION I", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALII", LocalizedName = "REGION II", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALIII", LocalizedName = "REGION III", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALIV", LocalizedName = "REGION IV", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALV", LocalizedName = "REGION V", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALVI", LocalizedName = "REGION VI", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALVII", LocalizedName = "REGION VII", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALVIII", LocalizedName = "REGION VIII", LocalizedDesc = "", StringAttr1 = "" }
                , new ReferenceData { Qualifier = RefDataQualifier.REGION, Code = "REGIONALIX", LocalizedName = "REGION IX", LocalizedDesc = "", StringAttr1 = "" }
                //BANK
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B0",LocalizedName="BANK MANDIRI"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B1",LocalizedName="ANGLOMAS INTERNATIONAL BANK"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B2",LocalizedName="BANGKOK BANK"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B3",LocalizedName="BANK AGRIS"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B4",LocalizedName="BANK ANDARA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B5",LocalizedName="BANK ANTAR DAERAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B6",LocalizedName="BANK ANZ INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B7",LocalizedName="BANK ARTHA GRAHA INTERNASIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B8",LocalizedName="BANK ARTOS INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B9",LocalizedName="BANK BISNIS INTERNASIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B10",LocalizedName="BANK BUMI ARTA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B11",LocalizedName="BANK CAPITAL INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B12",LocalizedName="BANK CENTRATAMA NASIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B13",LocalizedName="BANK CHINATRUST INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B14",LocalizedName="BANK COMMONWEALTH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B15",LocalizedName="BANK DANAMON INDONESIA UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B16",LocalizedName="BANK DANAMON INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B17",LocalizedName="BANK DBS INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B18",LocalizedName="BANK DINAR"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B19",LocalizedName="BANK DKI UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B20",LocalizedName="BANK EKONOMI RAHARJA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B21",LocalizedName="BANK FAMA INTERNATIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B22",LocalizedName="BANK GANESHA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B23",LocalizedName="BANK HANA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B24",LocalizedName="BANK HARDA INTERNASIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B25",LocalizedName="BANK HIMPUNAN SAUDARA 1906"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B26",LocalizedName="BANK ICBC INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B27",LocalizedName="BANK INA PERDANA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B28",LocalizedName="BANK INDEX SELINDO"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B29",LocalizedName="BANK JABAR BANTEN SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B30",LocalizedName="BANK JABAR BANTEN"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B31",LocalizedName="BANK JASA JAKARTA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B32",LocalizedName="BANK JATIM UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B33",LocalizedName="BANK JTRUST"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B34",LocalizedName="BANK KALBAR UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B35",LocalizedName="BANK KESEJAHTERAAN EKONOMI"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B36",LocalizedName="BANK LIMAN INTERNASIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B37",LocalizedName="BANK MANDIRI SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B38",LocalizedName="BANK CIMB NIAGA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B39",LocalizedName="BANK MASPION INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B40",LocalizedName="BANK MAYAPADA INTERNATIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B41",LocalizedName="BANK MAYORA INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B42",LocalizedName="BANK MEGA "}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B43",LocalizedName="BANK MEGA INDONESIA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B44",LocalizedName="BANK MESTIKA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B45",LocalizedName="BANK METRO EKSPRESS"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B46",LocalizedName="BANK MITRA NIAGA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B47",LocalizedName="BANK MIZUHO INDONESIA (FUJI)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B48",LocalizedName="BANK MUAMALAT INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B49",LocalizedName="BANK MULTI ARTA SENTOSA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B50",LocalizedName="BANK NATIONAL NOBU"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B51",LocalizedName="BANK NTB UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B52",LocalizedName="BANK NUSANTARA PARAHYANGAN"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B53",LocalizedName="BANK OF AMERICA, NA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B54",LocalizedName="BANK OF CHINA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B55",LocalizedName="BANK OF INDIA INDONESIA (BOII)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B56",LocalizedName="BANK OF TOKYO MITSUBISHI UFJ LTD."}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B57",LocalizedName="BANK PANIN SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B58",LocalizedName="BANK PANIN"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B59",LocalizedName="BANK PERMATA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B60",LocalizedName="BANK PUNDI INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B61",LocalizedName="BANK QNB KESAWAN"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B62",LocalizedName="BANK RESONA PERDANIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B63",LocalizedName="BANK ROYAL INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B64",LocalizedName="BANK SAHABAT PURBA DANARTA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B65",LocalizedName="BANK SAHABAT SAMPOERNA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B66",LocalizedName="BANK SBI INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B67",LocalizedName="BANK SINAR HARAPAN BALI"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B68",LocalizedName="BANK SINARMAS UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B69",LocalizedName="BANK SINARMAS"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B70",LocalizedName="BANK SULAWESI SELATAN (SULSEL)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B71",LocalizedName="BANK SULAWESI SELATAN UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B72",LocalizedName="BANK SUMITOMO MITSUI INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B73",LocalizedName="BANK SUMSEL UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B74",LocalizedName="BANK VICTORIA INTERNATIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B75",LocalizedName="BANK VICTORIA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B76",LocalizedName="BANK WINDU KENTJANA INTERNASIONAL"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B77",LocalizedName="BANK WOORI INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B78",LocalizedName="BANK YUDHA BHAKTI"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B79",LocalizedName="BCA (BANK CENTRAL ASIA) SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B80",LocalizedName="BCA (BANK CENTRAL ASIA)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B81",LocalizedName="BII (BANK INTERNASIONAL INDONESIA) UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B82",LocalizedName="BII (BANK INTERNASIONAL INDONESIA)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B83",LocalizedName="BNI 46"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B84",LocalizedName="BNP PARIBAS INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B85",LocalizedName="BPD ACEH UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B86",LocalizedName="BPD ACEH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B87",LocalizedName="BPD BALI"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B88",LocalizedName="BPD BENGKULU"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B89",LocalizedName="BPD DIY (DAERAH ISTIMEWA YOGYA) UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B90",LocalizedName="BPD DIY (DAERAH ISTIMEWA YOGYA)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B91",LocalizedName="BPD DKI JAKARTA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B92",LocalizedName="BPD JAMBI"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B93",LocalizedName="BPD JATENG (JAWA TENGAH) UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B94",LocalizedName="BPD JATENG (JAWA TENGAH)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B95",LocalizedName="BPD JATIM (JAWA TIMUR)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B96",LocalizedName="BPD KALBAR (KALIMANTAN BARAT)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B97",LocalizedName="BPD KALIMANTAN SELATAN UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B98",LocalizedName="BPD KALSEL (KALIMANTAN SELATAN)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B99",LocalizedName="BPD KALTENG (KALIMANTAN TENGAH)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B100",LocalizedName="BPD KALTIM (KALIMANTAN TIMUR)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B101",LocalizedName="BPD LAMPUNG"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B102",LocalizedName="BPD MALUKU"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B103",LocalizedName="BPD NTB (NUSA TENGGARA BARAT)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B104",LocalizedName="BPD NTT (NUSA TENGGARA TIMUR)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B105",LocalizedName="BPD PAPUA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B106",LocalizedName="BPD RIAU UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B107",LocalizedName="BPD RIAU"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B108",LocalizedName="BPD SULAWESI TENGAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B109",LocalizedName="BPD SULAWESI TENGGARA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B110",LocalizedName="BPD SULUT (SULAWESI UTARA)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B111",LocalizedName="BPD SUMATERA BARAT UNIT USAHA SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B112",LocalizedName="BPD SUMBAR (SUMATERA BARAT)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B113",LocalizedName="BPD SUMSEL DAN BABEL "}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B114",LocalizedName="BPD SUMUT (SUMATERA UTARA)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B115",LocalizedName="BPR Eka Bumi Artha"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B116",LocalizedName="BPR KS"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B117",LocalizedName="BRI (BANK RAKYAT INDONESIA)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B118",LocalizedName="BTN (BANK TABUNGAN NEGARA) UNIT USAHA SYARIAH "}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B119",LocalizedName="BTN (BANK TABUNGAN NEGARA)"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B120",LocalizedName="BTPN"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B121",LocalizedName="BUKOPIN SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B122",LocalizedName="BUKOPIN"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B123",LocalizedName="Bank MNC International"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B124",LocalizedName="CITIBANK"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B125",LocalizedName="DEUTSCHE BANK AG "}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B126",LocalizedName="HSBC UNIT SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B127",LocalizedName="HSBC"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B128",LocalizedName="INDOSAT DOMPETKU"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B129",LocalizedName="JP MORGAN CHASE BANK"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B130",LocalizedName="KEB INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B131",LocalizedName="MAYBANK SYARIAH INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B132",LocalizedName="Mitra"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B133",LocalizedName="OCBC NISP"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B134",LocalizedName="PANIN SYARIAH"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B135",LocalizedName="PRIMA MASTER BANK"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B136",LocalizedName="RABOBANK INTERNATIONAL INDONESIA"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B137",LocalizedName="STANDARD CHARTERED BANK"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B138",LocalizedName="SYARIAH BRI"}
                , new ReferenceData { Qualifier = RefDataQualifier.BANK, Code ="B139",LocalizedName="UUOB INDONESIA"}
                
                , new ReferenceData { Qualifier = RefDataQualifier.PeriodeAnggaran, Code ="PA001",LocalizedName="TA-2016"}
                , new ReferenceData { Qualifier = RefDataQualifier.PeriodeAnggaran, Code ="PA002",LocalizedName="TA-2017"}
                , new ReferenceData { Qualifier = RefDataQualifier.PeriodeAnggaran, Code ="PA003",LocalizedName="TA-2018"}

                , new ReferenceData { Qualifier = RefDataQualifier.UnitKerja, Code ="UK001",LocalizedName="Devisi Pengadaan"}
                , new ReferenceData { Qualifier = RefDataQualifier.UnitKerja, Code ="UK002",LocalizedName="Devisi SDM"}
                , new ReferenceData { Qualifier = RefDataQualifier.UnitKerja, Code ="UK003",LocalizedName="Devisi R&D"}

                , new ReferenceData { Qualifier = RefDataQualifier.JenisPekerjaan, Code ="JP001",LocalizedName="Non Sipil"}
                , new ReferenceData { Qualifier = RefDataQualifier.JenisPekerjaan, Code ="JP002",LocalizedName="Sipil"}
                , new ReferenceData { Qualifier = RefDataQualifier.JenisPekerjaan, Code ="JP003",LocalizedName="Sipil Non Konstruksi"}

                , new ReferenceData { Qualifier = RefDataQualifier.JenisPembelanjaan, Code ="JB001",LocalizedName="CAPEX"}
                , new ReferenceData { Qualifier = RefDataQualifier.JenisPembelanjaan, Code ="JB002",LocalizedName="OPEX"}
                , new ReferenceData { Qualifier = RefDataQualifier.JenisPembelanjaan, Code ="JB003",LocalizedName="Solcost"}
                , new ReferenceData { Qualifier = RefDataQualifier.JenisPembelanjaan, Code ="JB004",LocalizedName="Fixed Asset"}
            };

            //produk
            Produk[] produkData = new Produk[] { 
                new Produk{Nama = "Partisi Kayu Jati", Satuan = "unit",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 3950000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Partisi Kayu",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Lebar", Nilai="60cm-80cm"},new AtributSpesifikasi{Nama="Tinggi",Nilai = "190cm-210cm"}, new AtributSpesifikasi{Nama="Tebal", Nilai="3,5cm tanpa kusen "}}}}
                ,new Produk{Nama = "Partisi Kayu Akasia", Satuan = "unit",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 2950000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Partisi Kayu",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Lebar", Nilai="60cm-80cm"},new AtributSpesifikasi{Nama="Tinggi",Nilai = "190cm-210cm"}, new AtributSpesifikasi{Nama="Tebal", Nilai="3,5cm tanpa kusen "}}}}
                ,new Produk{Nama = "Partisi Kayu Jati Super", Satuan = "unit",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 5950000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Partisi Kayu Super",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Lebar", Nilai="60cm-80cm"},new AtributSpesifikasi{Nama="Tinggi",Nilai = "190cm-210cm"}, new AtributSpesifikasi{Nama="Tebal", Nilai="3,5cm tanpa kusen "}}}}
                ,new Produk{Nama = "Bata Merah", Satuan = "biji",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 2000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Batu Bata",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Tanah Liat"},new AtributSpesifikasi{Nama="dimensi",Nilai = "20x10x5"}}}}
                ,new Produk{Nama = "Semen Holcim", Satuan = "sak",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 61000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Semen",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Semen"}}}}
                ,new Produk{Nama = "Semen Tiga Roda", Satuan = "sak",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 61000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Semen",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Semen"}}}}
                ,new Produk{Nama = "Semen Baturaja", Satuan = "sak",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 62000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Semen",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Semen"}}}}
                ,new Produk{Nama = "Semen Padang", Satuan = "sak",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 60000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Semen",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Semen"}}}}
                ,new Produk{Nama = "Bata Merah Jumbo", Satuan = "biji",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 3500, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "Batu Bata",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Tanah Liat"}}}}
                ,new Produk{Nama = "Triplek 3mmx4x8 palm", Satuan = "lembar",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 41000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "triplek",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Kayu"}}}}
                ,new Produk{Nama = "Triplek 4mmx4x8 palm", Satuan = "lembar",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 51000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "triplek",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Kayu"}}}}
                ,new Produk{Nama = "Triplek 6mmx4x8 tunas", Satuan = "lembar",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 62000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "triplek",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Kayu"}}}}
                ,new Produk{Nama = "Triplek teakwood 4×8 TM", Satuan = "lembar",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 75000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "triplek",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Kayu"}}}}
                ,new Produk{Nama = "Triplek Blockboard 18mm 122×244", Satuan = "lembar",RiwayatHarga = new List<RiwayatHarga>(){new RiwayatHarga{Harga = 200000, Currency="IDR", Region="REGION I", Sumber="Perkiraan", Tanggal = DateTime.Now,User="default"}},
                    KategoriSpesifikasi = new KategoriSpesifikasi{Nama = "triplek",AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama = "Bahan", Nilai="Kayu"}}}}

            };

            //template
            KategoriSpesifikasi[] kategoriData = new KategoriSpesifikasi[]{
                new KategoriSpesifikasi{Nama = "Mobil", Deskripsi = "TEMPLATE", AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama="Manufaktur"}, new AtributSpesifikasi{Nama="Mesin"},new AtributSpesifikasi{Nama="Isi Silinder"},new AtributSpesifikasi{Nama="Sistem Kemudi"},new AtributSpesifikasi{Nama="Bahan Bakar"}}}  
                ,new KategoriSpesifikasi{Nama = "Buku", Deskripsi = "TEMPLATE", AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama="Manufaktur"}, new AtributSpesifikasi{Nama="Halaman"},new AtributSpesifikasi{Nama="Ukuran"},new AtributSpesifikasi{Nama="Sampul"},new AtributSpesifikasi{Nama="Warna"}}}  
                ,new KategoriSpesifikasi{Nama = "Alat Kebersihan", Deskripsi = "TEMPLATE", AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama="Manufaktur"}, new AtributSpesifikasi{Nama="Volume"},new AtributSpesifikasi{Nama="Material"},new AtributSpesifikasi{Nama="Dimensi"}}}  
                ,new KategoriSpesifikasi{Nama = "Meubel", Deskripsi = "TEMPLATE", AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama="Manufaktur"}, new AtributSpesifikasi{Nama="Bahan"},new AtributSpesifikasi{Nama="Ukuran"},new AtributSpesifikasi{Nama="Warna"}}}  
                ,new KategoriSpesifikasi{Nama = "Umum", Deskripsi = "TEMPLATE", AtributSpesifikasi = new List<AtributSpesifikasi>(){new AtributSpesifikasi{Nama="Ukuran"}, new AtributSpesifikasi{Nama="Bahan"},new AtributSpesifikasi{Nama="Isi Silinder"},new AtributSpesifikasi{Nama="Warna"}}}  
            };

            //pembobotan
            KreteriaPembobotan[] kriteriaPembobotan = new KreteriaPembobotan[]{
                new KreteriaPembobotan{NamaKreteria="Harga",Bobot=60},
                new KreteriaPembobotan{NamaKreteria="Teknis",Bobot=40}           
           };

            context.KategoriSpesifikasis.AddRange(kategoriData);
            context.ReferenceDatas.AddRange(masterData);
            context.Produks.AddRange(produkData);
            context.KreteriaPembobotans.AddRange(kriteriaPembobotan);
            context.SaveChanges();
        }

        private static void SeedMenu(JimbisContext context)
        {
            Reston.Eproc.Model.Entities.Menu[] menulst = new Reston.Eproc.Model.Entities.Menu[]{
                new Reston.Eproc.Model.Entities.Menu{Id=1,menu="Dashboard",url="dashboard.html",css="fa fa-bar-chart"},
                new Reston.Eproc.Model.Entities.Menu{Id=2,menu="Pengadaan-e",url="pengadaan-list.html",css="fa fa-cubes"},
                new Reston.Eproc.Model.Entities.Menu{Id=3,menu="Repository HPS",url="repository-rks.html",css="fa fa-book"},
                new Reston.Eproc.Model.Entities.Menu{Id=4,menu="Rekanan",url="rekanan.html",css="fa fa-vimeo"},
                new Reston.Eproc.Model.Entities.Menu{Id=5,menu="Katalog-e",url="eCatalogue2.html",css="fa fa-book"},
                new Reston.Eproc.Model.Entities.Menu{Id=6,menu="Report",url="report.html",css="fa fa-file-excel-o"},
                new Reston.Eproc.Model.Entities.Menu{Id=7,menu="Sistem Monitoring",url="monitoring-selection.html",css="fa fa-tv"},
                new Reston.Eproc.Model.Entities.Menu{Id=8,menu="PKS",url="pks.html",css="fa fa-pencil-square"},
                new Reston.Eproc.Model.Entities.Menu{Id=9,menu="SPK / PO",url="spk.html",css="fa fa-pencil"},
                new Reston.Eproc.Model.Entities.Menu{Id=10,menu="Budget Capex",url="anggaran.html",css="fa fa-money"},
                 new Reston.Eproc.Model.Entities.Menu{Id=11,menu="Pengadaan-e",url="pengadaan-rekanan.html",css="fa fa-cubes"},
                 new Reston.Eproc.Model.Entities.Menu{Id=12,menu="Profil Saya",url="rekanan-side-terdaftar.html",css="fa fa-user"},
                 new Reston.Eproc.Model.Entities.Menu{Id=13,menu="Monitoring",url="rekanan-monitoring.html",css="fa fa-tv"},
                 new Reston.Eproc.Model.Entities.Menu{Id=14,menu="User Management",url= IdLdapConstants.IDM.Url+"admin/userid",css="fa fa-user"},                 
                new Reston.Eproc.Model.Entities.Menu{Id=15,menu="LogOut",url="Api/Header/Signout",css="fa fa-sign-out"}
            };
            context.Menu.AddRange(menulst);
            context.SaveChanges();
        }
        private static void SeedRoleMenu(JimbisContext context)
        {
            
            Reston.Eproc.Model.Entities.RoleMenu[] roleMenulst = new Reston.Eproc.Model.Entities.RoleMenu[]{
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_approver},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_compliance},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_direksi},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_dirut},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_end_user},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_user},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=15,Role=IdLdapConstants.Roles.pRole_procurement_vendor},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_approver},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_compliance},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_direksi},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_dirut},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_procurement_end_user},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=1,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_approver},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_compliance},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_direksi},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_dirut},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_procurement_end_user},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=2,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=3,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=3,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=3,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=3,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=3,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=4,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=4,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=4,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=4,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=4,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=5,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=5,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=5,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=5,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=5,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_end_user},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_admin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=6,Role=IdLdapConstants.Roles.pRole_procurement_end_user},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=7,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=7,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=7,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=7,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=8,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=8,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=8,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=8,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=9,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=9,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=9,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=9,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=10,Role=IdLdapConstants.Roles.pRole_procurement_head},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=10,Role=IdLdapConstants.Roles.pRole_procurement_manager},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=10,Role=IdLdapConstants.Roles.pRole_procurement_staff},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=10,Role=IdLdapConstants.Roles.pRole_procurement_superadmin},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=11,Role=IdLdapConstants.Roles.pRole_procurement_vendor},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=12,Role=IdLdapConstants.Roles.pRole_procurement_vendor},
                new Reston.Eproc.Model.Entities.RoleMenu{MenuId=13,Role=IdLdapConstants.Roles.pRole_procurement_vendor}
            };
            
            context.RoleMenu.AddRange(roleMenulst);
            context.SaveChanges();
        }
    }
}
