using System;
using System.Globalization;
namespace Hidistro.UI.Common.Validator
{
	public class NumberRangeClientValidator : ClientValidator
	{
		private int minValue = -2147483648;
		private int maxValue = 2147483647;
		public int MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				this.minValue = value;
			}
		}
		public int MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				this.maxValue = value;
			}
		}
		internal override ValidateRenderControl GenerateInitScript()
		{
			return new ValidateRenderControl();
		}
		internal override ValidateRenderControl GenerateAppendScript()
		{
			return new ValidateRenderControl
			{
				Text = string.Format(CultureInfo.InvariantCulture, "appendValid(new NumberRangeValidator('{0}', {1}, {2}, '{3}'));", new object[]
				{
					base.Owner.TargetClientId,
					this.MinValue,
					this.MaxValue,
					this.ErrorMessage
				})
			};
		}
	}
}
