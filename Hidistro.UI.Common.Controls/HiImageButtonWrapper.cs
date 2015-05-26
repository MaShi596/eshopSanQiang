using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class HiImageButtonWrapper : IButton
	{
		private HiImageButton _button;
		public event EventHandler Click
		{
			add
			{
				HiImageButton expr_06 = this._button;
				expr_06.Click = (EventHandler)Delegate.Combine(expr_06.Click, value);
			}
			remove
			{
				HiImageButton expr_06 = this._button;
				expr_06.Click = (EventHandler)Delegate.Remove(expr_06.Click, value);
			}
		}
		public event CommandEventHandler Command
		{
			add
			{
				this._button.Command += value;
			}
			remove
			{
				this._button.Command -= value;
			}
		}
		public bool CausesValidation
		{
			get
			{
				return this._button.CausesValidation;
			}
			set
			{
				this._button.CausesValidation = value;
			}
		}
		public Control Control
		{
			get
			{
				return this._button;
			}
		}
		public bool Visible
		{
			get
			{
				return this._button.Visible;
			}
			set
			{
				this._button.Visible = value;
			}
		}
		public string Text
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}
		public AttributeCollection Attributes
		{
			get
			{
				return this._button.Attributes;
			}
		}
		public string CommandArgument
		{
			get
			{
				return this._button.CommandArgument;
			}
			set
			{
				this._button.CommandArgument = value;
			}
		}
		public string CommandName
		{
			get
			{
				return this._button.CommandName;
			}
			set
			{
				this._button.CommandName = value;
			}
		}
		internal HiImageButtonWrapper(HiImageButton button)
		{
			this._button = button;
		}
	}
}
