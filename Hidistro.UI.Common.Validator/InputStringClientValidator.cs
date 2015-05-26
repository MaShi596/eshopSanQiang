using System;
using System.Globalization;
namespace Hidistro.UI.Common.Validator
{
	public class InputStringClientValidator : ClientValidator
	{
		private int lowerBound;
		private int upperBound;
		private string regex;
		public int LowerBound
		{
			get
			{
				return this.lowerBound;
			}
			set
			{
				this.lowerBound = value;
			}
		}
		public int UpperBound
		{
			get
			{
				return this.upperBound;
			}
			set
			{
				this.upperBound = value;
			}
		}
		public string Regex
		{
			get
			{
				return this.regex;
			}
			set
			{
				this.regex = value;
			}
		}
		internal override ValidateRenderControl GenerateInitScript()
		{
			return new ValidateRenderControl
			{
				Text = string.Format(CultureInfo.InvariantCulture, "initValid(new InputValidator('{0}', {1}, {2}, {3}, {4}, '{5}', '{6}'))", new object[]
				{
					base.Owner.TargetClientId,
					this.LowerBound,
					this.UpperBound,
					base.Owner.Nullable ? "true" : "false",
					string.IsNullOrEmpty(this.Regex) ? "null" : ("'" + this.Regex + "'"),
					string.Empty,
					this.ErrorMessage
				})
			};
		}
		internal override ValidateRenderControl GenerateAppendScript()
		{
			return new ValidateRenderControl();
		}
	}
}
