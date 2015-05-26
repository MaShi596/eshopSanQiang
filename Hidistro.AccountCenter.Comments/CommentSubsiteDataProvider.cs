using Hidistro.Core;
using System;
namespace Hidistro.AccountCenter.Comments
{
	public abstract class CommentSubsiteDataProvider : CommentDataProvider
	{
		private static readonly CommentSubsiteDataProvider _defaultInstance;
		static CommentSubsiteDataProvider()
		{
			CommentSubsiteDataProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.CommentData,Hidistro.AccountCenter.DistributionData") as CommentSubsiteDataProvider);
		}
		public static CommentSubsiteDataProvider CreateInstance()
		{
			return CommentSubsiteDataProvider._defaultInstance;
		}
	}
}
