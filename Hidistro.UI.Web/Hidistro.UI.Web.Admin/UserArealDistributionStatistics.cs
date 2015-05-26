using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberArealDistributionStatistics)]
	public class UserArealDistributionStatistics : AdminPage
	{
		protected Grid grdUserStatistics;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.grdUserStatistics.RowDataBound += new GridViewRowEventHandler(this.grdUserStatistics_RowDataBound);
            this.grdUserStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdUserStatistics_ReBindData);
            this.grdUserStatistics.RowDataBound += new GridViewRowEventHandler(this.grdUserStatistics_RowDataBound);
            if (!base.IsPostBack)
            {
                this.BindUserStatistics();
            }
		}
		private void grdUserStatistics_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				int num = int.Parse(this.grdUserStatistics.DataKeys[e.Row.RowIndex].Value.ToString(), System.Globalization.NumberStyles.None);
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblReionName");
				if (num != 0 && label != null)
				{
					label.Text = RegionHelper.GetFullRegion(num, "");
				}
				if (num == 0 && label != null)
				{
					label.Text = "其它";
				}
			}
		}
		private void grdUserStatistics_ReBindData(object sender)
		{
			this.BindUserStatistics();
		}
		private void BindUserStatistics()
		{
			int num = 0;
			Pagination pagination = new Pagination();
			pagination.SortBy = this.grdUserStatistics.SortOrderBy;
			if (this.grdUserStatistics.SortOrder.ToLower() == "desc")
			{
				pagination.SortOrder = SortAction.Desc;
			}
			this.grdUserStatistics.DataSource = SalesHelper.GetUserStatistics(pagination, out num);
			this.grdUserStatistics.DataBind();
		}
	}
}
