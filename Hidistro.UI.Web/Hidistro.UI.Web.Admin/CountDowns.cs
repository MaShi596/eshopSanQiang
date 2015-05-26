using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.CountDown)]
	public class CountDowns : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.WebControls.LinkButton btnOrder;
		protected Grid grdCountDownsList;
		protected Pager pager1;
		private string productName = string.Empty;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdCountDownsList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdGroupBuyList_RowDeleting);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindCountDown();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void BindCountDown()
		{
			DbQueryResult countDownList = PromoteHelper.GetCountDownList(new GroupBuyQuery
			{
				ProductName = this.productName,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc
			});
			this.grdCountDownsList.DataSource = countDownList.Data;
			this.grdCountDownsList.DataBind();
            this.pager.TotalRecords = countDownList.TotalRecords;
            this.pager1.TotalRecords = countDownList.TotalRecords;
		}
		private void btnOrder_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCountDownsList.Rows)
			{
				int displaySequence = 0;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out displaySequence))
				{
					int countDownId = (int)this.grdCountDownsList.DataKeys[gridViewRow.RowIndex].Value;
					PromoteHelper.SwapCountDownSequence(countDownId, displaySequence);
				}
			}
			this.BindCountDown();
		}
		private void grdGroupBuyList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (PromoteHelper.DeleteCountDown((int)this.grdCountDownsList.DataKeys[e.RowIndex].Value))
			{
				this.BindCountDown();
				this.ShowMsg("成功删除了选择的限时抢购活动", true);
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtProductName.Text.Trim()));
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
			nameValueCollection.Add("SortBy", this.grdCountDownsList.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadHelpList(true);
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCountDownsList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int countDownId = System.Convert.ToInt32(this.grdCountDownsList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
					PromoteHelper.DeleteCountDown(countDownId);
				}
			}
			if (num != 0)
			{
				this.BindCountDown();
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除\"{0}\"条限时抢购活动", new object[]
				{
					num
				}), true);
				return;
			}
			this.ShowMsg("请先选择需要删除的限时抢购活动", false);
		}
		private void LoadParameters()
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
				{
					this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
				}
				this.txtProductName.Text = this.productName;
				return;
			}
			this.productName = this.txtProductName.Text;
		}
	}
}
