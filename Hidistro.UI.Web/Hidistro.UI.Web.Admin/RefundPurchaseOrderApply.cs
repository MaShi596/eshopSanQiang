using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PurchaseOrderRefundApply)]
	public class RefundPurchaseOrderApply : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPurchaseOrderId;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.WebControls.DataList dlstRefund;
		protected Pager pager;
		protected System.Web.UI.WebControls.Label refund_lblPurchaseOrderId;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderTotal;
		protected System.Web.UI.WebControls.Label lblRefundType;
		protected System.Web.UI.WebControls.Label lblRefundRemark;
		protected System.Web.UI.WebControls.Label lblContacts;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Label lblTelephone;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.TextBox txtAdminRemark;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;
		protected System.Web.UI.WebControls.Button btnAcceptRefund;
		protected System.Web.UI.WebControls.Button btnRefuseRefund;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstRefund.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstRefund_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptRefund.Click += new System.EventHandler(this.btnAcceptRefund_Click);
			this.btnRefuseRefund.Click += new System.EventHandler(this.btnRefuseRefund_Click);
			if (!base.IsPostBack)
			{
				this.BindRefund();
			}
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的退款申请单", false);
				return;
			}
			int num;
			SalesHelper.DelPurchaseRefundApply(text.Split(new char[]
			{
				','
			}), out num);
			this.BindRefund();
			string text2 = string.Format("成功删除了{0}个退款申请单", num);
			if (text.Split(new char[]
			{
				','
			}).Length != num)
			{
				text2 += ",待处理退款申请单不能删除";
			}
			this.ShowMsg(text2, true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadRefunds(true);
		}
		private void dlstRefund_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblHandleStatus");
				if (label.Text == "0")
				{
					htmlAnchor.Visible = true;
					label.Text = "待处理";
					return;
				}
				if (label.Text == "1")
				{
					label.Text = "已处理";
					return;
				}
				label.Text = "已拒绝";
			}
		}
		private void BindRefund()
		{
			RefundApplyQuery refundQuery = this.GetRefundQuery();
			DbQueryResult purchaseRefundApplys = SalesHelper.GetPurchaseRefundApplys(refundQuery);
			this.dlstRefund.DataSource = purchaseRefundApplys.Data;
			this.dlstRefund.DataBind();
			this.pager.TotalRecords=purchaseRefundApplys.TotalRecords;
			this.pager1.TotalRecords=purchaseRefundApplys.TotalRecords;
			this.txtOrderId.Text = refundQuery.OrderId;
			this.ddlHandleStatus.SelectedIndex = 0;
			if (refundQuery.HandleStatus.HasValue && refundQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = refundQuery.HandleStatus.Value.ToString();
			}
		}
		private RefundApplyQuery GetRefundQuery()
		{
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				refundApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					refundApplyQuery.HandleStatus = new int?(num);
				}
			}
			refundApplyQuery.PageIndex = this.pager.PageIndex;
			refundApplyQuery.PageSize = this.pager.PageSize;
			refundApplyQuery.SortBy = "ApplyForTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			return refundApplyQuery;
		}
		private void ReloadRefunds(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
			{
				nameValueCollection.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
			}
			base.ReloadPage(nameValueCollection);
		}
		protected void btnAcceptRefund_Click(object sender, System.EventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
			SalesHelper.CheckPurchaseRefund(purchaseOrder, Hidistro.Membership.Context.HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
			this.BindRefund();
			this.ShowMsg("成功的确认了采购单退款", true);
		}
		private void btnRefuseRefund_Click(object sender, System.EventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
			SalesHelper.CheckPurchaseRefund(purchaseOrder, Hidistro.Membership.Context.HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindRefund();
			this.ShowMsg("成功的拒绝了采购单退款", true);
		}
	}
}
