using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class SetCategoryTemplate : AdminPage
	{
		protected System.Web.UI.WebControls.FileUpload fileThame;
		protected System.Web.UI.WebControls.Button btnUpload;
		protected System.Web.UI.WebControls.DropDownList dropThmes;
		protected ImageLinkButton btnDelete;
		protected Grid grdCategries;
		protected DropdownColumn dropTheme;
		protected System.Web.UI.WebControls.Button btnSaveAll;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdCategries.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdCategries_RowCommand);
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindDorpDown();
				this.BindData();
			}
		}
		private void btnUpload_Click(object sender, System.EventArgs e)
		{
			if (!this.fileThame.HasFile)
			{
				this.ShowMsg("上传失败！", false);
				return;
			}
			if (!this.fileThame.PostedFile.FileName.EndsWith(".htm") && !this.fileThame.PostedFile.FileName.EndsWith(".html"))
			{
				this.ShowMsg("请检查您上传文件的格式是否为html或htm", false);
				return;
			}
			string virtualPath = Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/categorythemes/" + SetCategoryTemplate.GetFilename(System.IO.Path.GetFileName(this.fileThame.PostedFile.FileName), System.IO.Path.GetExtension(this.fileThame.PostedFile.FileName));
			this.fileThame.PostedFile.SaveAs(Hidistro.Membership.Context.HiContext.Current.Context.Request.MapPath(virtualPath));
			this.BindDorpDown();
			this.BindData();
			this.ShowMsg("上传成功", true);
		}
		private static string GetFilename(string filename, string extension)
		{
			return filename.Substring(0, filename.IndexOf(".")) + extension;
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.dropThmes.SelectedValue))
			{
				this.ShowMsg("请选择您要删除的模板", false);
				return;
			}
			if (!this.validata(this.dropThmes.SelectedItem.Text))
			{
				this.ShowMsg("您要删除的模板正在被使用，不能删除", false);
				return;
			}
			string text = Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/categorythemes/" + this.dropThmes.SelectedValue;
			text = Hidistro.Membership.Context.HiContext.Current.Context.Request.MapPath(text);
			if (!System.IO.File.Exists(text))
			{
				this.ShowMsg(string.Format("删除失败!模板{0}已经不存在", this.dropThmes.SelectedValue), false);
				return;
			}
			System.IO.File.Delete(text);
			this.BindDorpDown();
			this.BindData();
			this.ShowMsg("删除模板成功", true);
		}
		private void btnSaveAll_Click(object sender, System.EventArgs e)
		{
			this.SaveAll();
			this.BindDorpDown();
			this.BindData();
			this.ShowMsg("批量保存分类模板成功", true);
		}
		private bool validata(string theme)
		{
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdCategries.Columns[1];
			System.Collections.IEnumerator enumerator = this.grdCategries.Rows.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)enumerator.Current;
					string a = dropdownColumn.SelectedValues[gridViewRow.RowIndex];
					if (a == theme)
					{
						result = false;
						return result;
					}
				}
				return true;
			}
			finally
			{
				System.IDisposable disposable = enumerator as System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return result;
		}
		private void SaveAll()
		{
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdCategries.Columns[1];
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCategries.Rows)
			{
				string themeName = dropdownColumn.SelectedValues[gridViewRow.RowIndex];
				int categoryId = (int)this.grdCategries.DataKeys[gridViewRow.RowIndex].Value;
				CatalogHelper.SetCategoryThemes(categoryId, themeName);
			}
		}
		private void grdCategries_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Save")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int categoryId = (int)this.grdCategries.DataKeys[rowIndex].Value;
				DropdownColumn dropdownColumn = (DropdownColumn)this.grdCategries.Columns[1];
				string themeName = dropdownColumn.SelectedValues[rowIndex];
				if (CatalogHelper.SetCategoryThemes(categoryId, themeName))
				{
					this.BindData();
					this.ShowMsg("保存分类模板成功", true);
				}
			}
		}
		private void BindDorpDown()
		{
			this.dropThmes.Items.Clear();
			this.dropThmes.Items.Add(new System.Web.UI.WebControls.ListItem("请选择分类模板文件", ""));
			System.Collections.Generic.IList<ManageThemeInfo> themes = this.GetThemes();
			foreach (ManageThemeInfo current in themes)
			{
				this.dropThmes.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.ThemeName));
			}
		}
		private void BindData()
		{
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdCategries.Columns[1];
			dropdownColumn.DataSource = this.GetThemes();
			this.grdCategries.DataSource = CatalogHelper.GetMainCategories();
			this.grdCategries.DataBind();
		}
		protected System.Collections.Generic.IList<ManageThemeInfo> GetThemes()
		{
			System.Web.HttpContext context = Hidistro.Membership.Context.HiContext.Current.Context;
			System.Collections.Generic.IList<ManageThemeInfo> list = new System.Collections.Generic.List<ManageThemeInfo>();
			string path = context.Request.MapPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/categorythemes");
			string[] array = System.IO.Directory.Exists(path) ? System.IO.Directory.GetFiles(path) : null;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (text.EndsWith(".html"))
				{
					ManageThemeInfo manageThemeInfo = new ManageThemeInfo();
					manageThemeInfo.ThemeName = (manageThemeInfo.Name = System.IO.Path.GetFileName(text));
					list.Add(manageThemeInfo);
				}
			}
			return list;
		}
	}
}
