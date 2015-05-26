using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using kindeditor.Net;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class AddPaymentType : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.TextBox txtCharge;
		protected System.Web.UI.WebControls.CheckBox chkIsPercent;
		protected YesNoRadioButtonList radiIsUseInpour;
		protected YesNoRadioButtonList radiIsUseInDistributor;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnCreate;
		protected System.Web.UI.WebControls.HiddenField txtSelectedName;
		protected System.Web.UI.WebControls.HiddenField txtConfigData;
		protected Script Script1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			if (!this.Page.IsPostBack)
			{
				this.txtCharge.Text = "0";
			}
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			string gateway;
			ConfigData configData;
			decimal charge;
			if (!this.ValidateValues(out gateway, out configData, out charge))
			{
				return;
			}
			PaymentModeInfo paymentMode = new PaymentModeInfo
			{
				Name = this.txtName.Text,
				Description = this.fcContent.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", ""),
				Gateway = gateway,
				IsUseInpour = this.radiIsUseInpour.SelectedValue,
				IsUseInDistributor = this.radiIsUseInDistributor.SelectedValue,
				Charge = charge,
				IsPercent = this.chkIsPercent.Checked,
				Settings = HiCryptographer.Encrypt(configData.SettingsXml)
			};
			PaymentModeActionStatus paymentModeActionStatus = SalesHelper.CreatePaymentMode(paymentMode);
			if (paymentModeActionStatus == PaymentModeActionStatus.Success)
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("sales/PaymentTypes.aspx"));
				return;
			}
			if (paymentModeActionStatus == PaymentModeActionStatus.DuplicateGateway)
			{
				this.ShowMsg("已经添加了一个相同类型的支付接口", false);
				return;
			}
			if (paymentModeActionStatus == PaymentModeActionStatus.DuplicateName)
			{
				this.ShowMsg("已经存在一个相同的支付方式名称", false);
				return;
			}
			if (paymentModeActionStatus == PaymentModeActionStatus.OutofNumber)
			{
				this.ShowMsg("支付方式的数目已经超出系统设置的数目", false);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private ConfigData LoadConfig(out string selectedName)
		{
			selectedName = base.Request.Form["ddlPayments"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = PaymentRequest.CreateInstance(selectedName);
			if (configablePlugin == null)
			{
				return null;
			}
			ConfigData configData = configablePlugin.GetConfigData(base.Request.Form);
			if (configData != null)
			{
				this.txtConfigData.Value = configData.SettingsXml;
			}
			return configData;
		}
		private bool ValidateValues(out string selectedPlugin, out ConfigData data, out decimal payCharge)
		{
			string text = string.Empty;
			data = this.LoadConfig(out selectedPlugin);
			payCharge = 0m;
			if (string.IsNullOrEmpty(selectedPlugin))
			{
				this.ShowMsg("请先选择一个支付接口类型", false);
				return false;
			}
			if (!data.IsValid)
			{
				foreach (string current in data.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(current);
				}
			}
			if (!decimal.TryParse(this.txtCharge.Text, out payCharge))
			{
				text += Formatter.FormatErrorMessage("支付手续费无效,大小在0-10000000之间");
			}
			if (payCharge < 0m || payCharge > 10000000m)
			{
				text += Formatter.FormatErrorMessage("支付手续费大小1-10000000之间");
			}
			if (string.IsNullOrEmpty(this.txtName.Text) || this.txtName.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("支付方式名称不能为空，长度限制在1-60个字符之间");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
