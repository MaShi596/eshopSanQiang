using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
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
	[PrivilegeCheck(Privilege.Themes)]
	public class AddMyHeaderMenu : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMenuNameTip;
		protected HeaderMenuTypeRadioButtonList radHeaderMenu;
		protected TrimTextBox txtMenuType;
		protected SystemPageDropDownList dropSystemPageDropDownList;
		protected System.Web.UI.WebControls.TextBox txtMinPrice;
		protected System.Web.UI.WebControls.TextBox txtMaxPrice;
		protected System.Web.UI.WebControls.TextBox txtKeyword;
		protected DistributorProductCategoriesListBox listProductCategories;
		protected BrandCategoriesList listBrandCategories;
		protected System.Web.UI.WebControls.ListBox radProductTags;
		protected System.Web.UI.WebControls.TextBox txtCustomLink;
		protected System.Web.UI.WebControls.Button btnAdd;
		private string themName;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["ThemName"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.themName = base.Request.QueryString["ThemName"];
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			if (!base.IsPostBack)
			{
				this.dropSystemPageDropDownList.DataBind();
				this.listProductCategories.DataBind();
				this.listBrandCategories.DataBind();
				this.radProductTags.DataSource = CatalogHelper.GetTags();
				this.radProductTags.DataTextField = "TagName";
				this.radProductTags.DataValueField = "TagID";
				this.radProductTags.DataBind();
				this.radProductTags.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--任意--", "0"));
			}
		}
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			System.Xml.XmlElement xmlElement = xmlDocument.CreateElement("Menu");
			xmlElement.SetAttribute("Id", this.GetId(xmlNode));
			xmlElement.SetAttribute("Title", this.txtTitle.Text);
			xmlElement.SetAttribute("DisplaySequence", "1");
			xmlElement.SetAttribute("Category", this.txtMenuType.Text);
			xmlElement.SetAttribute("Url", this.GetUrl(this.txtMenuType.Text));
			xmlElement.SetAttribute("Where", this.GetWhere(this.txtMenuType.Text));
			xmlElement.SetAttribute("Visible", "true");
			xmlNode.AppendChild(xmlElement);
			xmlDocument.Save(filename);
			base.Response.Redirect("SetMyHeaderMenu.aspx");
		}
		private string GetId(System.Xml.XmlNode rootNode)
		{
			int num = 1;
			System.Xml.XmlNodeList childNodes = rootNode.ChildNodes;
			foreach (System.Xml.XmlNode xmlNode in childNodes)
			{
				if (int.Parse(xmlNode.Attributes["Id"].Value) > num)
				{
					num = int.Parse(xmlNode.Attributes["Id"].Value);
				}
			}
			return (num + 1).ToString();
		}
		private string GetUrl(string category)
		{
			if (category == "1")
			{
				return this.dropSystemPageDropDownList.SelectedValue;
			}
			if (category == "3")
			{
				return this.txtCustomLink.Text;
			}
			return string.Empty;
		}
		private string GetWhere(string category)
		{
			string text = string.Empty;
			if (category == "2")
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
