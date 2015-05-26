using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Comments;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ManageUnderlings : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected UnderlingGradeDropDownList dropMemberGrade;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
		protected ExportFormatRadioButtonList exportFormatRadioButtonList;
		protected System.Web.UI.WebControls.Button btnExport;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbDelectCheck1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span2;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span5;
		protected ApprovedDropDownList ddlApproved;
		protected Grid grdUnderlings;
		protected ImageLinkButton lkbDelectCheck;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span3;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span4;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span6;
		protected Pager pager1;
		protected System.Web.UI.WebControls.Literal litsmscount;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtmsgcontent;
		protected System.Web.UI.WebControls.Button btnSendMessage;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtemailcontent;
		protected System.Web.UI.WebControls.Button btnSendEmail;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtsitecontent;
		protected System.Web.UI.WebControls.Button btnsitecontent;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdenablemsg;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdenableemail;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdUnderlings.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdUnderlings_RowDeleting);
			this.lkbDelectCheck.Click += new System.EventHandler(this.lkbDelectCheck_Click);
			this.lkbDelectCheck1.Click += new System.EventHandler(this.lkbDelectCheck_Click);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.btnsitecontent.Click += new System.EventHandler(this.btnsitecontent_Click);
			this.ddlApproved.AutoPostBack = true;
			this.ddlApproved.SelectedIndexChanged += new System.EventHandler(this.ddlApproved_SelectedIndexChanged);
			this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
			this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropMemberGrade.DataBind();
				this.ddlApproved.DataBind();
				this.BindData();
				Hidistro.Membership.Context.SiteSettings siteSetting = this.GetSiteSetting();
				if (siteSetting.SMSEnabled)
				{
					this.litsmscount.Text = this.GetAmount(siteSetting).ToString();
					this.hdenablemsg.Value = "1";
				}
				if (siteSetting.EmailEnabled)
				{
					this.hdenableemail.Value = "1";
				}
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		protected void grdUnderlings_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int userId = (int)this.grdUnderlings.DataKeys[e.RowIndex].Value;
			if (UnderlingHelper.DeleteMember(userId))
			{
				this.BindData();
				this.ShowMsg("成功删除了选择的会员", true);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void ddlApproved_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReloadManageUnderlings(false);
		}
		protected void lkbDelectCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdUnderlings.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					int userId = System.Convert.ToInt32(this.grdUnderlings.DataKeys[gridViewRow.RowIndex].Value);
					if (UnderlingHelper.DeleteMember(userId))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.ShowMsg("请先选择要删除的会员账号", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的会员", true);
		}
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadManageUnderlings(true);
		}
		private void BindData()
		{
			MemberQuery memberQuery = this.GetMemberQuery();
			DbQueryResult members = UnderlingHelper.GetMembers(memberQuery);
			this.grdUnderlings.DataSource = members.Data;
			this.grdUnderlings.DataBind();
            this.pager.TotalRecords = members.TotalRecords;
            this.pager1.TotalRecords = members.TotalRecords;
			this.txtUsername.Text = memberQuery.Username;
			this.txtRealName.Text = memberQuery.Realname;
			this.dropMemberGrade.SelectedValue = memberQuery.GradeId;
			this.ddlApproved.SelectedValue = memberQuery.IsApproved;
		}
		private void ReloadManageUnderlings(bool isSeach)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (this.dropMemberGrade.SelectedValue.HasValue)
			{
				nameValueCollection.Add("GradeId", this.dropMemberGrade.SelectedValue.Value.ToString());
			}
			nameValueCollection.Add("Username", this.txtUsername.Text);
			nameValueCollection.Add("Realname", this.txtRealName.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("Approved", this.ddlApproved.SelectedValue.ToString());
			if (!isSeach)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void btnExport_Click(object sender, System.EventArgs e)
		{
			if (this.exportFieldsCheckBoxList.SelectedItem == null)
			{
				this.ShowMsg("请选择需要导出的会员信息", false);
				return;
			}
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.exportFieldsCheckBoxList.Items)
			{
				if (listItem.Selected)
				{
					list.Add(listItem.Value);
					list2.Add(listItem.Text);
				}
			}
			System.Data.DataTable membersNopage = UnderlingHelper.GetMembersNopage(new MemberQuery
			{
				Username = this.txtUsername.Text.Trim(),
				Realname = this.txtRealName.Text.Trim(),
				GradeId = this.dropMemberGrade.SelectedValue
			}, list);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (string current in list2)
			{
				stringBuilder.Append(current + ",");
				if (current == list2[list2.Count - 1])
				{
					stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("\r\n");
				}
			}
			foreach (System.Data.DataRow dataRow in membersNopage.Rows)
			{
				foreach (string current2 in list)
				{
					stringBuilder.Append(dataRow[current2]).Append(",");
					if (current2 == list[list2.Count - 1])
					{
						stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
						stringBuilder.Append("\r\n");
					}
				}
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			if (this.exportFormatRadioButtonList.SelectedValue == "csv")
			{
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
				this.Page.Response.ContentType = "application/octet-stream";
			}
			else
			{
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.txt");
				this.Page.Response.ContentType = "application/vnd.ms-word";
			}
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}
		private MemberQuery GetMemberQuery()
		{
			MemberQuery memberQuery = new MemberQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GradeId"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["GradeId"], out value))
				{
					memberQuery.GradeId = new int?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
			{
				memberQuery.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Realname"]))
			{
				memberQuery.Realname = base.Server.UrlDecode(this.Page.Request.QueryString["Realname"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
			{
				memberQuery.IsApproved = new bool?(System.Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
			}
			memberQuery.PageSize = this.pager.PageSize;
			memberQuery.PageIndex = this.pager.PageIndex;
			return memberQuery;
		}
		private void btnSendMessage_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings siteSetting = this.GetSiteSetting();
			string sMSSender = siteSetting.SMSSender;
			if (string.IsNullOrEmpty(sMSSender))
			{
				this.ShowMsg("请先选择发送方式", false);
				return;
			}
			ConfigData configData = null;
			if (siteSetting.SMSEnabled)
			{
				configData = new ConfigData(HiCryptographer.Decrypt(siteSetting.SMSSettings));
			}
			if (configData == null)
			{
				this.ShowMsg("请先选择发送方式并填写配置信息", false);
				return;
			}
			if (!configData.IsValid)
			{
				string text = "";
				foreach (string current in configData.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(current);
				}
				this.ShowMsg(text, false);
				return;
			}
			string text2 = this.txtmsgcontent.Value.Trim();
			if (string.IsNullOrEmpty(text2))
			{
				this.ShowMsg("请先填写发送的内容信息", false);
				return;
			}
			int num = System.Convert.ToInt32(this.litsmscount.Text);
			string text3 = null;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdUnderlings.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text4 = ((System.Web.UI.DataBoundLiteralControl)gridViewRow.Controls[2].Controls[0]).Text.Trim().Replace("<div></div>", "");
					if (!string.IsNullOrEmpty(text4) && System.Text.RegularExpressions.Regex.IsMatch(text4, "^(13|14|15|18)\\d{9}$"))
					{
						text3 = text3 + text4 + ",";
					}
				}
			}
			if (text3 == null)
			{
				this.ShowMsg("请先选择要发送的会员或检测所选手机号格式是否正确", false);
				return;
			}
			text3 = text3.Substring(0, text3.Length - 1);
			string[] array;
			if (text3.Contains(","))
			{
				array = text3.Split(new char[]
				{
					','
				});
			}
			else
			{
				array = new string[]
				{
					text3
				};
			}
			if (num < array.Length)
			{
				this.ShowMsg("发送失败，您的剩余短信条数不足", false);
				return;
			}
			SMSSender sMSSender2 = SMSSender.CreateInstance(sMSSender, configData.SettingsXml);
			string string_;
			bool success = sMSSender2.Send(array, text2, out string_);
			this.ShowMsg(string_, success);
			this.txtmsgcontent.Value = "输入发送内容……";
			this.litsmscount.Text = (num - array.Length).ToString();
		}
		private void btnSendEmail_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings siteSetting = this.GetSiteSetting();
			string text = siteSetting.EmailSender.ToLower();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择发送方式", false);
				return;
			}
			ConfigData configData = null;
			if (siteSetting.EmailEnabled)
			{
				configData = new ConfigData(HiCryptographer.Decrypt(siteSetting.EmailSettings));
			}
			if (configData == null)
			{
				this.ShowMsg("请先选择发送方式并填写配置信息", false);
				return;
			}
			if (!configData.IsValid)
			{
				string text2 = "";
				foreach (string current in configData.ErrorMsgs)
				{
					text2 += Formatter.FormatErrorMessage(current);
				}
				this.ShowMsg(text2, false);
				return;
			}
			string text3 = this.txtemailcontent.Value.Trim();
			if (string.IsNullOrEmpty(text3))
			{
				this.ShowMsg("请先填写发送的内容信息", false);
				return;
			}
			string text4 = null;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdUnderlings.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text5 = ((System.Web.UI.DataBoundLiteralControl)gridViewRow.Controls[3].Controls[0]).Text.Trim().Replace("<div></div>", "");
					if (!string.IsNullOrEmpty(text5) && System.Text.RegularExpressions.Regex.IsMatch(text5, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
					{
						text4 = text4 + text5 + ",";
					}
				}
			}
			if (text4 == null)
			{
				this.ShowMsg("请先选择要发送的会员或检测邮箱格式是否正确", false);
				return;
			}
			text4 = text4.Substring(0, text4.Length - 1);
			string[] array;
			if (text4.Contains(","))
			{
				array = text4.Split(new char[]
				{
					','
				});
			}
			else
			{
				array = new string[]
				{
					text4
				};
			}
			System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
			{
				IsBodyHtml = true,
				Priority = System.Net.Mail.MailPriority.High,
				SubjectEncoding = System.Text.Encoding.UTF8,
				BodyEncoding = System.Text.Encoding.UTF8,
				Body = text3,
				Subject = "来自" + siteSetting.SiteName
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string addresses = array2[i];
				mailMessage.To.Add(addresses);
			}
			EmailSender emailSender = EmailSender.CreateInstance(text, configData.SettingsXml);
			try
			{
				if (emailSender.Send(mailMessage, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
				{
					this.ShowMsg("发送邮件成功", true);
				}
				else
				{
					this.ShowMsg("发送邮件失败", false);
				}
			}
			catch (System.Exception)
			{
				this.ShowMsg("发送邮件成功,但存在无效的邮箱账号", true);
			}
			this.txtemailcontent.Value = "输入发送内容……";
		}
		private void btnsitecontent_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MessageBoxInfo> list = new System.Collections.Generic.List<MessageBoxInfo>();
			string text = this.txtsitecontent.Value.Trim();
			if (string.IsNullOrEmpty(text) || text.Equals("输入发送内容……"))
			{
				this.ShowMsg("请输入要发送的内容信息", false);
				return;
			}
			string title = text;
			if (text.Length > 10)
			{
				title = text.Substring(0, 10) + "……";
			}
			string username = Hidistro.Membership.Context.HiContext.Current.User.Username;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdUnderlings.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text2 = ((System.Web.UI.WebControls.Literal)gridViewRow.Controls[1].Controls[1]).Text.Trim();
					if (this.IsMembers(text2))
					{
						list.Add(new MessageBoxInfo
						{
							Sernder = username,
							Accepter = text2,
							Title = title,
							Content = text
						});
					}
				}
			}
			if (list.Count > 0)
			{
				SubsiteCommentsHelper.SendMessageToMember(list);
				this.txtsitecontent.Value = "输入发送内容……";
				this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", list.Count), true);
				return;
			}
			this.ShowMsg("没有要发送的对象", false);
		}
		private bool IsMembers(string name)
		{
			return name.Length >= 2 && name.Length <= 20;
		}
		protected int GetAmount(Hidistro.Membership.Context.SiteSettings settings)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(settings.SMSSettings))
			{
				string xml = HiCryptographer.Decrypt(settings.SMSSettings);
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.LoadXml(xml);
				string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
				string postData = "method=getAmount&Appkey=" + innerText;
				string text = this.PostData("http://sms.kuaidiantong.cn/getAmount.aspx", postData);
				int num;
				if (int.TryParse(text, out num))
				{
					result = System.Convert.ToInt32(text);
				}
			}
			return result;
		}
		public string PostData(string url, string postData)
		{
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
		}
		private Hidistro.Membership.Context.SiteSettings GetSiteSetting()
		{
			return Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
		}
	}
}
