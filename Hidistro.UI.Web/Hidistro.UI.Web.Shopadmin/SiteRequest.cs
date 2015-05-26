using Hidistro.Core;
using Hidistro.Entities.Distribution;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SiteRequest : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litServerIp;
		protected System.Web.UI.WebControls.TextBox txtFirstSiteUrl;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtFirstSiteUrlTip;
		protected System.Web.UI.WebControls.Button btnAddRequest;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddRequest.Click += new System.EventHandler(this.btnAddRequest_Click);
			if (!base.IsPostBack)
			{
				this.ProcessRequestStatus();
				this.litServerIp.Text = base.Request.ServerVariables.Get("Local_Addr").ToString();
			}
		}
		private void btnAddRequest_Click(object sender, System.EventArgs e)
		{
			SiteRequestInfo siteRequestInfo = new SiteRequestInfo();
			siteRequestInfo.FirstSiteUrl = this.txtFirstSiteUrl.Text.Trim();
			siteRequestInfo.RequestTime = System.DateTime.Now;
			siteRequestInfo.RequestStatus = SiteRequestStatus.Dealing;
			ValidationResults validationResults = Validation.Validate<SiteRequestInfo>(siteRequestInfo, new string[]
			{
				"ValSiteRequest"
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
			SiteRequestInfo mySiteRequest = SubsiteStoreHelper.GetMySiteRequest();
			if (mySiteRequest != null && mySiteRequest.RequestStatus == SiteRequestStatus.Dealing)
			{
				this.ShowMsg("您上一条申请还未处理，请联系供应商", false);
				return;
			}
			if (SubsiteStoreHelper.IsExitSiteUrl(siteRequestInfo.FirstSiteUrl))
			{
				this.ShowMsg("您输入的域名已经被其它分销商绑定了，请重新输入", false);
				return;
			}
			if (SubsiteStoreHelper.AddSiteRequest(siteRequestInfo))
			{
				base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ShowSiteRequestStatus.aspx");
				return;
			}
			this.ShowMsg("站点申请提交失败", false);
		}
		private void ProcessRequestStatus()
		{
			SiteRequestInfo mySiteRequest = SubsiteStoreHelper.GetMySiteRequest();
			if (mySiteRequest != null)
			{
				base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ShowSiteRequestStatus.aspx");
			}
		}
	}
}
