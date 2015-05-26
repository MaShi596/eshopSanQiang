using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ClientGroup)]
	public class ClientSet : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton radnewtime;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton radnewday;
		protected System.Web.UI.HtmlControls.HtmlSelect slsnewregist;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioactivyorder;
		protected System.Web.UI.HtmlControls.HtmlSelect slsactivyorder;
		protected System.Web.UI.HtmlControls.HtmlSelect slsactivyorderchar;
		protected System.Web.UI.HtmlControls.HtmlInputText txtactivyorder;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioactivymoney;
		protected System.Web.UI.HtmlControls.HtmlSelect slsactivymoney;
		protected System.Web.UI.HtmlControls.HtmlSelect slsactivymoneychar;
		protected System.Web.UI.HtmlControls.HtmlInputText txtactivymoney;
		protected System.Web.UI.HtmlControls.HtmlSelect slssleep;
		protected System.Web.UI.WebControls.Button btnSaveClientSettings;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveClientSettings.Click += new System.EventHandler(this.btnSaveClientSettings_Click);
			if (!base.IsPostBack)
			{
				System.Collections.Generic.Dictionary<int, MemberClientSet> memberClientSet = MemberHelper.GetMemberClientSet();
				if (memberClientSet.Count == 3)
				{
					foreach (int current in memberClientSet.Keys)
					{
						int num = current;
						switch (num)
						{
						case 1:
							if (memberClientSet[current].StartTime.HasValue)
							{
                                this.calendarStartDate.SelectedDate = new System.DateTime?(memberClientSet[current].StartTime.Value.Date);
							}
							if (memberClientSet[current].EndTime.HasValue)
							{
                                this.calendarEndDate.SelectedDate = new System.DateTime?(memberClientSet[current].EndTime.Value.Date);
							}
							break;
						case 2:
							this.slsnewregist.Value = memberClientSet[current].LastDay.ToString();
							this.radnewday.Checked = true;
							break;
						case 3:
						case 4:
						case 5:
							break;
						case 6:
							this.slsactivyorder.Value = memberClientSet[current].LastDay.ToString();
							this.slsactivyorderchar.Value = memberClientSet[current].ClientChar.ToString();
							this.txtactivyorder.Value = memberClientSet[current].ClientValue.ToString("F0");
							break;
						case 7:
							this.slsactivymoney.Value = memberClientSet[current].LastDay.ToString();
							this.slsactivymoneychar.Value = memberClientSet[current].ClientChar.ToString();
							this.txtactivymoney.Value = memberClientSet[current].ClientValue.ToString("F2");
							this.radioactivymoney.Checked = true;
							break;
						default:
							if (num == 11)
							{
								this.slssleep.Value = memberClientSet[current].LastDay.ToString();
							}
							break;
						}
					}
				}
			}
		}
		protected void btnSaveClientSettings_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.Dictionary<int, MemberClientSet> dictionary = new System.Collections.Generic.Dictionary<int, MemberClientSet>();
			MemberClientSet memberClientSet = new MemberClientSet();
			if (this.radnewtime.Checked)
			{
				memberClientSet.ClientTypeId = 1;
				if (this.calendarStartDate.SelectedDate.HasValue)
				{
					memberClientSet.StartTime = new System.DateTime?(this.calendarStartDate.SelectedDate.Value);
				}
				if (this.calendarEndDate.SelectedDate.HasValue)
				{
					memberClientSet.EndTime = new System.DateTime?(this.calendarEndDate.SelectedDate.Value);
				}
			}
			else
			{
				memberClientSet.ClientTypeId = 2;
				memberClientSet.LastDay = int.Parse(this.slsnewregist.Value);
			}
			dictionary.Add(memberClientSet.ClientTypeId, memberClientSet);
			memberClientSet = new MemberClientSet();
			if (this.radioactivyorder.Checked)
			{
				memberClientSet.ClientTypeId = 6;
				memberClientSet.LastDay = int.Parse(this.slsactivyorder.Value);
				memberClientSet.ClientChar = this.slsactivyorderchar.Value;
				if (!string.IsNullOrEmpty(this.txtactivyorder.Value))
				{
					memberClientSet.ClientValue = decimal.Parse(this.txtactivyorder.Value);
				}
			}
			else
			{
				memberClientSet.ClientTypeId = 7;
				memberClientSet.LastDay = int.Parse(this.slsactivymoney.Value);
				memberClientSet.ClientChar = this.slsactivymoneychar.Value;
				if (!string.IsNullOrEmpty(this.txtactivymoney.Value))
				{
					memberClientSet.ClientValue = decimal.Parse(this.txtactivymoney.Value);
				}
			}
			dictionary.Add(memberClientSet.ClientTypeId, memberClientSet);
			memberClientSet = new MemberClientSet();
			memberClientSet.ClientTypeId = 11;
			memberClientSet.LastDay = int.Parse(this.slssleep.Value);
			dictionary.Add(memberClientSet.ClientTypeId, memberClientSet);
			if (MemberHelper.InsertClientSet(dictionary))
			{
				this.ShowMsg("保存成功", true);
				return;
			}
			this.ShowMsg("保存失败", false);
		}
	}
}
