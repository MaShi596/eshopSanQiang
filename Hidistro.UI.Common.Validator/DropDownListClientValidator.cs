using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Validator
{
	public class DropDownListClientValidator : ClientValidator
	{
		internal override ValidateRenderControl GenerateInitScript()
		{
			ValidateRenderControl validateRenderControl = new ValidateRenderControl();
			WebControl webControl = (WebControl)base.Owner.NamingContainer.FindControl(base.Owner.ControlToValidate);
			if (webControl != null)
			{
				webControl.Attributes.Add("groupname", base.Owner.TargetClientId);
				validateRenderControl.Text = string.Format(CultureInfo.InvariantCulture, "initValid(new SelectValidator('{0}', {1}, '{2}'))", new object[]
				{
					base.Owner.TargetClientId,
					base.Owner.Nullable ? "true" : "false",
					this.ErrorMessage
				});
			}
			return validateRenderControl;
		}
		internal override ValidateRenderControl GenerateAppendScript()
		{
			return new ValidateRenderControl();
		}
	}
}
