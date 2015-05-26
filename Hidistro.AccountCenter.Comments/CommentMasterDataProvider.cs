using Hidistro.Core;
using System;
namespace Hidistro.AccountCenter.Comments
{
	public abstract class CommentMasterDataProvider : CommentDataProvider
	{
		private static readonly CommentMasterDataProvider _defaultInstance;
		static CommentMasterDataProvider()
		{
			CommentMasterDataProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.CommentData,Hidistro.AccountCenter.Data") as CommentMasterDataProvider);
		}
		public static CommentMasterDataProvider CreateInstance()
		{
			return CommentMasterDataProvider._defaultInstance;
		}
	}
}
