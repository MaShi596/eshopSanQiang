using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ShoppingCart : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtSKU;
		private System.Web.UI.WebControls.Button btnSKU;
		private ImageLinkButton btnClearCart;
		private Common_ShoppingCart_ProductList shoppingCartProductList;
		private Common_ShoppingCart_GiftList shoppingCartGiftList;
		private Common_ShoppingCart_PromoGiftList shoppingCartPromoGiftList;
		private FormatedMoneyLabel lblTotalPrice;
		private System.Web.UI.WebControls.Literal lblAmoutPrice;
		private System.Web.UI.WebControls.HyperLink hlkReducedPromotion;
		private System.Web.UI.WebControls.Button btnCheckout;
		private System.Web.UI.WebControls.Panel pnlShopCart;
		private System.Web.UI.WebControls.Panel pnlPromoGift;
		private System.Web.UI.WebControls.Literal litNoProduct;
		private System.Web.UI.WebControls.HiddenField hfdIsLogin;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShoppingCart.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.txtSKU = (System.Web.UI.WebControls.TextBox)this.FindControl("txtSKU");
			this.btnSKU = (System.Web.UI.WebControls.Button)this.FindControl("btnSKU");
			this.btnClearCart = (ImageLinkButton)this.FindControl("btnClearCart");
			this.shoppingCartProductList = (Common_ShoppingCart_ProductList)this.FindControl("Common_ShoppingCart_ProductList");
			this.shoppingCartGiftList = (Common_ShoppingCart_GiftList)this.FindControl("Common_ShoppingCart_GiftList");
			this.shoppingCartPromoGiftList = (Common_ShoppingCart_PromoGiftList)this.FindControl("Common_ShoppingCart_PromoGiftList");
			this.lblTotalPrice = (FormatedMoneyLabel)this.FindControl("lblTotalPrice");
			this.lblAmoutPrice = (System.Web.UI.WebControls.Literal)this.FindControl("lblAmoutPrice");
			this.hlkReducedPromotion = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlkReducedPromotion");
			this.btnCheckout = (System.Web.UI.WebControls.Button)this.FindControl("btnCheckout");
			this.pnlShopCart = (System.Web.UI.WebControls.Panel)this.FindControl("pnlShopCart");
			this.pnlPromoGift = (System.Web.UI.WebControls.Panel)this.FindControl("pnlPromoGift");
			this.litNoProduct = (System.Web.UI.WebControls.Literal)this.FindControl("litNoProduct");
			this.hfdIsLogin = (System.Web.UI.WebControls.HiddenField)this.FindControl("hfdIsLogin");
			this.btnSKU.Click += new System.EventHandler(this.btnSKU_Click);
			this.btnClearCart.Click += new System.EventHandler(this.btnClearCart_Click);
			this.shoppingCartProductList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.shoppingCartProductList_ItemCommand);
			this.shoppingCartGiftList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.shoppingCartGiftList_ItemCommand);
			this.shoppingCartGiftList.FreeItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.shoppingCartGiftList_FreeItemCommand);
			this.shoppingCartPromoGiftList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.shoppingCartPromoGiftList_ItemCommand);
			this.btnCheckout.Click += new System.EventHandler(this.btnCheckout_Click);
			if (!Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsOpenSiteSale && !Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				this.btnSKU.Visible = false;
				this.btnCheckout.Visible = false;
			}
			if (!this.Page.IsPostBack)
			{
				this.BindShoppingCart();
			}
			if (!Hidistro.Membership.Context.HiContext.Current.User.IsAnonymous)
			{
				this.hfdIsLogin.Value = "logined";
			}
		}
		protected void btnSKU_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtSKU.Text.Trim()))
			{
				this.ShowMessage("请输入货号", false);
			}
			else
			{
				System.Collections.Generic.IList<string> skuIdsBysku = ShoppingProcessor.GetSkuIdsBysku(this.txtSKU.Text.Trim());
				if (skuIdsBysku == null || skuIdsBysku.Count == 0)
				{
					this.ShowMessage("货号无效，请确认后重试", false);
				}
				else
				{
					foreach (string current in skuIdsBysku)
					{
						ShoppingCartProcessor.AddLineItem(current, 1);
					}
					this.BindShoppingCart();
				}
			}
		}
		protected void btnClearCart_Click(object sender, System.EventArgs e)
		{
			string text = this.Page.Request.Form["ck_productId"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMessage("请选择要清除的商品", false);
			}
			else
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					string skuId = array[i];
					ShoppingCartProcessor.RemoveLineItem(skuId);
				}
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
			}
		}
		protected void shoppingCartProductList_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			System.Web.UI.Control control = e.Item.Controls[0];
			System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)control.FindControl("txtBuyNum");
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)control.FindControl("litSkuId");
			int num;
			if (!int.TryParse(textBox.Text, out num) || textBox.Text.IndexOf(".") != -1)
			{
				this.ShowMessage("购买数量必须为整数", false);
			}
			else
			{
				if (num <= 0)
				{
					this.ShowMessage("购买数量必须为大于0的整数", false);
				}
				else
				{
					if (e.CommandName == "updateBuyNum")
					{
						if (ShoppingCartProcessor.GetSkuStock(literal.Text.Trim()) < num)
						{
							this.ShowMessage("该商品库存不够", false);
							return;
						}
						ShoppingCartProcessor.UpdateLineItemQuantity(literal.Text, num);
					}
					if (e.CommandName == "delete")
					{
						ShoppingCartProcessor.RemoveLineItem(literal.Text);
					}
					this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
				}
			}
		}
		protected void shoppingCartPromoGiftList_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName.Equals("change"))
			{
				int num = System.Convert.ToInt32(e.CommandArgument.ToString());
				if (num > 0)
				{
					int giftItemQuantity = ShoppingCartProcessor.GetGiftItemQuantity(PromoteType.SentGift);
					if (this.shoppingCartPromoGiftList.SumNum > giftItemQuantity)
					{
						ShoppingCartProcessor.AddGiftItem(num, 1, PromoteType.SentGift);
						this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
					}
					else
					{
						this.ShowMessage("礼品兑换失败，您不能超过最多兑换数", false);
					}
				}
			}
		}
		protected void shoppingCartGiftList_FreeItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (e.CommandName == "delete")
			{
				string text = e.CommandArgument.ToString();
				int giftId = 0;
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out giftId))
				{
					ShoppingCartProcessor.RemoveGiftItem(giftId, PromoteType.SentGift);
				}
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
			}
		}
		protected void shoppingCartGiftList_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			System.Web.UI.Control control = e.Item.Controls[0];
			System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)control.FindControl("txtBuyNum");
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)control.FindControl("litGiftId");
			int num;
			if (!int.TryParse(textBox.Text, out num) || textBox.Text.IndexOf(".") != -1)
			{
				this.ShowMessage("兑换数量必须为整数", false);
			}
			else
			{
				if (num <= 0)
				{
					this.ShowMessage("兑换数量必须为大于0的整数", false);
				}
				else
				{
					if (e.CommandName == "updateBuyNum")
					{
						ShoppingCartProcessor.UpdateGiftItemQuantity(System.Convert.ToInt32(literal.Text), num, PromoteType.NotSet);
					}
					if (e.CommandName == "delete")
					{
						ShoppingCartProcessor.RemoveGiftItem(System.Convert.ToInt32(literal.Text), PromoteType.NotSet);
					}
					this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
				}
			}
		}
		protected void btnCheckout_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.HiContext.Current.Context.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
		}
		private void BindShoppingCart()
		{
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			if (shoppingCart == null)
			{
				this.pnlShopCart.Visible = false;
				this.litNoProduct.Visible = true;
				ShoppingCartProcessor.ClearShoppingCart();
			}
			else
			{
				this.pnlShopCart.Visible = true;
				this.litNoProduct.Visible = false;
				if (shoppingCart.LineItems.Values.Count > 0)
				{
					this.shoppingCartProductList.DataSource = shoppingCart.LineItems.Values;
					this.shoppingCartProductList.DataBind();
					this.shoppingCartProductList.ShowProductCart();
				}
				if (shoppingCart.LineGifts.Count > 0)
				{
					System.Collections.Generic.IEnumerable<ShoppingCartGiftInfo> enumerable = 
						from shoppingCartGiftInfo_0 in shoppingCart.LineGifts
						where shoppingCartGiftInfo_0.PromoType == 0
						select shoppingCartGiftInfo_0;
					System.Collections.Generic.IEnumerable<ShoppingCartGiftInfo> enumerable2 = 
						from shoppingCartGiftInfo_0 in shoppingCart.LineGifts
						where shoppingCartGiftInfo_0.PromoType == 5
						select shoppingCartGiftInfo_0;
					this.shoppingCartGiftList.DataSource = enumerable;
					this.shoppingCartGiftList.FreeDataSource = enumerable2;
					this.shoppingCartGiftList.DataBind();
					this.shoppingCartGiftList.ShowGiftCart(enumerable.Count<ShoppingCartGiftInfo>() > 0, enumerable2.Count<ShoppingCartGiftInfo>() > 0);
				}
				if (shoppingCart.IsSendGift)
				{
					int num = 1;
					int giftItemQuantity = ShoppingCartProcessor.GetGiftItemQuantity(PromoteType.SentGift);
					System.Collections.Generic.IList<GiftInfo> onlinePromotionGifts = ProductBrowser.GetOnlinePromotionGifts();
					if (onlinePromotionGifts != null && onlinePromotionGifts.Count > 0)
					{
						this.shoppingCartPromoGiftList.DataSource = onlinePromotionGifts;
						this.shoppingCartPromoGiftList.DataBind();
						this.shoppingCartPromoGiftList.ShowPromoGift(num - giftItemQuantity, num);
					}
				}
				if (shoppingCart.IsReduced)
				{
					this.lblAmoutPrice.Text = string.Format("商品金额：{0}", shoppingCart.GetAmount().ToString("F2"));
					this.hlkReducedPromotion.Text = shoppingCart.ReducedPromotionName + string.Format(" 优惠：{0}", shoppingCart.ReducedPromotionAmount.ToString("F2"));
					this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
					{
						shoppingCart.ReducedPromotionId
					});
				}
				this.lblTotalPrice.Money = shoppingCart.GetTotal();
			}
		}
	}
}
