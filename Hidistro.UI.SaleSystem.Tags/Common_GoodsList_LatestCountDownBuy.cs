using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_GoodsList_LatestCountDownBuy : AscxTemplatedWebControl
	{
		private Repeater repcountdown;
		private int maxNum = 1;
		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_GoodsList/Skin-Common_GoodsList_LatestCountDownBuy.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.repcountdown = (Repeater)this.FindControl("repcountdown");
			this.repcountdown.DataSource = ProductBrowser.GetCounDownProducList(this.maxNum);
			this.repcountdown.DataBind();
		}
	}
}
