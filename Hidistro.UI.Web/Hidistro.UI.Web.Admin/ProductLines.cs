using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductLines)]
	public class ProductLines : AdminPage
	{
		protected Grid grdProductLine;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdProductLine.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProductLine_RowDeleting);
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void grdProductLine_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int lineId = (int)this.grdProductLine.DataKeys[e.RowIndex].Value;
			if (ProductLineHelper.DeleteProductLine(lineId))
			{
				this.BindData();
				this.ShowMsg("成功删除了已选定的产品线", true);
				return;
			}
			this.ShowMsg("不能删除有商品的产品线或最后一个产品线", false);
		}
		private void BindData()
		{
			this.grdProductLine.DataSource = ProductLineHelper.GetProductLines();
			this.grdProductLine.DataBind();
		}
	}
}
