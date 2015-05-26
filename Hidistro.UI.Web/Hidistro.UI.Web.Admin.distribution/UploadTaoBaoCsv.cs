using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin.distribution
{
	[PrivilegeCheck(Privilege.MakeProductsPack)]
	public class UploadTaoBaoCsv : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.Response.Redirect("http://order1.kuaidiangtong.com/ImporterTaoBaoCSV.aspx?SiteUrl=" + Hidistro.Membership.Context.HiContext.Current.SiteUrl);
		}
	}
}
