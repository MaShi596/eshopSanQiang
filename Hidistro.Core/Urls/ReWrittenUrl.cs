using System;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Hidistro.Core.Urls
{
	public class ReWrittenUrl
	{
		private string _name;
		private string _path;
		private string _pattern;
		private Regex _regex = null;
		public string Pattern
		{
			get
			{
				return this._pattern;
			}
		}
		public string Name
		{
			get
			{
				return this._name;
			}
		}
		public string Path
		{
			get
			{
				return this._path;
			}
		}
		public ReWrittenUrl(string name, string pattern, string path)
		{
			this._name = name;
			this._path = path;
			this._pattern = pattern;
			this._regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		public bool IsMatch(string string_0)
		{
			return this._regex.IsMatch(string_0);
		}
		public virtual string Convert(string string_0, string string_1)
		{
			if (string_1 != null && string_1.StartsWith("?"))
			{
				string_1 = string_1.Replace("?", "&");
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[]
			{
				this._regex.Replace(string_0, this._path),
				string_1
			});
		}
	}
}
