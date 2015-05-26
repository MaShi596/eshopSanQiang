using ASPNET.WebControls;
using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserRefundApply : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtOrderId;
		private IButton btnSearch;
		private Common_OrderManage_RefundApply listRefunds;
		private System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserRefundApply.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.txtOrderId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOrderId");
			this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
			this.listRefunds = (Common_OrderManage_RefundApply)this.FindControl("Common_OrderManage_RefundApply");
			this.ddlHandleStatus = (System.Web.UI.WebControls.DropDownList)this.FindControl("ddlHandleStatus");
			this.pager = (Pager)this.FindControl("pager");
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindRefund();
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadRefunds(true);
		}
		private void BindRefund()
		{
			RefundApplyQuery refundQuery = this.GetRefundQuery();
			DbQueryResult refundApplys = TradeHelper.GetRefundApplys(refundQuery, HiContext.Current.User.UserId);
			this.listRefunds.DataSource = refundApplys.Data;
			this.listRefunds.DataBind();
            this.pager.TotalRecords = refundApplys.TotalRecords;
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
			NameValueCollection nameValueCollection = new NameValueCollection();
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
	}
}
