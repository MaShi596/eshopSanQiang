using System;
using System.Globalization;
namespace Hidistro.UI.Common.Validator
{
	public class MoneyRangeClientValidator : ClientValidator
	{
		private decimal minValue = -79228162514264337593543950335m;
		private decimal maxValue = 79228162514264337593543950335m;
		public decimal MinValue
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
		public decimal MaxValue
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
				Text = string.Format(CultureInfo.InvariantCulture, "appendValid(new MoneyRangeValidator('{0}', {1}, {2}, '{3}', '{4}'));", new object[]
				{
					base.Owner.TargetClientId,
					this.MinValue,
					this.MaxValue,
					this.ErrorMessage,
					string.Empty
				})
			};
		}
	}
}
