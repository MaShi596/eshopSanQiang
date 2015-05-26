using System;
using System.Text.RegularExpressions;
namespace Hidistro.Core.Urls
{
	public class ApplicationKeyMapping
	{
		private string _locationName = null;
		private Regex regex = null;
		public string LocationName
		{
			get
			{
				return this._locationName;
			}
		}
		public ApplicationKeyMapping(string locationName, string pattern)
		{
			this._locationName = locationName;
			this.regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		public bool IsMatch(string string_0)
		{
			return this.regex.IsMatch(string_0);
		}
	}
}
