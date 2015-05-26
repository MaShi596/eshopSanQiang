using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Validator
{
	public class CompareClientValidator : ClientValidator
	{
		private string destinationId;
		public string DestinationId
		{
			get
			{
				return this.destinationId;
			}
			set
			{
				this.destinationId = value;
			}
		}
		internal override ValidateRenderControl GenerateInitScript()
		{
			ValidateRenderControl validateRenderControl = new ValidateRenderControl();
			WebControl webControl = (WebControl)base.Owner.NamingContainer.FindControl(this.DestinationId);
			if (webControl != null)
			{
				validateRenderControl.Text = string.Format(CultureInfo.InvariantCulture, "initValid(new CompareValidator('{0}', '{1}', '{2}'));", new object[]
				{
					base.Owner.TargetClientId,
					webControl.ClientID,
					this.ErrorMessage
				});
			}
			return validateRenderControl;
		}
		internal override ValidateRenderControl GenerateAppendScript()
		{
			ValidateRenderControl validateRenderControl = new ValidateRenderControl();
			WebControl webControl = (WebControl)base.Owner.NamingContainer.FindControl(this.DestinationId);
			if (webControl != null)
			{
				validateRenderControl.Text = string.Format(CultureInfo.InvariantCulture, "appendValid(new CompareValidator('{0}', '{1}', '{2}'));", new object[]
				{
					base.Owner.TargetClientId,
					webControl.ClientID,
					this.ErrorMessage
				});
			}
			return validateRenderControl;
		}
	}
}
