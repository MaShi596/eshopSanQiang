using ASPNET.WebControls;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_BatchBuy_ProductList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_BatchBuy_ProductList";
		private Grid grdProducts;
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
				return this.grdProducts.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.grdProducts.DataSource = value;
			}
		}
		public System.Web.UI.WebControls.GridViewRowCollection Rows
		{
			get
			{
				return this.grdProducts.Rows;
			}
		}
		public System.Web.UI.WebControls.DataKeyArray DataKeys
		{
			get
			{
				return this.grdProducts.DataKeys;
			}
		}
		public Common_BatchBuy_ProductList()
		{
			base.ID = "Common_BatchBuy_ProductList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_BatchBuy_ProductList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.grdProducts = (Grid)this.FindControl("grdProducts");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.grdProducts.DataSource != null)
			{
				this.grdProducts.DataBind();
			}
		}
	}
}
