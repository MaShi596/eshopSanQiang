using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class FormatedMoneyLabel : Label
	{
		private string nullToDisplay = "-";
		public object Money
		{
			get
			{
				if (this.ViewState["Money"] == null)
				{
					return null;
				}
				return this.ViewState["Money"];
			}
			set
			{
				if (value != null)
				{
					if (value != DBNull.Value)
					{
						this.ViewState["Money"] = value;
						return;
					}
				}
				this.ViewState["Money"] = null;
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
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Money != null && this.Money != DBNull.Value)
			{
				base.Text = Globals.FormatMoney((decimal)this.Money);
			}
			if (string.IsNullOrEmpty(base.Text))
			{
				base.Text = this.NullToDisplay;
			}
			base.Render(writer);
		}
	}
}
