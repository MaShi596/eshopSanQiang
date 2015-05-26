using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin.sales
{
	public class RefundApply : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstRefund;
		protected Pager pager;
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected System.Web.UI.WebControls.Label lblOrderTotal;
		protected System.Web.UI.WebControls.Label lblContacts;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Label lblTelephone;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.Label lblRefundType;
		protected System.Web.UI.WebControls.Label lblRefundRemark;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtAdminRemark;
		protected System.Web.UI.WebControls.Button btnAcceptRefund;
		protected System.Web.UI.WebControls.Button btnRefuseRefund;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;
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
			string text2 = "成功删除了{0}个退款申请单";
			int num;
			if (SubsiteSalesHelper.DelRefundApply(text.Split(new char[]
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
			this.BindRefund();
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
			DbQueryResult refundApplys = SubsiteSalesHelper.GetRefundApplys(refundQuery);
			this.dlstRefund.DataSource = refundApplys.Data;
			this.dlstRefund.DataBind();
			this.pager.TotalRecords=refundApplys.TotalRecords;
			this.pager1.TotalRecords=refundApplys.TotalRecords;
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
            OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
            if (SubsiteSalesHelper.CheckRefund(orderInfo, this.txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
            {
                this.BindRefund();
                orderInfo.GetTotal();
                if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Failed))
                {
                    decimal decimal1 = orderInfo.GetTotal() - orderInfo.NeedPrice;
                }
                this.ShowMsg("成功的确认了订单退款", true);
            }
		}
		private void btnRefuseRefund_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			SubsiteSalesHelper.CheckRefund(orderInfo, this.txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindRefund();
			this.ShowMsg("成功的拒绝了订单退款", true);
		}
	}
}
