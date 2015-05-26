using System;
using System.Globalization;
using System.Web;
using System.Xml;
namespace Hishop.Plugins
{
	public abstract class PaymentRequest : ConfigablePlugin, IPlugin
	{
		private const string FormFormat = "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>";
		private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";
		public abstract bool IsMedTrade
		{
			get;
		}
		public static PaymentRequest CreateInstance(string name, string configXml, string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[]
			{
				orderId,
				amount,
				subject,
				body,
				buyerEmail,
				date,
				showUrl,
				returnUrl,
				notifyUrl,
				attach
			};
			Type plugin = PaymentPlugins.Instance().GetPlugin("PaymentRequest", name);
			if (plugin == null)
			{
				return null;
			}
			PaymentRequest paymentRequest = Activator.CreateInstance(plugin, args) as PaymentRequest;
			if (paymentRequest != null && !string.IsNullOrEmpty(configXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(configXml);
				paymentRequest.InitConfig(xmlDocument.FirstChild);
			}
			return paymentRequest;
		}
		public static PaymentRequest CreateInstance(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type plugin = PaymentPlugins.Instance().GetPlugin("PaymentRequest", name);
			if (plugin == null)
			{
				return null;
			}
			return Activator.CreateInstance(plugin) as PaymentRequest;
		}
		protected virtual void RedirectToGateway(string string_0)
		{
			HttpContext.Current.Response.Redirect(string_0, true);
		}
		protected virtual string CreateField(string name, string strValue)
		{
			return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", new object[]
			{
				name,
				strValue
			});
		}
		protected virtual string CreateForm(string content, string action)
		{
			content += "<input type=\"submit\" value=\"在线支付\" style=\"display:none;\">";
			return string.Format(CultureInfo.InvariantCulture, "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>", new object[]
			{
				action,
				content
			});
		}
		protected virtual void SubmitPaymentForm(string formContent)
		{
			string s = formContent + "<script>document.forms['payform'].submit();</script>";
			HttpContext.Current.Response.Write(s);
			HttpContext.Current.Response.End();
		}
		public abstract void SendRequest();
		public abstract void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType);
	}
}
