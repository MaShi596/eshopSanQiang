using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Validator
{
	public class ValidatorContainer : WebControl
	{
		public ValidatorContainer()
		{
			this.Controls.Clear();
		}
		public void AddValidatorControl(ValidateRenderControl control)
		{
			if (control != null)
			{
				this.Controls.Add(control);
			}
		}
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			writer.WriteLine("<script type=\"text/javascript\" language=\"javascript\">");
			writer.WriteLine("function InitValidators()");
			writer.WriteLine("{");
		}
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			writer.WriteLine("}");
			writer.WriteLine("$(document).ready(function(){ InitValidators(); });");
			writer.WriteLine("</script>");
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.HasControls())
			{
				this.RenderBeginTag(writer);
				for (int i = 0; i < this.Controls.Count; i++)
				{
					this.Controls[i].RenderControl(writer);
					writer.WriteLine();
				}
				this.RenderEndTag(writer);
			}
		}
	}
}
