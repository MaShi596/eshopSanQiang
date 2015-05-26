using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PurchaseorderSendGood)]
	public class Supplier_Admin_POrderOption : AdminPage
	{
		protected ShippingModeRadioButtonList radioShippingMode;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litShipToDate;
		protected System.Web.UI.WebControls.Literal litRemark;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto2;
		protected System.Web.UI.WebControls.CheckBox btnShipPointSelf;
		protected System.Web.UI.HtmlControls.HtmlGenericControl htmlDivShipPoint;
		protected System.Web.UI.WebControls.Literal txtShipPointName;
		protected System.Web.UI.WebControls.Literal txtShipPointName2;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.Button btnSearchShipPoint;
		protected Grid grdShipPoints;
		protected Supplier_Admin_POrderItemsList itemsList;
		protected System.Web.UI.WebControls.Literal litFreight;
		protected System.Web.UI.WebControls.Literal lblModeName;
		protected System.Web.UI.WebControls.Literal litDiscount;
		protected System.Web.UI.WebControls.Literal litTotalPrice;
		protected System.Web.UI.WebControls.Button btnSendGoods;
		private string purchaseorderId;
		private PurchaseOrderInfo purchaseorder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.purchaseorderId = this.Page.Request.QueryString["PurchaseOrderId"];
			this.purchaseorder = SalesHelper.GetPurchaseOrder(this.purchaseorderId);
			if (this.purchaseorder == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.itemsList.PurchaseOrder = this.purchaseorder;
			this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
			this.btnShipPointSelf.CheckedChanged += new System.EventHandler(this.btnShipPointSelf_CheckedChanged);
			this.btnSearchShipPoint.Click += new System.EventHandler(this.btnSearchShipPoint_Click);
			this.grdShipPoints.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdShipPoints_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.radioShippingMode.DataBind();
				this.radioShippingMode.SelectedValue = new int?(this.purchaseorder.ShippingModeId);
				this.BindExpressCompany(this.purchaseorder.ShippingModeId);
				this.expressRadioButtonList.SelectedValue = this.purchaseorder.ExpressCompanyAbb;
				this.BindShippingAddress(this.purchaseorder);
				this.BindCharges(this.purchaseorder);
				this.txtShipOrderNumber.Text = this.purchaseorder.ShipOrderNumber;
				this.litShippingModeName.Text = this.purchaseorder.ModeName;
				this.litShipToDate.Text = this.purchaseorder.ShipToDate;
				this.litRemark.Text = this.purchaseorder.Remark;
				this.btnShipPointSelf.AutoPostBack = true;
				string[] array = this.purchaseorder.ShippingRegion.Split("，".ToCharArray());
				System.Data.DataTable dataTable = null;
				if (array.Length == 3)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], array[2]);
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					}
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
					}
				}
				if (array.Length == 2)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0]);
					}
				}
				if (array.Length == 1)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
				}
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					dataTable = Methods.Supplier_ShipPointGetByUserId(dataTable.Rows[0]["UserId"].ToString());
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						string text = "<b>" + (string)dataTable.Rows[0]["username"] + "</b> " + (string)dataTable.Rows[0]["Supplier_RegionName"];
						this.txtShipPointNameAuto.Text = text;
						if (dataTable.Rows[0]["comment"] != System.DBNull.Value)
						{
							this.txtShipPointNameAuto2.Text = (string)dataTable.Rows[0]["comment"];
						}
					}
					else
					{
						this.txtShipPointNameAuto.Text = "未匹配到";
					}
				}
				else
				{
					this.txtShipPointNameAuto.Text = "未匹配到";
				}
				this.rsddlRegion.SetSelectedRegionId(new int?(this.purchaseorder.RegionId));
				System.Data.DataTable dataTable2 = Methods.Supplier_POrderSupGet(this.purchaseorderId);
				if (dataTable2 != null && dataTable2.Rows.Count > 0)
				{
					this.btnShipPointSelf.Checked = true;
					this.htmlDivShipPoint.Attributes.Add("style", "");
					this.BindShipPoints();
				}
			}
		}
		private void grdShipPoints_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Select")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int value = (int)this.grdShipPoints.DataKeys[rowIndex].Value;
				Methods.Supplier_POrderShipPointIdUpdate(this.purchaseorderId, new int?(value));
				this.BindShipPoints();
			}
		}
		private void btnSearchShipPoint_Click(object sender, System.EventArgs e)
		{
			this.BindShipPoints();
		}
		private void btnShipPointSelf_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.btnShipPointSelf.Checked)
			{
				this.htmlDivShipPoint.Attributes.Add("style", "");
				Methods.Supplier_POrderShipPointIdUpdate(this.purchaseorderId, new int?(0));
				this.BindShipPoints();
				return;
			}
			this.htmlDivShipPoint.Attributes.Add("style", "display:none");
			Methods.Supplier_POrderShipPointIdUpdate(this.purchaseorderId, null);
		}
		private void BindShipPoints()
		{
			DbQueryResult dbQueryResult = Methods.Supplier_SGet(new ManagerQuery
			{
				RoleId = new System.Guid("5a26c830-b998-4569-bffc-c5ceae774a7a"),
				PageSize = 100000,
				PageIndex = 1
			}, this.rsddlRegion.SelectedRegions, null);
			this.grdShipPoints.DataSource = dbQueryResult.Data;
			this.grdShipPoints.DataBind();
			System.Data.DataTable dataTable = Methods.Supplier_POrderSupGet(this.purchaseorderId);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				string text = "<b>" + (string)dataTable.Rows[0]["username"] + "</b> " + (string)dataTable.Rows[0]["Supplier_RegionName"];
				this.txtShipPointName.Text = text;
				if (dataTable.Rows[0]["comment"] != System.DBNull.Value)
				{
					this.txtShipPointName2.Text = (string)dataTable.Rows[0]["comment"];
					return;
				}
			}
			else
			{
				this.txtShipPointName.Text = "无";
				this.txtShipPointName2.Text = string.Empty;
			}
		}
		private void BindCharges(PurchaseOrderInfo purchaseOrder)
		{
			this.lblModeName.Text = purchaseOrder.ModeName;
			this.litFreight.Text = Globals.FormatMoney(purchaseOrder.AdjustedFreight);
			this.litDiscount.Text = Globals.FormatMoney(purchaseOrder.AdjustedDiscount);
			this.litTotalPrice.Text = Globals.FormatMoney(purchaseOrder.GetPurchaseTotal());
		}
		private void BindShippingAddress(PurchaseOrderInfo purchaseorder)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(purchaseorder.ShippingRegion))
			{
				text = purchaseorder.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(purchaseorder.Address))
			{
				text += purchaseorder.Address;
			}
			if (!string.IsNullOrEmpty(purchaseorder.ZipCode))
			{
				text = text + "," + purchaseorder.ZipCode;
			}
			if (!string.IsNullOrEmpty(purchaseorder.ShipTo))
			{
				text = text + "," + purchaseorder.ShipTo;
			}
			if (!string.IsNullOrEmpty(purchaseorder.TelPhone))
			{
				text = text + "," + purchaseorder.TelPhone;
			}
			if (!string.IsNullOrEmpty(purchaseorder.CellPhone))
			{
				text = text + "," + purchaseorder.CellPhone;
			}
			this.litReceivingInfo.Text = text;
		}
		private void BindExpressCompany(int modeId)
		{
			this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
			this.expressRadioButtonList.DataBind();
		}
		private void radioShippingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.radioShippingMode.SelectedValue.HasValue)
			{
				this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
			}
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseorderId);
			if (purchaseOrder == null)
			{
				return;
			}
			if (!purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
			{
				this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
				return;
			}
			if (Methods.Supplier_POrderIsFenPei(this.purchaseorderId))
			{
				this.ShowMsg("生成成功", true);
				return;
			}
			string text = Methods.Supplier_POrderItemSupplierUpdate(purchaseOrder);
			if (text != "true")
			{
				this.ShowMsg(text, false);
				return;
			}
			purchaseOrder.RealShippingModeId = 0;
			purchaseOrder.RealModeName = "配送方式(已实际发货单为准)";
			purchaseOrder.ShipOrderNumber = string.Format("{0}", string.Format(" <a style=\"color:red;cursor:pointer;\" target=\"_blank\" onclick=\"{0}\">物流详细</a>", "showWindow_ShipInfoPage('" + purchaseOrder.PurchaseOrderId + "')"));
			if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
			{
				Methods.Supplier_POrderItemsSupplierFenPeiOverUpdate(purchaseOrder.PurchaseOrderId);
				this.CloseWindow();
				return;
			}
			this.ShowMsg("发货失败", false);
		}
	}
}
