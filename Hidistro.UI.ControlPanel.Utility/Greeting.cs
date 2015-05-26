using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class Greeting : WebControl
	{
		private bool displayWeekday = true;
		public bool DisplayWeekday
		{
			get
			{
				return this.displayWeekday;
			}
			set
			{
				this.displayWeekday = value;
			}
		}
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			IUser user = Hidistro.Membership.Context.HiContext.Current.User;
			if (!user.IsAnonymous)
			{
				Label label = new Label();
				label.CssClass = this.CssClass;
				label.Text = string.Format("欢迎回来<b>{0}</b>", user.Username);
				Label expr_4A = label;
				expr_4A.Text += string.Format("，今天是：{0}&nbsp;", DateTime.Now.ToString("yyyy-MM-dd"));
				if (this.DisplayWeekday)
				{
					string[] array = new string[]
					{
						"星期日",
						"星期一",
						"星期二",
						"星期三",
						"星期四",
						"星期五",
						"星期六"
					};
					Label expr_C4 = label;
					expr_C4.Text += array[(int)Convert.ToInt16(DateTime.Now.DayOfWeek)];
				}
				this.Controls.Add(label);
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderChildren(writer);
		}
		protected override void RenderChildren(HtmlTextWriter writer)
		{
			if (this.HasControls())
			{
				for (int i = 0; i < this.Controls.Count; i++)
				{
					this.Controls[i].RenderControl(writer);
				}
			}
		}
	}
}
