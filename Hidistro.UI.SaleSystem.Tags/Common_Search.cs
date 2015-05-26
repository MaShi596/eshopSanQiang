using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Search : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_Search/Skin-Common_Search.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
		}
	}
}
