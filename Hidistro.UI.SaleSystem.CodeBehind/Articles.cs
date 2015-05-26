using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Articles : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptArticles;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Articles.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rptArticles = (ThemedTemplatedRepeater)this.FindControl("rptArticles");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
				{
					int categoryId = 0;
					int.TryParse(this.Page.Request.QueryString["CategoryId"], out categoryId);
					ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(categoryId);
					if (articleCategory != null)
					{
						PageTitle.AddSiteNameTitle(articleCategory.Name, Hidistro.Membership.Context.HiContext.Current.Context);
					}
				}
				else
				{
					PageTitle.AddSiteNameTitle("文章中心", Hidistro.Membership.Context.HiContext.Current.Context);
				}
				this.BindList();
			}
		}
		private void BindList()
		{
			ArticleQuery articleQuery = new ArticleQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					articleQuery.CategoryId = new int?(value);
				}
			}
			articleQuery.PageIndex = this.pager.PageIndex;
			articleQuery.PageSize = this.pager.PageSize;
			articleQuery.SortBy = "AddedDate";
			articleQuery.SortOrder = SortAction.Desc;
			DbQueryResult articleList = CommentBrowser.GetArticleList(articleQuery);
			this.rptArticles.DataSource = articleList.Data;
			this.rptArticles.DataBind();
            this.pager.TotalRecords = articleList.TotalRecords;
		}
	}
}
