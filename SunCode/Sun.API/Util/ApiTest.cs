using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Zone;
using Sun.Hubble;
using Sun.Toolkit;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;

namespace Sun.API.Util
{
    public class ApiTest
    {

        [Action(Verb = "GET")]
        public string sun()
        {
            var txt = Sun.HelperContext.GetQueryString("txt");


            return getCount();

            //return Sun.Util.String.ClearSpace(txt);
        }


        public static string getCount()
        {
            string sql = "SELECT     COUNT(groupID) AS archivecount  FROM         sun_archive  WHERE     (groupID = 12)";

            var cc = DataHelper.GetExecuteScalar(sql);
            var count = 0;

            count = Toolkit.Parse.ToInt(cc);

            return Toolkit.JSON.GetJSON(cc);
        }

        [Action(Verb = "GET")]
        public string demo()
        {
            //Sun.diagnostics.log.recordError("wo aaaaa");

            return "asdfasdf";
        }

        /// <summary>
        /// //正则表达式匹配测试
        /// 参数为 ptn   exp  group
        /// </summary>
        [Action(Verb = "GET")]
        public string zz(string regtxt, string txt)
        {

            //string pattern = @"\[(?<value>sun:[A-Za-z_]+:[A-Za-z_]*?)\]";
            string pattern = @"\{(?<value>[一-龥A-Za-z_]+?[^;]*?)\}";

            txt = "adsf{id}asdf";
            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var flag = reg.IsMatch(txt);

            return flag.ToString();


        }

        [Action(Verb = "GET")]
        public string systemConfig()
        {
            return Sun.Toolkit.JSON.GetJSON(Sun.configSystem.getConfig());
        }
    }
}
