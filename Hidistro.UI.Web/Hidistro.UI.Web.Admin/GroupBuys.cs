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
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.GroupBuy)]
	public class GroupBuys : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.WebControls.LinkButton btnOrder;
		protected Grid grdGroupBuyList;
		protected Pager pager1;
		private string productName = string.Empty;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
			this.grdGroupBuyList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdGroupBuyList_RowDeleting);
			this.grdGroupBuyList.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdGroupBuyList_RowDataBound);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindGroupBuy();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnOrder_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdGroupBuyList.Rows)
			{
				int displaySequence = 0;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out displaySequence))
				{
					int groupBuyId = (int)this.grdGroupBuyList.DataKeys[gridViewRow.RowIndex].Value;
					PromoteHelper.SwapGroupBuySequence(groupBuyId, displaySequence);
				}
			}
			this.BindGroupBuy();
		}
		private void BindGroupBuy()
		{
			DbQueryResult groupBuyList = PromoteHelper.GetGroupBuyList(new GroupBuyQuery
			{
				ProductName = this.productName,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc
			});
			this.grdGroupBuyList.DataSource = groupBuyList.Data;
			this.grdGroupBuyList.DataBind();
            this.pager.TotalRecords = groupBuyList.TotalRecords;
            this.pager1.TotalRecords = groupBuyList.TotalRecords;
		}
		private void grdGroupBuyList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				FormatedMoneyLabel formatedMoneyLabel = (FormatedMoneyLabel)e.Row.FindControl("lblCurrentPrice");
				int groupBuyId = System.Convert.ToInt32(this.grdGroupBuyList.DataKeys[e.Row.RowIndex].Value.ToString());
				int prodcutQuantity = int.Parse(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ProdcutQuantity").ToString());
				formatedMoneyLabel.Money = PromoteHelper.GetCurrentPrice(groupBuyId, prodcutQuantity);
			}
		}
		private void grdGroupBuyList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy((int)this.grdGroupBuyList.DataKeys[e.RowIndex].Value);
			if (groupBuy.Status != GroupBuyStatus.UnderWay)
			{
				if (groupBuy.Status != GroupBuyStatus.EndUntreated)
				{
					if (PromoteHelper.DeleteGroupBuy((int)this.grdGroupBuyList.DataKeys[e.RowIndex].Value))
					{
						this.BindGroupBuy();
						this.ShowMsg("成功删除了选择的团购活动", true);
						return;
					}
					this.ShowMsg("删除失败", false);
					return;
				}
			}
			this.ShowMsg("团购活动正在进行中或结束未处理，不允许删除", false);
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
			nameValueCollection.Add("SortBy", this.grdGroupBuyList.SortOrderBy);
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
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdGroupBuyList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int groupBuyId = System.Convert.ToInt32(this.grdGroupBuyList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
					PromoteHelper.DeleteGroupBuy(groupBuyId);
				}
			}
			if (num != 0)
			{
				this.BindGroupBuy();
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除\"{0}\"条团购活动", new object[]
				{
					num
				}), true);
				return;
			}
			this.ShowMsg("请先选择需要删除的团购活动", false);
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
