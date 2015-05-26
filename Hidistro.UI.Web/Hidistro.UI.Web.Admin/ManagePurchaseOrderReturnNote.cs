using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class ManagePurchaseOrderReturnNote : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtPurchaseOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.WebControls.DataList dlstReturnNote;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			if (!base.IsPostBack)
			{
				this.BindRefundNote();
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
				this.ShowMsg("请选要删除的退货单", false);
				return;
			}
			int num;
			SalesHelper.DelPurchaseReturnsApply(text.Split(new char[]
			{
				','
			}), out num);
			this.BindRefundNote();
			this.ShowMsg(string.Format("成功删除了{0}个退货单", num), true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadRefundNotes(true);
		}
		private void BindRefundNote()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["PurchaseOrderId"]);
			}
			returnsApplyQuery.HandleStatus = new int?(1);
			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
			returnsApplyQuery.SortBy = "HandleTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			DbQueryResult purchaseReturnsApplys = SalesHelper.GetPurchaseReturnsApplys(returnsApplyQuery);
			this.dlstReturnNote.DataSource = purchaseReturnsApplys.Data;
			this.dlstReturnNote.DataBind();
			this.pager.TotalRecords=purchaseReturnsApplys.TotalRecords;
			this.pager1.TotalRecords=purchaseReturnsApplys.TotalRecords;
			this.txtPurchaseOrderId.Text = returnsApplyQuery.OrderId;
		}
		private void ReloadRefundNotes(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("PurchaseOrderId", this.txtPurchaseOrderId.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
