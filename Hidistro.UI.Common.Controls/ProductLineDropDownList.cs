using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class ProductLineDropDownList : DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "全部";
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
		public bool IsShowNoset
		{
			get;
			set;
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
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue));
			}
			set
			{
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
					return;
				}
				base.SelectedValue = value.ToString();
			}
		}
		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			if (this.IsShowNoset)
			{
				base.Items.Add(new ListItem("未设置产品线", "0"));
			}
			IList<ProductLineInfo> productLineList = ControlProvider.Instance().GetProductLineList();
			foreach (ProductLineInfo current in productLineList)
			{
				base.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.LineId.ToString()));
			}
		}
	}
}
