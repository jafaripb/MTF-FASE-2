using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Helper
{
    public static class Common
    {
        
        public static DateTime ConvertDate(string date,string formatDate)
        {            
            DateTime Date = DateTime.ParseExact(date, formatDate, CultureInfo.InvariantCulture);
            return Date;
        }
        public static string SaveSukses()
        {
            string result = "Data Behasil di Tambah";

            return result;
        }

        public static string UpdateSukses()
        {
            string result = "Data Behasil di Update";

            return result;
        }
        public static string DeleteSukses()
        {
            string result = "Data Behasil di Update";

            return result;
        }

        public static string ConvertNamaBulan(int bulan)
        {
            switch (bulan)
            {
                case 1: return "Januari";
                case 2: return "Febuari";
                case 3: return "Maret";
                case 4: return "April";
                case 5: return "Mei";
                case 6: return "Juni";
                case 7: return "Juli";
                case 8: return "Agustus";
                case 9: return "September";
                case 10: return "Oktober";
                case 11: return "November";
                case 12: return "Desember";
                default: return "";
            }
        }

        public static string ConvertHari(int hari)
        {
            switch (hari)
            {
                case 1: return "Senin";
                case 2: return "Selasa";
                case 3: return "Rabu";
                case 4: return "Kamis";
                case 5: return "Jum'at";
                case 6: return "Sabtu";
                case 7: return "Minggu";
                default: return "";
            }
        }

        public static string ConvertBulanRomawi(int bulan)
        {
            switch (bulan)
            {
                case 1: return "I";
                case 2: return "II";
                case 3: return "III";
                case 4: return "IV";
                case 5: return "V";
                case 6: return "VI";
                case 7: return "VII";
                case 8: return "VIII";
                case 9: return "IX";
                case 10: return "X";
                case 11: return "XI";
                case 12: return "XII";
                default: return "";
            }
        }

        public static string DecodeToBase64(string encodedData)
        {
            string salt = "JIMBIS";
            System.Text.Decoder decoder = new UTF8Encoding().GetDecoder();
            string encodeDataSalt = salt + encodedData;
            byte[] bytes = Convert.FromBase64String(encodeDataSalt);
            char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
            decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
            return new string(chars);
        }

        public static string EncodeToBase64(string decodData)
        {
            string str2;
            string salt = "JIMBIS";
            try
            {
                byte[] buffer = new byte[decodData.Length];
                str2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(decodData));
                str2.Replace(salt,"");
            }
            catch (Exception exception)
            {
                throw new Exception("Error in base64Encode" + exception.Message);
            }
            return str2;
        }

        
    }

    public static class Cultures
    {
        public static readonly CultureInfo Indonesia =
            CultureInfo.GetCultureInfo("id-ID");
    }

    public class JimbisEncrypt{
        public string Encrypt(string value)
        {
            //byte[] bytes = Encoding.ASCII.GetBytes("035huB@!859197007083061185919800");
            //byte[] iV = Encoding.ASCII.GetBytes   ("Ch35t3rit2110293");
            byte[] bytes = Encoding.ASCII.GetBytes("jimbisuper");
            byte[] iV = Encoding.ASCII.GetBytes("jimbisKripTograp");
            EncryptorWrapper wrapper = new EncryptorWrapper(EncryptorWrapper.EncryptionTypes.Rijndael, bytes, iV);
            return wrapper.Encrypt(value);
        }

        public string Decrypt(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("jimbisuper");
            byte[] iV = Encoding.ASCII.GetBytes("jimbisKripTograp");
            EncryptorWrapper wrapper = new EncryptorWrapper(EncryptorWrapper.EncryptionTypes.Rijndael, bytes, iV);
            return wrapper.Decrypt(value);
        }
    }

    public class EncryptorWrapper
    {
        private EncryptionTypes encTypeValue;
        private byte[] iVValue;
        private byte[] keyValue;
        private UTF8Encoding utf8 = new UTF8Encoding();

        public EncryptorWrapper(EncryptionTypes encType, byte[] key, byte[] iV)
        {
            this.encTypeValue = encType;
            this.keyValue = key;
            this.iVValue = iV;
        }

        public byte[] Decrypt(byte[] bytes)
        {
            ICryptoTransform transform;
            if (this.encTypeValue == EncryptionTypes.DES)
            {
                transform = new DESCryptoServiceProvider().CreateDecryptor(this.keyValue, this.iVValue);
            }
            else if (this.encTypeValue == EncryptionTypes.TripleDES)
            {
                transform = new TripleDESCryptoServiceProvider().CreateDecryptor(this.keyValue, this.iVValue);
            }
            else
            {
                transform = new RijndaelManaged().CreateDecryptor(this.keyValue, this.iVValue);
            }
            return this.Transform(bytes, transform);
        }

        public string Decrypt(string text)
        {
            ICryptoTransform transform;
            if (this.encTypeValue == EncryptionTypes.DES)
            {
                transform = new DESCryptoServiceProvider().CreateDecryptor(this.keyValue, this.iVValue);
            }
            else if (this.encTypeValue == EncryptionTypes.TripleDES)
            {
                transform = new TripleDESCryptoServiceProvider().CreateDecryptor(this.keyValue, this.iVValue);
            }
            else
            {
                transform = new RijndaelManaged().CreateDecryptor(this.keyValue, this.iVValue);
            }
            byte[] input = Convert.FromBase64String(text);
            byte[] bytes = this.Transform(input, transform);
            return this.utf8.GetString(bytes);
        }

        public byte[] Encrypt(byte[] bytes)
        {
            ICryptoTransform transform;
            if (this.encTypeValue == EncryptionTypes.DES)
            {
                transform = new DESCryptoServiceProvider().CreateEncryptor(this.keyValue, this.iVValue);
            }
            else if (this.encTypeValue == EncryptionTypes.TripleDES)
            {
                transform = new TripleDESCryptoServiceProvider().CreateEncryptor(this.keyValue, this.iVValue);
            }
            else
            {
                transform = new RijndaelManaged().CreateEncryptor(this.keyValue, this.iVValue);
            }
            return this.Transform(bytes, transform);
        }

        public string Encrypt(string text)
        {
            ICryptoTransform transform;
            if (this.encTypeValue == EncryptionTypes.DES)
            {
                transform = new DESCryptoServiceProvider().CreateEncryptor(this.keyValue, this.iVValue);
            }
            else if (this.encTypeValue == EncryptionTypes.TripleDES)
            {
                transform = new TripleDESCryptoServiceProvider().CreateEncryptor(this.keyValue, this.iVValue);
            }
            else
            {
                transform = new RijndaelManaged().CreateEncryptor(this.keyValue, this.iVValue);
            }
            byte[] bytes = this.utf8.GetBytes(text);
            return Convert.ToBase64String(this.Transform(bytes, transform));
        }

        private byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);
            stream2.Write(input, 0, input.Length);
            stream2.FlushFinalBlock();
            stream.Position = 0L;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            stream2.Close();
            return buffer;
        }

        public EncryptionTypes EncType
        {
            get
            {
                return this.encTypeValue;
            }
            set
            {
                this.encTypeValue = value;
            }
        }

        public byte[] IV
        {
            get
            {
                return this.iVValue;
            }
            set
            {
                this.iVValue = value;
            }
        }

        public byte[] Key
        {
            get
            {
                return this.keyValue;
            }
            set
            {
                this.keyValue = value;
            }
        }

        public enum EncryptionTypes
        {
            DES = 2,
            Rijndael = 4,
            TripleDES = 1
        }
    }

    public class ResultMessage
    {
        public HttpStatusCode status { get; set; }
        public string message { get; set; }
        public string Id { get; set; }
        public IQueryable data { get; set; }
        public string exData { get; set; }
    }

    public class MyConverter
    {
        private static int BELAS = 11;
        private static int JUTA = 15;
        private static int KOMA = 0x12;
        private static string[] Ltr = new string[] { 
            "nol", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sen", "belas", "puluh", "ratus", "ribu", "juta", 
            "milyar", "trilyun", "koma", "se"
         };
        private static int MILYAR = 0x10;
        private static int PULUH = 12;
        private static int RATUS = 13;
        private static int RIBU = 14;
        private static int SATU = 0x13;
        private static int SEN = 10;
        private static int TRILYUN = 0x11;

        private static string konversi(string u, string akhir, string koma, bool adakoma, string StringUtamaPLUS, string hasilnya)
        {
            char[] separator = new char[] { '.' };
            string[] strArray = new string[6];
            int index = 0;
            foreach (string str in u.Split(separator))
            {
                strArray[index] = str;
                index++;
            }
            string str2 = strArray[0];
            string str3 = "";
            for (int i = str2.Length - 1; i >= 0; i--)
            {
                str3 = str3 + str2.Substring(i, 1);
            }
            int length = str3.Length;
            string[] strArray2 = new string[length];
            char[] chArray2 = new char[length];
            string[] strArray3 = new string[length];
            for (int j = 0; j < length; j++)
            {
                chArray2[j] = str3[j];
            }
            for (int k = 0; k < length; k++)
            {
                switch (chArray2[k])
                {
                    case '1':
                        strArray3[k] = "satu";
                        break;

                    case '2':
                        strArray3[k] = "dua";
                        break;

                    case '3':
                        strArray3[k] = "tiga";
                        break;

                    case '4':
                        strArray3[k] = "empat";
                        break;

                    case '5':
                        strArray3[k] = "lima";
                        break;

                    case '6':
                        strArray3[k] = "enam";
                        break;

                    case '7':
                        strArray3[k] = "tujuh";
                        break;

                    case '8':
                        strArray3[k] = "delapan";
                        break;

                    case '9':
                        strArray3[k] = "sembilan";
                        break;

                    default:
                        strArray3[k] = "";
                        break;
                }
                switch (k)
                {
                    case 1:
                        if (!(strArray3[k] == "satu"))
                        {
                            break;
                        }
                        strArray2[k] = " sepuluh ";
                        goto Label_09FA;

                    case 2:
                        if (!(strArray3[k] == "satu"))
                        {
                            goto Label_0574;
                        }
                        strArray2[k] = " seratus ";
                        goto Label_09FA;

                    case 3:
                        strArray2[k] = strArray3[k].ToString() + " ribu ";
                        goto Label_09FA;

                    case 4:
                        if (!(strArray3[k] == "satu"))
                        {
                            goto Label_05EA;
                        }
                        strArray2[k] = " sepuluh ";
                        goto Label_09FA;

                    case 5:
                        if (!(strArray3[k] == "satu"))
                        {
                            goto Label_0642;
                        }
                        strArray2[k] = " seratus ";
                        goto Label_09FA;

                    case 6:
                        strArray2[k] = strArray3[k].ToString() + " juta ";
                        goto Label_09FA;

                    case 7:
                        if (!(strArray3[k] == ""))
                        {
                            goto Label_06E0;
                        }
                        strArray2[k] = "";
                        goto Label_09FA;

                    case 8:
                        if ((!(strArray3[k] == "") || !(strArray3[k - 1] == "")) || !(strArray3[k - 2] == ""))
                        {
                            goto Label_0760;
                        }
                        strArray2[k - 2] = "";
                        goto Label_09FA;

                    case 9:
                        if ((!(strArray3[k] == "") || !(strArray3[k + 1] == "")) || !(strArray3[k + 2] == ""))
                        {
                            goto Label_07DE;
                        }
                        strArray2[k] = "";
                        goto Label_09FA;

                    case 10:
                        if (!(strArray3[k] == "satu"))
                        {
                            goto Label_0819;
                        }
                        strArray2[k] = " sepuluh ";
                        goto Label_09FA;

                    case 11:
                        if (!(strArray3[k] == "satu"))
                        {
                            goto Label_0871;
                        }
                        strArray2[k] = " seratus ";
                        goto Label_09FA;

                    case 12:
                        if ((!(strArray3[k] == "") || !(strArray3[k + 1] == "")) || !(strArray3[k + 2] == ""))
                        {
                            goto Label_0917;
                        }
                        strArray2[k] = "";
                        goto Label_09FA;

                    case 13:
                        if (!(strArray3[k] == ""))
                        {
                            goto Label_0952;
                        }
                        strArray2[k] = "";
                        goto Label_09FA;

                    case 14:
                        if (!(strArray3[k] == ""))
                        {
                            goto Label_09A7;
                        }
                        strArray2[k] = "";
                        goto Label_09FA;

                    default:
                        strArray2[k] = strArray3[k].ToString() + " ";
                        goto Label_09FA;
                }
                if (strArray3[k] == "")
                {
                    strArray2[k] = "";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " puluh ";
                }
                goto Label_09FA;
            Label_0574:
                if (strArray3[k] == "")
                {
                    strArray2[k] = "";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " ratus ";
                }
                goto Label_09FA;
            Label_05EA:
                if (strArray3[k] == "")
                {
                    strArray2[k] = "";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " puluh ";
                }
                goto Label_09FA;
            Label_0642:
                if (((strArray3[k] == "") && (strArray3[k - 1] == "")) && (strArray3[k - 2] == ""))
                {
                    strArray2[k - 2] = "";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " ratus ";
                }
                goto Label_09FA;
            Label_06E0:
                if (strArray3[k] == "satu")
                {
                    strArray2[k] = " sepuluh ";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " puluh ";
                }
                goto Label_09FA;
            Label_0760:
                if (strArray3[k] == "satu")
                {
                    strArray2[k] = " seratus ";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " ratus ";
                }
                goto Label_09FA;
            Label_07DE:
                strArray2[k] = strArray3[k].ToString() + " milyar ";
                goto Label_09FA;
            Label_0819:
                if (strArray3[k] == "")
                {
                    strArray2[k] = "";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " puluh ";
                }
                goto Label_09FA;
            Label_0871:
                if (((strArray3[k] == "") && (strArray3[k - 1] == "")) && (strArray3[k - 2] == ""))
                {
                    strArray2[k - 2] = "";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " ratus ";
                }
                goto Label_09FA;
            Label_0917:
                strArray2[k] = strArray3[k].ToString() + " trilyun ";
                goto Label_09FA;
            Label_0952:
                if (strArray3[k] == "satu")
                {
                    strArray2[k] = " sepuluh ";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " puluh ";
                }
                goto Label_09FA;
            Label_09A7:
                if (strArray3[k] == "satu")
                {
                    strArray2[k] = " seratus ";
                }
                else
                {
                    strArray2[k] = strArray3[k].ToString() + " ratus ";
                }
            Label_09FA:
                if (((((k > 8) && (strArray3[0] == "")) && ((strArray3[1] == "") && (strArray3[2] == ""))) && (((strArray3[3] == "") && (strArray3[4] == "")) && ((strArray3[5] == "") && (strArray3[6] == "")))) && ((strArray3[7] == "") && (strArray3[8] == "")))
                {
                    for (int num6 = 0; num6 < 8; num6++)
                    {
                        strArray2[num6] = "";
                    }
                }
            }
            for (int m = 0; m < (length - 1); m++)
            {
                if (strArray2[m + 1] == " sepuluh ")
                {
                    switch (strArray2[m])
                    {
                        case "satu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sebelas ";
                            break;

                        case "dua ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " dua belas ";
                            break;

                        case "tiga ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tiga belas ";
                            break;

                        case "empat ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " empat belas ";
                            break;

                        case "lima ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " lima belas ";
                            break;

                        case "enam ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " enam belas ";
                            break;

                        case "tujuh ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tujuh belas ";
                            break;

                        case "delapan ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " delapan belas ";
                            break;

                        case "sembilan ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sembilan belas ";
                            break;

                        case "satu ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sebelas ribu";
                            break;

                        case "dua ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " dua belas ribu";
                            break;

                        case "tiga ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tiga belas ribu";
                            break;

                        case "empat ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " empat belas ribu";
                            break;

                        case "lima ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " lima belas ribu";
                            break;

                        case "enam ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " enam belas ribu";
                            break;

                        case "tujuh ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tujuh belas ribu";
                            break;

                        case "delapan ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " delapan belas ribu";
                            break;

                        case "sembilan ribu ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sembilan belas ribu";
                            break;

                        case "satu juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sebelas juta";
                            break;

                        case "dua juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " dua belas juta";
                            break;

                        case "tiga juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tiga belas juta";
                            break;

                        case "empat juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " empat belas juta";
                            break;

                        case "lima juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " lima belas juta";
                            break;

                        case "enam juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " enam belas juta";
                            break;

                        case "tujuh juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tujuh belas juta";
                            break;

                        case "delapan juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " delapan belas juta";
                            break;

                        case "sembilan juta ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sembilan belas juta";
                            break;

                        case "satu milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sebelas milyar";
                            break;

                        case "dua milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " dua belas milyar";
                            break;

                        case "tiga milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tiga belas milyar";
                            break;

                        case "empat milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " empat belas milyar";
                            break;

                        case "lima milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " lima belas milyar";
                            break;

                        case "enam milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " enam belas milyar";
                            break;

                        case "tujuh milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tujuh belas milyar";
                            break;

                        case "delapan milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " delapan belas milyar";
                            break;

                        case "sembilan milyar ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sembilan belas milyar";
                            break;

                        case "satu trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sebelas trilyun";
                            break;

                        case "dua trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " dua belas trilyun";
                            break;

                        case "tiga trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tiga belas trilyun";
                            break;

                        case "empat trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " empat belas trilyun";
                            break;

                        case "lima trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " lima belas trilyun";
                            break;

                        case "enam trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " enam belas trilyun";
                            break;

                        case "tujuh trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " tujuh belas trilyun";
                            break;

                        case "delapan trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " delapan belas trilyun";
                            break;

                        case "sembilan trilyun ":
                            strArray2[m] = "";
                            strArray2[m + 1] = " sembilan belas trilyun";
                            break;
                    }
                }
                else
                {
                    strArray2[m] = strArray2[m];
                    strArray2[m + 1] = strArray2[m + 1];
                }
            }
            bool flag = true;
            koma = "";
            try
            {
                string str4 = strArray[1];
                for (int num8 = str4.Length - 1; num8 >= 0; num8--)
                {
                    if (str4.Substring(num8, 1) == "0")
                    {
                        str4 = str4.Substring(0, num8);
                    }
                }
                int num9 = str4.Length;
                char[] chArray3 = new char[num9];
                string[] strArray4 = new string[num9];
                for (int num10 = 0; num10 < num9; num10++)
                {
                    chArray3[num10] = str4[num10];
                }
                for (int num11 = 0; num11 < num9; num11++)
                {
                    switch (chArray3[num11])
                    {
                        case '1':
                            strArray4[num11] = "satu";
                            break;

                        case '2':
                            strArray4[num11] = "dua";
                            break;

                        case '3':
                            strArray4[num11] = "tiga";
                            break;

                        case '4':
                            strArray4[num11] = "empat";
                            break;

                        case '5':
                            strArray4[num11] = "lima";
                            break;

                        case '6':
                            strArray4[num11] = "enam";
                            break;

                        case '7':
                            strArray4[num11] = "tujuh";
                            break;

                        case '8':
                            strArray4[num11] = "delapan";
                            break;

                        case '9':
                            strArray4[num11] = "sembilan";
                            break;

                        default:
                            strArray4[num11] = "nol";
                            break;
                    }
                    koma = koma + strArray4[num11] + " ";
                }
            }
            catch
            {
                flag = false;
            }
            if (koma == "")
            {
                flag = false;
            }
            for (int n = length - 1; n >= 0; n--)
            {
                akhir = akhir + strArray2[n] + " ";
            }
            if (flag)
            {
                if (akhir == "  ")
                {
                    hasilnya = "nol koma " + koma;
                    return hasilnya;
                }
                hasilnya = akhir + " koma " + koma;
                return hasilnya;
            }
            if (akhir == "  ")
            {
                hasilnya = "nol ";
                return hasilnya;
            }
            hasilnya = akhir;
            return hasilnya;
        }

        public static string MoneyFormat(double dbl)
        {
            return dbl.ToString("N");
        }

        public static string MoneyFormat(string str)
        {
            double dbl = 0.0;
            try
            {
                dbl = double.Parse(str);
            }
            catch
            {
            }
            return MoneyFormat(dbl);
        }

        public static string MoneyRoundUp(double currValue)
        {
            return MoneyRoundUp(currValue, 0x3e8);
        }

        public static string MoneyRoundUp(double currValue, int Index)
        {
            long num;
            if ((currValue % ((double)Index)) == 0.0)
            {
                num = (long)currValue;
            }
            else
            {
                long num2 = ((long)currValue) / ((long)Index);
                num = (num2 + 1L) * Index;
            }
            return num.ToString();
        }


        public static string Terbilang(string nominal_decimal)
        {
            string str = "";
            string[] strArray = new string[] { "", "" };
            if (!(nominal_decimal != ""))
            {
                return str;
            }
            if ((nominal_decimal.IndexOf(".") < 0) && (nominal_decimal.IndexOf(",") < 0))
            {
                if (nominal_decimal == "0")
                {
                    return "nol";
                }
                return Num2Let(nominal_decimal);
            }
            if (nominal_decimal.IndexOf(".") >= 0)
            {
                strArray = nominal_decimal.Split(".".ToCharArray());
            }
            else
            {
                strArray = nominal_decimal.Split(",".ToCharArray());
            }
            string str2 = strArray[1];
            for (int i = str2.Length - 1; i >= 0; i--)
            {
                if (!(str2.Substring(i, 1) == "0"))
                {
                    break;
                }
                str2 = str2.Substring(0, i);
            }
            int length = str2.Length;
            for (int j = 0; j < length; j++)
            {
                string n = str2.Substring(j, 1);
                if (n == "0")
                {
                    str = str + " nol";
                }
                else
                {
                    str = str + " " + Num2Let(n);
                }
            }
            if (length > 0)
            {
                if (strArray[0] == "0")
                {
                    return ("nol koma" + str);
                }
                return (Num2Let(strArray[0]) + " koma" + str);
            }
            if (strArray[0] == "0")
            {
                return "nol";
            }
            return Num2Let(strArray[0]);
        }

        public static string Terbilang2(string StringUtama)
        {
            string akhir = "";
            string koma = "";
            bool adakoma = false;
            string stringUtamaPLUS = "";
            string hasilnya = "";
            try
            {
                double num = double.Parse(StringUtama);
            }
            catch
            {
                return " <bukan angka!> ";
            }
            string str5 = "";
            if (StringUtama != "")
            {
                int length = StringUtama.Length;
                char[] chArray = new char[length];
                string[] strArray = new string[length];
                for (int i = 0; i < length; i++)
                {
                    chArray[i] = StringUtama[i];
                }
                for (int j = 0; j < length; j++)
                {
                    if (chArray[j] != '.')
                    {
                        adakoma = false;
                        stringUtamaPLUS = StringUtama + ".00";
                    }
                    else
                    {
                        adakoma = true;
                    }
                }
                if (adakoma)
                {
                    str5 = konversi(StringUtama, akhir, koma, adakoma, stringUtamaPLUS, hasilnya);
                }
                else
                {
                    str5 = konversi(stringUtamaPLUS, akhir, koma, adakoma, stringUtamaPLUS, hasilnya);
                }
            }
            return str5;
        }

        private static string Num2Let(string N)
        {
            int num2 = 1;
            string str = "";
            string s = "";
            string str2 = N;
            int length = str2.Length;
            while (length >= 1)
            {
                if (str2.Length >= 3)
                {
                    s = str2.Substring(str2.Length - 3, str2.Length - (str2.Length - 3)) + "" + s;
                    str2 = str2.Substring(0, str2.Length - 3);
                }
                else
                {
                    s = str2 + "" + s;
                }
                length -= s.Length;
                if (s.Trim() == "")
                {
                    s = "0";
                }
                int n = int.Parse(s);
                s = "";
                if ((n > 0) || (num2 == 1))
                {
                    switch (num2)
                    {
                        case 0:
                            str = u1000(n) + " " + str;
                            break;

                        case 1:
                            str = str + " " + u1000(n);
                            break;

                        case 2:
                            if (n != 1)
                            {
                                goto Label_012C;
                            }
                            str = "se" + Ltr[RIBU] + " " + str;
                            break;

                        case 3:
                            str = u1000(n) + " " + Ltr[JUTA] + " " + str;
                            break;

                        case 4:
                            str = u1000(n) + " " + Ltr[MILYAR] + " " + str;
                            break;

                        case 5:
                            str = u1000(n) + " " + Ltr[TRILYUN] + " " + str;
                            break;
                    }
                }
                goto Label_023E;
            Label_012C: ;
                str = u1000(n) + " " + Ltr[RIBU] + " " + str;
            Label_023E:
                num2++;
            }
            return str;
        }

        private static string u1000(int n)
        {
            int num = 0;
            string str = "";
            if (n > 0)
            {
                num = n / 100;
            }
            if (num > 0)
            {
                if (num == 1)
                {
                    str = SE(num) + "" + Ltr[RATUS];
                }
                else
                {
                    str = SE(num) + " " + Ltr[RATUS];
                }
            }
            num = n % 100;
            switch (num)
            {
                case 0:
                    return str;

                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return (str + " " + Ltr[num]);

                case 10:
                    return (str + " " + Ltr[SATU] + "" + Ltr[PULUH]);

                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 0x10:
                case 0x11:
                case 0x12:
                case 0x13:
                    {
                        int num2 = num % 10;
                        if (num2 == 1)
                        {
                            return (str + " " + SE(num2) + "" + Ltr[BELAS]);
                        }
                        return (str + " " + SE(num2) + " " + Ltr[BELAS]);
                    }
                case 20:
                case 0x15:
                case 0x16:
                case 0x17:
                case 0x18:
                case 0x19:
                case 0x1a:
                case 0x1b:
                case 0x1c:
                case 0x1d:
                case 30:
                case 0x1f:
                case 0x20:
                case 0x21:
                case 0x22:
                case 0x23:
                case 0x24:
                case 0x25:
                case 0x26:
                case 0x27:
                case 40:
                case 0x29:
                case 0x2a:
                case 0x2b:
                case 0x2c:
                case 0x2d:
                case 0x2e:
                case 0x2f:
                case 0x30:
                case 0x31:
                case 50:
                case 0x33:
                case 0x34:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38:
                case 0x39:
                case 0x3a:
                case 0x3b:
                case 60:
                case 0x3d:
                case 0x3e:
                case 0x3f:
                case 0x40:
                case 0x41:
                case 0x42:
                case 0x43:
                case 0x44:
                case 0x45:
                case 70:
                case 0x47:
                case 0x48:
                case 0x49:
                case 0x4a:
                case 0x4b:
                case 0x4c:
                case 0x4d:
                case 0x4e:
                case 0x4f:
                case 80:
                case 0x51:
                case 0x52:
                case 0x53:
                case 0x54:
                case 0x55:
                case 0x56:
                case 0x57:
                case 0x58:
                case 0x59:
                case 90:
                case 0x5b:
                case 0x5c:
                case 0x5d:
                case 0x5e:
                case 0x5f:
                case 0x60:
                case 0x61:
                case 0x62:
                case 0x63:
                    {
                        int index = num / 10;
                        int num4 = num % 10;
                        str = str + " " + Ltr[index] + " " + Ltr[PULUH];
                        if (num4 > 0)
                        {
                            str = str + " " + Ltr[num4];
                        }
                        return str;
                    }
            }
            return str;
        }

        private static string SE(int n)
        {
            if (n == 1)
            {
                return Ltr[SATU];
            }
            return Ltr[n];
        }

        public static CultureInfo formatCurrencyIndo()
        {
            var info = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            info.NumberFormat.CurrencyDecimalDigits = 0;
            info.NumberFormat.CurrencySymbol = "Rp ";
            info.NumberFormat.CurrencyGroupSeparator = ".";
            info.NumberFormat.CurrencyDecimalSeparator = ",";
            return info;
        }

        public static CultureInfo formatCurrencyIndoTanpaSymbol()
        {
            var info = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            info.NumberFormat.CurrencyDecimalDigits = 0;
            info.NumberFormat.CurrencySymbol = "";
            info.NumberFormat.CurrencyGroupSeparator = ".";
            info.NumberFormat.CurrencyDecimalSeparator = ",";
            return info;
        }
        
    }
}
