using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
namespace Hidistro.Entities.Commodities
{
	public class ProductLineInfo
	{
		public int LineId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValProductLine", MessageTemplate = "产品线名称不能为空，长度限制在1-60个字符之间")]
		public string Name
		{
			get;
			set;
		}
		public string SupplierName
		{
			get;
			set;
		}
	}
}
