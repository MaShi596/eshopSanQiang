using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Comments
{
	public abstract class CommentMasterProvider : CommentProvider
	{
		private static readonly CommentMasterProvider _defaultInstance;
		static CommentMasterProvider()
		{
			CommentMasterProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.CommentData,Hidistro.SaleSystem.Data") as CommentMasterProvider);
		}
		public static CommentMasterProvider CreateInstance()
		{
			return CommentMasterProvider._defaultInstance;
		}
	}
}
