using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_CategoriesDropDownList : System.Web.UI.WebControls.DropDownList
	{
		private string m_NullToDisplay = "";
		private bool m_AutoDataBind = false;
		private string strDepth = "\u3000\u3000";
		public string NullToDisplay
		{
			get
			{
				return this.m_NullToDisplay;
			}
			set
			{
				this.m_NullToDisplay = value;
			}
		}
		public bool AutoDataBind
		{
			get
			{
				return this.m_AutoDataBind;
			}
			set
			{
				this.m_AutoDataBind = value;
			}
		}
		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (!string.IsNullOrEmpty(base.SelectedValue))
				{
					result = new int?(int.Parse(base.SelectedValue, System.Globalization.CultureInfo.InvariantCulture));
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
				}
			}
		}
		protected override void OnLoad(System.EventArgs eventArgs_0)
		{
			if (this.AutoDataBind && !this.Page.IsPostBack)
			{
				this.DataBind();
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new System.Web.UI.WebControls.ListItem(this.NullToDisplay, string.Empty));
			System.Collections.Generic.IList<CategoryInfo> sequenceCategories = CategoryBrowser.GetSequenceCategories();
			for (int i = 0; i < sequenceCategories.Count; i++)
			{
				this.Items.Add(new System.Web.UI.WebControls.ListItem(this.FormatDepth(sequenceCategories[i].Depth, Globals.HtmlDecode(sequenceCategories[i].Name)), sequenceCategories[i].CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
			}
		}
		private string FormatDepth(int depth, string categoryName)
		{
			for (int i = 1; i < depth; i++)
			{
				categoryName = this.strDepth + categoryName;
			}
			return categoryName;
		}
	}
}
