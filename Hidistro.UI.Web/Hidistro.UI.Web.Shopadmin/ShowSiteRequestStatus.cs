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
	public class ShowSiteRequestStatus : DistributorPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl liFail;
		protected System.Web.UI.WebControls.Literal litRefuseReason;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liWait;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liSuccess;
		protected System.Web.UI.WebControls.Literal litFirstUrl;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divRequestAgain;
		protected System.Web.UI.WebControls.TextBox txtFirstSiteUrl;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtFirstSiteUrlTip;
		protected System.Web.UI.WebControls.Button btnRequestAgain;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnRequestAgain.Click += new System.EventHandler(this.btnRequestAgain_Click);
			if (!this.Page.IsPostBack)
			{
				SiteRequestInfo mySiteRequest = SubsiteStoreHelper.GetMySiteRequest();
				if (mySiteRequest == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litFirstUrl.Text = mySiteRequest.FirstSiteUrl;
				this.litRefuseReason.Text = mySiteRequest.RefuseReason;
				if (mySiteRequest.RequestStatus == SiteRequestStatus.Dealing)
				{
					this.liWait.Visible = true;
					return;
				}
				if (mySiteRequest.RequestStatus == SiteRequestStatus.Fail)
				{
					this.liFail.Visible = true;
					this.divRequestAgain.Visible = true;
					return;
				}
				if (mySiteRequest.RequestStatus == SiteRequestStatus.Success)
				{
					this.liSuccess.Visible = true;
				}
			}
		}
		private void btnRequestAgain_Click(object sender, System.EventArgs e)
		{
			SubsiteStoreHelper.DeleteSiteRequest();
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
				this.ShowMsg("你上一条申请还未处理，请联系供应商", false);
				return;
			}
			if (SubsiteStoreHelper.AddSiteRequest(siteRequestInfo))
			{
				base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ShowSiteRequestStatus.aspx");
				return;
			}
			this.ShowMsg("站点申请提交失败", false);
		}
	}
}
