using ASPNET.WebControls;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Messages_UserSendedMessageList : AscxTemplatedWebControl
	{
		public const string TagID = "Grid_Common_Messages_UserSendedMessageList";
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
		public Common_Messages_UserSendedMessageList()
		{
			base.ID = "Grid_Common_Messages_UserSendedMessageList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Messages_UserSendedMessageList.ascx";
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
