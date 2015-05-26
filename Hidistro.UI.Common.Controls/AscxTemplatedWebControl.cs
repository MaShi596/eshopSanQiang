using Hidistro.Membership.Context;
using System;
using System.Globalization;
using System.Web.UI;
namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class AscxTemplatedWebControl : TemplatedWebControl
	{
		private string skinName;
		protected virtual string SkinPath
		{
			get
			{
				if (this.SkinName.StartsWith("/"))
				{
					return HiContext.Current.GetSkinPath() + this.SkinName;
				}
				return HiContext.Current.GetSkinPath() + "/" + this.SkinName;
			}
		}
		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				value = value.ToLower(CultureInfo.InvariantCulture);
				if (!value.EndsWith(".ascx"))
				{
					return;
				}
				this.skinName = value;
			}
		}
		private bool SkinFileExists
		{
			get
			{
				return !string.IsNullOrEmpty(this.SkinName);
			}
		}
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (!this.LoadThemedControl())
			{
				throw new SkinNotFoundException(this.SkinPath);
			}
			this.AttachChildControls();
		}
		protected virtual bool LoadThemedControl()
		{
			if (this.SkinFileExists && this.Page != null)
			{
				Control control = this.Page.LoadControl(this.SkinPath);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}
	}
}
