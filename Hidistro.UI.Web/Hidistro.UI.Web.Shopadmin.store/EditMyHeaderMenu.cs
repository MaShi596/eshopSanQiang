using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin.store
{
	public class EditMyHeaderMenu : DistributorPage
	{
		private string themName;
		private int id;
		private string title;
		private string category;
		private string url;
		private string where;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMenuNameTip;
		protected TrimTextBox txtMenuType;
		protected SystemPageDropDownList dropSystemPageDropDownList;
		protected System.Web.UI.WebControls.TextBox txtMinPrice;
		protected System.Web.UI.WebControls.TextBox txtMaxPrice;
		protected System.Web.UI.WebControls.TextBox txtKeyword;
		protected DistributorProductCategoriesListBox listProductCategories;
		protected BrandCategoriesList listBrandCategories;
		protected System.Web.UI.WebControls.ListBox radProductTags;
		protected System.Web.UI.WebControls.TextBox txtCustomLink;
		protected System.Web.UI.WebControls.Button btnSave;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["ThemName"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.themName = base.Request.QueryString["ThemName"];
			int.TryParse(base.Request.QueryString["Id"], out this.id);
			string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			System.Xml.XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (System.Xml.XmlNode xmlNode in childNodes)
			{
				if (xmlNode.Attributes["Id"].Value == this.id.ToString())
				{
					this.title = xmlNode.Attributes["Title"].Value;
					this.category = xmlNode.Attributes["Category"].Value;
					this.url = xmlNode.Attributes["Url"].Value;
					this.where = xmlNode.Attributes["Where"].Value;
					break;
				}
			}
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			if (!base.IsPostBack)
			{
				this.txtMenuType.Text = this.category;
				this.dropSystemPageDropDownList.DataBind();
				this.listProductCategories.DataBind();
				this.listBrandCategories.DataBind();
				this.radProductTags.DataSource = CatalogHelper.GetTags();
				this.radProductTags.DataTextField = "TagName";
				this.radProductTags.DataValueField = "TagID";
				this.radProductTags.DataBind();
				this.radProductTags.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--任意--", "0"));
				this.txtTitle.Text = this.title;
				if (this.category == "1")
				{
					this.dropSystemPageDropDownList.SelectedValue = this.url;
					return;
				}
				if (this.category == "3")
				{
					this.txtCustomLink.Text = this.url;
					return;
				}
				string[] array = this.where.Split(new char[]
				{
					','
				});
				int selectedCategoryId = 0;
				if (int.TryParse(array[0], out selectedCategoryId))
				{
					this.listProductCategories.SelectedCategoryId = selectedCategoryId;
				}
				this.listBrandCategories.SelectedValue = array[1];
				this.radProductTags.SelectedValue = array[2];
				this.txtMinPrice.Text = array[3];
				this.txtMaxPrice.Text = array[4];
				this.txtKeyword.Text = array[5];
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			System.Xml.XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (System.Xml.XmlNode xmlNode in childNodes)
			{
				if (xmlNode.Attributes["Id"].Value == this.id.ToString())
				{
					xmlNode.Attributes["Title"].Value = this.txtTitle.Text;
					xmlNode.Attributes["Url"].Value = this.GetUrl();
					xmlNode.Attributes["Where"].Value = this.GetWhere();
					break;
				}
			}
			xmlDocument.Save(filename);
			base.Response.Redirect("SetMyHeaderMenu.aspx");
		}
		private string GetUrl()
		{
			if (this.category == "1")
			{
				return this.dropSystemPageDropDownList.SelectedValue;
			}
			if (this.category == "3")
			{
				return this.txtCustomLink.Text;
			}
			return string.Empty;
		}
		private string GetWhere()
		{
			string text = string.Empty;
			if (this.category == "2")
			{
				decimal num = 0m;
				decimal num2 = 0m;
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					this.listProductCategories.SelectedCategoryId.ToString(),
					",",
					this.listBrandCategories.SelectedValue,
					",",
					this.radProductTags.SelectedValue,
					","
				});
				if (decimal.TryParse(this.txtMinPrice.Text, out num))
				{
					text += this.txtMinPrice.Text;
				}
				text += ",";
				if (decimal.TryParse(this.txtMaxPrice.Text, out num2))
				{
					text += this.txtMaxPrice.Text;
				}
				text += ",";
				text += this.txtKeyword.Text;
			}
			return text;
		}
	}
}
