using System;
using System.IO;
using System.Web;
using System.Web.Caching;
namespace Hidistro.Core
{
	public static class LicenseHelper
	{
		private const string PublicKeyCache = "FileCache-Publickey";
		public static string GetPublicKey()
		{
			string text = HiCache.Get("FileCache-Publickey") as string;
			if (string.IsNullOrEmpty(text))
			{
				HttpContext current = HttpContext.Current;
				string text2;
				if (current != null)
				{
					text2 = current.Request.MapPath("~/config/publickey.xml");
				}
				else
				{
					text2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config\\publickey.xml");
				}
				text = File.ReadAllText(text2);
				HiCache.Max("FileCache-Publickey", text, new CacheDependency(text2));
			}
			return text;
		}
	}
}
