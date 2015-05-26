using ASPNET.WebControls;
using Hidistro.AccountCenter.Comments;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Favorites : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtKeyWord;
		private Common_Favorite_ProductList favorites;
		private Pager pager;
		private IButton btnSearch;
		private System.Web.UI.WebControls.LinkButton btnDeleteSelect;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-Favorites.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.favorites = (Common_Favorite_ProductList)this.FindControl("list_Common_Favorite_ProList");
			this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
			this.txtKeyWord = (System.Web.UI.WebControls.TextBox)this.FindControl("txtKeyWord");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = (System.Web.UI.WebControls.LinkButton)this.FindControl("btnDeleteSelect");
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.favorites.ItemCommand += new Common_Favorite_ProductList.CommandEventHandler(this.favorites_ItemCommand);
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			PageTitle.AddSiteNameTitle("商品收藏夹", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductId"]))
				{
					int productId = 0;
					int.TryParse(this.Page.Request.QueryString["ProductId"], out productId);
					if (!CommentsHelper.ExistsProduct(productId) && !CommentsHelper.AddProductToFavorite(productId))
					{
						this.ShowMessage("添加商品到收藏夹失败", false);
					}
				}
				this.BindList();
			}
		}
		protected void favorites_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			int favoriteId = (int)this.favorites.DataKeys[e.Item.ItemIndex];
			if (e.CommandName == "Edit")
			{
				this.favorites.EditItemIndex = e.Item.ItemIndex;
				this.BindList();
			}
			if (e.CommandName == "Cancel")
			{
				this.favorites.EditItemIndex = -1;
				this.BindList();
			}
			if (e.CommandName == "Update")
			{
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtTags");
				System.Web.UI.WebControls.TextBox textBox2 = (System.Web.UI.WebControls.TextBox)e.Item.FindControl("txtRemark");
				if (textBox.Text.Length > 100)
				{
					this.ShowMessage("修改商品收藏信息失败，标签信息的长度限制在100个字符以內", false);
					return;
				}
				if (textBox2.Text.Length > 500)
				{
					this.ShowMessage("修改商品收藏信息失败，备注信息的长度限制在500個字符以內", false);
					return;
				}
				if (CommentsHelper.UpdateFavorite(favoriteId, Globals.HtmlEncode(textBox.Text.Trim()), Globals.HtmlEncode(textBox2.Text.Trim())) > 0)
				{
					this.favorites.EditItemIndex = -1;
					this.BindList();
					this.ShowMessage("成功的修改了收藏夹的信息", true);
				}
				else
				{
					this.ShowMessage("没有修改你要修改的內容", false);
				}
			}
			if (e.CommandName == "Deleted")
			{
				if (CommentsHelper.DeleteFavorite(favoriteId) > 0)
				{
					this.BindList();
					this.ShowMessage("成功删除了选择的收藏商品", true);
				}
				else
				{
					this.ShowMessage("删除失败", false);
				}
			}
		}
		protected void btnDeleteSelect_Click(object sender, System.EventArgs e)
		{
			string string_ = this.Page.Request["CheckboxGroup"];
			if (!CommentsHelper.DeleteFavorites(string_))
			{
				this.ShowMessage("删除失败", false);
			}
			else
			{
				this.BindList();
			}
		}
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadFavorites();
		}
		private void BindList()
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = this.pager.PageSize;
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
			{
				text = this.Page.Request.QueryString["keyword"];
			}
			DbQueryResult dbQueryResult = CommentsHelper.GetFavorites(text, pagination);
			this.favorites.DataSource = dbQueryResult.Data;
			this.favorites.DataBind();
			this.txtKeyWord.Text = text;
            this.pager.TotalRecords = dbQueryResult.TotalRecords;
		}
		private void ReloadFavorites()
		{
			base.ReloadPage(new NameValueCollection
			{

				{
					"keyword",
					this.txtKeyWord.Text.Trim()
				}
			});
		}
	}
}
