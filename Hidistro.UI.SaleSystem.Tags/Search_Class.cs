using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Search_Class : DropDownList
	{
		public const string TagID = "drop_Search_Class";
		private string nullToDisplay = "商品分类";
		private string textStyle;
		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}
		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
					return;
				}
				base.SelectedIndex = -1;
			}
		}
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
		public string TextStyle
		{
			get
			{
				return this.textStyle;
			}
			set
			{
				this.textStyle = value;
			}
		}
		public Search_Class()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			IList<CategoryInfo> maxMainCategories = CategoryBrowser.GetMaxMainCategories(1000);
			foreach (CategoryInfo current in maxMainCategories)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.CategoryId.ToString(CultureInfo.InvariantCulture)));
			}
			base.ID = "drop_Search_Class";
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.TextStyle))
			{
				this.CssClass = this.TextStyle;
			}
			base.Render(writer);
		}
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "drop_Search_Class");
		}
	}
}
