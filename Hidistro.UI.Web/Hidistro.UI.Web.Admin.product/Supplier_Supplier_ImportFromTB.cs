using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
	public class Supplier_Supplier_ImportFromTB : AdminPage
	{
		private string _dataPath;
		protected System.Web.UI.WebControls.DropDownList dropImportVersions;
		protected System.Web.UI.WebControls.DropDownList dropFiles;
		protected System.Web.UI.WebControls.FileUpload fileUploader;
		protected System.Web.UI.WebControls.Button btnUpload;
		protected ProductCategoriesDropDownList dropCategories;
		protected ProductLineDropDownList dropProductLines;
		protected BrandCategoriesDropDownList dropBrandList;
		protected System.Web.UI.WebControls.RadioButton radOnSales;
		protected System.Web.UI.WebControls.RadioButton radUnSales;
		protected System.Web.UI.WebControls.RadioButton radInStock;
		protected System.Web.UI.WebControls.Button btnImport;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this._dataPath = this.Page.Request.MapPath(string.Format("~/storage/Cpage/Supplier/{0}/taobao", Hidistro.Membership.Context.HiContext.Current.User.UserId));
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropProductLines.DataBind();
				this.dropBrandList.DataBind();
				this.BindImporters();
				this.BindFiles();
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		private void BindImporters()
		{
			this.dropImportVersions.Items.Clear();
			this.dropImportVersions.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.Collections.Generic.Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "淘宝助理");
			foreach (string current in importAdapters.Keys)
			{
				this.dropImportVersions.Items.Add(new System.Web.UI.WebControls.ListItem(importAdapters[current], current));
			}
		}
		private void BindFiles()
		{
			if (!System.IO.Directory.Exists(this._dataPath))
			{
				System.IO.Directory.CreateDirectory(this._dataPath);
			}
			this.dropFiles.Items.Clear();
			this.dropFiles.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(this._dataPath);
			System.IO.FileInfo[] files = directoryInfo.GetFiles("*.zip", System.IO.SearchOption.TopDirectoryOnly);
			System.IO.FileInfo[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				System.IO.FileInfo fileInfo = array[i];
				string name = fileInfo.Name;
				this.dropFiles.Items.Add(new System.Web.UI.WebControls.ListItem(name, name));
			}
		}
		private void btnUpload_Click(object sender, System.EventArgs e)
		{
			if (this.dropImportVersions.SelectedValue.Length == 0)
			{
				this.ShowMsg("请先选择一个导入插件", false);
				return;
			}
			if (!this.fileUploader.HasFile)
			{
				this.ShowMsg("请先选择一个数据包文件", false);
				return;
			}
			if (this.fileUploader.PostedFile.ContentLength != 0 && (!(this.fileUploader.PostedFile.ContentType != "application/x-zip-compressed") || !(this.fileUploader.PostedFile.ContentType != "application/zip") || !(this.fileUploader.PostedFile.ContentType != "application/octet-stream")))
			{
				string text = System.IO.Path.GetFileName(this.fileUploader.PostedFile.FileName);
				text = text.ToLower().Replace(".zip", "_" + Hidistro.Membership.Context.HiContext.Current.User.UserId + ".zip");
				this.fileUploader.PostedFile.SaveAs(System.IO.Path.Combine(this._dataPath, text));
				this.BindFiles();
				this.dropFiles.SelectedValue = text;
				return;
			}
			this.ShowMsg("请上传正确的数据包文件", false);
		}
		private void btnImport_Click(object sender, System.EventArgs e)
		{
			if (!this.CheckItems())
			{
				return;
			}
			string text = this.dropFiles.SelectedValue;
			string text2 = System.IO.Path.Combine(this._dataPath, System.IO.Path.GetFileNameWithoutExtension(text));
			ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue, new object[0]);
			int value = this.dropCategories.SelectedValue.Value;
			int value2 = this.dropProductLines.SelectedValue.Value;
			int? selectedValue = this.dropBrandList.SelectedValue;
			ProductSaleStatus saleStatus = ProductSaleStatus.OnStock;
			text = System.IO.Path.Combine(this._dataPath, text);
			if (!System.IO.File.Exists(text))
			{
				this.ShowMsg("选择的数据包文件有问题！", false);
				return;
			}
			importer.PrepareDataFiles(new object[]
			{
				text
			});
			text2 = text2.ToLower().Replace("cpage\\supplier\\" + Hidistro.Membership.Context.HiContext.Current.User.UserId, "data");
			object[] array = importer.ParseProductData(new object[]
			{
				text2
			});
			Methods.Supplier_PtImportProducts((System.Data.DataTable)array[0], value, value2, selectedValue, saleStatus, true);
			System.IO.File.Delete(text);
			System.IO.Directory.Delete(text2, true);
			this.BindFiles();
			this.ShowMsg("此次商品批量导入操作已成功！", true);
		}
		private bool CheckItems()
		{
			string text = "";
			if (this.dropImportVersions.SelectedValue.Length == 0)
			{
				text += Formatter.FormatErrorMessage("请选择一个导入插件！");
			}
			if (this.dropFiles.SelectedValue.Length == 0)
			{
				text += Formatter.FormatErrorMessage("请选择要导入的数据包文件！");
			}
			if (!this.dropCategories.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择要导入的店铺分类！");
			}
			if (!this.dropProductLines.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择要导入的产品线！");
			}
			if (string.IsNullOrEmpty(text) && text.Length <= 0)
			{
				return true;
			}
			this.ShowMsg(text, false);
			return false;
		}
	}
}
