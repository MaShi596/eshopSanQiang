using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class RankPrice : Label
	{
		private string priceLogin = string.Empty;
		private string priceNoLogin = string.Empty;
		private string classNoLogin = string.Empty;
		private string classLogin = string.Empty;
		public string PriceLogin
		{
			get
			{
				return this.priceLogin;
			}
			set
			{
				this.priceLogin = value;
			}
		}
		public string PriceNoLogin
		{
			get
			{
				return this.priceNoLogin;
			}
			set
			{
				this.priceNoLogin = value;
			}
		}
		public string ClassNoLogin
		{
			get
			{
				return this.classNoLogin;
			}
			set
			{
				this.classNoLogin = value;
			}
		}
		public string ClassLogin
		{
			get
			{
				return this.classLogin;
			}
			set
			{
				this.classLogin = value;
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.User.IsAnonymous)
			{
				base.Text = this.PriceNoLogin;
				base.CssClass = this.classNoLogin;
			}
			else
			{
				decimal money;
				if (decimal.TryParse(this.PriceLogin, out money))
				{
					base.Text = Globals.FormatMoney(money);
					base.CssClass = this.ClassLogin;
				}
			}
			base.Render(writer);
		}
	}
}
