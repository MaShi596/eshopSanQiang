using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorClosePurchaseOrderReasonDropDownList : System.Web.UI.WebControls.DropDownList
	{
		public DistributorClosePurchaseOrderReasonDropDownList()
		{
			this.Items.Clear();
			this.Items.Add(new System.Web.UI.WebControls.ListItem("请选择取消的理由", "请选择取消的理由"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("分销商的会员不想买了", "分销商的会员不想买了"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("暂时缺货", "暂时缺货"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("其他原因", "其他原因"));
		}
	}
}
