using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShoppingCart_GiftList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_ShoppingCart_GiftList";
		private Panel pnlShopGiftCart;
		private Panel pnlFreeShopGiftCart;
		private DataList dataListGiftShoppingCrat;
		private DataList dataShopFreeGift;
		public event DataListCommandEventHandler ItemCommand;
		public event DataListCommandEventHandler FreeItemCommand;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.dataListGiftShoppingCrat.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListGiftShoppingCrat.DataSource = value;
			}
		}
		[Browsable(false)]
		public object FreeDataSource
		{
			get
			{
				return this.dataShopFreeGift.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataShopFreeGift.DataSource = value;
			}
		}
		public Common_ShoppingCart_GiftList()
		{
			base.ID = "Common_ShoppingCart_GiftList";
		}
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_GiftList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dataListGiftShoppingCrat = (DataList)this.FindControl("dataListGiftShoppingCrat");
			this.dataShopFreeGift = (DataList)this.FindControl("dataShopFreeGift");
			this.pnlFreeShopGiftCart = (Panel)this.FindControl("pnlFreeShopGiftCart");
			this.pnlFreeShopGiftCart.Visible = false;
			this.pnlShopGiftCart = (Panel)this.FindControl("pnlShopGiftCart");
			this.pnlShopGiftCart.Visible = false;
			this.dataListGiftShoppingCrat.ItemCommand += new DataListCommandEventHandler(this.dataListGiftShoppingCrat_ItemCommand);
			this.dataShopFreeGift.ItemCommand += new DataListCommandEventHandler(this.dataShopFreeGift_ItemCommand);
		}
		private void dataListGiftShoppingCrat_ItemCommand(object sender, DataListCommandEventArgs e)
		{
			this.ItemCommand(sender, e);
		}
		private void dataShopFreeGift_ItemCommand(object sender, DataListCommandEventArgs e)
		{
			this.FreeItemCommand(sender, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListGiftShoppingCrat.DataSource != null)
			{
				this.dataListGiftShoppingCrat.DataBind();
			}
			if (this.dataShopFreeGift.DataSource != null)
			{
				this.dataShopFreeGift.DataBind();
			}
		}
		public void ShowGiftCart(bool pointgift, bool freegift)
		{
			this.pnlShopGiftCart.Visible = pointgift;
			this.pnlFreeShopGiftCart.Visible = freegift;
		}
	}
}
