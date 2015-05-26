using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class OrderRemarkImage : Literal
	{
		private string imageFormat = "<img border=\"0\" src=\"{0}\"  />";
		private string dataField;
		public string DataField
		{
			get
			{
				return this.dataField;
			}
			set
			{
				this.dataField = value;
			}
		}
		protected override void OnDataBinding(EventArgs e)
		{
			object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
			if (obj != null && obj != DBNull.Value)
			{
				base.Text = string.Format(this.imageFormat, this.GetImageSrc(obj));
			}
			else
			{
				base.Text = string.Format(this.imageFormat, Globals.ApplicationPath + "/Admin/images/xi.gif");
			}
			base.OnDataBinding(e);
		}
		protected string GetImageSrc(object managerMark)
		{
			string text = Globals.ApplicationPath + "/Admin/images/";
			switch ((OrderMark)managerMark)
			{
			case OrderMark.Draw:
				text += "iconaf.gif";
				break;
			case OrderMark.ExclamationMark:
				text += "iconb.gif";
				break;
			case OrderMark.Red:
				text += "iconc.gif";
				break;
			case OrderMark.Green:
				text += "icona.gif";
				break;
			case OrderMark.Yellow:
				text += "iconad.gif";
				break;
			case OrderMark.Gray:
				text += "iconae.gif";
				break;
			default:
				text = string.Format(this.imageFormat, Globals.ApplicationPath + "/Admin/images/xi.gif");
				break;
			}
			return text;
		}
	}
}
