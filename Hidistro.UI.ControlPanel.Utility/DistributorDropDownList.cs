using Hidistro.ControlPanel.Distribution;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class DistributorDropDownList : DropDownList
	{
		private string nullToDisplay = "--请选择分销商--";
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
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			IList<Hidistro.Membership.Context.Distributor> distributors = DistributorHelper.GetDistributors();
			foreach (Hidistro.Membership.Context.Distributor current in distributors)
			{
				base.Items.Add(new ListItem(current.Username, current.UserId.ToString()));
			}
		}
	}
}
