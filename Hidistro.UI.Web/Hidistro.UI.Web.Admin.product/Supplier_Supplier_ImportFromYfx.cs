using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin.product
{
	public class Supplier_Supplier_ImportFromYfx : AdminPage
	{
		protected System.Web.UI.WebControls.DropDownList dropImportVersions;
		protected System.Web.UI.WebControls.DropDownList dropFiles;
		protected System.Web.UI.WebControls.FileUpload fileUploader;
		protected System.Web.UI.WebControls.Button btnUpload;
		protected System.Web.UI.WebControls.TextBox txtProductTypeXml;
		protected System.Web.UI.WebControls.TextBox txtPTXml;
		protected System.Web.UI.WebControls.CheckBox chkFlag;
		protected System.Web.UI.WebControls.Literal lblVersion;
		protected System.Web.UI.WebControls.Literal lblQuantity;
		protected System.Web.UI.WebControls.CheckBox chkIncludeCostPrice;
		protected System.Web.UI.WebControls.CheckBox chkIncludeStock;
		protected System.Web.UI.WebControls.CheckBox chkIncludeImages;
		protected ProductCategoriesDropDownList dropCategories;
		protected ProductLineDropDownList dropProductLines;
		protected BrandCategoriesDropDownList dropBrandList;
		protected System.Web.UI.WebControls.RadioButton radOnSales;
		protected System.Web.UI.WebControls.RadioButton radUnSales;
		protected System.Web.UI.WebControls.RadioButton radInStock;
		protected System.Web.UI.WebControls.TextBox txtMappedTypes;
		protected System.Web.UI.WebControls.Button btnImport;
		private string _dataPath;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			if (base.Request.QueryString["isCallback"] == "true")
			{
				this.DoCallback();
				return;
			}
			this._dataPath = this.Page.Request.MapPath(string.Format("~/storage/Cpage/Supplier/{0}/yfx", Hidistro.Membership.Context.HiContext.Current.User.UserId));
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			this.dropFiles.SelectedIndexChanged += new System.EventHandler(this.dropFiles_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropProductLines.DataBind();
				this.dropBrandList.DataBind();
				this.BindImporters();
				this.BindFiles();
				this.OutputProductTypes();
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		private void BindImporters()
		{
			this.dropImportVersions.Items.Clear();
			this.dropImportVersions.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.Collections.Generic.Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "分销商城");
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
				string fileName = System.IO.Path.GetFileName(this.fileUploader.PostedFile.FileName);
				this.fileUploader.PostedFile.SaveAs(System.IO.Path.Combine(this._dataPath, fileName));
				this.BindFiles();
				this.dropFiles.SelectedValue = fileName;
				this.PrepareZipFile(fileName);
				return;
			}
			this.ShowMsg("请上传正确的数据包文件", false);
		}
		private void dropFiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.dropFiles.SelectedValue.Length > 0 && this.dropImportVersions.SelectedValue.Length == 0)
			{
				this.ShowMsg("请先选择一个导入插件", false);
				this.dropFiles.SelectedValue = "";
				return;
			}
			this.PrepareZipFile(this.dropFiles.SelectedValue);
		}
		private void btnImport_Click(object sender, System.EventArgs e)
		{
			if (!this.CheckItems())
			{
				return;
			}
			string selectedValue = this.dropFiles.SelectedValue;
			string text = System.IO.Path.Combine(this._dataPath, System.IO.Path.GetFileNameWithoutExtension(selectedValue));
			ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue, new object[0]);
			text = text.ToLower().Replace("cpage\\supplier\\" + Hidistro.Membership.Context.HiContext.Current.User.UserId, "data");
			System.Data.DataSet dataSet = null;
			if (this.txtMappedTypes.Text.Length > 0)
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.LoadXml(this.txtMappedTypes.Text);
				dataSet = (importer.CreateMapping(new object[]
				{
					xmlDocument,
					text
				})[0] as System.Data.DataSet);
				ProductHelper.EnsureMapping(dataSet);
			}
			bool @checked = this.chkIncludeCostPrice.Checked;
			bool checked2 = this.chkIncludeStock.Checked;
			bool checked3 = this.chkIncludeImages.Checked;
			int value = this.dropCategories.SelectedValue.Value;
			int value2 = this.dropProductLines.SelectedValue.Value;
			int? selectedValue2 = this.dropBrandList.SelectedValue;
			object[] array = importer.ParseProductData(new object[]
			{
				dataSet,
				text,
				@checked,
				checked2,
				checked3
			});
			Methods.Supplier_PtImportProducts((System.Data.DataSet)array[0], value, value2, selectedValue2, ProductSaleStatus.OnStock, @checked, checked2, checked3);
			System.IO.File.Delete(System.IO.Path.Combine(this._dataPath, selectedValue));
			System.IO.Directory.Delete(text, true);
			this.chkFlag.Checked = false;
			this.txtMappedTypes.Text = string.Empty;
			this.txtProductTypeXml.Text = string.Empty;
			this.txtPTXml.Text = string.Empty;
			this.OutputProductTypes();
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
		private void PrepareZipFile(string filename)
		{
			if (string.IsNullOrEmpty(filename) || filename.Length == 0)
			{
				this.chkFlag.Checked = false;
				this.txtPTXml.Text = string.Empty;
				return;
			}
			filename = System.IO.Path.Combine(this._dataPath, filename);
			if (!System.IO.File.Exists(filename))
			{
				this.chkFlag.Checked = false;
				this.txtPTXml.Text = string.Empty;
				return;
			}
			ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue, new object[0]);
			string text = importer.PrepareDataFiles(new object[]
			{
				filename
			});
			object[] array = importer.ParseIndexes(new object[]
			{
				text
			});
			this.lblVersion.Text = (string)array[0];
			this.lblQuantity.Text = array[1].ToString();
			this.chkIncludeCostPrice.Checked = (bool)array[2];
			this.chkIncludeStock.Checked = (bool)array[3];
			this.chkIncludeImages.Checked = (bool)array[4];
			this.txtPTXml.Text = (string)array[5];
			this.chkFlag.Checked = true;
		}
		private void OutputProductTypes()
		{
			System.Collections.Generic.IList<ProductTypeInfo> productTypes = ControlProvider.Instance().GetProductTypes();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<xml><types>");
			foreach (ProductTypeInfo current in productTypes)
			{
				stringBuilder.Append("<item typeId=\"").Append(current.TypeId.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" typeName=\"").Append(current.TypeName).Append("\" />");
			}
			stringBuilder.Append("</types></xml>");
			this.txtProductTypeXml.Text = stringBuilder.ToString();
		}
		private void DoCallback()
		{
			base.Response.Clear();
			base.Response.ContentType = "text/xml";
			string a = base.Request.QueryString["action"];
			if (a == "getAttributes")
			{
				System.Collections.Generic.IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(int.Parse(base.Request.QueryString["typeId"]));
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.Append("<xml><attributes>");
				foreach (AttributeInfo current in attributes)
				{
					stringBuilder.Append("<item attributeId=\"").Append(current.AttributeId.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" attributeName=\"").Append(current.AttributeName).Append("\" typeId=\"").Append(current.TypeId.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" />");
				}
				stringBuilder.Append("</attributes></xml>");
				base.Response.Write(stringBuilder.ToString());
			}
			if (a == "getValues")
			{
				AttributeInfo attribute = ProductTypeHelper.GetAttribute(int.Parse(base.Request.QueryString["attributeId"]));
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				stringBuilder2.Append("<xml><values>");
				if (attribute != null && attribute.AttributeValues.Count > 0)
				{
					foreach (AttributeValueInfo current2 in attribute.AttributeValues)
					{
						stringBuilder2.Append("<item valueId=\"").Append(current2.ValueId.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" valueStr=\"").Append(current2.ValueStr).Append("\" attributeId=\"").Append(current2.AttributeId.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" />");
					}
				}
				stringBuilder2.Append("</values></xml>");
				base.Response.Write(stringBuilder2.ToString());
			}
			base.Response.End();
		}
	}
}
