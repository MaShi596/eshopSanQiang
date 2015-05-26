using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin.store
{
	[PrivilegeCheck(Privilege.Themes)]
	public class SetMyHeaderMenu : DistributorPage
	{
		private string themName;
		protected System.Web.UI.WebControls.Literal litThemName;
		protected System.Web.UI.WebControls.HyperLink hlinkAddHeaderMenu;
		protected System.Web.UI.WebControls.TextBox txtCategoryNum;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.LinkButton lbtnSaveSequence;
		protected Grid grdMyHeaderMenu;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.themName = Hidistro.Membership.Context.HiContext.Current.SiteSettings.Theme;
			this.grdMyHeaderMenu.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdHeaderMenu_RowDataBound);
			this.grdMyHeaderMenu.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdHeaderMenu_RowCommand);
			this.grdMyHeaderMenu.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdHeaderMenu_RowDeleting);
			this.lbtnSaveSequence.Click += new System.EventHandler(this.lbtnSaveSequence_Click);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			if (!this.Page.IsPostBack)
			{
				this.litThemName.Text = this.themName;
				this.hlinkAddHeaderMenu.NavigateUrl = "AddMyHeaderMenu.aspx?ThemName=" + this.themName;
				this.BindHeaderMenu();
			}
		}
		private void grdHeaderMenu_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				int num = (int)this.grdMyHeaderMenu.DataKeys[e.Row.RowIndex].Value;
				System.Web.UI.WebControls.HyperLink hyperLink = e.Row.FindControl("lkbEdit") as System.Web.UI.WebControls.HyperLink;
				hyperLink.NavigateUrl = string.Format("EditMyHeaderMenu.aspx?Id={0}&ThemName={1}", num, this.themName);
				System.Web.UI.WebControls.Literal literal = e.Row.FindControl("litCategory") as System.Web.UI.WebControls.Literal;
				string text = literal.Text;
				if (text == "1")
				{
					text = "系统页面";
				}
				else
				{
					if (text == "2")
					{
						text = "商品搜索链接";
					}
					else
					{
						text = "自定义链接";
					}
				}
				literal.Text = text;
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			int num;
			if (!int.TryParse(this.txtCategoryNum.Text, out num))
			{
				this.ShowMsg("请输入有效果的数字", false);
				return;
			}
			xmlNode.Attributes["CategoryNum"].Value = num.ToString();
			xmlDocument.Save(filename);
		}
		private void grdHeaderMenu_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "SetYesOrNo")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int num = (int)this.grdMyHeaderMenu.DataKeys[rowIndex].Value;
				string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.Load(filename);
				System.Xml.XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
				foreach (System.Xml.XmlNode xmlNode in childNodes)
				{
					if (xmlNode.Attributes["Id"].Value == num.ToString())
					{
						if (xmlNode.Attributes["Visible"].Value == "true")
						{
							xmlNode.Attributes["Visible"].Value = "false";
						}
						else
						{
							xmlNode.Attributes["Visible"].Value = "true";
						}
						break;
					}
				}
				xmlDocument.Save(filename);
				this.BindHeaderMenu();
			}
		}
		private void grdHeaderMenu_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int num = (int)this.grdMyHeaderMenu.DataKeys[e.RowIndex].Value;
			string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			System.Xml.XmlNodeList childNodes = xmlNode.ChildNodes;
			foreach (System.Xml.XmlNode xmlNode2 in childNodes)
			{
				if (xmlNode2.Attributes["Id"].Value == num.ToString())
				{
					xmlNode.RemoveChild(xmlNode2);
					break;
				}
			}
			xmlDocument.Save(filename);
			this.BindHeaderMenu();
		}
		private void lbtnSaveSequence_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMyHeaderMenu.Rows)
			{
				int id = (int)this.grdMyHeaderMenu.DataKeys[gridViewRow.RowIndex].Value;
				int displaySequence = 0;
				int.TryParse(((System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtDisplaySequence")).Text, out displaySequence);
				this.SaveHeaderMenu(id, displaySequence);
			}
			this.BindHeaderMenu();
		}
		private void SaveHeaderMenu(int id, int displaySequence)
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			System.Xml.XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (System.Xml.XmlNode xmlNode in childNodes)
			{
				if (xmlNode.Attributes["Id"].Value == id.ToString())
				{
					xmlNode.Attributes["DisplaySequence"].Value = displaySequence.ToString();
					break;
				}
			}
			xmlDocument.Save(filename);
		}
		private void BindHeaderMenu()
		{
			string filename = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString() + "/{0}/config/HeaderMenu.xml", this.themName));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(filename);
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("Id", typeof(int));
			dataTable.Columns.Add("Title");
			dataTable.Columns.Add("DisplaySequence", typeof(int));
			dataTable.Columns.Add("Url");
			dataTable.Columns.Add("Category");
			dataTable.Columns.Add("Visible");
			System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			this.txtCategoryNum.Text = xmlNode.Attributes["CategoryNum"].Value;
			System.Xml.XmlNodeList childNodes = xmlNode.ChildNodes;
			foreach (System.Xml.XmlNode xmlNode2 in childNodes)
			{
				System.Data.DataRow dataRow = dataTable.NewRow();
				dataRow["Id"] = int.Parse(xmlNode2.Attributes["Id"].Value);
				dataRow["Title"] = xmlNode2.Attributes["Title"].Value;
				dataRow["DisplaySequence"] = int.Parse(xmlNode2.Attributes["DisplaySequence"].Value);
				dataRow["Category"] = xmlNode2.Attributes["Category"].Value;
				dataRow["Url"] = xmlNode2.Attributes["Url"].Value;
				dataRow["Visible"] = xmlNode2.Attributes["Visible"].Value;
				dataTable.Rows.Add(dataRow);
			}
			dataTable.DefaultView.Sort = "DisplaySequence ASC";
			this.grdMyHeaderMenu.DataSource = dataTable;
			this.grdMyHeaderMenu.DataBind();
		}
	}
}
