using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using PanGu;
using Sun.Zone;

namespace Sun.API.Util
{
    public class ApiPanGu
    {
        //得到 字符窜中的TAGs
        public static List<DictionaryEntry> getTTags(string text, int count)
        {
            Segment segment = new Segment();
            ICollection<WordInfo> words = segment.DoSegment(text);

            Hashtable hash = new Hashtable();
            byte[] wordBytes;
            int i = 0;

            count = count == -1 ? text.Length : count;

            foreach (var item in words)
            {
                wordBytes = Encoding.Default.GetBytes(item.Word);

                if (i > count - 1)
                {
                    break;
                }
                if (wordBytes.Length < 4)
                {
                    continue;
                }
                if (hash.Contains(item.Word))
                {
                    var _rank = (int)hash[item.Word];
                    _rank = _rank + 1;
                    hash.Remove(item.Word);
                    hash.Add(item.Word, _rank);
                }
                else
                {
                    hash.Add(item.Word, 1);
                }
                i++;
            }

            DictionaryEntry[] wordNumDE = new DictionaryEntry[hash.Count];
            hash.CopyTo(wordNumDE, 0);
            //排序
            Array.Sort(wordNumDE, new myArrange());

            List<DictionaryEntry> listWord = new List<DictionaryEntry>(wordNumDE) { };

            return listWord;
        }

        public static string getStrTags(string text, int count)
        {
            List<DictionaryEntry> t_tags = getTTags(text, count);
            int i = 0;
            string result = "";

            foreach (var item in t_tags)
            {
                if (i == 0)
                {
                    result = item.Key.ToString();
                }
                else
                {
                    result = result + "," + item.Key.ToString();
                }
                i++;
            }

            return result;
        }

        [Action(Verb = "GET")]
        public static string tags()
        {
            var _text = Sun.HelperContext.GetQueryString("text");
            var _count = Sun.Toolkit.Parse.ToInt(Sun.HelperContext.GetQueryString("count"));

            List<DictionaryEntry> t_tags = getTTags(_text, _count);
            int i = 0;
            string result = "";
            List<string> resultList = new List<string>() { };

            foreach (var item in t_tags)
            {
                if (i == 0)
                {
                    result = item.Key.ToString();
                }
                else
                {
                    result = result + "," + item.Key.ToString();
                }
                resultList.Add(item.Key.ToString());
                i++;
            }

            return Sun.Toolkit.JSON.GetPackJSON(true, null, new { str = result, arr = resultList });
        }
    }

    //实现了IComparer接口的排序类。
    class myArrange : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            DictionaryEntry dx = (DictionaryEntry)x;
            DictionaryEntry dy = (DictionaryEntry)y;
            return (int)dy.Value - (int)dx.Value;
        }
    }
}
