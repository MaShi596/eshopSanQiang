using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class AuthorizeProductLineDropDownList : System.Web.UI.WebControls.DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "--请选择--";
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}
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
				int? result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = null;
				}
				else
				{
					result = new int?(int.Parse(base.SelectedValue));
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString()));
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new System.Web.UI.WebControls.ListItem(this.NullToDisplay, string.Empty));
			}
			System.Collections.Generic.IList<ProductLineInfo> authorizeProductLineList = SubSiteProducthelper.GetAuthorizeProductLineList();
			foreach (ProductLineInfo current in authorizeProductLineList)
			{
				this.Items.Add(new System.Web.UI.WebControls.ListItem(Globals.HtmlDecode(current.Name), current.LineId.ToString()));
			}
		}
	}
}
