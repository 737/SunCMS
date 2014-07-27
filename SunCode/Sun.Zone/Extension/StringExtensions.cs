using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Sun.Zone
{
	/// <summary>
	/// 用于UI输出方面的常用字符串扩展
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// 将字符串转换为 HTML 编码的字符串。
		/// </summary>
		/// <param name="str">要编码的字符串。</param>
		/// <returns>一个已编码的字符串。</returns>
		public static string HtmlEncode(this string str)
		{
			if( string.IsNullOrEmpty(str) )
				return string.Empty;

			return HttpUtility.HtmlEncode(str);
		}

		/// <summary>
		/// 将字符串最小限度地转换为 HTML 编码的字符串。
		/// </summary>
		/// <param name="str">要编码的字符串。</param>
		/// <returns>一个已编码的字符串。</returns>
		public static string HtmlAttributeEncode(this string str)
		{
			if( string.IsNullOrEmpty(str) )
				return string.Empty;

			return HttpUtility.HtmlAttributeEncode(str);
		}

		/// <summary>
		/// 判断二个字符串是否相等，忽略大小写的比较方式。
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool IsSame(this string a, string b)
		{
			return string.Compare(a, b, StringComparison.OrdinalIgnoreCase) == 0;
		}


		/// <summary>
		/// 等效于 string.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)
		/// 且为每个拆分后的结果又做了Trim()操作。
		/// </summary>
		/// <param name="value">要拆分的字符串</param>
		/// <param name="separator">分隔符</param>
		/// <returns></returns>
		public static string[] SplitTrim(this string str, params char[] separator)
		{
			if( string.IsNullOrEmpty(str) )
				return null;
			else
				return (from s in str.Split(',')
						let u = s.Trim()
						where u.Length > 0
						select u).ToArray();
		}


		internal static readonly char[] CommaSeparatorArray = new char[] { ',' };


		/// <summary>
		/// 将对象执行JSON序列化
		/// </summary>
		/// <param name="obj">要序列化的对象</param>
		/// <returns>JSON序列化的结果</returns>
		internal static string ToJson(this object obj)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			return jss.Serialize(obj);
		}


		/// <summary>
		/// 从JSON字符串中反序列化对象
		/// </summary>
		/// <typeparam name="T">反序列化的结果类型</typeparam>
		/// <param name="json">JSON字符串</param>
		/// <returns>反序列化的结果</returns>
		internal static T DeserializeFromJson<T>(this string json)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			return jss.Deserialize<T>(json);
		}

		/// <summary>
		/// 将对象执行XML序列化
		/// </summary>
		/// <param name="obj">要序列化的对象</param>
		/// <returns>XML序列化的结果</returns>
		internal static string ToXml(this object obj)
		{
			return XmlHelper.XmlSerialize(obj, Encoding.UTF8);
		}


		/// <summary>
		/// 从XML字符串中反序列化对象
		/// </summary>
		/// <typeparam name="T">反序列化的结果类型</typeparam>
		/// <param name="json">XML字符串</param>
		/// <returns>反序列化的结果</returns>
		internal static T DeserializeFromXml<T>(this string xml)
		{
			return XmlHelper.XmlDeserialize<T>(xml, Encoding.UTF8);
		}
	}


	
}
