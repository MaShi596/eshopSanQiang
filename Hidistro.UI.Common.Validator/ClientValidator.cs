using System;
using System.ComponentModel;
namespace Hidistro.UI.Common.Validator
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public abstract class ClientValidator
	{
		private ValidateTarget owner;
		private string errorMessage;
		protected ValidateTarget Owner
		{
			get
			{
				return this.owner;
			}
		}
		public virtual string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
			set
			{
				this.errorMessage = value;
			}
		}
		internal void SetOwner(ValidateTarget owner)
		{
			this.owner = owner;
		}
		internal abstract ValidateRenderControl GenerateInitScript();
		internal abstract ValidateRenderControl GenerateAppendScript();
	}
}
