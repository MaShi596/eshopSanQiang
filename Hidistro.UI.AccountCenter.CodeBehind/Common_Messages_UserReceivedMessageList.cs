using ASPNET.WebControls;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Messages_UserReceivedMessageList : AscxTemplatedWebControl
	{
		public const string TagID = "Grid_Common_Messages_UserReceivedMessageList";
		private Grid gridMessageList;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.gridMessageList.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.gridMessageList.DataSource = value;
			}
		}
		public Common_Messages_UserReceivedMessageList()
		{
			base.ID = "Grid_Common_Messages_UserReceivedMessageList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Messages_UserReceivedMessageList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.gridMessageList = (Grid)this.FindControl("gridMessageList");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.gridMessageList.DataSource != null)
			{
				this.gridMessageList.DataBind();
			}
		}
	}
}
