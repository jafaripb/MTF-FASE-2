using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Reston.Helper.Repository;
using Reston.Helper.Model;


namespace Reston.Helper
{
    public class CaptchaHelper
    {

        private Random random = new Random();
        private IHelperRepo _repository;

        public CaptchaHelper()
        {
            _repository = new HelperRepo(new Reston.Helper.HelperContext());
        }

        public CaptchaHelper(HelperRepo repository)
        {
            _repository = repository;
        }

        public Captcha GetCaptcha() {
            Captcha cr = new Captcha();
            cr.Id = Guid.NewGuid();
            cr.Text = GetCaptchaString(5);
            cr.ExpiredDate = DateTime.Now.AddMinutes(10);
            _repository.AddCaptchaRegistration(cr); //remove expired included.
            return cr;
        }

        public Bitmap GenerateImage(Guid id) {
            Captcha cr = _repository.GetCaptcha(id);
            if (cr != null) {
                return GenerateImage(cr.Text, 300, 75);
            }
            return null;
        }

        public CaptchaViewModel Generate()
        {
            Captcha cr = new Captcha();
            cr.Text = GetCaptchaString(5);
            cr.ExpiredDate = DateTime.Now.AddMinutes(10);
            _repository.AddCaptchaRegistration(cr);
            CaptchaViewModel v = new CaptchaViewModel()
            { 
                Id = cr.Id, Text = cr.Text, ExpiredDate = cr.ExpiredDate, Image = GenerateImage(cr.Text,250,75)
            };
            return v;
        }

        public bool Verify(Guid id, string answer) {
            Captcha c = _repository.GetCaptcha(id);
            if (c != null) {
                if (DateTime.Now <= c.ExpiredDate) {
                    if (c.Text == answer)
                        return true;
                }
            }
            return false;
        }

        private string GetCaptchaString(int length)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'A';
            int intZ = 'Z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strCaptchaString = "";

            Random random = new Random(System.DateTime.Now.Millisecond);

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }
            return strCaptchaString;
        }

        private Bitmap GenerateImage(string text, int width, int height)
        {
            //taken from http://www.codeproject.com/Articles/169371/Captcha-Image-using-C-in-ASP-NET//
            Bitmap bitmap = new Bitmap
              (width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti,
                Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);
            SizeF size;
            float fontSize = rect.Height - 1;
            Font font;

            do
            {
                fontSize -=2;
                font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Italic);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            GraphicsPath path = new GraphicsPath();
            //path.AddString(this.text, font.FontFamily, (int) font.Style, 
            //    font.Size, rect, format);
            path.AddString(text, font.FontFamily, (int)font.Style, 75, rect, format);
            float v = 4F;
            PointF[] points =
          {
                new PointF(this.random.Next(rect.Width) / v, this.random.Next(
                   rect.Height) / v),
                new PointF(rect.Width - this.random.Next(rect.Width) / v, 
                    this.random.Next(rect.Height) / v),
                new PointF(this.random.Next(rect.Width) / v, 
                    rect.Height - this.random.Next(rect.Height) / v),
                new PointF(rect.Width - this.random.Next(rect.Width) / v,
                    rect.Height - this.random.Next(rect.Height) / v)
          };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            hatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.SkyBlue);
            g.FillPath(hatchBrush, path);
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = this.random.Next(rect.Width);
                int y = this.random.Next(rect.Height);
                int w = this.random.Next(m / 50);
                int h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            return bitmap;
        }

    }

        
}
