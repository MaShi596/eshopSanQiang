using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditeMyThems : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litThemeName;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkDefault;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkLogin;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkBrand;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkBrandDetails;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkProduct;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkProductDetail;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkArticle;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkArticleDetail;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkCountdown;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkCountdownDetail;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkGroup;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkGroupDetail;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkHelp;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkHelpDetail;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkGift;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkGriftDetail;
		protected System.Web.UI.HtmlControls.HtmlAnchor alinkShopcart;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.litThemeName.Text = Hidistro.Membership.Context.HiContext.Current.SiteSettings.Theme;
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			if (siteSettings != null)
			{
				this.alinkDefault.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=default";
				this.alinkLogin.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=login";
				this.alinkBrand.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=brand";
				this.alinkBrandDetails.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=branddetail";
				this.alinkProduct.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=product";
				this.alinkProductDetail.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=productdetail";
				this.alinkArticle.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=article";
				this.alinkArticleDetail.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=articledetail";
				this.alinkCountdown.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=cuountdown";
				this.alinkCountdownDetail.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=cuountdowndetail";
				this.alinkGroup.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=groupbuy";
				this.alinkGroupDetail.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=groupbuydetail";
				this.alinkHelp.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=help";
				this.alinkHelpDetail.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=helpdetail";
				this.alinkGift.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=gift";
				this.alinkGriftDetail.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=giftdetail";
				this.alinkShopcart.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=shopcart";
			}
		}
	}
}
