using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_OrderItemsList : System.Web.UI.UserControl
	{
		protected Supplier_Drop_Suppliers drpSupplier;
		protected System.Web.UI.WebControls.Button btnFenPei;
		protected System.Web.UI.WebControls.Literal litlSupplierComment;
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		protected System.Web.UI.WebControls.Literal litWeight;
		protected System.Web.UI.WebControls.Literal lblAmoutPrice;
		protected System.Web.UI.WebControls.HyperLink hlkReducedPromotion;
		protected FormatedMoneyLabel lblTotalPrice;
		protected System.Web.UI.WebControls.Literal lblBundlingPrice;
		protected System.Web.UI.WebControls.Label lblOrderGifts;
		protected System.Web.UI.WebControls.DataList grdOrderGift;
		private OrderInfo order;
		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.btnFenPei.Click += new System.EventHandler(this.btnFenPei_Click);
			this.drpSupplier.SelectedIndexChanged += new System.EventHandler(this.drpSupplier_SelectedIndexChanged);
			this.drpSupplier.AutoPostBack = true;
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void drpSupplier_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.drpSupplier.SelectedValue) && this.drpSupplier.SelectedValue != "0")
			{
				System.Data.DataTable dataTable = Methods.Supplier_SupGet(int.Parse(this.drpSupplier.SelectedValue));
				if (dataTable.Rows[0]["comment"] != System.DBNull.Value)
				{
					this.litlSupplierComment.Text = (string)dataTable.Rows[0]["comment"];
					return;
				}
			}
			else
			{
				this.litlSupplierComment.Text = string.Empty;
			}
		}
		private void btnFenPei_Click(object sender, System.EventArgs e)
		{
			int value = 0;
			string username = string.Empty;
			if (!string.IsNullOrEmpty(this.drpSupplier.SelectedValue))
			{
				value = int.Parse(this.drpSupplier.SelectedValue);
				username = this.drpSupplier.SelectedItem.Text;
			}
			int num = 0;
			foreach (System.Web.UI.WebControls.DataListItem dataListItem in this.dlstOrderItems.Items)
			{
				System.Web.UI.WebControls.CheckBox checkBox = dataListItem.FindControl("cbDBId") as System.Web.UI.WebControls.CheckBox;
				if (checkBox.Checked)
				{
					num++;
					string string_ = (string)this.dlstOrderItems.DataKeys[dataListItem.ItemIndex];
					Methods.Supplier_OrderItemSupplierUpdate(this.order.OrderId, string_, new int?(value), username);
				}
			}
			this.BindData();
			if (num == 0)
			{
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "error", "<script language=\"javascript\" >alert('请选择商品！');</script>");
				return;
			}
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", "<script language=\"javascript\" >alert('分配完成！');</script>");
		}
		private void BindData()
		{
			this.dlstOrderItems.DataSource = Methods.Supplier_OrderItemsGet(this.order.OrderId);
			this.dlstOrderItems.DataBind();
			if (this.order.Gifts.Count == 0)
			{
				this.grdOrderGift.Visible = false;
				this.lblOrderGifts.Visible = false;
			}
			else
			{
				this.grdOrderGift.DataSource = this.order.Gifts;
				this.grdOrderGift.DataBind();
			}
			this.litWeight.Text = this.order.Weight.ToString();
			if (this.order.IsReduced)
			{
				this.lblAmoutPrice.Text = string.Format("商品金额：{0}", Globals.FormatMoney(this.order.GetAmount()));
				this.hlkReducedPromotion.Text = this.order.ReducedPromotionName + string.Format(" 优惠：{0}", Globals.FormatMoney(this.order.ReducedPromotionAmount));
				this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.ReducedPromotionId
				});
			}
			if (this.order.BundlingID > 0)
			{
				this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(this.order.BundlingPrice));
			}
			this.lblTotalPrice.Money = this.order.GetAmount() - this.order.ReducedPromotionAmount;
		}
	}
}
