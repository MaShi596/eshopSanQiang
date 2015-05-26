using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Favorite_ProductList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e);
		public const string TagID = "list_Common_Favorite_ProList";
		private System.Web.UI.WebControls.DataList dtlstFavorite;
		private System.Web.UI.WebControls.RepeatDirection repeatDirection;
		private int repeatColumns = 1;
		public event Common_Favorite_ProductList.CommandEventHandler ItemCommand;
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
				return this.dtlstFavorite.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dtlstFavorite.DataSource = value;
			}
		}
		public System.Web.UI.WebControls.DataListItemCollection Items
		{
			get
			{
				return this.dtlstFavorite.Items;
			}
		}
		public System.Web.UI.WebControls.DataKeyCollection DataKeys
		{
			get
			{
				return this.dtlstFavorite.DataKeys;
			}
		}
		public int EditItemIndex
		{
			get
			{
				return this.dtlstFavorite.EditItemIndex;
			}
			set
			{
				this.dtlstFavorite.EditItemIndex = value;
			}
		}
		public System.Web.UI.WebControls.RepeatDirection RepeatDirection
		{
			get
			{
				return this.repeatDirection;
			}
			set
			{
				this.repeatDirection = value;
			}
		}
		public int RepeatColumns
		{
			get
			{
				return this.repeatColumns;
			}
			set
			{
				this.repeatColumns = value;
			}
		}
		public Common_Favorite_ProductList()
		{
			base.ID = "list_Common_Favorite_ProList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Favorite_ProductList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dtlstFavorite = (System.Web.UI.WebControls.DataList)this.FindControl("dtlstFavorite");
			this.dtlstFavorite.RepeatDirection = this.RepeatDirection;
			this.dtlstFavorite.RepeatColumns = this.RepeatColumns;
			this.dtlstFavorite.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dtlstFavorite_ItemCommand);
		}
		private void dtlstFavorite_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			this.ItemCommand(sender, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.dtlstFavorite.DataSource = this.DataSource;
			this.dtlstFavorite.DataBind();
		}
	}
}
