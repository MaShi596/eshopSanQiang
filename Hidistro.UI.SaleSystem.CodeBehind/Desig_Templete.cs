using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Desig_Templete : HtmlTemplatedWebControl
	{
		private const string templetestr = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:Hidistro_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:Hidistro_designer.moveUp()\" class=\"up updisable\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:Hidistro_designer.moveDown()\" class=\"down downdisable\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"Hidistro_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"Hidistro_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><script>Hidistro_designer.Design_Page_Init();</script>";
		protected string skintemp = "";
		protected string tempurl = "";
		protected string viewname = "";
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			Hidistro.Membership.Core.IUser contexUser = Hidistro.Membership.Context.Users.GetContexUser();
			if (!Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings && contexUser.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("login.aspx"), true);
			}
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings && contexUser.UserRole != Hidistro.Membership.Core.Enums.UserRole.Distributor)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "Shopadmin/DistributorLogin.aspx", true);
			}
			this.SetDesignSkinName();
			if (this.SkinName == null || this.tempurl == "")
			{
				base.GotoResourceNotFound();
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)this.FindControl("litPageName");
			System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)this.FindControl("litTempete");
			System.Web.UI.WebControls.Literal literal3 = (System.Web.UI.WebControls.Literal)this.FindControl("litaccount");
			System.Web.UI.WebControls.Literal literal4 = (System.Web.UI.WebControls.Literal)this.FindControl("litview");
			System.Web.UI.WebControls.Literal literal5 = (System.Web.UI.WebControls.Literal)this.FindControl("litDefault");
			if (!this.Page.IsPostBack)
			{
				if (literal != null)
				{
					literal.Text = "<script>Hidistro_designer.CurrentPageName='" + this.skintemp + "'</script>";
				}
				if (literal2 != null)
				{
					literal2.Text = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:Hidistro_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:Hidistro_designer.moveUp()\" class=\"up updisable\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:Hidistro_designer.moveDown()\" class=\"down downdisable\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"Hidistro_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"Hidistro_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><script>Hidistro_designer.Design_Page_Init();</script>";
				}
				if (literal3 != null)
				{
					Hidistro.Membership.Core.IUser contexUser = Hidistro.Membership.Context.Users.GetContexUser();
					if (contexUser != null)
					{
						literal3.Text = "<a>我的账号：" + contexUser.Username + "</a>";
					}
				}
				if (literal5 != null)
				{
					literal5.Text = "<a href=\"" + Globals.ApplicationPath + "/\">查看店铺</a>";
				}
				if (literal4 != null)
				{
					string str = Globals.ApplicationPath + "/";
					if (this.viewname != "")
					{
						str = Globals.GetSiteUrls().UrlData.FormatUrl(this.viewname);
					}
					literal4.Text = "<a href=\"" + str + "\" target=\"_blank\" class=\"button\">预览</a>";
				}
			}
		}
		protected void SetDesignSkinName()
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["skintemp"]))
			{
				base.GotoResourceNotFound();
			}
			this.skintemp = this.Page.Request.QueryString["skintemp"];
			string text = this.skintemp;
			switch (text)
			{
			case "default":
				this.SkinName = "Skin-Desig_Templete.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-Default.html");
				return;
			case "login":
				this.SkinName = "Skin-Desig_login.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-Login.html");
				return;
			case "brand":
				this.SkinName = "Skin-Desig_Brand.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-Brand.html");
				return;
			case "branddetail":
				this.SkinName = "Skin-Desig_BrandDetails.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-BrandDetails.html");
				return;
			case "product":
				this.SkinName = "Skin-Desig_SubCategory.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-SubCategory.html");
				return;
			case "productdetail":
				this.SkinName = "Skin-Desig_ProductDetails.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-ProductDetails.html");
				return;
			case "article":
				this.SkinName = "Skin-Desig_Articles.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-Articles.html");
				return;
			case "articledetail":
				this.SkinName = "Skin-Desig_ArticleDetails.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-ArticleDetails.html");
				return;
			case "cuountdown":
				this.SkinName = "Skin-Desig_CountDownProducts.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-CountDownProducts.html");
				return;
			case "cuountdowndetail":
				this.SkinName = "Skin-Desig_CountDownProductsDetails.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-CountDownProductsDetails.html");
				return;
			case "groupbuy":
				this.SkinName = "Skin-Desig_GroupBuyProducts.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-GroupBuyProducts.html");
				return;
			case "groupbuydetail":
				this.SkinName = "Skin-Desig_GroupBuyProductDetails.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-GroupBuyProductDetails.html");
				return;
			case "help":
				this.SkinName = "Skin-Desig_Helps.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-Helps.html");
				return;
			case "helpdetail":
				this.SkinName = "Skin-Desig_HelpDetails.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-HelpDetails.html");
				return;
			case "gift":
				this.SkinName = "Skin-Desig_OnlineGifts.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-OnlineGifts.html");
				return;
			case "giftdetail":
				this.SkinName = "Skin-Desig_GiftDetails.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-GiftDetails.html");
				return;
			case "shopcart":
				this.SkinName = "Skin-Desig_ShoppingCart.html";
				this.tempurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/Skin-ShoppingCart.html");
				return;
			}
			this.SkinName = null;
		}
	}
}
