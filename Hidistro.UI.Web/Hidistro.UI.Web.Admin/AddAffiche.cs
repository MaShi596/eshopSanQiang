using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Affiches)]
	public class AddAffiche : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtAfficheTitle;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnAddAffiche;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddAffiche.Click += new System.EventHandler(this.btnAddAffiche_Click);
		}
		private void btnAddAffiche_Click(object sender, System.EventArgs e)
		{
			AfficheInfo afficheInfo = new AfficheInfo();
			afficheInfo.Title = this.txtAfficheTitle.Text.Trim();
			afficheInfo.Content = this.fcContent.Text;
			afficheInfo.AddedDate = System.DateTime.Now;
			ValidationResults validationResults = Validation.Validate<AfficheInfo>(afficheInfo, new string[]
			{
				"ValAfficheInfo"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			if (NoticeHelper.CreateAffiche(afficheInfo))
			{
				this.txtAfficheTitle.Text = string.Empty;
                this.fcContent.Text = string.Empty;
				this.ShowMsg("成功发布了一条公告", true);
				return;
			}
			this.ShowMsg("添加公告失败", false);
		}
	}
}
