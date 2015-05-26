using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.UI.Common.Data
{
	public class SqlCommonDataProvider : ControlProvider
	{
		private Database database;
		public SqlCommonDataProvider()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override IList<ProductTypeInfo> GetProductTypes()
		{
			IList<ProductTypeInfo> list = new List<ProductTypeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateType(dataReader));
				}
			}
			return list;
		}
		public override System.Data.DataTable GetBrandCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence DESC");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE ProductTypeId=@ProductTypeId ORDER BY DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override IList<ProductLineInfo> GetProductLineList()
		{
			IList<ProductLineInfo> list = new List<ProductLineInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines");
			if (HiContext.Current.User.UserRole == UserRole.Distributor)
			{
				System.Data.Common.DbCommand expr_2F = sqlStringCommand;
				expr_2F.CommandText += " WHERE LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = @UserId)";
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ProductLineInfo item = new ProductLineInfo
					{
						LineId = (int)dataReader["LineId"],
						Name = (string)dataReader["Name"]
					};
					list.Add(item);
				}
			}
			return list;
		}
		public override IList<ShippingModeInfo> GetShippingModes()
		{
			IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShippingMode(dataReader));
				}
			}
			return list;
		}
		public override void GetMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name FROM aspnet_MemberGrades WHERE GradeId = @GradeId;SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_MemberMessageBox WHERE Accepter = @Accepter AND IsRead=0");
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				sqlStringCommand.CommandText = "SELECT Name FROM distro_MemberGrades WHERE GradeId = @GradeId;SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_MemberMessageBox WHERE Accepter = @Accepter AND IsRead=0";
			}
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "Addressee", System.Data.DbType.String, userName);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					gradeName = (string)dataReader["Name"];
				}
				else
				{
					gradeName = string.Empty;
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					messageNum = (int)dataReader["NoReadMessageNum"];
				}
				else
				{
					messageNum = 0;
				}
			}
		}
		public override System.Data.DataTable GetSkuContentBySku(string skuId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT AttributeName, ValueStr");
			stringBuilder.Append(" FROM Hishop_SKUs s join Hishop_SKUItems si on s.SkuId = si.SkuId");
			stringBuilder.Append(" join Hishop_Attributes a on si.AttributeId = a.AttributeId join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetTags()
		{
			System.Data.DataTable result = new System.Data.DataTable();
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_Tags");
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
