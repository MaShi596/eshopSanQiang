using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Comments;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SendMyMessageSelectUser : DistributorPage
	{
		private int userId;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoName;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoRank;
		protected System.Web.UI.WebControls.TextBox txtMemberNames;
		protected UnderlingGradeDropDownList rankList;
		protected System.Web.UI.WebControls.Button btnSendToRank;
		public string MessageTitle
		{
			get
			{
				if (this.Session["Title"] != null)
				{
					return Globals.UrlDecode(this.Session["Title"].ToString());
				}
				return string.Empty;
			}
		}
		public string Content
		{
			get
			{
				if (this.Session["Content"] != null)
				{
					return Globals.UrlDecode(this.Session["Content"].ToString());
				}
				return string.Empty;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnSendToRank.Click += new System.EventHandler(this.btnSendToRank_Click);
			if (!this.Page.IsPostBack)
			{
				this.rankList.DataBind();
				if (this.userId > 0)
				{
					Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(this.userId) as Hidistro.Membership.Context.Member;
					if (member == null)
					{
						base.GotoResourceNotFound();
						return;
					}
					this.txtMemberNames.Text = member.Username;
				}
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private Hidistro.Membership.Context.Member GetMember(string name)
		{
			Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.FindUserByUsername(name) as Hidistro.Membership.Context.Member;
			if (member != null && member.UserRole == Hidistro.Membership.Core.Enums.UserRole.Underling && member.ParentUserId.Value == Hidistro.Membership.Context.HiContext.Current.User.UserId)
			{
				return member;
			}
			return null;
		}
		private void btnSendToRank_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MessageBoxInfo> list = new System.Collections.Generic.List<MessageBoxInfo>();
			if (this.rdoName.Checked && !string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
			{
				string text = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n");
				string[] array = text.Replace("\n", "*").Split(new char[]
				{
					'*'
				});
				for (int i = 0; i < array.Length; i++)
				{
					Hidistro.Membership.Context.Member member = this.GetMember(array[i]);
					if (member != null)
					{
						list.Add(new MessageBoxInfo
						{
							Accepter = array[i],
							Sernder = Hidistro.Membership.Context.HiContext.Current.User.Username,
							Title = this.MessageTitle,
							Content = this.Content
						});
					}
				}
				if (list.Count <= 0)
				{
					this.ShowMsg("没有要发送的对象", false);
					return;
				}
				SubsiteCommentsHelper.SendMessageToMember(list);
				this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", list.Count), true);
			}
			if (this.rdoRank.Checked)
			{
				System.Collections.Generic.IList<Hidistro.Membership.Context.Member> list2 = new System.Collections.Generic.List<Hidistro.Membership.Context.Member>();
				list2 = SubsitePromoteHelper.GetMembersByRank(this.rankList.SelectedValue);
				foreach (Hidistro.Membership.Context.Member current in list2)
				{
					list.Add(new MessageBoxInfo
					{
						Accepter = current.Username,
						Sernder = Hidistro.Membership.Context.HiContext.Current.User.Username,
						Title = this.MessageTitle,
						Content = this.Content
					});
				}
				if (list.Count > 0)
				{
					SubsiteCommentsHelper.SendMessageToMember(list);
					this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", list.Count), true);
					return;
				}
				this.ShowMsg("没有要发送的对象", false);
			}
		}
	}
}
