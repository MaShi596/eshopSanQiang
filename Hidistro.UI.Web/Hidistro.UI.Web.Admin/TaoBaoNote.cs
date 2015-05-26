using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.TaobaoNote)]
	public class TaoBaoNote : AdminPage
	{
		protected ShippingModeDropDownList dropShippingType;
		protected System.Web.UI.WebControls.Button btnSave;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropShippingType.DataBind();
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
				this.dropShippingType.SelectedValue = new int?(masterSettings.TaobaoShippingType);
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (!this.dropShippingType.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择一个配送方式", false);
				return;
			}
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			masterSettings.TaobaoShippingType = this.dropShippingType.SelectedValue.Value;
			Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
			this.ShowMsg("成功的保存了淘宝代销配送方式", true);
		}
	}
}
