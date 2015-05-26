using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.store
{
	[AdministerCheck(true)]
	public class Backup : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnBackup;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
		}
		private void btnBackup_Click(object sender, System.EventArgs e)
		{
			string text = StoreHelper.BackupData();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("备份数据失败，可能是您的数据库服务器和web服务器不是同一台服务器", false);
				return;
			}
			string text2 = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/" + text);
			System.IO.FileInfo fileInfo = new System.IO.FileInfo(text2);
			if (StoreHelper.InserBackInfo(text, "2.2", fileInfo.Length))
			{
				this.ShowMsg("备份数据成功", true);
				return;
			}
			System.IO.File.Delete(text2);
			this.ShowMsg("备份数据失败，可能是同时备份的人太多，请重试", false);
		}
	}
}
