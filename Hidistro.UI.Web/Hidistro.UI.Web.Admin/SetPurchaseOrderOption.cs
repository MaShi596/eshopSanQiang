using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class SetPurchaseOrderOption : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtClosePurchaseOrderDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtClosePurchaseOrderDaysTip;
		protected System.Web.UI.WebControls.TextBox txtFinishPurchaseOrderDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtFinishPurchaseOrderDaysTip;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
				this.txtClosePurchaseOrderDays.Text = masterSettings.ClosePurchaseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtFinishPurchaseOrderDays.Text = masterSettings.FinishPurchaseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
			}
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			int closePurchaseOrderDays;
			int finishPurchaseOrderDays;
			if (!this.ValidateValues(out closePurchaseOrderDays, out finishPurchaseOrderDays))
			{
				return;
			}
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			masterSettings.ClosePurchaseOrderDays = closePurchaseOrderDays;
			masterSettings.FinishPurchaseOrderDays = finishPurchaseOrderDays;
			if (!this.ValidationSettings(masterSettings))
			{
				return;
			}
			Globals.EntityCoding(masterSettings, true);
			Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功", true);
		}
		private bool ValidateValues(out int closePurchaseOrderDays, out int finishPurchaseOrderDays)
		{
			string text = string.Empty;
			if (!int.TryParse(this.txtClosePurchaseOrderDays.Text, out closePurchaseOrderDays))
			{
				text += Formatter.FormatErrorMessage("过期几天自动关闭采购单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtFinishPurchaseOrderDays.Text, out finishPurchaseOrderDays))
			{
				text += Formatter.FormatErrorMessage("发货几天自动完成采购单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private bool ValidationSettings(Hidistro.Membership.Context.SiteSettings setting)
		{
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.SiteSettings>(setting, new string[]
			{
				"ValMasterSettings"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
