using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class DistributorInpourReturn_url : DistributorInpourReturnBasePage
	{
		protected System.Web.UI.WebControls.Literal litMessage;
		public DistributorInpourReturn_url() : base(false)
		{
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		protected override void DisplayMessage(string status)
		{
			if (status != null)
			{
				if (status == "gatewaynotfound")
				{
					this.litMessage.Text = "没有找到与此次充值对应的支付方式，系统无法自动完成操作，请联系管理员";
					return;
				}
				if (status == "waitconfirm")
				{
					this.litMessage.Text = "您使用的是担保交易付款，在您确认收货以后系统就会为您的预付款账户充入相应的金额";
					return;
				}
				if (status == "success")
				{
					this.litMessage.Text = string.Format("恭喜您，此次预付款充值已成功完成，本次充值金额：{0}", this.Amount.ToString("F"));
					return;
				}
				if (status == "fail")
				{
					this.litMessage.Text = string.Format("您已成功支付，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", this.Amount.ToString("F"));
					return;
				}
				if (status == "verifyfaild")
				{
					this.litMessage.Text = "支付返回验证失败，操作已停止";
					return;
				}
			}
			this.litMessage.Text = "未知错误，操作已停止";
		}
	}
}