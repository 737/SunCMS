using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Caching;

namespace Sun
{
    /// <summary>
    /// // SunCMS 应用程序“缓存中心”
    /// </summary>
    public class SunCache
    {
        private static readonly Cache _cache;
        public static readonly int Day = 0x4380;
        private static int Factor = 5;
        public static readonly int Hour = 720;
        public static readonly int Minute = 12;

        static SunCache()
        {
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                _cache = current.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        public static void Clear()
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            ArrayList list = new ArrayList();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Key);
            }
            foreach (string str in list)
            {
                _cache.Remove(str);
            }
        }

        /// <summary>
        /// //返回 key ---> value
        /// </summary>
        public static object GetValue(string key)
        {
            return _cache[key];
        }

        public static void InsertCache(string key, object obj)
        {
            InsertCache(key, obj, "", 1);
        }

        public static void InsertCache(string key, object obj, int seconds)
        {
            InsertCache(key, obj, "", seconds);
        }

        public static void InsertCache(string key, object obj, string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                InsertCache(key, obj, "", Hour * 12);
            }
            else
            {
                InsertCache(key, obj, new CacheDependency(filename), Hour * 12);
            }
        }

        public static void InsertCache(string key, object obj, CacheDependency dep)
        {
            InsertCache(key, obj, dep, Hour * 12);
        }

        public static void InsertCache(string key, object obj, int seconds, CacheItemPriority priority)
        {
            InsertCache(key, obj, null, seconds, priority);
        }

        public static void InsertCache(string key, object obj, string filename, int seconds)
        {
            if (string.IsNullOrEmpty(filename))
            {
                InsertCache(key, obj, null, seconds, CacheItemPriority.Normal);
            }
            else
            {
                InsertCache(key, obj, new CacheDependency(filename), seconds, CacheItemPriority.Normal);
            }
        }

        /// <summary>
        /// //指定时间插入缓存
        /// </summary>
        public static void InsertCache(string key, object obj, CacheDependency dep, int seconds)
        {
            InsertCache(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        public static void InsertCache(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds((double)(Factor * seconds)), TimeSpan.Zero, priority, null);
            }
        }

        public static void InsertMaxCache(string key, object obj)
        {
            InsertMaxCache(key, obj, "");
        }

        public static void InsertMaxCache(string key, object obj, string filename)
        {
            if (obj != null)
            {
                if (string.IsNullOrEmpty(filename))
                {
                    _cache.Insert(key, obj, null, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
                }
                else
                {
                    _cache.Insert(key, obj, new CacheDependency(filename), DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
                }
            }
        }

        /// <summary>
        /// //在缓存中保存最长时间
        /// </summary>
        public static void InsertMaxCache(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            }
        }

        public static void InsertMaxCache(string key, object obj, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, null, DateTime.MaxValue, TimeSpan.Zero, priority, null);
            }
        }

        public static void MicroInsert(string key, object obj, int secondFactor)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, null, DateTime.Now.AddSeconds((double)(Factor * secondFactor)), TimeSpan.Zero);
            }
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                {
                    _cache.Remove(enumerator.Key.ToString());
                }
            }
        }

        public static void ResetFactor(int cacheFactor)
        {
            Factor = cacheFactor;
        }

    }
}
