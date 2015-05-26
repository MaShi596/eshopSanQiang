using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorRequestInstruction)]
	public class DistributorRequestInstruction : AdminPage
	{
		protected KindeditorControl fkFooter;
		protected KindeditorControl fkProtocols;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
				this.fkFooter.Text=masterSettings.DistributorRequestInstruction;
				this.fkProtocols.Text=masterSettings.DistributorRequestProtocols;
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			masterSettings.DistributorRequestInstruction = this.fkFooter.Text;
			masterSettings.DistributorRequestProtocols = this.fkProtocols.Text;
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.SiteSettings>(masterSettings, new string[]
			{
				"ValRequestProtocols"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功", true);
		}
	}
}
