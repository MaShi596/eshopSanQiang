using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Distribution;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class SendMessageSelectDistributor : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoName;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoRank;
		protected System.Web.UI.WebControls.TextBox txtDistributorNames;
		protected DistributorGradeDropDownList rankList;
		protected System.Web.UI.WebControls.Button btnSendToRank;
		protected System.Web.UI.WebControls.CheckBox chkIsSendEmail;
		private int userId;
		public string MessageTitle
		{
			get
			{
				if (!string.IsNullOrEmpty(this.Session["Title"].ToString()))
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
				if (!string.IsNullOrEmpty(this.Session["Content"].ToString()))
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
					Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
					if (distributor == null)
					{
						base.GotoResourceNotFound();
						return;
					}
					this.txtDistributorNames.Text = distributor.Username;
				}
			}
		}
		private Hidistro.Membership.Context.Distributor GetDistributor(string name)
		{
			Hidistro.Membership.Context.Distributor result = null;
			if (name.Length >= 2 && name.Length <= 20)
			{
				result = (Hidistro.Membership.Context.Users.FindUserByUsername(name) as Hidistro.Membership.Context.Distributor);
			}
			return result;
		}
		private void btnSendToRank_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MessageBoxInfo> list = new System.Collections.Generic.List<MessageBoxInfo>();
			if (this.rdoName.Checked)
			{
				if (string.IsNullOrEmpty(this.txtDistributorNames.Text.Trim()))
				{
					this.ShowMsg("请输入您要发送的用户", false);
					return;
				}
				string text = this.txtDistributorNames.Text.Trim().Replace("\r\n", "\n");
				string[] array = text.Replace("\n", "*").Split(new char[]
				{
					'*'
				});
				for (int i = 0; i < array.Length; i++)
				{
					Hidistro.Membership.Context.Distributor distributor = this.GetDistributor(array[i]);
					if (distributor != null)
					{
						list.Add(new MessageBoxInfo
						{
							Accepter = array[i],
							Sernder = "admin",
							Title = this.MessageTitle,
							Content = this.Content
						});
						if (this.chkIsSendEmail.Checked && System.Text.RegularExpressions.Regex.IsMatch(distributor.Email, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
						{
							string text2;
							Messenger.SendMail(this.MessageTitle, this.Content, distributor.Email, Hidistro.Membership.Context.HiContext.Current.SiteSettings, out text2);
						}
					}
				}
				if (list.Count <= 0)
				{
					this.ShowMsg("没有要发送的对象", false);
					return;
				}
				NoticeHelper.SendMessageToDistributor(list);
				this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", list.Count), true);
			}
			if (this.rdoRank.Checked)
			{
				System.Collections.Generic.IList<Hidistro.Membership.Context.Distributor> list2 = new System.Collections.Generic.List<Hidistro.Membership.Context.Distributor>();
				list2 = NoticeHelper.GetDistributorsByRank(this.rankList.SelectedValue);
				foreach (Hidistro.Membership.Context.Distributor current in list2)
				{
					list.Add(new MessageBoxInfo
					{
						Accepter = current.Username,
						Sernder = "admin",
						Title = this.MessageTitle,
						Content = this.Content
					});
					if (this.chkIsSendEmail.Checked && System.Text.RegularExpressions.Regex.IsMatch(current.Email, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
					{
						string text3;
						Messenger.SendMail(this.MessageTitle, this.Content, current.Email, Hidistro.Membership.Context.HiContext.Current.SiteSettings, out text3);
					}
				}
				if (list.Count > 0)
				{
					NoticeHelper.SendMessageToDistributor(list);
					this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", list.Count), true);
					return;
				}
				this.ShowMsg("没有要发送的对象", false);
			}
		}
	}
}
