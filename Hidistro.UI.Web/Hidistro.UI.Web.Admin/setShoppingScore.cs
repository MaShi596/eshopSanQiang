using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class setShoppingScore : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtProductPointSet;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtProductPointSetTip;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
				this.txtProductPointSet.Text = masterSettings.PointsRate.ToString(System.Globalization.CultureInfo.InvariantCulture);
			}
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			decimal pointsRate;
			if (decimal.TryParse(this.txtProductPointSet.Text.Trim(), out pointsRate) && (!this.txtProductPointSet.Text.Trim().Contains(".") || this.txtProductPointSet.Text.Trim().Substring(this.txtProductPointSet.Text.Trim().IndexOf(".") + 1).Length <= 2))
			{
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
				masterSettings.PointsRate = pointsRate;
				Globals.EntityCoding(masterSettings, true);
				Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
				this.ShowMsg("保存成功", true);
				return;
			}
			this.ShowMsg("几元一积分不能为空,为非负数字,范围在0.1-10000000之间", false);
		}
	}
}
