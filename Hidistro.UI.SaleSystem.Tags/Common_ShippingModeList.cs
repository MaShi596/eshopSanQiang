using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShippingModeList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_ShippingModeList";
		private GridView grdShippingMode;
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.grdShippingMode.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.grdShippingMode.DataSource = value;
			}
		}
		public Common_ShippingModeList()
		{
			base.ID = "Common_ShippingModeList";
		}
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_ShippingModeList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.grdShippingMode = (GridView)this.FindControl("grdShippingMode");
			this.grdShippingMode.RowDataBound += new GridViewRowEventHandler(this.grdShippingMode_RowDataBound);
		}
		private void grdShippingMode_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				int modeId = (int)this.grdShippingMode.DataKeys[e.Row.RowIndex].Value;
				Literal literal = e.Row.FindControl("litExpressCompanyName") as Literal;
				IList<string> expressCompanysByMode = ShoppingProcessor.GetExpressCompanysByMode(modeId);
				string text = string.Empty;
				foreach (string current in expressCompanysByMode)
				{
					text = text + current + "，";
				}
				literal.Text = text.Remove(text.Length - 1);
			}
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.grdShippingMode.DataSource = this.DataSource;
			this.grdShippingMode.DataBind();
		}
	}
}
