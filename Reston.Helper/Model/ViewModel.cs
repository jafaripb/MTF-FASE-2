using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Helper.Model
{
    public class CaptchaViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime ExpiredDate { get; set; }
        public System.Drawing.Bitmap Image { get; set; }
    }

}
