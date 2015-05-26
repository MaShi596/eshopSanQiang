using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyPaymentTypes : DistributorPage
	{
		protected System.Web.UI.WebControls.GridView grdPaymentMode;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdPaymentMode.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdPaymentMode_RowDeleting);
			this.grdPaymentMode.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdPaymentMode_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void grdPaymentMode_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName != "Sort")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int modeId = (int)this.grdPaymentMode.DataKeys[rowIndex].Value;
				int displaySequence = System.Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
				int num = 0;
				int num2 = 0;
				if (e.CommandName == "Fall")
				{
					if (rowIndex < this.grdPaymentMode.Rows.Count - 1)
					{
						num = (int)this.grdPaymentMode.DataKeys[rowIndex + 1].Value;
						num2 = System.Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
					}
				}
				else
				{
					if (e.CommandName == "Rise" && rowIndex > 0)
					{
						num = (int)this.grdPaymentMode.DataKeys[rowIndex - 1].Value;
						num2 = System.Convert.ToInt32((this.grdPaymentMode.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
					}
				}
				if (num > 0 && num2 > 0)
				{
					SubsiteSalesHelper.SwapPaymentModeSequence(modeId, num, displaySequence, num2);
					this.BindData();
				}
			}
		}
		private void grdPaymentMode_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (SubsiteSalesHelper.DeletePaymentMode((int)this.grdPaymentMode.DataKeys[e.RowIndex].Value))
			{
				this.BindData();
				this.ShowMsg("成功删除了一个支付方式", true);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void BindData()
		{
			this.grdPaymentMode.DataSource = SubsiteSalesHelper.GetPaymentModes();
			this.grdPaymentMode.DataBind();
		}
	}
}
