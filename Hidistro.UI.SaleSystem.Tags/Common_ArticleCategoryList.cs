using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ArticleCategoryList : ThemedTemplatedRepeater
	{
		public const string TagID = "list_Common_ArticleCategory";
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public int MaxNum
		{
			get;
			set;
		}
		public Common_ArticleCategoryList()
		{
			base.ID = "list_Common_ArticleCategory";
		}
		protected override void OnLoad(EventArgs eventArgs_0)
		{
			if (!this.Page.IsPostBack)
			{
				base.DataSource = this.GetDataSource();
				base.DataBind();
			}
		}
		private IList<ArticleCategoryInfo> GetDataSource()
		{
			IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
			if (this.MaxNum > 0 && this.MaxNum < articleMainCategories.Count)
			{
				for (int i = articleMainCategories.Count - 1; i >= this.MaxNum; i--)
				{
					articleMainCategories.RemoveAt(i);
				}
			}
			return articleMainCategories;
		}
	}
}
