using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_POrderItemsList : System.Web.UI.UserControl
	{
		protected Supplier_Drop_Suppliers drpSupplier;
		protected System.Web.UI.WebControls.Button btnFenPei;
		protected System.Web.UI.WebControls.Literal litlSupplierComment;
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		protected PurchaseOrderItemUpdateHyperLink purchaseOrderItemUpdateHyperLink;
		protected FormatedMoneyLabel lblGoodsAmount;
		protected System.Web.UI.WebControls.Literal lblWeight;
		protected System.Web.UI.HtmlControls.HtmlGenericControl giftsList;
		protected System.Web.UI.WebControls.DataList grdOrderGift;
		private PurchaseOrderInfo purchaseOrder;
		public PurchaseOrderInfo PurchaseOrder
		{
			get
			{
				return this.purchaseOrder;
			}
			set
			{
				this.purchaseOrder = value;
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
					Methods.Supplier_POrderItemSupplierUpdate(this.purchaseOrder.PurchaseOrderId, string_, new int?(value), username);
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
			this.dlstOrderItems.DataSource = Methods.Supplier_POrderItemsGet(this.purchaseOrder.PurchaseOrderId);
			this.dlstOrderItems.DataBind();
			if (this.purchaseOrder.PurchaseOrderGifts.Count <= 0)
			{
				this.giftsList.Visible = false;
			}
			else
			{
				this.grdOrderGift.DataSource = this.purchaseOrder.PurchaseOrderGifts;
				this.grdOrderGift.DataBind();
			}
			this.lblGoodsAmount.Money = this.purchaseOrder.GetProductAmount();
			this.lblWeight.Text = this.purchaseOrder.Weight.ToString(System.Globalization.CultureInfo.InvariantCulture);
			this.purchaseOrderItemUpdateHyperLink.PurchaseOrderId = this.purchaseOrder.PurchaseOrderId;
			this.purchaseOrderItemUpdateHyperLink.PurchaseStatusCode = this.purchaseOrder.PurchaseStatus;
			this.purchaseOrderItemUpdateHyperLink.DistorUserId = this.purchaseOrder.DistributorId;
			this.purchaseOrderItemUpdateHyperLink.DataBind();
		}
	}
}
