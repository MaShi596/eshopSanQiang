using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class BankDropDownList : System.Web.UI.WebControls.DropDownList
	{
		public BankDropDownList()
		{
			this.Items.Clear();
			this.Items.Add(new System.Web.UI.WebControls.ListItem("请选择", "请选择"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("中国工商银行", "中国工商银行"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("中国建设银行", "中国建设银行"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("中国农业银行", "中国农业银行"));
		}
	}
}
