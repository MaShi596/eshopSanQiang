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
	public class UserReplaceApply : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtOrderId;
		private IButton btnSearch;
		private Common_OrderManage_ReplaceApply listReplace;
		private System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReplaceApply.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.txtOrderId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOrderId");
			this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
			this.listReplace = (Common_OrderManage_ReplaceApply)this.FindControl("Common_OrderManage_ReplaceApply");
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
			this.ReloadReplace(true);
		}
		private void BindRefund()
		{
			ReplaceApplyQuery replaceQuery = this.GetReplaceQuery();
			DbQueryResult replaceApplys = TradeHelper.GetReplaceApplys(replaceQuery, HiContext.Current.User.UserId);
			this.listReplace.DataSource = replaceApplys.Data;
			this.listReplace.DataBind();
            this.pager.TotalRecords = replaceApplys.TotalRecords;
			this.txtOrderId.Text = replaceQuery.OrderId;
			this.ddlHandleStatus.SelectedIndex = 0;
			if (replaceQuery.HandleStatus.HasValue && replaceQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = replaceQuery.HandleStatus.Value.ToString();
			}
		}
		private ReplaceApplyQuery GetReplaceQuery()
		{
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				replaceApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					replaceApplyQuery.HandleStatus = new int?(num);
				}
			}
			replaceApplyQuery.PageIndex = this.pager.PageIndex;
			replaceApplyQuery.PageSize = this.pager.PageSize;
			replaceApplyQuery.SortBy = "ApplyForTime";
			replaceApplyQuery.SortOrder = SortAction.Desc;
			return replaceApplyQuery;
		}
		private void ReloadReplace(bool isSearch)
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
