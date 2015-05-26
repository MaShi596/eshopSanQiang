using ASPNET.WebControls;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AuthorizeProductLines : DistributorPage
	{
		protected Grid grdProductLine;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void BindData()
		{
			this.grdProductLine.DataSource = SubSiteProducthelper.GetAuthorizeProductLines();
			this.grdProductLine.DataBind();
		}
	}
}
