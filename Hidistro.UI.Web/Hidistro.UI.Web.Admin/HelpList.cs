using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Helps)]
	public class HelpList : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtkeyWords;
		protected HelpCategoryDropDownList dropHelpCategory;
		protected WebCalendar calendarStartDataTime;
		protected WebCalendar calendarEndDataTime;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected Grid grdHelpList;
		protected Pager pager1;
		private string keywords = string.Empty;
		private int? categoryId;
		private System.DateTime? startTime;
		private System.DateTime? endTime;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdHelpList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdHelpList_RowDeleting);
            this.grdHelpList.ReBindData += new Grid.ReBindDataEventHandler(this.grdHelpList_ReBindData);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.dropHelpCategory.DataBind();
				this.dropHelpCategory.SelectedValue = this.categoryId;
				this.BindSearch();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void grdHelpList_ReBindData(object sender)
		{
			this.BindSearch();
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadHelpList(true);
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdHelpList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int item = System.Convert.ToInt32(this.grdHelpList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
					list.Add(item);
				}
			}
			if (num != 0)
			{
				int num2 = ArticleHelper.DeleteHelps(list);
				this.BindSearch();
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除\"{0}\"篇帮助", new object[]
				{
					num2
				}), true);
				return;
			}
			this.ShowMsg("请先选择需要删除的帮助", false);
		}
		private void grdHelpList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (ArticleHelper.DeleteHelp((int)this.grdHelpList.DataKeys[e.RowIndex].Value))
			{
				this.BindSearch();
				this.ShowMsg("成功删除了选择的帮助", true);
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void BindSearch()
		{
			DbQueryResult helpList = ArticleHelper.GetHelpList(new HelpQuery
			{
				StartArticleTime = this.startTime,
				EndArticleTime = this.endTime,
				Keywords = Globals.HtmlEncode(this.keywords),
				CategoryId = this.categoryId,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "AddedDate",
				SortOrder = SortAction.Desc
			});
			this.grdHelpList.DataSource = helpList.Data;
			this.grdHelpList.DataBind();
            this.pager.TotalRecords = helpList.TotalRecords;
            this.pager1.TotalRecords = helpList.TotalRecords;
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Keywords", this.txtkeyWords.Text.Trim());
			nameValueCollection.Add("CategoryId", this.dropHelpCategory.SelectedValue.ToString());
			if (this.calendarStartDataTime.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartTime", this.calendarStartDataTime.SelectedDate.ToString());
			}
			if (this.calendarEndDataTime.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndTime", this.calendarEndDataTime.SelectedDate.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
			nameValueCollection.Add("SortBy", this.grdHelpList.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
				{
					this.keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
				}
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					this.categoryId = new int?(value);
				}
				System.DateTime now = System.DateTime.Now;
				if (System.DateTime.TryParse(this.Page.Request.QueryString["StartTime"], out now))
				{
					this.startTime = new System.DateTime?(now);
				}
				System.DateTime now2 = System.DateTime.Now;
				if (System.DateTime.TryParse(this.Page.Request.QueryString["EndTime"], out now2))
				{
					this.endTime = new System.DateTime?(now2);
				}
				this.txtkeyWords.Text = this.keywords;
                this.calendarStartDataTime.SelectedDate = this.startTime;
                this.calendarEndDataTime.SelectedDate = this.endTime;
				return;
			}
			this.keywords = this.txtkeyWords.Text;
			this.startTime = this.calendarStartDataTime.SelectedDate;
			this.endTime = this.calendarEndDataTime.SelectedDate;
		}
	}
}
