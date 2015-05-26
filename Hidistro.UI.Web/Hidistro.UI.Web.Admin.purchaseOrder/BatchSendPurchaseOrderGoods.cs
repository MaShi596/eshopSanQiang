using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Net;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.purchaseOrder
{
	public class BatchSendPurchaseOrderGoods : AdminPage
	{
		private string strIds;
		protected ShippingModeDropDownList dropShippingMode;
		protected System.Web.UI.WebControls.Button btnSetShippingMode;
		protected System.Web.UI.WebControls.DropDownList dropExpressComputerpe;
		protected System.Web.UI.WebControls.Button btnSetExpressComputerpe;
		protected System.Web.UI.WebControls.TextBox txtStartShipOrderNumber;
		protected System.Web.UI.WebControls.Button btnSetShipOrderNumber;
		protected Grid grdOrderGoods;
		protected DropdownColumn dropShippId;
		protected DropdownColumn dropExpress;
		protected System.Web.UI.WebControls.Button btnBatchSendGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["PurchaseOrderIds"]))
			{
				this.strIds = base.Request.QueryString["PurchaseOrderIds"];
			}
			if (this.strIds.Length <= 0)
			{
				return;
			}
			this.btnSetShippingMode.Click += new System.EventHandler(this.btnSetShippingMode_Click);
			this.btnSetExpressComputerpe.Click += new System.EventHandler(this.btnSetExpressComputerpe_Click);
			this.btnSetShipOrderNumber.Click += new System.EventHandler(this.btnSetShipOrderNumber_Click);
			this.grdOrderGoods.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdOrderGoods_RowDataBound);
			this.btnBatchSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropShippingMode.DataBind();
				this.dropExpressComputerpe.DataSource = ExpressHelper.GetAllExpress();
				this.dropExpressComputerpe.DataTextField = "name";
				this.dropExpressComputerpe.DataValueField = "Kuaidi100Code";
				this.dropExpressComputerpe.DataBind();
				this.dropExpressComputerpe.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
				this.BindData();
			}
		}
		private void grdOrderGoods_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				string purchaseOrderId = (string)this.grdOrderGoods.DataKeys[e.Row.RowIndex].Value;
				System.Web.UI.WebControls.DropDownList dropDownList = e.Row.FindControl("dropExpress") as System.Web.UI.WebControls.DropDownList;
				PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
				if (purchaseOrder != null && purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)
				{
					ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(purchaseOrder.ExpressCompanyName);
					if (expressCompanyInfo != null)
					{
						dropDownList.SelectedValue = expressCompanyInfo.Kuaidi100Code;
					}
				}
			}
		}
		private void btnSetShipOrderNumber_Click(object sender, System.EventArgs e)
		{
			string[] orderIds = this.strIds.Split(new char[]
			{
				','
			});
			long num = 0L;
			if (!string.IsNullOrEmpty(this.txtStartShipOrderNumber.Text.Trim()) && long.TryParse(this.txtStartShipOrderNumber.Text.Trim(), out num))
			{
				SalesHelper.SetPurchaseOrderShipNumber(orderIds, this.txtStartShipOrderNumber.Text.Trim());
				this.BindData();
				return;
			}
			this.ShowMsg("起始发货单号不允许为空且必须为正整数", false);
		}
		private void btnSetExpressComputerpe_Click(object sender, System.EventArgs e)
		{
			string orderIds = "'" + this.strIds.Replace(",", "','") + "'";
			if (!string.IsNullOrEmpty(this.dropExpressComputerpe.SelectedValue))
			{
				SalesHelper.SetPurchaseOrderExpressComputerpe(orderIds, this.dropExpressComputerpe.SelectedItem.Text, this.dropExpressComputerpe.SelectedValue);
			}
			this.BindData();
		}
		private void btnSetShippingMode_Click(object sender, System.EventArgs e)
		{
			string orderIds = "'" + this.strIds.Replace(",", "','") + "'";
			if (this.dropShippingMode.SelectedValue.HasValue)
			{
				SalesHelper.SetPurchaseOrderShippingMode(orderIds, this.dropShippingMode.SelectedValue.Value, this.dropShippingMode.SelectedItem.Text);
			}
			this.BindData();
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			if (this.grdOrderGoods.Rows.Count <= 0)
			{
				this.ShowMsg("没有要进行发货的订单。", false);
				return;
			}
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdOrderGoods.Columns[4];
			System.Web.UI.WebControls.ListItemCollection selectedItems = dropdownColumn.SelectedItems;
			DropdownColumn dropdownColumn2 = (DropdownColumn)this.grdOrderGoods.Columns[5];
			System.Web.UI.WebControls.ListItemCollection selectedItems2 = dropdownColumn2.SelectedItems;
			int num = 0;
			for (int i = 0; i < selectedItems.Count; i++)
			{
				string text = (string)this.grdOrderGoods.DataKeys[this.grdOrderGoods.Rows[i].RowIndex].Value;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)this.grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
				System.Web.UI.WebControls.ListItem listItem = selectedItems[i];
				System.Web.UI.WebControls.ListItem listItem2 = selectedItems2[i];
				int num2 = 0;
				int.TryParse(listItem.Value, out num2);
				if (!string.IsNullOrEmpty(textBox.Text.Trim()) && textBox.Text.Length <= 20 && num2 > 0)
				{
					PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(text);
					if (purchaseOrder != null && (purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid || (purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay && purchaseOrder.Gateway == "hishop.plugins.payment.podrequest")) && !string.IsNullOrEmpty(listItem2.Value))
					{
						ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(int.Parse(listItem.Value), true);
						purchaseOrder.RealShippingModeId = shippingMode.ModeId;
						purchaseOrder.RealModeName = shippingMode.Name;
						purchaseOrder.ExpressCompanyAbb = listItem2.Value;
						purchaseOrder.ExpressCompanyName = listItem2.Text;
						purchaseOrder.ShipOrderNumber = textBox.Text;
						if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
						{
							SendNote sendNote = new SendNote();
							sendNote.NoteId = Globals.GetGenerateId() + num;
							sendNote.OrderId = text;
							sendNote.Operator = Hidistro.Membership.Context.HiContext.Current.User.Username;
							sendNote.Remark = "后台" + sendNote.Operator + "发货成功";
							SalesHelper.SavePurchaseSendNote(sendNote);
							if (!string.IsNullOrEmpty(purchaseOrder.TaobaoOrderId))
							{
								try
								{
									ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(purchaseOrder.ExpressCompanyName);
									string requestUriString = string.Format("http://order1.kuaidiangtong.com/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}", purchaseOrder.TaobaoOrderId, expressCompanyInfo.TaobaoCode, purchaseOrder.ShipOrderNumber);
									System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
									webRequest.GetResponse();
									goto IL_27F;
								}
								catch
								{
									goto IL_27F;
								}
								goto IL_276;
							}
							IL_27F:
							num++;
						}
					}
				}
				IL_276:;
			}
			if (num == 0)
			{
				this.ShowMsg("批量发货失败！", false);
				return;
			}
			if (num > 0)
			{
				this.BindData();
				this.ShowMsg(string.Format("批量发货成功！发货数量{0}个", num), true);
			}
		}
		private void BindData()
		{
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdOrderGoods.Columns[4];
			dropdownColumn.DataSource = SalesHelper.GetShippingModes();
			DropdownColumn dropdownColumn2 = (DropdownColumn)this.grdOrderGoods.Columns[5];
			dropdownColumn2.DataSource = ExpressHelper.GetAllExpress();
			string purchaseOrderIds = "'" + this.strIds.Replace(",", "','") + "'";
			this.grdOrderGoods.DataSource = SalesHelper.GetSendGoodsPurchaseOrders(purchaseOrderIds);
			this.grdOrderGoods.DataBind();
		}
	}
}
