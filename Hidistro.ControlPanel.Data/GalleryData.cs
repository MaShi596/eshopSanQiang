using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.ControlPanel.Data
{
	public class GalleryData : GalleryProvider
	{
		private Database database;
		public GalleryData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override int MovePhotoType(List<int> pList, int pTypeId)
		{
			int result;
			if (pList.Count <= 0)
			{
				result = 0;
			}
			else
			{
				string text = string.Empty;
				foreach (int current in pList)
				{
					text = text + current + ",";
				}
				text = text.Remove(text.Length - 1);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_PhotoGallery SET CategoryId = @CategoryId WHERE PhotoId IN ({0})", text));
				this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, pTypeId);
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public override bool AddPhotoCategory(string name)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence INT; SELECT @DisplaySequence = ISNULL(MAX(DisplaySequence), 0) + 1 FROM Hishop_PhotoCategories; INSERT Hishop_PhotoCategories (CategoryName, DisplaySequence) VALUES (@CategoryName, @DisplaySequence)");
			this.database.AddInParameter(sqlStringCommand, "CategoryName", System.Data.DbType.String, name);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
		{
			int result;
			if (photoCategorys.Count <= 0)
			{
				result = 0;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				StringBuilder stringBuilder = new StringBuilder();
				foreach (int current in photoCategorys.Keys)
				{
					string text = current.ToString();
					stringBuilder.AppendFormat("UPDATE Hishop_PhotoCategories SET CategoryName = @CategoryName{0} WHERE CategoryId = {0}", text);
					this.database.AddInParameter(sqlStringCommand, "CategoryName" + text, System.Data.DbType.String, photoCategorys[current]);
				}
				sqlStringCommand.CommandText = stringBuilder.ToString();
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public override bool DeletePhotoCategory(int categoryId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId; UPDATE Hishop_PhotoGallery SET CategoryId = 0 WHERE CategoryId = @CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetPhotoCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT COUNT(PhotoId) FROM Hishop_PhotoGallery WHERE CategoryId = pc.CategoryId) AS PhotoCounts FROM Hishop_PhotoCategories pc ORDER BY DisplaySequence DESC");
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override void SwapSequence(int categoryId1, int categoryId2)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence1 INT , @DisplaySequence2 INT;  SELECT @DisplaySequence1 = DisplaySequence FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId1; SELECT @DisplaySequence2 = DisplaySequence FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId2; UPDATE Hishop_PhotoCategories SET DisplaySequence = @DisplaySequence1 WHERE CategoryId = @CategoryId2; UPDATE Hishop_PhotoCategories SET DisplaySequence = @DisplaySequence2 WHERE CategoryId = @CategoryId1");
			this.database.AddInParameter(sqlStringCommand, "CategoryId1", System.Data.DbType.Int32, categoryId1);
			this.database.AddInParameter(sqlStringCommand, "CategoryId2", System.Data.DbType.Int32, categoryId2);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override DbQueryResult GetPhotoList(string keyword, int? categoryId, Pagination page)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(keyword))
			{
				text += string.Format("PhotoName LIKE '%{0}%'", DataHelper.CleanSearchString(keyword));
			}
			if (categoryId.HasValue)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " AND";
				}
				text += string.Format(" CategoryId = {0}", categoryId.Value);
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_PhotoGallery", "ProductId", text, "*");
		}
		public override bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PhotoGallery(CategoryId, PhotoName, PhotoPath, FileSize, UploadTime, LastUpdateTime) VALUES (@CategoryId, @PhotoName, @PhotoPath, @FileSize, @UploadTime, @LastUpdateTime)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "PhotoName", System.Data.DbType.String, photoName);
			this.database.AddInParameter(sqlStringCommand, "PhotoPath", System.Data.DbType.String, photoPath);
			this.database.AddInParameter(sqlStringCommand, "FileSize", System.Data.DbType.Int32, fileSize);
			this.database.AddInParameter(sqlStringCommand, "UploadTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "LastUpdateTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeletePhoto(int photoId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PhotoGallery WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", System.Data.DbType.Int32, photoId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void RenamePhoto(int photoId, string newName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PhotoGallery SET PhotoName = @PhotoName WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", System.Data.DbType.Int32, photoId);
			this.database.AddInParameter(sqlStringCommand, "PhotoName", System.Data.DbType.String, newName);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void ReplacePhoto(int photoId, int fileSize)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PhotoGallery SET FileSize = @FileSize, LastUpdateTime = @LastUpdateTime WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", System.Data.DbType.Int32, photoId);
			this.database.AddInParameter(sqlStringCommand, "FileSize", System.Data.DbType.Int32, fileSize);
			this.database.AddInParameter(sqlStringCommand, "LastUpdateTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override string GetPhotoPath(int photoId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT PhotoPath FROM Hishop_PhotoGallery WHERE PhotoId = @PhotoId");
			this.database.AddInParameter(sqlStringCommand, "PhotoId", System.Data.DbType.Int32, photoId);
			return this.database.ExecuteScalar(sqlStringCommand).ToString();
		}
		public override int GetPhotoCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PhotoGallery");
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
		public override int GetDefaultPhotoCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PhotoGallery where CategoryId=0");
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
	}
}
