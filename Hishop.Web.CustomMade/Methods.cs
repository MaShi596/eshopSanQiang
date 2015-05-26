using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Shopping;
using Hishop.Web.CustomMade.Supplier;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace Hishop.Web.CustomMade
{
	public class Methods
	{
		private static Database database;
		public static System.Data.DataTable Supplier_aspnet_UserRegionSelect(int userid)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * ", new object[0]);
			stringBuilder.AppendFormat(" from CustomMade_Supplier_UsersRegion ", new object[0]);
			stringBuilder.AppendFormat(" where userid=@userid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, userid);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static void Supplier_aspnet_UserRegionInsert(int UserId, string Province, string City, string Area, int RegionId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" insert into CustomMade_Supplier_UsersRegion(userid,Province,City,Area,RegionId) ", new object[0]);
			stringBuilder.AppendFormat(" values(@UserId,@Province,@City,@Area,@RegionId) ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			Methods.database.AddInParameter(sqlStringCommand, "Province", System.Data.DbType.String, Province);
			Methods.database.AddInParameter(sqlStringCommand, "City", System.Data.DbType.String, City);
			Methods.database.AddInParameter(sqlStringCommand, "Area", System.Data.DbType.String, Area);
			Methods.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, RegionId);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static System.Data.DataTable Supplier_aspnet_UserRegionForRegionId(int UserId, int RegionId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * ", new object[0]);
			stringBuilder.AppendFormat(" from CustomMade_Supplier_UsersRegion ", new object[0]);
			stringBuilder.AppendFormat(" where userid=@userid and RegionId=@RegionId", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, UserId);
			Methods.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, RegionId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static void Supplier_aspnet_UserRegionDelete(int UserId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" delete from CustomMade_Supplier_UsersRegion where Id=@Id", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, UserId);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_ShipPointUpdate(int userid, string remark, int? regionid, string regionname, string realname, string address, string zipcode, string cellphone, string phone)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update aspnet_Users set ", new object[0]);
			stringBuilder.AppendFormat(" Comment = @Comment ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_RegionId = @Supplier_RegionId ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_RegionName = @Supplier_RegionName ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_RealName = @Supplier_RealName ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_Address = @Supplier_Address ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_Zipcode = @Supplier_Zipcode ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_TelPhone = @Supplier_TelPhone ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_CellPhone = @Supplier_CellPhone ", new object[0]);
			stringBuilder.AppendFormat(" where userid=@userid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "Comment", System.Data.DbType.String, remark);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_RegionId", System.Data.DbType.Int32, regionid);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_RegionName", System.Data.DbType.String, regionname);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_RealName", System.Data.DbType.String, realname);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_Address", System.Data.DbType.String, address);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_Zipcode", System.Data.DbType.String, zipcode);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_TelPhone", System.Data.DbType.String, phone);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_CellPhone", System.Data.DbType.String, cellphone);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_SupUpdate(int userid, string remark, int gradeid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update aspnet_Users set ", new object[0]);
			stringBuilder.AppendFormat(" Comment = @Comment ", new object[0]);
			stringBuilder.AppendFormat(" ,Supplier_GradeId = @Supplier_GradeId ", new object[0]);
			stringBuilder.AppendFormat(" where userid=@userid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "Comment", System.Data.DbType.String, remark);
			Methods.database.AddInParameter(sqlStringCommand, "Supplier_GradeId", System.Data.DbType.Int32, gradeid);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_UpdateSupProjectsByUserId(int userid, int _userId, string SupplierName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_Products set ", new object[0]);
			stringBuilder.AppendFormat(" supplierid = @supplierid,SupplierName=@SupplierName", new object[0]);
			stringBuilder.AppendFormat(" where supplierid=@supplierids ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "supplierid", System.Data.DbType.Int32, _userId);
			Methods.database.AddInParameter(sqlStringCommand, "supplierids", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, SupplierName);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static System.Data.DataTable Supplier_SupGet(int userid)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select aspnet_Users.* ", new object[0]);
			stringBuilder.AppendFormat(" ,b.LowestSalePrice,b.SalePrice,b.PurchasePrice,b.Name as SupGradeName ", new object[0]);
			stringBuilder.AppendFormat(" from aspnet_Users ", new object[0]);
			stringBuilder.AppendFormat(" left join CustomMade_Supplier_Grades b on aspnet_Users.Supplier_GradeId=b.auto ", new object[0]);
			stringBuilder.AppendFormat(" where userid=@userid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, userid);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static DbQueryResult Supplier_SGet(ManagerQuery query, string regionname, int? userid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			if (query.RoleId != Guid.Empty)
			{
				stringBuilder.AppendFormat(" AND UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", query.RoleId);
			}
			if (!string.IsNullOrEmpty(regionname))
			{
				stringBuilder.AppendFormat(" AND charindex('{0}',Supplier_RegionName)>0 ", DataHelper.CleanSearchString(regionname));
			}
			if (userid.HasValue && userid > 0)
			{
				stringBuilder.AppendFormat(" AND userid={0} ", userid.Value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Managers", "UserId", stringBuilder.ToString(), "*,(select name from CustomMade_Supplier_Grades where auto=vw_aspnet_Managers.Supplier_GradeId) as SupGradeName");
		}
		public static System.Data.DataTable Supplier_SupShipOrderTjGet(OrderQuery query)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" and datediff(dd,'{0}',ShippingDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" and datediff(dd,'{0}',ShippingDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendFormat(" select ", new object[0]);
			stringBuilder2.AppendFormat(" UserName,UserId ", new object[0]);
			stringBuilder2.AppendFormat(" ,(select isnull(Sum(OrderCostPrice),0) from Hishop_Orders where userid=aspnet_Users.userid and OrderStatus=3 {0}) as price ", stringBuilder.ToString());
			stringBuilder2.AppendFormat(" from aspnet_Users ", new object[0]);
			stringBuilder2.AppendFormat(" where UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '625a27cc-7a55-41d6-8449-c6fe736003e5') ", new object[0]);
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder2.AppendFormat(" and UserName='{0}' ", DataHelper.CleanSearchString(query.UserName));
			}
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder2.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static IList<string> Supplier_SupSGet()
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand("SELECT username FROM vw_aspnet_Managers where UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '625a27cc-7a55-41d6-8449-c6fe736003e5') ");
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader.GetString(0));
				}
			}
			return list;
		}
		public static System.Data.DataTable Supplier_ShipPointShipOrderTjGet(OrderQuery query)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" and datediff(dd,'{0}',ShippingDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" and datediff(dd,'{0}',ShippingDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendFormat(" select ", new object[0]);
			stringBuilder2.AppendFormat(" UserName,UserId ", new object[0]);
			stringBuilder2.AppendFormat(" ,(select isnull(Sum(Amount),0) from Hishop_Orders where userid=aspnet_Users.userid and OrderStatus=3 {0}) as price ", stringBuilder.ToString());
			stringBuilder2.AppendFormat(" from aspnet_Users ", new object[0]);
			stringBuilder2.AppendFormat(" where UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '5a26c830-b998-4569-bffc-c5ceae774a7a') ", new object[0]);
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder2.AppendFormat(" and UserName='{0}' ", DataHelper.CleanSearchString(query.UserName));
			}
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder2.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_SupplierGradeGet(int auto)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * from CustomMade_Supplier_Grades where auto=@auto", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "auto", System.Data.DbType.Int32, auto);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_SupplierGradeInfoGet(int auto)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * from CustomMade_Supplier_GradesInfo where auto=@auto", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "auto", System.Data.DbType.Int32, auto);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_SupplierGradeSGet()
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select *,(select count(*) from aspnet_Users where Supplier_GradeId=CustomMade_Supplier_Grades.auto) as num from CustomMade_Supplier_Grades ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_SupplierGradeInfoSGet()
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * from CustomMade_Supplier_GradesInfo ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static void Supplier_SupplierGradeInsert(string name, decimal LowestSalePrice, decimal SalePrice, decimal PurchasePrice, string Remark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" insert into CustomMade_Supplier_Grades(name,LowestSalePrice,SalePrice,PurchasePrice,Remark) ", new object[0]);
			stringBuilder.AppendFormat(" values(@name,@LowestSalePrice,@SalePrice,@PurchasePrice,@Remark) ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "name", System.Data.DbType.String, name);
			Methods.database.AddInParameter(sqlStringCommand, "LowestSalePrice", System.Data.DbType.Decimal, LowestSalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Decimal, SalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "PurchasePrice", System.Data.DbType.Decimal, PurchasePrice);
			Methods.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, Remark);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_SupplierGradeInfoInsert(string name, decimal LowestSalePrice, decimal SalePrice, decimal PurchasePrice, string Remark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" insert into CustomMade_Supplier_GradesInfo(name,LowestSalePrice,SalePrice,PurchasePrice,Remark) ", new object[0]);
			stringBuilder.AppendFormat(" values(@name,@LowestSalePrice,@SalePrice,@PurchasePrice,@Remark) ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "name", System.Data.DbType.String, name);
			Methods.database.AddInParameter(sqlStringCommand, "LowestSalePrice", System.Data.DbType.Decimal, LowestSalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Decimal, SalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "PurchasePrice", System.Data.DbType.Decimal, PurchasePrice);
			Methods.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, Remark);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_SupplierGradeUpdate(int auto, string name, decimal LowestSalePrice, decimal SalePrice, decimal PurchasePrice, string Remark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update CustomMade_Supplier_Grades set ", new object[0]);
			stringBuilder.AppendFormat(" name=@name, ", new object[0]);
			stringBuilder.AppendFormat(" LowestSalePrice=@LowestSalePrice, ", new object[0]);
			stringBuilder.AppendFormat(" SalePrice=@SalePrice, ", new object[0]);
			stringBuilder.AppendFormat(" PurchasePrice=@PurchasePrice, ", new object[0]);
			stringBuilder.AppendFormat(" Remark=@Remark ", new object[0]);
			stringBuilder.AppendFormat(" where auto=@auto ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "auto", System.Data.DbType.Int32, auto);
			Methods.database.AddInParameter(sqlStringCommand, "name", System.Data.DbType.String, name);
			Methods.database.AddInParameter(sqlStringCommand, "LowestSalePrice", System.Data.DbType.Decimal, LowestSalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Decimal, SalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "PurchasePrice", System.Data.DbType.Decimal, PurchasePrice);
			Methods.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, Remark);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_SupplierGradeInfoUpdate(int auto, string name, decimal LowestSalePrice, decimal SalePrice, decimal PurchasePrice, string Remark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update CustomMade_Supplier_GradesInfo set ", new object[0]);
			stringBuilder.AppendFormat(" name=@name, ", new object[0]);
			stringBuilder.AppendFormat(" LowestSalePrice=@LowestSalePrice, ", new object[0]);
			stringBuilder.AppendFormat(" SalePrice=@SalePrice, ", new object[0]);
			stringBuilder.AppendFormat(" PurchasePrice=@PurchasePrice, ", new object[0]);
			stringBuilder.AppendFormat(" Remark=@Remark ", new object[0]);
			stringBuilder.AppendFormat(" where auto=@auto ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "auto", System.Data.DbType.Int32, auto);
			Methods.database.AddInParameter(sqlStringCommand, "name", System.Data.DbType.String, name);
			Methods.database.AddInParameter(sqlStringCommand, "LowestSalePrice", System.Data.DbType.Decimal, LowestSalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Decimal, SalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "PurchasePrice", System.Data.DbType.Decimal, PurchasePrice);
			Methods.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, Remark);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_ChooseModeHishop_SKUsUpdate(int productId, decimal SalePrice, decimal PurchasePrice, decimal LowestSalePrice)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_SKUs set ", new object[0]);
			stringBuilder.AppendFormat(" SalePrice=(CostPrice*@SalePrice/100), ", new object[0]);
			stringBuilder.AppendFormat(" PurchasePrice=(CostPrice*@PurchasePrice/100) ", new object[0]);
			stringBuilder.AppendFormat(" where productId=@productId; ", new object[0]);
			stringBuilder.AppendFormat(" update Hishop_Products set ", new object[0]);
			stringBuilder.AppendFormat(" LowestSalePrice=((select top 1 CostPrice from Hishop_SKUs where productid=@productid)*@LowestSalePrice/100) ", new object[0]);
			stringBuilder.AppendFormat(" where productId=@productId; ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "productId", System.Data.DbType.Int32, productId);
			Methods.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Decimal, SalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "PurchasePrice", System.Data.DbType.Decimal, PurchasePrice);
			Methods.database.AddInParameter(sqlStringCommand, "LowestSalePrice", System.Data.DbType.Decimal, LowestSalePrice);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_SupplierGradeDelete(int auto)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" delete from CustomMade_Supplier_Grades where auto=@auto ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "auto", System.Data.DbType.Int32, auto);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_SupplierGradeInfoDelete(int auto)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" delete from CustomMade_Supplier_GradesInfo where auto=@auto ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "auto", System.Data.DbType.Int32, auto);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static int Supplier_SupplierGradeHasSupNum(int auto)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select count(*) from aspnet_Users where Supplier_GradeId=@auto ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "auto", System.Data.DbType.Int32, auto);
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public static System.Data.DataTable Supplier_StockInfoSelect(int Stock_Id)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * ", new object[0]);
			stringBuilder.AppendFormat(" from CustomMade_Supplier_StockInfo ", new object[0]);
			stringBuilder.AppendFormat(" where Stock_Id=@Stock_Id ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_StockItemSelect(int Stock_Id)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select a.Id,c.ProductName,c.ProductCode,d.Status,a.SalePrice,a.SkuId,a.Stock_Id,a.Stock,a.UpdateStock,c.ProductId,c.ThumbnailUrl40 ", new object[0]);
			stringBuilder.AppendFormat("  from CustomMade_Supplier_StockItem a,Hishop_SKUs b,Hishop_Products c,CustomMade_Supplier_StockInfo d  ", new object[0]);
			stringBuilder.AppendFormat("  where a.SkuId=b.SkuId and b.ProductId=c.ProductId and a.Stock_Id=@Stock_Id and  a.Stock_Id=d.Stock_Id", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static string Supplier_StockInfoInsert(DateTime AddDate, string Stock_Code, int Status, int AllCount, string Options, int UserId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" insert into CustomMade_Supplier_StockInfo(AddDate,Stock_Code,Status,AllCount,Options,UserId) ", new object[0]);
			stringBuilder.AppendFormat(" values(@AddDate,@Stock_Code,@Status,@AllCount,@Options,@UserId);select @@IDENTITY ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "AddDate", System.Data.DbType.Date, AddDate);
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Code", System.Data.DbType.String, Stock_Code);
			Methods.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, Status);
			Methods.database.AddInParameter(sqlStringCommand, "AllCount", System.Data.DbType.Int32, AllCount);
			Methods.database.AddInParameter(sqlStringCommand, "Options", System.Data.DbType.String, Options);
			Methods.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null && obj != DBNull.Value)
			{
				result = obj.ToString();
			}
			else
			{
				result = "";
			}
			return result;
		}
		public static void Supplier_StockItemInsert(int Stock_Id, string SkuId, int Stock, decimal SalePrice)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" insert into CustomMade_Supplier_StockItem(Stock_Id,SkuId,Stock,SalePrice) ", new object[0]);
			stringBuilder.AppendFormat(" values(@Stock_Id,@SkuId,@Stock,@SalePrice)", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, SkuId);
			Methods.database.AddInParameter(sqlStringCommand, "Stock", System.Data.DbType.Int32, Stock);
			Methods.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Decimal, SalePrice);
			Methods.database.ExecuteScalar(sqlStringCommand);
		}
		public static void Supplier_StockInfoDelete(int Stock_Id)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" delete from CustomMade_Supplier_StockInfo where Stock_Id=@Stock_Id ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_StockItemDelete(int Stock_Id)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" delete from CustomMade_Supplier_StockItem where Stock_Id=@Stock_Id ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_StockInfoUpdate(int Stock_Id, DateTime AddDate, string Stock_Code, int Status, int AllCount, string Options)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update CustomMade_Supplier_StockInfo set ", new object[0]);
			stringBuilder.AppendFormat(" AddDate=@AddDate, ", new object[0]);
			stringBuilder.AppendFormat(" Status=@Status, ", new object[0]);
			stringBuilder.AppendFormat(" AllCount=@AllCount, ", new object[0]);
			stringBuilder.AppendFormat(" Options=@Options, ", new object[0]);
			stringBuilder.AppendFormat(" Remark=@Remark ", new object[0]);
			stringBuilder.AppendFormat(" where Stock_Id=@Stock_Id ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "AddDate", System.Data.DbType.Date, AddDate);
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Code", System.Data.DbType.String, Stock_Code);
			Methods.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, Status);
			Methods.database.AddInParameter(sqlStringCommand, "AllCount", System.Data.DbType.Int32, AllCount);
			Methods.database.AddInParameter(sqlStringCommand, "Options", System.Data.DbType.String, Options);
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			Methods.database.ExecuteScalar(sqlStringCommand);
		}
		public static void Supplier_StockItemUpdate(int Id, int Stock_Id, string SkuId, int Stock, decimal SalePrice)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update CustomMade_Supplier_StockItem set ", new object[0]);
			stringBuilder.AppendFormat(" SalePrice=@SalePrice, ", new object[0]);
			stringBuilder.AppendFormat(" Stock=@Stock, ", new object[0]);
			stringBuilder.AppendFormat(" SkuId=@SkuId, ", new object[0]);
			stringBuilder.AppendFormat(" Stock_Id=@Stock_Id ", new object[0]);
			stringBuilder.AppendFormat(" where Stock_Id=@Stock_Id and Id=@Id ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Decimal, SalePrice);
			Methods.database.AddInParameter(sqlStringCommand, "Stock", System.Data.DbType.Int32, Stock);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, SkuId);
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			Methods.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
			Methods.database.ExecuteScalar(sqlStringCommand);
		}
		public static DbQueryResult Supplier_S_StockInfoGet(Supplier_QueryInfo query, string regionname, int? userid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.Code))
			{
				stringBuilder.AppendFormat(" and Stock_Code='{0}'", query.Code);
			}
			if (userid.HasValue)
			{
				stringBuilder.AppendFormat(" and UserId={0}", userid);
			}
			if (!string.IsNullOrEmpty(query.Status))
			{
				stringBuilder.AppendFormat(" and Status={0}", query.Status);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" and datediff(dd,'{0}',AddDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" and datediff(dd,'{0}',AddDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.DateDay == "1")
			{
				stringBuilder.Append(" and datediff(dd,AddDate,getdate())=0");
			}
			if (query.DateDay == "7")
			{
				stringBuilder.Append(" and datediff(week,AddDate,getdate())=0");
			}
			if (query.DateDay == "30")
			{
				stringBuilder.Append(" and datediff(month,AddDate,getdate())=0");
			}
			if (query.DateDay == "3")
			{
				stringBuilder.Append(" and AddDate >= dateadd(mm,-3,getdate()) ");
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CustomMade_Supplier_StockInfo", "Stock_Id", stringBuilder.ToString(), "*,(case Status when 1 then '入库' when 2 then '出库' end) as StatusName");
		}
		public static void Supplier_StockAddfor_UpdateSkus(string SkuId, int Stock)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_SKUs set ", new object[0]);
			stringBuilder.AppendFormat(" Stock=Stock+@Stock", new object[0]);
			stringBuilder.AppendFormat(" where SkuId=@SkuId", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Stock", System.Data.DbType.String, Stock);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, SkuId);
			Methods.database.ExecuteScalar(sqlStringCommand);
		}
		public static void Supplier_StockAddfor_UpdateStockInfo_Stock(int Stock_Id, int AllCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update CustomMade_Supplier_StockInfo set ", new object[0]);
			stringBuilder.AppendFormat(" AllCount=@AllCount", new object[0]);
			stringBuilder.AppendFormat(" where Stock_Id=@Stock_Id", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "AllCount", System.Data.DbType.Int32, AllCount);
			Methods.database.AddInParameter(sqlStringCommand, "Stock_Id", System.Data.DbType.Int32, Stock_Id);
			Methods.database.ExecuteScalar(sqlStringCommand);
		}
		public static void Supplier_StockAddfor_UpdateStockItem_Stock(int Id, int Stock)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update CustomMade_Supplier_StockItem set ", new object[0]);
			stringBuilder.AppendFormat(" UpdateStock=@UpdateStock", new object[0]);
			stringBuilder.AppendFormat(" where Id=@Id", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "UpdateStock", System.Data.DbType.Int32, Stock);
			Methods.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
			Methods.database.ExecuteScalar(sqlStringCommand);
		}
		public static void Supplier_StockAddfor_UpdateSkus_Stock(string SkuId, int Stock, int Stock_t)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_SKUs set ", new object[0]);
			stringBuilder.AppendFormat(" Stock=(Stock+@Stock-@Stock_t)", new object[0]);
			stringBuilder.AppendFormat(" where SkuId=@SkuId", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "Stock", System.Data.DbType.String, Stock);
			Methods.database.AddInParameter(sqlStringCommand, "Stock_t", System.Data.DbType.String, Stock_t);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, SkuId);
			Methods.database.ExecuteScalar(sqlStringCommand);
		}
		public static System.Data.DataTable Supplier_Hishop_Products(string ProductCode)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select a.ProductId,a.ProductName,b.SalePrice,a.ThumbnailUrl40,b.Stock,a.ProductCode,b.skuid ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_Products a,Hishop_SKUs b ", new object[0]);
			stringBuilder.AppendFormat(" where a.ProductId=b.ProductId and a.ProductCode=@ProductCode ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "ProductCode", System.Data.DbType.String, ProductCode);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_Hishop_OrderItems(string OrderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select SkuId,CostPrice,ShipmentQuantity ", new object[0]);
			stringBuilder.AppendFormat(" ,(select SUM(ShipmentQuantity) from Hishop_OrderItems a where a.OrderId=@OrderId) as nums ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_OrderItems ", new object[0]);
			stringBuilder.AppendFormat(" where OrderId=@OrderId ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, OrderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_Hishop_Products_ProductId(int ProductId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select a.ProductId,a.ProductName,b.SalePrice,a.ThumbnailUrl40,b.Stock,a.ProductCode,b.skuid ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_Products a,Hishop_SKUs b ", new object[0]);
			stringBuilder.AppendFormat(" where a.ProductId=b.ProductId and a.ProductId=@ProductId ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, ProductId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static DbQueryResult Supplier_PtSGet(ProductQuery query, int? checkedstatus, int? userid, int? checkstatus)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (query.ProductLineId.HasValue && query.ProductLineId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.PenetrationStatus != PenetrationStatus.NotSet)
			{
				stringBuilder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (userid.HasValue && userid > 0)
			{
				stringBuilder.AppendFormat(" AND supplierid ={0} ", userid);
			}
			if (checkedstatus.HasValue)
			{
				if (checkedstatus == 0)
				{
					stringBuilder.AppendFormat(" AND checkstatus = 3 ", new object[0]);
				}
				else
				{
					if (checkedstatus == 1)
					{
						stringBuilder.AppendFormat(" AND checkstatus < 3 ", new object[0]);
					}
					else
					{
						if (checkedstatus == 2)
						{
							stringBuilder.AppendFormat(" AND charindex('申请取消通过',CheckRemark)>0 ", new object[0]);
						}
					}
				}
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND typeid ={0} ", query.TypeId.Value);
			}
			if (checkstatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND checkstatus = {0} ", checkstatus.Value);
			}
			string selectFields = "ProductId,ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice,Supplierid,SupplierName,CheckStatus,CheckRemark, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,PenetrationStatus,SaleStatus";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_CustomMade_Supplier_Pts p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public static System.Data.DataTable Supplier_PtGet(int productid)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select ", new object[0]);
			stringBuilder.AppendFormat(" * ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_Products ", new object[0]);
			stringBuilder.AppendFormat(" where productid=@productid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "productid", System.Data.DbType.Int32, productid);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static void Supplier_PtUpdate(int productid, int checkstatus, string checkremark, int userid, string username)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_Products set ", new object[0]);
			stringBuilder.AppendFormat(" Supplierid = @Supplierid ", new object[0]);
			stringBuilder.AppendFormat(" ,SupplierName = @SupplierName ", new object[0]);
			stringBuilder.AppendFormat(" ,CheckStatus = @CheckStatus ", new object[0]);
			stringBuilder.AppendFormat(" ,CheckRemark = @CheckRemark ", new object[0]);
			stringBuilder.AppendFormat(" where productid=@productid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "productid", System.Data.DbType.Int32, productid);
			Methods.database.AddInParameter(sqlStringCommand, "Supplierid", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, username);
			Methods.database.AddInParameter(sqlStringCommand, "CheckStatus", System.Data.DbType.Int32, checkstatus);
			Methods.database.AddInParameter(sqlStringCommand, "CheckRemark", System.Data.DbType.String, checkremark);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_PtCheck(string productIds, int userid)
		{
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET CheckStatus = 1,CheckRemark ='审核中' WHERE ProductId IN ({0}) and Supplierid={1} and CheckStatus<3 ", productIds, userid));
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_PtChecked(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET CheckStatus = 3,CheckRemark ='<span style=\"color:green\">通过</span>' WHERE ProductId IN ({0}) and CheckStatus=1 ", productIds));
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_PtCheckError(string productIds, string remark)
		{
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET CheckStatus = 2,SaleStatus=3,CheckRemark ='<span style=\"color:red\">{1}</span>' WHERE ProductId IN ({0}) ", productIds, remark));
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_PtCheckErrorRefer(string productIds, int userid)
		{
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET CheckRemark = '<span style=\"color:green\">通过</span><div style=\"color:red\">申请取消通过</div>' WHERE ProductId IN ({0}) and Supplierid={1} and CheckStatus=3 ", productIds, userid));
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_PtCheckErrorReferError(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET CheckRemark ='<span style=\"color:green\">通过</span>' WHERE ProductId IN ({0}) and CheckStatus=3 ", productIds));
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_PtCheckErrorReferAgree(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET CheckStatus = 0,SaleStatus=3,CheckRemark ='<span style=\"color:red\">同意取消通过</span>' WHERE ProductId IN ({0}) and CheckStatus=3 ", productIds));
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_PtCheckNumTjGetUpdate(int userid, out int CheckedNum, out int NoCheckNum, out int CheckNum, out int ErrorReferNum)
		{
			CheckNum = 0;
			CheckedNum = 0;
			NoCheckNum = 0;
			ErrorReferNum = 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (userid > 0)
			{
				stringBuilder.AppendFormat(" and supplierid={0} ", userid);
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendFormat(" select count(*) as num from hishop_products where checkstatus=3 {0};", stringBuilder.ToString());
			stringBuilder2.AppendFormat(" select count(*) as num from hishop_products where checkstatus<3 {0};", stringBuilder.ToString());
			stringBuilder2.AppendFormat(" select count(*) as num from hishop_products where checkstatus=1 {0};", stringBuilder.ToString());
			stringBuilder2.AppendFormat(" select count(*) as num from hishop_products where charindex('申请取消通过',CheckRemark)>0 {0};", stringBuilder.ToString());
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder2.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					CheckedNum = (int)dataReader["num"];
				}
				dataReader.NextResult();
				if (dataReader.Read())
				{
					NoCheckNum = (int)dataReader["num"];
				}
				dataReader.NextResult();
				if (dataReader.Read())
				{
					CheckNum = (int)dataReader["num"];
				}
				dataReader.NextResult();
				if (dataReader.Read())
				{
					ErrorReferNum = (int)dataReader["num"];
				}
			}
		}
		public static void Supplier_PtImportProducts(System.Data.DataTable productData, int categoryId, int lineId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
		{
			if (productData != null && productData.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in productData.Rows)
				{
					ProductInfo productInfo = new ProductInfo();
					productInfo.CategoryId = categoryId;
					productInfo.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
					productInfo.ProductName = (string)dataRow["ProductName"];
					productInfo.ProductCode = (string)dataRow["SKU"];
					productInfo.LineId = lineId;
					productInfo.BrandId = brandId;
					if (dataRow["Description"] != DBNull.Value)
					{
						productInfo.Description = (string)dataRow["Description"];
					}
					productInfo.PenetrationStatus = PenetrationStatus.Notyet;
					productInfo.AddedDate = DateTime.Now;
					productInfo.SaleStatus = saleStatus;
					productInfo.HasSKU = false;
					HttpContext current = HttpContext.Current;
					if (dataRow["ImageUrl1"] != DBNull.Value)
					{
						productInfo.ImageUrl1 = (string)dataRow["ImageUrl1"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
					{
						string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl1);
						productInfo.ThumbnailUrl40 = array[0];
						productInfo.ThumbnailUrl60 = array[1];
						productInfo.ThumbnailUrl100 = array[2];
						productInfo.ThumbnailUrl160 = array[3];
						productInfo.ThumbnailUrl180 = array[4];
						productInfo.ThumbnailUrl220 = array[5];
						productInfo.ThumbnailUrl310 = array[6];
						productInfo.ThumbnailUrl410 = array[7];
					}
					if (dataRow["ImageUrl2"] != DBNull.Value)
					{
						productInfo.ImageUrl2 = (string)dataRow["ImageUrl2"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
					{
						string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl2);
					}
					if (dataRow["ImageUrl3"] != DBNull.Value)
					{
						productInfo.ImageUrl3 = (string)dataRow["ImageUrl3"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
					{
						string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl3);
					}
					if (dataRow["ImageUrl4"] != DBNull.Value)
					{
						productInfo.ImageUrl4 = (string)dataRow["ImageUrl4"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
					{
						string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl4);
					}
					if (dataRow["ImageUrl5"] != DBNull.Value)
					{
						productInfo.ImageUrl5 = (string)dataRow["ImageUrl5"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
					{
						string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl5);
					}
					SKUItem sKUItem = new SKUItem();
					sKUItem.SkuId = "_0";
					sKUItem.SKU = (string)dataRow["SKU"];
					productInfo.LowestSalePrice = (sKUItem.PurchasePrice = (sKUItem.SalePrice = (decimal)dataRow["SalePrice"]));
					if (dataRow["Stock"] != DBNull.Value)
					{
						sKUItem.Stock = (int)dataRow["Stock"];
					}
					if (dataRow["Weight"] != DBNull.Value)
					{
						sKUItem.Weight = (decimal)dataRow["Weight"];
					}
					ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, new Dictionary<string, SKUItem>
					{

						{
							sKUItem.SkuId,
							sKUItem
						}
					}, null, null);
					if (productActionStatus == ProductActionStatus.Success)
					{
						Methods.Supplier_PtUpdate(productInfo.ProductId, 0, "仓库中", HiContext.Current.User.UserId, HiContext.Current.User.Username);
						if (isImportFromTaobao)
						{
							TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
							taobaoProductInfo.ProductId = productInfo.ProductId;
							taobaoProductInfo.ProTitle = productInfo.ProductName;
							taobaoProductInfo.Cid = (long)dataRow["Cid"];
							if (dataRow["StuffStatus"] != DBNull.Value)
							{
								taobaoProductInfo.StuffStatus = (string)dataRow["StuffStatus"];
							}
							taobaoProductInfo.Num = (long)dataRow["Num"];
							taobaoProductInfo.LocationState = (string)dataRow["LocationState"];
							taobaoProductInfo.LocationCity = (string)dataRow["LocationCity"];
							taobaoProductInfo.FreightPayer = (string)dataRow["FreightPayer"];
							if (dataRow["PostFee"] != DBNull.Value)
							{
								taobaoProductInfo.PostFee = (decimal)dataRow["PostFee"];
							}
							if (dataRow["ExpressFee"] != DBNull.Value)
							{
								taobaoProductInfo.ExpressFee = (decimal)dataRow["ExpressFee"];
							}
							if (dataRow["EMSFee"] != DBNull.Value)
							{
								taobaoProductInfo.EMSFee = (decimal)dataRow["EMSFee"];
							}
							taobaoProductInfo.HasInvoice = (bool)dataRow["HasInvoice"];
							taobaoProductInfo.HasWarranty = (bool)dataRow["HasWarranty"];
							taobaoProductInfo.HasDiscount = (bool)dataRow["HasDiscount"];
							taobaoProductInfo.ValidThru = (long)dataRow["ValidThru"];
							if (dataRow["ListTime"] != DBNull.Value)
							{
								taobaoProductInfo.ListTime = (DateTime)dataRow["ListTime"];
							}
							else
							{
								taobaoProductInfo.ListTime = DateTime.Now;
							}
							if (dataRow["PropertyAlias"] != DBNull.Value)
							{
								taobaoProductInfo.PropertyAlias = (string)dataRow["PropertyAlias"];
							}
							if (dataRow["InputPids"] != DBNull.Value)
							{
								taobaoProductInfo.InputPids = (string)dataRow["InputPids"];
							}
							if (dataRow["InputStr"] != DBNull.Value)
							{
								taobaoProductInfo.InputStr = (string)dataRow["InputStr"];
							}
							if (dataRow["SkuProperties"] != DBNull.Value)
							{
								taobaoProductInfo.SkuProperties = (string)dataRow["SkuProperties"];
							}
							if (dataRow["SkuQuantities"] != DBNull.Value)
							{
								taobaoProductInfo.SkuQuantities = (string)dataRow["SkuQuantities"];
							}
							if (dataRow["SkuPrices"] != DBNull.Value)
							{
								taobaoProductInfo.SkuPrices = (string)dataRow["SkuPrices"];
							}
							if (dataRow["SkuOuterIds"] != DBNull.Value)
							{
								taobaoProductInfo.SkuOuterIds = (string)dataRow["SkuOuterIds"];
							}
							ProductHelper.UpdateToaobProduct(taobaoProductInfo);
						}
					}
				}
			}
		}
		private static string[] Supplier_PtProcessImages(HttpContext context, string originalSavePath)
		{
			string fileName = Path.GetFileName(originalSavePath);
			string text = "/Storage/master/product/thumbs40/40_" + fileName;
			string text2 = "/Storage/master/product/thumbs60/60_" + fileName;
			string text3 = "/Storage/master/product/thumbs100/100_" + fileName;
			string text4 = "/Storage/master/product/thumbs160/160_" + fileName;
			string text5 = "/Storage/master/product/thumbs180/180_" + fileName;
			string text6 = "/Storage/master/product/thumbs220/220_" + fileName;
			string text7 = "/Storage/master/product/thumbs310/310_" + fileName;
			string text8 = "/Storage/master/product/thumbs410/410_" + fileName;
			string text9 = context.Request.MapPath(Globals.ApplicationPath + originalSavePath);
			if (File.Exists(text9))
			{
				try
				{
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text), 40, 40);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text2), 60, 60);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text3), 100, 100);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text4), 160, 160);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text5), 180, 180);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text6), 220, 220);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text7), 310, 310);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text8), 410, 410);
				}
				catch
				{
				}
			}
			return new string[]
			{
				text,
				text2,
				text3,
				text4,
				text5,
				text6,
				text7,
				text8
			};
		}
		public static void Supplier_PtImportProducts(System.Data.DataSet productData, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeCostPrice, bool includeStock, bool includeImages)
		{
			foreach (System.Data.DataRow dataRow in productData.Tables["products"].Rows)
			{
				int mappedProductId = (int)dataRow["ProductId"];
				ProductInfo productInfo = Methods.Supplier_PtConverToProduct(dataRow, categoryId, lineId, bandId, saleStatus, includeImages);
				Dictionary<string, SKUItem> skus = Methods.Supplier_PtConverToSkus(mappedProductId, productData, includeCostPrice, includeStock);
				Dictionary<int, IList<int>> attrs = Methods.Supplier_PtConvertToAttributes(mappedProductId, productData);
				ProductHelper.AddProduct(productInfo, skus, attrs, null);
				Methods.Supplier_PtUpdate(productInfo.ProductId, 0, "仓库中", HiContext.Current.User.UserId, HiContext.Current.User.Username);
			}
		}
		private static Dictionary<int, IList<int>> Supplier_PtConvertToAttributes(int mappedProductId, System.Data.DataSet productData)
		{
			System.Data.DataRow[] array = productData.Tables["attributes"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			Dictionary<int, IList<int>> result;
			if (array.Length == 0)
			{
				result = null;
			}
			else
			{
				Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					int key = (int)dataRow["SelectedAttributeId"];
					if (!dictionary.ContainsKey(key))
					{
						IList<int> value = new List<int>();
						dictionary.Add(key, value);
					}
					dictionary[key].Add((int)dataRow["SelectedValueId"]);
				}
				result = dictionary;
			}
			return result;
		}
		private static Dictionary<string, SKUItem> Supplier_PtConverToSkus(int mappedProductId, System.Data.DataSet productData, bool includeCostPrice, bool includeStock)
		{
			System.Data.DataRow[] array = productData.Tables["skus"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			Dictionary<string, SKUItem> result;
			if (array.Length == 0)
			{
				result = null;
			}
			else
			{
				Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					string text = (string)dataRow["NewSkuId"];
					SKUItem sKUItem = new SKUItem
					{
						SkuId = text,
						SKU = (string)dataRow["SKU"],
						SalePrice = (decimal)dataRow["SalePrice"],
						PurchasePrice = (decimal)dataRow["PurchasePrice"],
						AlertStock = (int)dataRow["AlertStock"]
					};
					if (dataRow["Weight"] != DBNull.Value)
					{
						sKUItem.Weight = (decimal)dataRow["Weight"];
					}
					if (includeCostPrice && dataRow["CostPrice"] != DBNull.Value)
					{
						sKUItem.CostPrice = (decimal)dataRow["CostPrice"];
					}
					if (includeStock)
					{
						sKUItem.Stock = (int)dataRow["Stock"];
					}
					System.Data.DataRow[] array3 = productData.Tables["skuItems"].Select("NewSkuId='" + text + "' AND MappedProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
					System.Data.DataRow[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						System.Data.DataRow dataRow2 = array4[j];
						sKUItem.SkuItems.Add((int)dataRow2["SelectedAttributeId"], (int)dataRow2["SelectedValueId"]);
					}
					dictionary.Add(text, sKUItem);
				}
				result = dictionary;
			}
			return result;
		}
		private static ProductInfo Supplier_PtConverToProduct(System.Data.DataRow productRow, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeImages)
		{
			ProductInfo productInfo = new ProductInfo
			{
				CategoryId = categoryId,
				TypeId = new int?((int)productRow["SelectedTypeId"]),
				ProductName = (string)productRow["ProductName"],
				ProductCode = (string)productRow["ProductCode"],
				LineId = lineId,
				BrandId = bandId,
				LowestSalePrice = (decimal)productRow["LowestSalePrice"],
				Unit = (string)productRow["Unit"],
				ShortDescription = (string)productRow["ShortDescription"],
				Description = (string)productRow["Description"],
				PenetrationStatus = PenetrationStatus.Notyet,
				Title = (string)productRow["Title"],
				MetaDescription = (string)productRow["Meta_Description"],
				MetaKeywords = (string)productRow["Meta_Keywords"],
				AddedDate = DateTime.Now,
				SaleStatus = saleStatus,
				HasSKU = (bool)productRow["HasSKU"],
				MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
				ImageUrl1 = (string)productRow["ImageUrl1"],
				ImageUrl2 = (string)productRow["ImageUrl2"],
				ImageUrl3 = (string)productRow["ImageUrl3"],
				ImageUrl4 = (string)productRow["ImageUrl4"],
				ImageUrl5 = (string)productRow["ImageUrl5"]
			};
			if (productRow["MarketPrice"] != DBNull.Value)
			{
				productInfo.MarketPrice = new decimal?((decimal)productRow["MarketPrice"]);
			}
			if (includeImages)
			{
				HttpContext current = HttpContext.Current;
				if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
				{
					string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl1);
					productInfo.ThumbnailUrl40 = array[0];
					productInfo.ThumbnailUrl60 = array[1];
					productInfo.ThumbnailUrl100 = array[2];
					productInfo.ThumbnailUrl160 = array[3];
					productInfo.ThumbnailUrl180 = array[4];
					productInfo.ThumbnailUrl220 = array[5];
					productInfo.ThumbnailUrl310 = array[6];
					productInfo.ThumbnailUrl410 = array[7];
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
				{
					string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl2);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
				{
					string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl3);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
				{
					string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl4);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
				{
					string[] array = Methods.Supplier_PtProcessImages(current, productInfo.ImageUrl5);
				}
			}
			return productInfo;
		}
		public static System.Data.DataTable Supplier_ShipPointGetByRegionName(string RegionName)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select top 1 * from aspnet_Users where charindex('{0}',Supplier_RegionName)>0 ", DataHelper.CleanSearchString(RegionName));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_ShipPointGetByUserId(string UserId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select top 1 * from aspnet_Users where UserId='{0}'", DataHelper.CleanSearchString(UserId));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_ShipPointGetByRegionName(string Province, string City, string Area)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(Province))
			{
				stringBuilder.AppendFormat(" select top 1 * from CustomMade_Supplier_UsersRegion where Province=@Province ", new object[0]);
			}
			if (!string.IsNullOrEmpty(City))
			{
				stringBuilder.AppendFormat(" and City=@City", new object[0]);
			}
			if (!string.IsNullOrEmpty(Area))
			{
				stringBuilder.AppendFormat(" and Area=@Area", new object[0]);
			}
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			if (!string.IsNullOrEmpty(Province))
			{
				Methods.database.AddInParameter(sqlStringCommand, "Province", System.Data.DbType.String, Province);
			}
			if (!string.IsNullOrEmpty(City))
			{
				Methods.database.AddInParameter(sqlStringCommand, "City", System.Data.DbType.String, City);
			}
			if (!string.IsNullOrEmpty(Area))
			{
				Methods.database.AddInParameter(sqlStringCommand, "Area", System.Data.DbType.String, Area);
			}
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static DbQueryResult Supplier_OrderSGetForAdmin(OrderQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = Methods.database.GetStoredProcCommand("CustomMade_paginationMax");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" OrderId, OrderDate, UserId, Username, Wangwang, RealName, ShipTo, OrderTotal,ISNULL(GroupBuyId,0) as GroupBuyId,ISNULL(GroupBuyStatus,0) as GroupBuyStatus, PaymentType,ManagerMark, OrderStatus, RefundStatus,ManagerRemark,ISNULL(IsPrinted,0) IsPrinted,Gateway ", new object[0]);
			stringBuilder.AppendFormat(" ,isnull(IsFenPei,0) as IsFenPei ", new object[0]);
			Methods.database.AddInParameter(storedProcCommand, "tblName", System.Data.DbType.String, "Hishop_Orders");
			Methods.database.AddInParameter(storedProcCommand, "strGetFields", System.Data.DbType.String, stringBuilder.ToString());
			Methods.database.AddInParameter(storedProcCommand, "keyFldName", System.Data.DbType.String, "OrderDate");
			Methods.database.AddInParameter(storedProcCommand, "doCount", System.Data.DbType.String, "1");
			Methods.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			Methods.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			Methods.database.AddInParameter(storedProcCommand, "OrderType", System.Data.DbType.Boolean, 1);
			Methods.database.AddInParameter(storedProcCommand, "strWhere", System.Data.DbType.String, Methods.Supplier_OrderSGetForAdminQuery(query));
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(storedProcCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				if (dataReader.Read())
				{
					dbQueryResult.TotalRecords = (int)dataReader["Total"];
				}
			}
			return dbQueryResult;
		}
		private static string Supplier_OrderSGetForAdminQuery(OrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" CHARINDEX('_',OrderId)=0 ", new object[0]);
			if (query.OrderId != string.Empty && query.OrderId != null)
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			else
			{
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.RegionId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
				}
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
				}
				if (query.Status == OrderStatus.History)
				{
					stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderDate < '{2}'", 1, 4, DateTime.Now.AddMonths(-3));
				}
				else
				{
					if (query.Status != OrderStatus.All)
					{
						stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
					}
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (query.ShippingModeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
				}
				if (query.IsPrinted.HasValue)
				{
					stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
				}
			}
			return stringBuilder.ToString();
		}
		public static System.Data.DataTable Supplier_OrderSupGet2(string orderid)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select ShipPointId from hishop_orders where orderid='{0}' ", DataHelper.CleanSearchString(orderid));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_OrderSupGet(string orderid)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * from aspnet_Users ", new object[0]);
			stringBuilder.AppendFormat(" where userid=isnull((select ShipPointId from hishop_orders where orderid='{0}'),0) ", DataHelper.CleanSearchString(orderid));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_OrderItemsGet(string orderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT a.* ", new object[0]);
			stringBuilder.AppendFormat(" ,case when a.Supplierid is null then isnull(b.SupplierName,'主站') when a.Supplierid=0 then '主站' else a.SupplierName end as SupplierNameAuto ", new object[0]);
			stringBuilder.AppendFormat(" ,isnull(b.SupplierName,'主站') as SupplierNamePt ", new object[0]);
			stringBuilder.AppendFormat(" FROM Hishop_OrderItems a ", new object[0]);
			stringBuilder.AppendFormat(" left join Hishop_Products b on a.productid = b.productid ", new object[0]);
			stringBuilder.AppendFormat(" Where OrderId = @OrderId ", new object[0]);
			stringBuilder.AppendFormat(" order by b.Supplierid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static int Supplier_OrderItemSupplieridGet(string orderId, string skuid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT ", new object[0]);
			stringBuilder.AppendFormat(" Supplierid ", new object[0]);
			stringBuilder.AppendFormat(" FROM Hishop_OrderItems ", new object[0]);
			stringBuilder.AppendFormat(" Where OrderId = @OrderId and SkuId=@SkuId ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuid);
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public static System.Data.DataTable Supplier_OrderSuppliersGet(string orderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT ", new object[0]);
			stringBuilder.AppendFormat(" isnull(SupplierId,0) as SupplierId,isnull(SupplierName,'主站') as SupplierName  ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_orderitems  ", new object[0]);
			stringBuilder.AppendFormat(" Where OrderId = @OrderId ", new object[0]);
			stringBuilder.AppendFormat(" group by SupplierId,SupplierName ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static bool Supplier_OrderIsFenPei(string orderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT ", new object[0]);
			stringBuilder.AppendFormat(" orderid  ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_orders  ", new object[0]);
			stringBuilder.AppendFormat(" Where OrderId = @OrderId and IsPrinted=1 ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			return obj != null && obj != DBNull.Value;
		}
		public static void Supplier_OrderShipPointIdUpdate(string orderid, int? userid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_Orders set ", new object[0]);
			stringBuilder.AppendFormat(" ShipPointId = @ShipPointId ", new object[0]);
			stringBuilder.AppendFormat(" where orderid=@orderid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "ShipPointId", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "orderid", System.Data.DbType.String, orderid);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_OrderItemSupplierUpdate(string orderid, string string_0, int? userid, string username)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_OrderItems set ", new object[0]);
			stringBuilder.AppendFormat(" SupplierId = @SupplierId ", new object[0]);
			stringBuilder.AppendFormat(" ,SupplierName = @SupplierName ", new object[0]);
			stringBuilder.AppendFormat(" where orderid=@orderid and SkuId=@SkuId ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "SupplierId", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, username);
			Methods.database.AddInParameter(sqlStringCommand, "orderid", System.Data.DbType.String, orderid);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, string_0);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static System.Data.DataTable Supplier_OrderItemsSupplierMasterUpdate(string orderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" delete from Hishop_Orders where charindex('_{0}',OrderId)>0; ", DataHelper.CleanSearchString(orderId));
			stringBuilder.AppendFormat(" delete from Hishop_Orderitems where charindex('_{0}',OrderId)>0; ", DataHelper.CleanSearchString(orderId));
			stringBuilder.AppendFormat(" update Hishop_orderitems set ", new object[0]);
			stringBuilder.AppendFormat(" SupplierId=(select Supplierid from Hishop_Products where productid=Hishop_orderitems.productid)  ", new object[0]);
			stringBuilder.AppendFormat(" ,SupplierName=(select SupplierName from Hishop_Products where productid=Hishop_orderitems.productid)  ", new object[0]);
			stringBuilder.AppendFormat(" Where OrderId = @OrderId and SupplierId is null ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static void Supplier_OrderItemsSupplierFenPeiOverUpdate(string orderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_orders set ", new object[0]);
			stringBuilder.AppendFormat(" IsFenPei=1  ", new object[0]);
			stringBuilder.AppendFormat(" Where charindex('{0}',OrderId)>0 ", DataHelper.CleanSearchString(orderId));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static string Supplier_OrderItemSupplierUpdate(OrderInfo order)
		{
			Methods.Supplier_OrderItemsSupplierMasterUpdate(order.OrderId);
			System.Data.DataTable dataTable = null;
			System.Data.DataTable dataTable2 = Methods.Supplier_OrderSupGet2(order.OrderId);
			string result;
			if (dataTable2 != null && dataTable2.Rows.Count > 0 && dataTable2.Rows[0]["ShipPointId"] != DBNull.Value && (int)dataTable2.Rows[0]["ShipPointId"] > 0)
			{
				dataTable = Methods.Supplier_SupGet((int)dataTable2.Rows[0]["ShipPointId"]);
				if (dataTable == null || dataTable.Rows.Count == 0)
				{
					result = "错误：失败.";
					return result;
				}
			}
			else
			{
				string[] array = order.ShippingRegion.Split("，".ToCharArray());
				System.Data.DataTable dataTable3 = null;
				if (array.Length == 3)
				{
					dataTable3 = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], array[2]);
					if (dataTable3 == null || dataTable3.Rows.Count == 0)
					{
						dataTable3 = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					}
					if (dataTable3 == null || dataTable3.Rows.Count == 0)
					{
						dataTable3 = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
					}
				}
				if (array.Length == 2)
				{
					dataTable3 = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					if (dataTable3 == null || dataTable3.Rows.Count == 0)
					{
						dataTable3 = Methods.Supplier_ShipPointGetByRegionName(array[0]);
					}
				}
				if (array.Length == 1)
				{
					dataTable3 = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
				}
				if (dataTable3 != null && dataTable3.Rows.Count > 0)
				{
					dataTable = Methods.Supplier_SupGet((int)dataTable3.Rows[0]["userid"]);
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						result = "错误：失败.";
						return result;
					}
				}
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(order.OrderId);
				orderInfo.OrderId = dataTable.Rows[0]["userid"].ToString() + "_" + orderInfo.OrderId;
				orderInfo.UserId = (int)dataTable.Rows[0]["userid"];
				orderInfo.Username = dataTable.Rows[0]["username"].ToString();
				orderInfo.CouponCode = string.Empty;
				orderInfo.Gateway = string.Empty;
				orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
				if (!ShoppingProcessor.CreatOrder(orderInfo))
				{
					result = "错误：失败..";
					return result;
				}
				Methods.Supplier_ShipOrderSupplierTypeUpdate(orderInfo.OrderId, 1);
			}
			System.Data.DataTable dataTable4 = Methods.Supplier_OrderSuppliersGet(order.OrderId);
			if (dataTable4 != null && dataTable4.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable4.Rows)
				{
					int num = 0;
					string username = "主站";
					if (dataRow["SupplierId"] != DBNull.Value)
					{
						num = (int)dataRow["SupplierId"];
						username = (string)dataRow["SupplierName"];
					}
					if (num == 0)
					{
						username = "主站";
					}
					OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(order.OrderId);
					orderInfo2.OrderId = num + "_" + orderInfo2.OrderId;
					orderInfo2.UserId = num;
					orderInfo2.Username = username;
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						System.Data.DataRow dataRow2 = dataTable.Rows[0];
						if (dataRow2["Supplier_RegionId"] != DBNull.Value)
						{
							orderInfo2.RegionId = (int)dataRow2["Supplier_RegionId"];
							orderInfo2.ShippingRegion = RegionHelper.GetFullRegion(orderInfo2.RegionId, "，");
						}
						if (dataRow2["Supplier_RealName"] != DBNull.Value)
						{
							orderInfo2.ShipTo = (string)dataRow2["Supplier_RealName"];
						}
						if (dataRow2["Supplier_Address"] != DBNull.Value)
						{
							orderInfo2.Address = (string)dataRow2["Supplier_Address"];
						}
						if (dataRow2["Supplier_Zipcode"] != DBNull.Value)
						{
							orderInfo2.ZipCode = (string)dataRow2["Supplier_Zipcode"];
						}
						if (dataRow2["Supplier_TelPhone"] != DBNull.Value)
						{
							orderInfo2.TelPhone = (string)dataRow2["Supplier_TelPhone"];
						}
						if (dataRow2["Supplier_CellPhone"] != DBNull.Value)
						{
							orderInfo2.CellPhone = (string)dataRow2["Supplier_CellPhone"];
						}
					}
					orderInfo2.LineItems.Clear();
					foreach (string current in order.LineItems.Keys)
					{
						int num2 = Methods.Supplier_OrderItemSupplieridGet(order.OrderId, current);
						if (num2 == num)
						{
							orderInfo2.LineItems.Add(current, order.LineItems[current]);
						}
					}
					if (num != 0)
					{
						orderInfo2.Gifts.Clear();
					}
					orderInfo2.CouponCode = string.Empty;
					orderInfo2.Gateway = string.Empty;
					orderInfo2.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					if (!ShoppingProcessor.CreatOrder(orderInfo2))
					{
						result = "错误：失败.";
						return result;
					}
					Methods.Supplier_ShipOrderSupplierTypeUpdate(orderInfo2.OrderId, 0);
				}
			}
			result = "true";
			return result;
		}
		public static DbQueryResult Supplier_POrderSGetForAdmin(PurchaseOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" 0=0 and CHARINDEX('_',PurchaseOrderId)=0 ", query.PurchaseOrderId);
			if (!string.IsNullOrEmpty(query.PurchaseOrderId))
			{
				stringBuilder.AppendFormat("PurchaseOrderId = '{0}'", query.PurchaseOrderId);
			}
			else
			{
				if (!string.IsNullOrEmpty(query.DistributorName))
				{
					stringBuilder.AppendFormat(" AND DistributorName = '{0}'", query.DistributorName);
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", query.ShipTo);
				}
				if (!string.IsNullOrEmpty(query.OrderId))
				{
					stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					stringBuilder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND PurchaseDate >= '{0}'", query.StartDate.Value);
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND PurchaseDate <= '{0}'", query.EndDate.Value);
				}
				if (query.PurchaseStatus != OrderStatus.All)
				{
					stringBuilder.AppendFormat(" AND PurchaseStatus ={0}", Convert.ToInt32(query.PurchaseStatus));
				}
				if (query.ShippingModeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
				}
				if (query.IsPrinted.HasValue)
				{
					stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
				}
			}
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = Methods.database.GetStoredProcCommand("CustomMade_paginationMax");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendFormat(" OrderId, PurchaseOrderId, PurchaseDate,PayDate,RefundStatus, ShipTo, OrderTotal, PurchaseTotal,AdjustedDiscount, PurchaseStatus,Distributorname,DistributorWangwang,ManagerMark,ManagerRemark,DistributorId,ISNULL(IsPrinted,0) IsPrinted ", new object[0]);
			stringBuilder2.AppendFormat(" ,TaobaoOrderId ", new object[0]);
			Methods.database.AddInParameter(storedProcCommand, "tblName", System.Data.DbType.String, "Hishop_PurchaseOrders");
			Methods.database.AddInParameter(storedProcCommand, "strGetFields", System.Data.DbType.String, stringBuilder2.ToString());
			Methods.database.AddInParameter(storedProcCommand, "keyFldName", System.Data.DbType.String, "PurchaseDate");
			Methods.database.AddInParameter(storedProcCommand, "doCount", System.Data.DbType.String, "1");
			Methods.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			Methods.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			Methods.database.AddInParameter(storedProcCommand, "OrderType", System.Data.DbType.Boolean, 1);
			Methods.database.AddInParameter(storedProcCommand, "strWhere", System.Data.DbType.String, stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(storedProcCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				if (dataReader.Read())
				{
					dbQueryResult.TotalRecords = (int)dataReader["Total"];
				}
			}
			return dbQueryResult;
		}
		public static System.Data.DataTable Supplier_POrderSupGet2(string orderid)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select isnull(ShipPointId,0) as ShipPointId from Hishop_PurchaseOrders where PurchaseOrderId='{0}' ", DataHelper.CleanSearchString(orderid));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_POrderSupGet(string orderid)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select * from aspnet_Users ", new object[0]);
			stringBuilder.AppendFormat(" where userid=isnull((select ShipPointId from Hishop_PurchaseOrders where PurchaseOrderId='{0}'),0) ", DataHelper.CleanSearchString(orderid));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static System.Data.DataTable Supplier_POrderItemsGet(string orderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT a.* ", new object[0]);
			stringBuilder.AppendFormat(" ,case when a.Supplierid is null then isnull(b.SupplierName,'主站') when a.Supplierid=0 then '主站' else a.SupplierName end as SupplierNameAuto ", new object[0]);
			stringBuilder.AppendFormat(" ,isnull(b.SupplierName,'主站') as SupplierNamePt ", new object[0]);
			stringBuilder.AppendFormat(" FROM Hishop_PurchaseOrderItems a ", new object[0]);
			stringBuilder.AppendFormat(" left join Hishop_Products b on a.productid = b.productid ", new object[0]);
			stringBuilder.AppendFormat(" Where PurchaseOrderId = @OrderId ", new object[0]);
			stringBuilder.AppendFormat(" order by b.Supplierid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static bool Supplier_POrderIsFenPei(string orderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT ", new object[0]);
			stringBuilder.AppendFormat(" PurchaseOrderId  ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_PurchaseOrders  ", new object[0]);
			stringBuilder.AppendFormat(" Where PurchaseOrderId = @OrderId and IsFenPei=1 ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			return obj != null && obj != DBNull.Value;
		}
		public static System.Data.DataTable Supplier_POrderSuppliersGet(string orderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT ", new object[0]);
			stringBuilder.AppendFormat(" isnull(SupplierId,0) as SupplierId,isnull(SupplierName,'主站') as SupplierName  ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_PurchaseOrderItems  ", new object[0]);
			stringBuilder.AppendFormat(" Where PurchaseOrderId = @OrderId ", new object[0]);
			stringBuilder.AppendFormat(" group by SupplierId,SupplierName ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static int Supplier_POrderItemSupplieridGet(string orderId, string skuid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT ", new object[0]);
			stringBuilder.AppendFormat(" Supplierid ", new object[0]);
			stringBuilder.AppendFormat(" FROM Hishop_PurchaseOrderItems ", new object[0]);
			stringBuilder.AppendFormat(" Where PurchaseOrderId = @OrderId and SkuId=@SkuId ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuid);
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public static void Supplier_POrderShipPointIdUpdate(string orderid, int? userid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_PurchaseOrders set ", new object[0]);
			stringBuilder.AppendFormat(" ShipPointId = @ShipPointId ", new object[0]);
			stringBuilder.AppendFormat(" where PurchaseOrderId=@orderid ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "ShipPointId", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "orderid", System.Data.DbType.String, orderid);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_POrderItemSupplierUpdate(string orderid, string string_0, int? userid, string username)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_PurchaseOrderItems set ", new object[0]);
			stringBuilder.AppendFormat(" SupplierId = @SupplierId ", new object[0]);
			stringBuilder.AppendFormat(" ,SupplierName = @SupplierName ", new object[0]);
			stringBuilder.AppendFormat(" where PurchaseOrderId=@orderid and SkuId=@SkuId ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "SupplierId", System.Data.DbType.Int32, userid);
			Methods.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, username);
			Methods.database.AddInParameter(sqlStringCommand, "orderid", System.Data.DbType.String, orderid);
			Methods.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, string_0);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static System.Data.DataTable Supplier_POrderItemsSupplierMasterUpdate(string orderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" delete from Hishop_Orders where charindex('_{0}',OrderId)>0; ", DataHelper.CleanSearchString(orderId));
			stringBuilder.AppendFormat(" delete from Hishop_Orderitems where charindex('_{0}',OrderId)>0; ", DataHelper.CleanSearchString(orderId));
			stringBuilder.AppendFormat(" update Hishop_PurchaseOrderItems set ", new object[0]);
			stringBuilder.AppendFormat(" SupplierId=(select Supplierid from Hishop_Products where productid=Hishop_PurchaseOrderItems.productid)  ", new object[0]);
			stringBuilder.AppendFormat(" ,SupplierName=(select SupplierName from Hishop_Products where productid=Hishop_PurchaseOrderItems.productid)  ", new object[0]);
			stringBuilder.AppendFormat(" Where PurchaseOrderId = @OrderId and SupplierId is null ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static OrderInfo Supplier_POrderToOrder(PurchaseOrderInfo porder)
		{
			OrderInfo orderInfo = new OrderInfo();
			if (porder.PurchaseOrderItems.Count > 0)
			{
				foreach (PurchaseOrderItemInfo current in porder.PurchaseOrderItems)
				{
					LineItemInfo lineItemInfo = new LineItemInfo();
					lineItemInfo.SkuId = current.SkuId;
					lineItemInfo.ProductId = current.ProductId;
					lineItemInfo.SKU = current.SKU;
					lineItemInfo.Quantity = current.Quantity;
					lineItemInfo.ShipmentQuantity = current.Quantity;
					lineItemInfo.ItemCostPrice = current.ItemCostPrice;
					lineItemInfo.ItemListPrice = current.ItemPurchasePrice;
					lineItemInfo.ItemAdjustedPrice = current.ItemListPrice;
					lineItemInfo.ItemDescription = current.ItemDescription;
					lineItemInfo.ThumbnailsUrl = current.ThumbnailsUrl;
					lineItemInfo.ItemWeight = current.ItemWeight;
					lineItemInfo.SKUContent = current.SKUContent;
					orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
				}
			}
			orderInfo.Tax = 0.00m;
			orderInfo.InvoiceTitle = "";
			if (porder.PurchaseOrderGifts.Count > 0)
			{
				foreach (PurchaseOrderGiftInfo current2 in porder.PurchaseOrderGifts)
				{
					OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
					orderGiftInfo.GiftId = current2.GiftId;
					orderGiftInfo.GiftName = current2.GiftName;
					orderGiftInfo.Quantity = current2.Quantity;
					orderGiftInfo.ThumbnailsUrl = current2.ThumbnailsUrl;
					orderGiftInfo.CostPrice = current2.PurchasePrice;
					orderInfo.Gifts.Add(orderGiftInfo);
				}
			}
			orderInfo.OrderId = porder.PurchaseOrderId;
			orderInfo.Remark = porder.Remark;
			orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
			orderInfo.RefundStatus = RefundStatus.None;
			orderInfo.RegionId = porder.RegionId;
			orderInfo.ShippingRegion = porder.ShippingRegion;
			orderInfo.Address = porder.Address;
			orderInfo.ZipCode = porder.ZipCode;
			orderInfo.ShipTo = porder.ShipTo;
			orderInfo.TelPhone = porder.TelPhone;
			orderInfo.CellPhone = porder.CellPhone;
			orderInfo.ShipToDate = porder.ShipToDate;
			orderInfo.ShippingModeId = porder.ShippingModeId;
			orderInfo.ModeName = orderInfo.ModeName;
			return orderInfo;
		}
		public static void Supplier_POrderItemsSupplierFenPeiOverUpdate(string orderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_PurchaseOrders set ", new object[0]);
			stringBuilder.AppendFormat(" IsPrinted=1  ", new object[0]);
			stringBuilder.AppendFormat(" Where charindex('{0}',PurchaseOrderId)>0 ", DataHelper.CleanSearchString(orderId));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static string Supplier_POrderItemSupplierUpdate(PurchaseOrderInfo porder)
		{
			OrderInfo orderInfo = Methods.Supplier_POrderToOrder(SalesHelper.GetPurchaseOrder(porder.PurchaseOrderId));
			Methods.Supplier_POrderItemsSupplierMasterUpdate(orderInfo.OrderId);
			int num = 0;
			System.Data.DataTable dataTable = null;
			System.Data.DataTable dataTable2 = Methods.Supplier_POrderSupGet2(orderInfo.OrderId);
			string result;
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				string username = string.Empty;
				if (dataTable2.Rows[0]["ShipPointId"] == DBNull.Value || (int)dataTable2.Rows[0]["ShipPointId"] == 0)
				{
					string[] array = orderInfo.ShippingRegion.Split("，".ToCharArray());
					if (array.Length == 3)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], array[2]);
						if (dataTable == null || dataTable.Rows.Count == 0)
						{
							dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
						}
						if (dataTable == null || dataTable.Rows.Count == 0)
						{
							dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
						}
					}
					if (array.Length == 2)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
						if (dataTable == null || dataTable.Rows.Count == 0)
						{
							dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0]);
						}
					}
					if (array.Length == 1)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
					}
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						dataTable = Methods.Supplier_SupGet((int)dataTable.Rows[0]["userid"]);
						if (dataTable == null || dataTable.Rows.Count == 0)
						{
							result = "错误：失败.";
							return result;
						}
						num = (int)dataTable.Rows[0]["userid"];
						username = (string)dataTable.Rows[0]["username"];
					}
				}
				else
				{
					if ((int)dataTable2.Rows[0]["ShipPointId"] > 0)
					{
						num = (int)dataTable2.Rows[0]["ShipPointId"];
						dataTable = Methods.Supplier_SupGet(num);
						if (dataTable == null || dataTable.Rows.Count == 0)
						{
							result = "错误：失败.";
							return result;
						}
						username = (string)dataTable.Rows[0]["username"];
					}
				}
				if (num > 0)
				{
					OrderInfo orderInfo2 = Methods.Supplier_POrderToOrder(SalesHelper.GetPurchaseOrder(orderInfo.OrderId));
					orderInfo2.OrderId = num + "_" + orderInfo2.OrderId;
					orderInfo2.UserId = num;
					orderInfo2.Username = username;
					orderInfo2.OrderDate = DateTime.Now;
					orderInfo2.CouponCode = string.Empty;
					orderInfo2.Gateway = string.Empty;
					orderInfo2.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					if (!ShoppingProcessor.CreatOrder(orderInfo2))
					{
						result = "错误：失败..";
						return result;
					}
					Methods.Supplier_ShipOrderSupplierTypeUpdate(orderInfo2.OrderId, 1);
				}
			}
			System.Data.DataTable dataTable3 = Methods.Supplier_POrderSuppliersGet(orderInfo.OrderId);
			if (dataTable3 != null && dataTable3.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable3.Rows)
				{
					int num2 = 0;
					string username = "主站";
					if (dataRow["SupplierId"] != DBNull.Value)
					{
						num2 = (int)dataRow["SupplierId"];
						username = (string)dataRow["SupplierName"];
					}
					if (num2 == 0)
					{
						username = "主站";
					}
					OrderInfo orderInfo3 = Methods.Supplier_POrderToOrder(SalesHelper.GetPurchaseOrder(orderInfo.OrderId));
					orderInfo3.OrderId = num2 + "_" + orderInfo3.OrderId;
					orderInfo3.UserId = num2;
					orderInfo3.Username = username;
					orderInfo3.OrderDate = DateTime.Now;
					if (num > 0)
					{
						System.Data.DataRow dataRow2 = dataTable.Rows[0];
						if (dataRow2["Supplier_RegionId"] != DBNull.Value)
						{
							orderInfo3.RegionId = (int)dataRow2["Supplier_RegionId"];
							orderInfo3.ShippingRegion = RegionHelper.GetFullRegion(orderInfo3.RegionId, "，");
						}
						if (dataRow2["Supplier_RealName"] != DBNull.Value)
						{
							orderInfo3.ShipTo = (string)dataRow2["Supplier_RealName"];
						}
						if (dataRow2["Supplier_Address"] != DBNull.Value)
						{
							orderInfo3.Address = (string)dataRow2["Supplier_Address"];
						}
						if (dataRow2["Supplier_Zipcode"] != DBNull.Value)
						{
							orderInfo3.ZipCode = (string)dataRow2["Supplier_Zipcode"];
						}
						if (dataRow2["Supplier_TelPhone"] != DBNull.Value)
						{
							orderInfo3.TelPhone = (string)dataRow2["Supplier_TelPhone"];
						}
						if (dataRow2["Supplier_CellPhone"] != DBNull.Value)
						{
							orderInfo3.CellPhone = (string)dataRow2["Supplier_CellPhone"];
						}
					}
					orderInfo3.LineItems.Clear();
					foreach (string current in orderInfo.LineItems.Keys)
					{
						int num3 = Methods.Supplier_POrderItemSupplieridGet(orderInfo.OrderId, current);
						if (num3 == num2)
						{
							orderInfo3.LineItems.Add(current, orderInfo.LineItems[current]);
						}
					}
					if (num2 != 0)
					{
						orderInfo3.Gifts.Clear();
					}
					orderInfo3.CouponCode = string.Empty;
					orderInfo3.Gateway = string.Empty;
					orderInfo3.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					if (!ShoppingProcessor.CreatOrder(orderInfo3))
					{
						result = "错误：失败.";
						return result;
					}
					Methods.Supplier_ShipOrderSupplierTypeUpdate(orderInfo3.OrderId, 0);
				}
			}
			result = "true";
			return result;
		}
		public static OrderInfo Supplier_DisOrderGet(string orderId)
		{
			OrderInfo orderInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand("SELECT * FROM distro_Orders Where OrderId = @OrderId ; SELECT * FROM distro_OrderGifts Where OrderId = @OrderId; SELECT * FROM distro_OrderItems Where OrderId = @OrderId ");
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader);
					orderInfo.Gifts.Add(item);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.LineItems.Add((string)dataReader["SkuId"], DataMapper.PopulateLineItem(dataReader));
				}
			}
			return orderInfo;
		}
		public static bool Supplier_DisSendGoods(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand("UPDATE distro_Orders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, OrderStatus = @OrderStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName, ExpressCompanyAbb = @ExpressCompanyAbb WHERE OrderId = @OrderId ");
			Methods.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, order.ShipOrderNumber);
			Methods.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, order.RealShippingModeId);
			Methods.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, order.RealModeName);
			Methods.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
			Methods.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, order.ExpressCompanyName);
			Methods.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, order.ExpressCompanyAbb);
			Methods.database.AddInParameter(sqlStringCommand, "ShippingDate", System.Data.DbType.DateTime, DateTime.Now);
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			return Methods.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public static PaymentModeInfo Supplier_DisPaymentModeGet(int modeId, int disid)
		{
			PaymentModeInfo result = new PaymentModeInfo();
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE ModeId = @ModeId AND DistributorUserId=@DistributorUserId");
			Methods.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			Methods.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, disid);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public static void Supplier_DisOrderItemsSupplierFenPeiOverUpdate(string orderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update distro_Orders set ", new object[0]);
			stringBuilder.AppendFormat(" IsFenPei=1  ", new object[0]);
			stringBuilder.AppendFormat(" Where charindex('{0}',OrderId)>0 ", DataHelper.CleanSearchString(orderId));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static void Supplier_ShipOrderSupplierTypeUpdate(string orderId, int SupplierType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_orders set ", new object[0]);
			stringBuilder.AppendFormat(" SupplierType=@SupplierType  ", new object[0]);
			stringBuilder.AppendFormat(" Where OrderId=@OrderId ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			Methods.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			Methods.database.AddInParameter(sqlStringCommand, "SupplierType", System.Data.DbType.Int32, SupplierType);
			Methods.database.ExecuteNonQuery(sqlStringCommand);
		}
		public static DbQueryResult Supplier_ShipOrderSGet(OrderQuery query, int userid)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = Methods.database.GetStoredProcCommand("CustomMade_paginationMax");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" OrderId, OrderDate,OrderStatus, amount as OrderTotal,ManagerMark,ManagerRemark,shiptodate,remark,RealModeName,ShipOrderNumber ", new object[0]);
			stringBuilder.AppendFormat(" ,ShipTo,CellPhone,TelPhone,ShippingRegion,Address,ZipCode,ModeName,OrderCostPrice,UserName,ShippingDate,Amount ", new object[0]);
			stringBuilder.AppendFormat(" ,ISNULL(IsPrinted,0) IsPrinted ", new object[0]);
			string text = Methods.Supplier_ShipOrderSGetQuery(query);
			if (userid > 0)
			{
				text += string.Format(" and userid = {0} ", userid);
			}
			Methods.database.AddInParameter(storedProcCommand, "tblName", System.Data.DbType.String, "Hishop_Orders");
			Methods.database.AddInParameter(storedProcCommand, "strGetFields", System.Data.DbType.String, stringBuilder.ToString());
			Methods.database.AddInParameter(storedProcCommand, "keyFldName", System.Data.DbType.String, "OrderDate");
			Methods.database.AddInParameter(storedProcCommand, "doCount", System.Data.DbType.String, "1");
			Methods.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			Methods.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			Methods.database.AddInParameter(storedProcCommand, "OrderType", System.Data.DbType.Boolean, 1);
			Methods.database.AddInParameter(storedProcCommand, "strWhere", System.Data.DbType.String, text);
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(storedProcCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				if (dataReader.Read())
				{
					dbQueryResult.TotalRecords = (int)dataReader["Total"];
				}
			}
			return dbQueryResult;
		}
		private static string Supplier_ShipOrderSGetQuery(OrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" 0=0 ", new object[0]);
			if (HiContext.Current.User.IsInRole("供应商") || HiContext.Current.User.IsInRole("区域发货点"))
			{
				stringBuilder.AppendFormat(" AND  userid  = {0} ", HiContext.Current.User.UserId);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
			}
			stringBuilder.AppendFormat(" and charindex('_',orderid)>0 ", new object[0]);
			if (query.OrderId != string.Empty && query.OrderId != null)
			{
				stringBuilder.AppendFormat(" AND charindex('{0}',orderid)>0 ", DataHelper.CleanSearchString(query.OrderId));
			}
			else
			{
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.RegionId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
				}
				if (query.Status == OrderStatus.History)
				{
					stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderDate < '{2}'", 1, 4, DateTime.Now.AddMonths(-3));
				}
				else
				{
					if (query.Status != OrderStatus.All)
					{
						stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
					}
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (query.ShippingModeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
				}
				if (query.IsPrinted.HasValue)
				{
					stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
				}
			}
			return stringBuilder.ToString();
		}
		public static decimal Supplier_ShipOrderPriceAllGet(OrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select ", new object[0]);
			stringBuilder.AppendFormat(" sum(amount) ", new object[0]);
			stringBuilder.AppendFormat(" from hishop_orders ", new object[0]);
			stringBuilder.AppendFormat(" where {0} and orderstatus = 3 ", Methods.Supplier_ShipOrderSGetQuery(query));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			decimal result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (decimal)obj;
			}
			else
			{
				result = 0m;
			}
			return result;
		}
		public static decimal Supplier_ShipOrderCostPriceAllGet(OrderQuery query, int userid)
		{
			string text = Methods.Supplier_ShipOrderSGetQuery(query);
			if (userid > 0)
			{
				text += string.Format(" and userid={0}  ", userid);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select ", new object[0]);
			stringBuilder.AppendFormat(" isnull(sum(OrderCostPrice),0) ", new object[0]);
			stringBuilder.AppendFormat(" from hishop_orders ", new object[0]);
			stringBuilder.AppendFormat(" where {0} and orderstatus = 3 ", text);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			decimal result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (decimal)obj;
			}
			else
			{
				result = 0m;
			}
			return result;
		}
		public static decimal Supplier_ShipOrderAmountPriceAllGet(OrderQuery query, int userid)
		{
			string text = Methods.Supplier_ShipOrderSGetQuery(query);
			if (userid > 0)
			{
				text += string.Format(" and userid={0}  ", userid);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select ", new object[0]);
			stringBuilder.AppendFormat(" isnull(sum(amount),0) ", new object[0]);
			stringBuilder.AppendFormat(" from hishop_orders ", new object[0]);
			stringBuilder.AppendFormat(" where {0} and orderstatus = 3 ", text);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			decimal result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (decimal)obj;
			}
			else
			{
				result = 0m;
			}
			return result;
		}
		public static System.Data.DataSet Supplier_ShipOrderShipInfoGet(string orderId, int userid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select orderid ", new object[0]);
			stringBuilder.AppendFormat(" ,(select OrderStatus from Hishop_Orders  where orderid = a.orderid) as OrderStatus ", new object[0]);
			stringBuilder.AppendFormat(" ,(select RealModeName from Hishop_Orders  where orderid = a.orderid) as RealModeName ", new object[0]);
			stringBuilder.AppendFormat(" ,(select ShipOrderNumber from Hishop_Orders  where orderid = a.orderid) as ShipOrderNumber ", new object[0]);
			stringBuilder.AppendFormat(" ,(select count(*) from Hishop_OrderGifts  where orderid = a.orderid) as giftnum ", new object[0]);
			if (Methods.Supplier_ShipOrderHasShipPointGet(orderId))
			{
				stringBuilder.AppendFormat(" ,(select case when SupplierType=0 then '仓库配送至发货点' when SupplierType=1 then '发货点配送至客户' end from Hishop_Orders  where orderid = a.orderid) as SupplierTypeName ", new object[0]);
			}
			else
			{
				stringBuilder.AppendFormat(" ,(select case when SupplierType=0 then '仓库配送至客户' when SupplierType=1 then '发货点配送至客户' end from Hishop_Orders  where orderid = a.orderid) as SupplierTypeName ", new object[0]);
			}
			stringBuilder.AppendFormat(" from Hishop_Orders a where charindex('{0}',orderid)>0 and orderid<>'{0}' ", orderId);
			if (userid > 0)
			{
				stringBuilder.AppendFormat(" and a.userid={0} ", userid);
			}
			stringBuilder.AppendFormat(" group by orderid; ", new object[0]);
			stringBuilder.AppendFormat("select * from Hishop_OrderItems where charindex('{0}',orderid)>0 ; ", orderId);
			stringBuilder.AppendFormat("select * from Hishop_OrderGifts where charindex('{0}',orderid)>0 ; ", orderId);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataSet dataSet = Methods.database.ExecuteDataSet(sqlStringCommand);
			dataSet.Relations.Add("Two", dataSet.Tables[0].Columns["orderid"], dataSet.Tables[1].Columns["orderid"], false);
			dataSet.Relations.Add("Two2", dataSet.Tables[0].Columns["orderid"], dataSet.Tables[2].Columns["orderid"], false);
			return dataSet;
		}
		public static System.Data.DataTable Supplier_ShipOrderShipInfo2SGet(string orderId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select username,orderid,orderstatus,RealModeName,ShipOrderNumber,SupplierType,case when SupplierType=0 then '供应商' when SupplierType=1 then '发货点' end as SupplierTypeName ", new object[0]);
			stringBuilder.AppendFormat(" from Hishop_Orders a where charindex('_{0}',orderid)>0 order by SupplierType desc; ", orderId);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = Methods.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public static bool Supplier_ShipOrderHasShipPointGet(string orderid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select ", new object[0]);
			stringBuilder.AppendFormat(" orderid ", new object[0]);
			stringBuilder.AppendFormat(" from hishop_orders ", new object[0]);
			stringBuilder.AppendFormat(" where charindex('{0}',orderid)>0 and orderid<>'{0}' and SupplierType = 1 ", DataHelper.CleanSearchString(orderid));
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			return obj != null && obj != DBNull.Value;
		}
		public static bool Supplier_ShipOrderHasAllSendGood(string orderid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" select ", new object[0]);
			stringBuilder.AppendFormat("  ((select count(orderid) from hishop_orders where charindex('{0}',orderid)>0 and orderid<>'{0}') - ", DataHelper.CleanSearchString(orderid));
			stringBuilder.AppendFormat("  (select count(orderid) from hishop_orders where charindex('{0}',orderid)>0 and orderid<>'{0}' and OrderStatus={1})) as Num ", DataHelper.CleanSearchString(orderid), 3);
			System.Data.Common.DbCommand sqlStringCommand = Methods.database.GetSqlStringCommand(stringBuilder.ToString());
			object obj = Methods.database.ExecuteScalar(sqlStringCommand);
			return obj != null && obj != DBNull.Value && (int)obj == 0;
		}
		public static DbQueryResult SendNote_Gets(RefundApplyQuery query, int ctype)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (ctype == 1)
			{
				stringBuilder.Append(" and charindex('PO',orderid)=0 ");
			}
			if (ctype == 2)
			{
				stringBuilder.Append(" and charindex('PO',orderid)>0 ");
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and charindex('{0}',OrderId) >0 ", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderSendNote", "OrderId", stringBuilder.ToString(), "*");
		}
		static Methods()
		{
			Methods.database = DatabaseFactory.CreateDatabase();
		}
	}
}
