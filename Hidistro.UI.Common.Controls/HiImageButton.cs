using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class HiImageButton : ImageButton
	{
		public new EventHandler Click;
		public override string ImageUrl
		{
			get
			{
				return base.ImageUrl;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					string imageUrl;
					if (value.StartsWith("~"))
					{
						imageUrl = base.ResolveUrl(value);
					}
					else
					{
						if (value.StartsWith("/"))
						{
							imageUrl = HiContext.Current.GetSkinPath() + value;
						}
						else
						{
							imageUrl = HiContext.Current.GetSkinPath() + "/" + value;
						}
					}
					base.ImageUrl = imageUrl;
					return;
				}
				base.ImageUrl = null;
			}
		}
		protected override void OnClick(ImageClickEventArgs e)
		{
			if (this.Click != null)
			{
				this.Click(this, e);
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			base.Click += new ImageClickEventHandler(this.HiImageButton_Click);
			base.OnLoad(e);
		}
		private void HiImageButton_Click(object sender, ImageClickEventArgs e)
		{
			this.OnClick(e);
		}
	}
}
