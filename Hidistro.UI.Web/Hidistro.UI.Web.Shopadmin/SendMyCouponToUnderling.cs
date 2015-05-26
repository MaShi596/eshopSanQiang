using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SendMyCouponToUnderling : DistributorPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoName;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoRank;
		protected UnderlingGradeDropDownList rankList;
		protected System.Web.UI.WebControls.TextBox txtMemberNames;
		protected System.Web.UI.WebControls.Button btnSend;
		private int couponId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			if (!this.Page.IsPostBack)
			{
				this.rankList.DataBind();
			}
		}
		private void btnSend_Click(object sender, System.EventArgs e)
		{
			CouponItemInfo item = new CouponItemInfo();
			System.Collections.Generic.IList<CouponItemInfo> list = new System.Collections.Generic.List<CouponItemInfo>();
			System.Collections.Generic.IList<Hidistro.Membership.Context.Member> list2 = new System.Collections.Generic.List<Hidistro.Membership.Context.Member>();
			if (this.rdoName.Checked)
			{
				if (!string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
				{
					System.Collections.Generic.IList<string> list3 = new System.Collections.Generic.List<string>();
					string text = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n");
					string[] array = text.Replace("\n", "*").Split(new char[]
					{
						'*'
					});
					for (int i = 0; i < array.Length; i++)
					{
						if (this.IsMembers(array[i]))
						{
							list3.Add(array[i]);
						}
					}
					list2 = SubsitePromoteHelper.GetMemdersByNames(list3);
				}
				string claimCode = string.Empty;
				foreach (Hidistro.Membership.Context.Member current in list2)
				{
					claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
					item = new CouponItemInfo(this.couponId, claimCode, new int?(current.UserId), current.Username, current.Email, System.DateTime.Now);
					list.Add(item);
				}
				if (list.Count <= 0)
				{
					this.ShowMsg("你输入的会员名中没有一个正确的，请输入正确的会员名", false);
					return;
				}
				SubsiteCouponHelper.SendClaimCodes(this.couponId, list);
				this.txtMemberNames.Text = string.Empty;
				this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", list.Count), true);
			}
			if (this.rdoRank.Checked)
			{
				list2 = SubsitePromoteHelper.GetMembersByRank(this.rankList.SelectedValue);
				string claimCode2 = string.Empty;
				foreach (Hidistro.Membership.Context.Member current2 in list2)
				{
					claimCode2 = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
					item = new CouponItemInfo(this.couponId, claimCode2, new int?(current2.UserId), current2.Username, current2.Email, System.DateTime.Now);
					list.Add(item);
				}
				if (list.Count <= 0)
				{
					this.ShowMsg("您选择的会员等级下面没有会员", false);
					return;
				}
				SubsiteCouponHelper.SendClaimCodes(this.couponId, list);
				this.txtMemberNames.Text = string.Empty;
				this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", list.Count), true);
			}
		}
		private bool IsMembers(string name)
		{
			string pattern = "[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
			new System.Text.RegularExpressions.Regex(pattern);
			return name.Length >= 2 && name.Length <= 20;
		}
	}
}
