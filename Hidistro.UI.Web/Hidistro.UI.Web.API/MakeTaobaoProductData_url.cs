using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.HOP;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.API
{
	public class MakeTaobaoProductData_url : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Literal litmsg;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
			string s = base.Request.Form["productid"];
			string stuffStatus = base.Request.Form["stuffstatus"];
			string proTitle = base.Request.Form["title"];
			long num = long.Parse(base.Request.Form["num"]);
			string locationCity = base.Request.Form["locationcity"];
			string locationState = base.Request.Form["LocationState"];
			string value = base.Request.Form["Cid"];
			string value2 = base.Request.Form["hasinvoice"];
			string value3 = base.Request.Form["HasWarranty"];
			string propertyAlias = base.Request.Form["props"];
			string text = base.Request.Form["inputpids"];
			string text2 = base.Request.Form["inputstr"];
			string text3 = base.Request.Form["skuproperties"];
			string text4 = base.Request.Form["skuquantities"];
			string text5 = base.Request.Form["skuprices"];
			string text6 = base.Request.Form["skuouterids"];
			string text7 = base.Request.Form["freightpayer"];
			string text8 = base.Request.Form["FoodAttributes"];
			if (text7 == "buyer")
			{
				string s2 = base.Request.Form["postfee"];
				string s3 = base.Request.Form["expressfee"];
				string s4 = base.Request.Form["emsfee"];
				taobaoProductInfo.PostFee = decimal.Parse(s2);
				taobaoProductInfo.EMSFee = decimal.Parse(s4);
				taobaoProductInfo.ExpressFee = decimal.Parse(s3);
			}
			if (!string.IsNullOrEmpty(text8))
			{
				taobaoProductInfo.FoodAttributes = System.Web.HttpUtility.UrlDecode(text8);
			}
			taobaoProductInfo.ProductId = int.Parse(s);
			taobaoProductInfo.StuffStatus = stuffStatus;
			taobaoProductInfo.PropertyAlias = propertyAlias;
			taobaoProductInfo.ProTitle = proTitle;
			taobaoProductInfo.Num = num;
			taobaoProductInfo.LocationState = locationState;
			taobaoProductInfo.LocationCity = locationCity;
			taobaoProductInfo.FreightPayer = text7;
			taobaoProductInfo.Cid = System.Convert.ToInt64(value);
			if (!string.IsNullOrEmpty(text))
			{
				taobaoProductInfo.InputPids = text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				taobaoProductInfo.InputStr = text2;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				taobaoProductInfo.SkuProperties = text3;
			}
			if (!string.IsNullOrEmpty(text5))
			{
				taobaoProductInfo.SkuPrices = text5;
			}
			if (!string.IsNullOrEmpty(text6))
			{
				taobaoProductInfo.SkuOuterIds = text6;
			}
			if (!string.IsNullOrEmpty(text4))
			{
				taobaoProductInfo.SkuQuantities = text4;
			}
			taobaoProductInfo.HasInvoice = System.Convert.ToBoolean(value2);
			taobaoProductInfo.HasWarranty = System.Convert.ToBoolean(value3);
			taobaoProductInfo.HasDiscount = false;
			taobaoProductInfo.ListTime = System.DateTime.Now;
			if (ProductHelper.UpdateToaobProduct(taobaoProductInfo))
			{
				this.litmsg.Text = "制作淘宝格式的商品数据成功";
				return;
			}
			this.litmsg.Text = "制作淘宝格式的商品数据失败";
		}
	}
}
