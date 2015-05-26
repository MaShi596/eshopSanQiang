using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PurchaseOrderReplaceApply)]
	public class ReplacePurchaseOrderApply : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPurchaseOrderId;
		protected System.Web.UI.WebControls.DataList dlstReplace;
		protected Pager pager;
		protected System.Web.UI.WebControls.Label replace_lblOrderId;
		protected System.Web.UI.WebControls.Label replace_lblOrderTotal;
		protected System.Web.UI.WebControls.Label replace_lblComments;
		protected System.Web.UI.WebControls.Label replace_lblContacts;
		protected System.Web.UI.WebControls.Label replace_lblEmail;
		protected System.Web.UI.WebControls.Label replace_lblTelephone;
		protected System.Web.UI.WebControls.Label replace_lblAddress;
		protected System.Web.UI.WebControls.Label replace_lblPostCode;
		protected System.Web.UI.WebControls.TextBox replace_txtAdminRemark;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;
		protected System.Web.UI.WebControls.Button btnAcceptReplace;
		protected System.Web.UI.WebControls.Button btnRefuseReplace;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstReplace.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstReplace_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptReplace.Click += new System.EventHandler(this.btnAcceptReplace_Click);
			this.btnRefuseReplace.Click += new System.EventHandler(this.btnRefuseReplace_Click);
			if (!base.IsPostBack)
			{
				this.BindReplace();
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
				this.ShowMsg("请选要删除的换货申请单", false);
				return;
			}
			string text2 = "成功删除了{0}个换货申请单";
			int num;
			if (SalesHelper.DelPurchaseReplaceApply(text.Split(new char[]
			{
				','
			}), out num))
			{
				text2 = string.Format(text2, num);
			}
			else
			{
				text2 = string.Format(text2, num) + ",待处理的申请不能删除";
			}
			this.BindReplace();
			this.ShowMsg(text2, true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadReplaces(true);
		}
		private void dlstReplace_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
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
		private void BindReplace()
		{
			ReplaceApplyQuery replaceQuery = this.GetReplaceQuery();
			DbQueryResult purchaseReplaceApplys = SalesHelper.GetPurchaseReplaceApplys(replaceQuery);
			this.dlstReplace.DataSource = purchaseReplaceApplys.Data;
			this.dlstReplace.DataBind();
            this.pager.TotalRecords = purchaseReplaceApplys.TotalRecords;
            this.pager1.TotalRecords = purchaseReplaceApplys.TotalRecords;
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
		private void ReloadReplaces(bool isSearch)
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
		protected void btnAcceptReplace_Click(object sender, System.EventArgs e)
		{
			SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, true);
			this.BindReplace();
			this.ShowMsg("成功的确认了采购单换货", true);
		}
		private void btnRefuseReplace_Click(object sender, System.EventArgs e)
		{
			SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, false);
			this.BindReplace();
			this.ShowMsg("成功的拒绝了采购单换货", true);
		}
	}
}
