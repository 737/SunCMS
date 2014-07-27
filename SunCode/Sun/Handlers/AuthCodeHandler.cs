using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Sun
{
    public class AuthCodeHandler : IHttpHandler, IRequiresSessionState
    {
        private HelperNameValueCollection _config;
        private static string[] _fonts = new string[] { "Helvetica", "Geneva", "sans-serif", "Verdana", "Times New Roman", "Courier New" };
        private const string _validateCodeString = "a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z|0|1|2|3|4|5|6|7|8|9";
        private static string _validateCodeSessionName = "____SunCMS_ValidateCodeSession";
        private string _validateCode;

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            this.SetHeader();
            this.PaintValidateCode();
        }

        /// <summary>
        /// //设置输出 头部信息
        /// </summary>
        private void SetHeader()
        {
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.AppendHeader("Pragma", "No-Cache");
            HttpContext.Current.Response.ContentType = "image/jpeg";
        }

        /// <summary>
        /// //绘制 验证码
        /// </summary>
        private void PaintValidateCode()
        {
            int _width = this.GetValidateCodeWidth();
            int _height = this.GetValidateCodeHeight();
            Bitmap img = new Bitmap(_width, _height);
            Graphics g = Graphics.FromImage(img);
            try
            {
                g.Clear(this.ValidateCodeBgColor);
                this.PaintLine(g, _width, _height);
                this.PaintCode(g);
                this._validateCode = null;  //必须设置null,否则无法得到新的验证码
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Jpeg);
                    Util.Context.CurrentHttpContext.Response.BinaryWrite(stream.ToArray());
                }
            }
            catch
            {
            }
            finally
            {
                img.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// //画无规律线
        /// </summary>
        private void PaintLine(Graphics g, int width, int height)
        {
            Random _random = new Random();
            for (int i = 0; i < 10; i++)
            {
                Point p1 = new Point(_random.Next(0, width), _random.Next(0, height));
                Point p2 = new Point(p1.X + _random.Next(-10, 10), p1.Y + _random.Next(-10, 10));
                using (Pen _pen = new Pen(Color.FromArgb(_random.Next(0, 0xff), Color.FromArgb(_random.Next(0, 0xff), Color.FromArgb(_random.Next(0, 0xff))))))
                {
                    g.DrawLine(_pen, p1, p2);
                }
            }
        }

        /// <summary>
        /// //画 验证码
        /// </summary>
        private void PaintCode(Graphics g)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            string _code = this.GetValidateCode();
            using (Brush brush = new SolidBrush(this.ValidateCodeFontColor))
            {
                Rectangle rect = new Rectangle(0, 0, this.GetValidateCodeWidth(), this.GetValidateCodeHeight());
                g.DrawString(_code, this.GetValidateCodeFont(), brush, rect, format);
            }
            format.Dispose();
        }

        /// <summary>
        /// //输入的验证输入是否正确
        /// </summary>        
        public static bool IsMatch(string input)
        {
            if (input == null)
            {
                return false;
            }
            if (!ConfigSun.GetSection("authcode").GetKeyBoolValue("IgnoreCase"))
            {
                return (input == Util.Context.CurrentHttpContext.Session[_validateCodeSessionName].ToString());
            }
            return (input.ToLower() == Util.Context.CurrentHttpContext.Session[_validateCodeSessionName].ToString().ToLower());
        }

        /// <summary>
        /// //返回 验证码字符窜   (更改或者生成验证码)
        /// </summary>
        private string GetValidateCode()
        {
            if (this._validateCode == null)
            {
                Random _random = new Random();
                int _codeLength = this.ValidateCodeArray.Length;
                int limitCodeLength = this.ValidateCodeMaxLength;
                string _code = "";
                for (int i = 0; i < limitCodeLength; i++)
                {
                    _code += this.ValidateCodeArray[_random.Next(_codeLength)];
                }
                if (Util.Context.CurrentHttpContext.Session[_validateCodeSessionName] == null)
                {
                    Util.Context.CurrentHttpContext.Session.Add(_validateCodeSessionName, _code);
                }
                else
                {
                    Util.Context.CurrentHttpContext.Session[_validateCodeSessionName] = _code;
                }
                this._validateCode = _code;
            }
            return this._validateCode;
        }

        /// <summary>
        /// //返回 验证码 宽度
        /// </summary>
        private int GetValidateCodeWidth()
        {
            return (int)(this.ValidateCodeMaxLength * this.ValidateCodeFontSize * this.ValidateCodeWidthModulus);
        }

        /// <summary>
        /// //返回 验证码 高度
        /// </summary>        
        private int GetValidateCodeHeight()
        {
            return (this.ValidateCodeFontSize * 2);
        }

        /// <summary>
        /// //返回 验证码 字体样式
        /// </summary>
        private Font GetValidateCodeFont()
        {
            Random _random = new Random();
            FontStyle _style = FontStyle.Regular;
            switch (_random.Next(0, 4))
            {
                case 0:
                    _style = FontStyle.Bold;
                    break;
                case 1:
                    _style = FontStyle.Italic;
                    break;
                case 2:
                    _style = FontStyle.Strikeout;
                    break;
                case 3:
                    _style = (FontStyle.Italic | FontStyle.Bold);
                    break;
            }
            Font _font = new Font(_fonts[_random.Next(_fonts.Length)], (float)this.ValidateCodeFontSize, _style, GraphicsUnit.Pixel);
            return _font;
        }

        /// <summary>
        /// //返回 分割后 验证码字符窜
        /// </summary>
        private string[] ValidateCodeArray
        {
            get
            {
                return this.ValidateCodeString.Split(new char[] { '|' });
            }
        }

        /// <summary>
        /// //返回 验证码字符窜
        /// </summary>
        private string ValidateCodeString
        {
            get
            {
                string _str = this.ValidateCodeInfoFromConfig.GetKeyValue("code");
                if (!string.IsNullOrEmpty(_str))
                {
                    return _str;
                }
                return _validateCodeString;
            }
        }

        /// <summary>
        /// //返回 验证码 字符的长度
        /// </summary>
        private byte ValidateCodeMaxLength
        {
            get
            {
                int _length = Util.Parse.ToInt(this.ValidateCodeInfoFromConfig.GetKeyValue("maxLength"), 4);
                _length = Util.Context.GetIntFromQueryString("maxLength", _length);
                if (_length != -1)
                {
                    return (byte)_length;
                }
                return 4;
            }
        }

        /// <summary>
        /// //返回 验证码 字体大小
        /// </summary>
        private byte ValidateCodeFontSize
        {
            get
            {
                int _size = Util.Parse.ToInt(this.ValidateCodeInfoFromConfig.GetKeyValue("fontSize"), 12);
                _size = Util.Context.GetIntFromQueryString("fontSize", _size);
                return (byte)_size;
            }
        }

        /// <summary>
        /// //返回 验证码 宽度系统
        /// </summary>
        private float ValidateCodeWidthModulus
        {
            get
            {
                return 1f;
            }
        }

        /// <summary>
        /// //返回 验证码 背景色
        /// </summary>
        private Color ValidateCodeBgColor
        {
            get
            {
                string _color = Util.Context.QueryString["backGroundColor"];
                if (string.IsNullOrEmpty(_color))
                {
                    _color = this.ValidateCodeInfoFromConfig.GetKeyValue("backGroundColor");
                }
                if (string.IsNullOrEmpty(_color))
                {
                    return Color.FromArgb(0xff, 0xff, 0xcc);
                }
                return ColorTranslator.FromHtml(_color);
            }
        }

        /// <summary>
        /// //返回 验证码 字体颜色
        /// </summary>
        private Color ValidateCodeFontColor
        {
            get
            {
                string _color = Util.Context.QueryString["fontColor"];
                if (string.IsNullOrEmpty(_color))
                {
                    _color = this.ValidateCodeInfoFromConfig.GetKeyValue("fontColor");
                }
                if (string.IsNullOrEmpty(_color))
                {
                    return Color.Black;
                }
                return ColorTranslator.FromHtml(_color);
            }
        }

        /// <summary>
        /// //返回sun.config文件中定义的 字符窜
        /// </summary>
        private HelperNameValueCollection ValidateCodeInfoFromConfig
        {
            get
            {
                if (this._config == null)
                {
                    this._config = ConfigSun.GetSection("authcode");
                }
                return this._config;
            }
        }
    }
}
