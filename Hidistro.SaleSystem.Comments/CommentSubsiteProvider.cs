using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Comments
{
	public abstract class CommentSubsiteProvider : CommentProvider
	{
		private static readonly CommentSubsiteProvider _defaultInstance;
		static CommentSubsiteProvider()
		{
			CommentSubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.CommentData,Hidistro.SaleSystem.DistributionData") as CommentSubsiteProvider);
		}
		public static CommentSubsiteProvider CreateInstance()
		{
			return CommentSubsiteProvider._defaultInstance;
		}
	}
}
