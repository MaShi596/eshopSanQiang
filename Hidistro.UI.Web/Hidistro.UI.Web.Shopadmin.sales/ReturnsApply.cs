using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
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
	public class ReturnsApply : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.Repeater dlstReturns;
		protected Pager pager;
		protected System.Web.UI.WebControls.Label return_lblOrderId;
		protected System.Web.UI.WebControls.Label return_lblOrderTotal;
		protected System.Web.UI.WebControls.Label return_lblContacts;
		protected System.Web.UI.WebControls.Label return_lblEmail;
		protected System.Web.UI.WebControls.Label return_lblTelephone;
		protected System.Web.UI.WebControls.Label return_lblAddress;
		protected System.Web.UI.WebControls.Label return_lblRefundType;
		protected System.Web.UI.WebControls.Label return_lblReturnRemark;
		protected System.Web.UI.WebControls.TextBox return_txtRefundMoney;
		protected System.Web.UI.HtmlControls.HtmlTextArea return_txtAdminRemark;
		protected System.Web.UI.WebControls.Button btnAcceptReturn;
		protected System.Web.UI.WebControls.Button btnRefuseReturn;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstReturns.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.dlstReturns_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptReturn.Click += new System.EventHandler(this.btnAcceptReturn_Click);
			this.btnRefuseReturn.Click += new System.EventHandler(this.btnRefuseReturn_Click);
			if (!base.IsPostBack)
			{
				this.BindReturns();
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
				this.ShowMsg("请选要删除的退货申请单", false);
				return;
			}
			string text2 = "成功删除了{0}个退货申请单";
			int num;
			if (SubsiteSalesHelper.DelReturnsApply(text.Split(new char[]
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
			this.BindReturns();
			this.ShowMsg(text2, true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadReturns(true);
		}
		private void dlstReturns_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturns");
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
		private void BindReturns()
		{
			ReturnsApplyQuery returnsQuery = this.GetReturnsQuery();
			DbQueryResult returnsApplys = SubsiteSalesHelper.GetReturnsApplys(returnsQuery);
			this.dlstReturns.DataSource = returnsApplys.Data;
			this.dlstReturns.DataBind();
			this.pager.TotalRecords=returnsApplys.TotalRecords;
			this.pager1.TotalRecords=returnsApplys.TotalRecords;
			this.txtOrderId.Text = returnsQuery.OrderId;
			this.ddlHandleStatus.SelectedIndex = 0;
			if (returnsQuery.HandleStatus.HasValue && returnsQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = returnsQuery.HandleStatus.Value.ToString();
			}
		}
		private ReturnsApplyQuery GetReturnsQuery()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					returnsApplyQuery.HandleStatus = new int?(num);
				}
			}
			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			return returnsApplyQuery;
		}
		private void ReloadReturns(bool isSearch)
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
		protected void btnRefuseReturn_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			SubsiteSalesHelper.CheckReturn(orderInfo, 0m, this.return_txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindReturns();
			this.ShowMsg("成功的拒绝了订单退货", true);
		}
		protected void btnAcceptReturn_Click(object sender, System.EventArgs e)
		{
			decimal num;
			if (!decimal.TryParse(this.return_txtRefundMoney.Text, out num))
			{
				this.ShowMsg("退款金额需为数字格式", false);
				return;
			}
			decimal d;
			decimal.TryParse(this.hidOrderTotal.Value, out d);
			if (num > d)
			{
				this.ShowMsg("退款金额不能大于订单金额", false);
				return;
			}
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			SubsiteSalesHelper.CheckReturn(orderInfo, num, this.return_txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
			this.BindReturns();
			this.ShowMsg("成功的确认了订单退货", true);
		}
	}
}
