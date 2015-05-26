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
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Articles)]
	public class ArticleList : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtKeywords;
		protected ArticleCategoryDropDownList dropArticleCategory;
		protected WebCalendar calendarStartDataTime;
		protected WebCalendar calendarEndDataTime;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected Grid grdArticleList;
		protected Pager pager;
		private string keywords = string.Empty;
		private int? categoryId;
		private System.DateTime? startArticleTime;
		private System.DateTime? endArticleTime;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdArticleList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdArticleList_RowDeleting);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
            this.grdArticleList.ReBindData += new Grid.ReBindDataEventHandler(this.grdArticleList_ReBindData);
			this.grdArticleList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdArticleList_RowCommand);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.dropArticleCategory.DataBind();
				this.dropArticleCategory.SelectedValue = this.categoryId;
				this.BindSearch();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void grdArticleList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Release")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int articId = (int)this.grdArticleList.DataKeys[rowIndex].Value;
				bool release = false;
				string str = "取消";
				if (e.CommandArgument.ToString().ToLower() == "false")
				{
					release = true;
					str = "发布";
				}
				if (ArticleHelper.UpdateRelease(articId, release))
				{
					this.ShowMsg(str + "当前文章成功！", true);
				}
				else
				{
					this.ShowMsg(str + "当前文章失败！", false);
				}
				this.ReloadActicleList(false);
			}
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdArticleList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int item = System.Convert.ToInt32(this.grdArticleList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
					list.Add(item);
				}
			}
			if (num != 0)
			{
				int num2 = ArticleHelper.DeleteArticles(list);
				this.BindSearch();
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除{0}篇文章", new object[]
				{
					num2
				}), true);
				return;
			}
			this.ShowMsg("请先选择需要删除的文章", false);
		}
		private void grdArticleList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int articleId = (int)this.grdArticleList.DataKeys[e.RowIndex].Value;
			if (ArticleHelper.DeleteArticle(articleId))
			{
				this.BindSearch();
				this.ShowMsg("成功删除了一篇文章", true);
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void grdArticleList_ReBindData(object sender)
		{
			this.BindSearch();
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadActicleList(true);
		}
		private void BindSearch()
		{
			DbQueryResult articleList = ArticleHelper.GetArticleList(new ArticleQuery
			{
				StartArticleTime = this.startArticleTime,
				EndArticleTime = this.endArticleTime,
				Keywords = Globals.HtmlEncode(this.keywords),
				CategoryId = this.categoryId,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = this.grdArticleList.SortOrderBy,
				SortOrder = SortAction.Desc
			});
			this.grdArticleList.DataSource = articleList.Data;
			this.grdArticleList.DataBind();
            this.pager.TotalRecords = articleList.TotalRecords;
            this.pager1.TotalRecords = articleList.TotalRecords;
		}
		private void ReloadActicleList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Keywords", Globals.UrlEncode(this.txtKeywords.Text.Trim()));
			nameValueCollection.Add("CategoryId", this.dropArticleCategory.SelectedValue.ToString());
			if (this.calendarStartDataTime.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartArticleTime", this.calendarStartDataTime.SelectedDate.ToString());
			}
			if (this.calendarEndDataTime.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndArticleTime", this.calendarEndDataTime.SelectedDate.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			nameValueCollection.Add("SortBy", this.grdArticleList.SortOrderBy);
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
				if (System.DateTime.TryParse(this.Page.Request.QueryString["StartArticleTime"], out now))
				{
					this.startArticleTime = new System.DateTime?(now);
				}
				System.DateTime now2 = System.DateTime.Now;
				if (System.DateTime.TryParse(this.Page.Request.QueryString["EndArticleTime"], out now2))
				{
					this.endArticleTime = new System.DateTime?(now2);
				}
				this.txtKeywords.Text = this.keywords;
                this.calendarStartDataTime.SelectedDate = this.startArticleTime;
                this.calendarEndDataTime.SelectedDate = this.endArticleTime;
				return;
			}
			this.keywords = this.txtKeywords.Text;
			this.startArticleTime = this.calendarStartDataTime.SelectedDate;
			this.endArticleTime = this.calendarEndDataTime.SelectedDate;
		}
	}
}
