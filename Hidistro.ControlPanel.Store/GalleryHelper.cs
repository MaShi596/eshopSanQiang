using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.ControlPanel.Store
{
	public static class GalleryHelper
	{
		public static bool AddPhotoCategory(string name)
		{
			return GalleryProvider.Instance().AddPhotoCategory(name);
		}
		public static int MovePhotoType(List<int> pList, int pTypeId)
		{
			return GalleryProvider.Instance().MovePhotoType(pList, pTypeId);
		}
		public static int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
		{
			return GalleryProvider.Instance().UpdatePhotoCategories(photoCategorys);
		}
		public static bool DeletePhotoCategory(int categoryId)
		{
			return GalleryProvider.Instance().DeletePhotoCategory(categoryId);
		}
		public static System.Data.DataTable GetPhotoCategories()
		{
			return GalleryProvider.Instance().GetPhotoCategories();
		}
		public static void SwapSequence(int categoryId1, int categoryId2)
		{
			GalleryProvider.Instance().SwapSequence(categoryId1, categoryId2);
		}
		public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, PhotoListOrder order)
		{
			Pagination pagination = new Pagination();
			pagination.PageSize = 20;
			pagination.PageIndex = pageIndex;
			pagination.IsCount = true;
			switch (order)
			{
			case PhotoListOrder.UploadTimeDesc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UploadTimeAsc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.NameAsc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.NameDesc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UpdateTimeDesc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UpdateTimeAsc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.SizeDesc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.SizeAsc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Asc;
				break;
			}
			return GalleryProvider.Instance().GetPhotoList(keyword, categoryId, pagination);
		}
		public static bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize)
		{
			return GalleryProvider.Instance().AddPhote(categoryId, photoName, photoPath, fileSize);
		}
		public static bool DeletePhoto(int photoId)
		{
			return GalleryProvider.Instance().DeletePhoto(photoId);
		}
		public static void RenamePhoto(int photoId, string newName)
		{
			GalleryProvider.Instance().RenamePhoto(photoId, newName);
		}
		public static void ReplacePhoto(int photoId, int fileSize)
		{
			GalleryProvider.Instance().ReplacePhoto(photoId, fileSize);
		}
		public static string GetPhotoPath(int photoId)
		{
			return GalleryProvider.Instance().GetPhotoPath(photoId);
		}
		public static int GetPhotoCount()
		{
			return GalleryProvider.Instance().GetPhotoCount();
		}
		public static int GetDefaultPhotoCount()
		{
			return GalleryProvider.Instance().GetDefaultPhotoCount();
		}
	}
}
