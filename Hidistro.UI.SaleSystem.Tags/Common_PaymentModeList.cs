using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_PaymentModeList : AscxTemplatedWebControl
	{
		public const string TagID = "grd_Common_PaymentModeList";
		private GridView grdPayment;
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
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.grdPayment.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.grdPayment.DataSource = value;
			}
		}
		public Common_PaymentModeList()
		{
			base.ID = "grd_Common_PaymentModeList";
		}
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_PaymentModeList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.grdPayment = (GridView)this.FindControl("grdPayment");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.grdPayment.DataSource = this.DataSource;
			this.grdPayment.DataBind();
		}
	}
}
