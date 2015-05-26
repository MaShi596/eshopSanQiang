using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Distributor)]
	public class ManageDistributor : AdminPage
	{
		private string userName;
		private string realName;
		private int? gradeId;
		private int? lineId;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtTrueName;
		protected DistributorGradeDropDownList dropGrade;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
		protected ExportFormatRadioButtonList exportFormatRadioButtonList;
		protected System.Web.UI.WebControls.Button btnExport;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span2;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span3;
		protected Grid grdDistributorList;
		protected Pager pager1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl litName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl litUser;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblAccountAmount;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblUseableBalance;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblFreezeBalance;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblDrawRequestBalance;
		protected System.Web.UI.WebControls.Literal litsmscount;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtmsgcontent;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtemailcontent;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtsitecontent;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdenablemsg;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdenableemail;
		protected System.Web.UI.WebControls.Button btnsitecontent;
		protected System.Web.UI.WebControls.Button btnSendEmail;
		protected System.Web.UI.WebControls.Button btnSendMessage;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdDistributorList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdDistributorList_RowCommand);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
			this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
			this.btnsitecontent.Click += new System.EventHandler(this.btnsitecontent_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.CallBack();
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindDistributors();
				this.exportFieldsCheckBoxList.Items.Remove(new System.Web.UI.WebControls.ListItem("积分", "Points"));
				this.exportFieldsCheckBoxList.Items.Remove(new System.Web.UI.WebControls.ListItem("生日", "BirthDate"));
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
		private Hidistro.Membership.Context.SiteSettings GetSiteSetting()
		{
			return Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
		}
		private void CallBack()
		{
			bool flag = !string.IsNullOrEmpty(base.Request["showMessage"]) && base.Request["showMessage"] == "true";
			if (!string.IsNullOrEmpty(base.Request["showDistributorAccountSummary"]) && base.Request["showDistributorAccountSummary"] == "true")
			{
				int userId = 0;
				if (string.IsNullOrEmpty(base.Request["id"]) || !int.TryParse(base.Request["id"], out userId))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(userId);
				if (distributor == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.AppendFormat(",\"AccountAmount\":\"{0}\"", distributor.Balance);
				stringBuilder.AppendFormat(",\"UseableBalance\":\"{0}\"", distributor.Balance - distributor.RequestBalance);
				stringBuilder.AppendFormat(",\"FreezeBalance\":\"{0}\"", distributor.RequestBalance);
				stringBuilder.AppendFormat(",\"DrawRequestBalance\":\"{0}\"", distributor.RequestBalance);
				stringBuilder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
				stringBuilder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
				base.Response.End();
			}
			if (flag)
			{
				int userId2 = 0;
				if (string.IsNullOrEmpty(base.Request["id"]) || !int.TryParse(base.Request["id"], out userId2))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				Hidistro.Membership.Context.Distributor distributor2 = DistributorHelper.GetDistributor(userId2);
				if (distributor2 == null)
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				stringBuilder2.AppendFormat(",\"UserName\":\"{0}\"", distributor2.Username);
				stringBuilder2.AppendFormat(",\"RealName\":\"{0}\"", distributor2.RealName);
				stringBuilder2.AppendFormat(",\"CompanyName\":\"{0}\"", distributor2.CompanyName);
				stringBuilder2.AppendFormat(",\"Email\":\"{0}\"", distributor2.Email);
				stringBuilder2.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor2.RegionId, string.Empty));
				stringBuilder2.AppendFormat(",\"Address\":\"{0}\"", distributor2.Address);
				stringBuilder2.AppendFormat(",\"QQ\":\"{0}\"", distributor2.QQ);
				stringBuilder2.AppendFormat(",\"MSN\":\"{0}\"", distributor2.MSN);
				stringBuilder2.AppendFormat(",\"PostCode\":\"{0}\"", distributor2.Zipcode);
				stringBuilder2.AppendFormat(",\"Wangwang\":\"{0}\"", distributor2.Wangwang);
				stringBuilder2.AppendFormat(",\"CellPhone\":\"{0}\"", distributor2.CellPhone);
				stringBuilder2.AppendFormat(",\"Telephone\":\"{0}\"", distributor2.TelPhone);
				stringBuilder2.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor2.CreateDate);
				stringBuilder2.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor2.LastLoginDate);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder2.ToString() + "}");
				base.Response.End();
			}
		}
		private void grdDistributorList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int num = (int)this.grdDistributorList.DataKeys[rowIndex].Value;
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(num);
			if (e.CommandName == "StopCooperation")
			{
				if (siteSettings != null && !siteSettings.Disabled)
				{
					this.ShowMsg("请先暂停该分销商的站点", false);
					return;
				}
				if (DistributorHelper.Delete(num))
				{
					this.DeleteDistributorFile(num);
					this.BindDistributors();
					this.ShowMsg("成功的清除了该分销商及该分销商下的所有数据", true);
					return;
				}
				this.ShowMsg("清除去失败", false);
			}
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userName", this.txtUserName.Text.Trim());
			nameValueCollection.Add("realName", this.txtTrueName.Text.Trim());
			nameValueCollection.Add("gradeId", this.dropGrade.SelectedValue.HasValue ? this.dropGrade.SelectedValue.Value.ToString() : "");
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		private void btnExport_Click(object sender, System.EventArgs e)
		{
			if (this.exportFieldsCheckBoxList.SelectedItem == null)
			{
				this.ShowMsg("请选择需要导出的分销商信息", false);
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
			System.Data.DataTable distributorsNopage = DistributorHelper.GetDistributorsNopage(new DistributorQuery
			{
				GradeId = this.gradeId,
				Username = this.userName,
				RealName = this.realName
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
			foreach (System.Data.DataRow dataRow in distributorsNopage.Rows)
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
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorInfo.csv");
				this.Page.Response.ContentType = "application/octet-stream";
			}
			else
			{
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorInfo.txt");
				this.Page.Response.ContentType = "application/vnd.ms-word";
			}
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
				{
					this.userName = this.Page.Request.QueryString["userName"];
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
				{
					this.realName = this.Page.Request.QueryString["realName"];
				}
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["gradeId"], out value))
				{
					this.gradeId = new int?(value);
				}
				int value2 = 0;
				if (int.TryParse(this.Page.Request.QueryString["LineId"], out value2))
				{
					this.lineId = new int?(value2);
				}
				this.txtUserName.Text = this.userName;
				this.txtTrueName.Text = this.realName;
				this.dropGrade.DataBind();
				this.dropGrade.SelectedValue = this.gradeId;
				return;
			}
			this.userName = this.txtUserName.Text;
			this.realName = this.txtTrueName.Text;
			if (this.dropGrade.SelectedValue.HasValue)
			{
				this.gradeId = new int?(this.dropGrade.SelectedValue.Value);
			}
		}
		private void BindDistributors()
		{
			DistributorQuery distributorQuery = new DistributorQuery();
			distributorQuery.IsApproved = true;
			distributorQuery.PageIndex = this.pager.PageIndex;
			distributorQuery.PageSize = this.pager.PageSize;
			distributorQuery.GradeId = this.gradeId;
			distributorQuery.LineId = this.lineId;
			distributorQuery.Username = this.userName;
			distributorQuery.RealName = this.realName;
			distributorQuery.SortBy = this.grdDistributorList.SortOrderBy;
			if (this.grdDistributorList.SortOrder.ToLower() == "desc")
			{
				distributorQuery.SortOrder = SortAction.Desc;
			}
			DbQueryResult distributors = DistributorHelper.GetDistributors(distributorQuery);
			this.grdDistributorList.DataSource = distributors.Data;
			this.grdDistributorList.DataBind();
            this.pager.TotalRecords = distributors.TotalRecords;
            this.pager1.TotalRecords = distributors.TotalRecords;
		}
		private bool DeleteDistributorFile(int distributorUserId)
		{
			bool result = false;
			string text = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/sites/") + distributorUserId;
			string text2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + distributorUserId;
			if (System.IO.Directory.Exists(text) && System.IO.Directory.Exists(text2))
			{
				try
				{
					ManageDistributor.DeleteFolder(text);
					ManageDistributor.DeleteFolder(text2);
					result = true;
					return result;
				}
				catch
				{
					result = false;
					return result;
				}
			}
			result = true;
			return result;
		}
		private static void DeleteFolder(string dir)
		{
			string[] fileSystemEntries = System.IO.Directory.GetFileSystemEntries(dir);
			for (int i = 0; i < fileSystemEntries.Length; i++)
			{
				string text = fileSystemEntries[i];
				if (System.IO.File.Exists(text))
				{
					System.IO.FileInfo fileInfo = new System.IO.FileInfo(text);
					if (fileInfo.Attributes.ToString().IndexOf("Readonly") != 1)
					{
						fileInfo.Attributes = System.IO.FileAttributes.Normal;
					}
					System.IO.File.Delete(text);
				}
				else
				{
					ManageDistributor.DeleteFolder(text);
				}
			}
			System.IO.Directory.Delete(dir);
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
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdDistributorList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text4 = ((System.Web.UI.DataBoundLiteralControl)gridViewRow.Controls[4].Controls[0]).Text.Trim().Replace("<div></div>", "");
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
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdDistributorList.Rows)
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
			catch (System.Exception ex)
			{
				this.ShowMsg(ex.Message, false);
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
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdDistributorList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text2 = "";
					foreach (object current in gridViewRow.Controls[1].Controls)
					{
						if (!(current is System.Web.UI.WebControls.Literal))
						{
							if (!(current is System.Web.UI.DataBoundLiteralControl))
							{
								continue;
							}
							text2 = ((System.Web.UI.DataBoundLiteralControl)current).Text.Trim();
						}
						else
						{
							text2 = ((System.Web.UI.WebControls.Literal)current).Text.Trim();
						}
						break;
					}
					if (this.IsMembers(text2))
					{
						list.Add(new MessageBoxInfo
						{
							Sernder = "Admin",
							Accepter = text2,
							Title = title,
							Content = text
						});
					}
				}
			}
			if (list.Count > 0)
			{
				NoticeHelper.SendMessageToDistributor(list);
				this.ShowMsg(string.Format("成功给{0}个分销商发送了消息.", list.Count), true);
				return;
			}
			this.ShowMsg("没有要发送的对象", false);
		}
		private bool IsMembers(string name)
		{
			string pattern = "[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
			new System.Text.RegularExpressions.Regex(pattern);
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
	}
}
