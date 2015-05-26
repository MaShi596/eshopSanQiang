using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AliPayNotify
	{
		private string _partner = "";
		private string _key = "";
		private string _input_charset = "";
		private string _sign_type = "";
		private string _veryfy_url = "";
		private System.Collections.Generic.Dictionary<string, string> sPara = new System.Collections.Generic.Dictionary<string, string>();
		private string Https_veryfy_url = "https://www.alipay.com/cooperate/gateway.do?service=notify_verify&";
		private string preSignStr = "";
		private string mysign = "";
		private string responseTxt = "";
		public string Mysign
		{
			get
			{
				return this.mysign;
			}
		}
		public string ResponseTxt
		{
			get
			{
				return this.responseTxt;
			}
		}
		public string PreSignStr
		{
			get
			{
				return this.preSignStr;
			}
		}
		public AliPayNotify(System.Collections.Generic.SortedDictionary<string, string> inputPara, string notify_id, string partner, string string_0)
		{
			this._partner = partner;
			this._key = string_0;
			this._input_charset = "utf-8";
			this._sign_type = "MD5";
			this._veryfy_url = string.Concat(new string[]
			{
				this.Https_veryfy_url,
				"partner=",
				this._partner,
				"&notify_id=",
				notify_id
			});
			this.sPara = OpenIdFunction.FilterPara(inputPara);
			this.preSignStr = OpenIdFunction.CreateLinkString(this.sPara);
			this.mysign = OpenIdFunction.BuildMysign(this.sPara, this._key, this._sign_type, this._input_charset);
			this.responseTxt = this.Get_Http(this._veryfy_url, 120000);
		}
		private string Get_Http(string strUrl, int timeout)
		{
			string result;
			try
			{
				System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
				httpWebRequest.Timeout = timeout;
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
				System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
				System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.Default);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				while (-1 != streamReader.Peek())
				{
					stringBuilder.Append(streamReader.ReadLine());
				}
				result = stringBuilder.ToString();
			}
			catch (System.Exception ex)
			{
				result = "错误：" + ex.Message;
			}
			return result;
		}
	}
}
