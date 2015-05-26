using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.ControlPanel.Data
{
	public class SalesData : SalesProvider
	{
		private Database database;
		public SalesData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override bool AddShipper(ShippersInfo shipper)
		{
			string text = string.Empty;
			if (shipper.IsDefault)
			{
				text += "UPDATE Hishop_Shippers SET IsDefault = 0";
			}
			text += " INSERT INTO Hishop_Shippers (IsDefault, ShipperTag, ShipperName, RegionId, Address, CellPhone, TelPhone, Zipcode, Remark) VALUES (@IsDefault, @ShipperTag, @ShipperName, @RegionId, @Address, @CellPhone, @TelPhone, @Zipcode, @Remark)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, shipper.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "ShipperTag", System.Data.DbType.String, shipper.ShipperTag);
			this.database.AddInParameter(sqlStringCommand, "ShipperName", System.Data.DbType.String, shipper.ShipperName);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shipper.RegionId);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shipper.Address);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shipper.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shipper.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shipper.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, shipper.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShipper(ShippersInfo shipper)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET ShipperTag = @ShipperTag, ShipperName = @ShipperName, RegionId = @RegionId, Address = @Address, CellPhone = @CellPhone, TelPhone = @TelPhone, Zipcode = @Zipcode, Remark =@Remark WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperTag", System.Data.DbType.String, shipper.ShipperTag);
			this.database.AddInParameter(sqlStringCommand, "ShipperName", System.Data.DbType.String, shipper.ShipperName);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shipper.RegionId);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shipper.Address);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shipper.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shipper.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shipper.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, shipper.Remark);
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipper.ShipperId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteShipper(int shipperId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipperId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override ShippersInfo GetShipper(int shipperId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipperId);
			ShippersInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShipper(dataReader);
				}
			}
			return result;
		}
		public override IList<ShippersInfo> GetShippers(bool includeDistributor)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers");
			if (!includeDistributor)
			{
				System.Data.Common.DbCommand expr_15 = sqlStringCommand;
				expr_15.CommandText += " WHERE DistributorUserId = 0";
			}
			IList<ShippersInfo> list = new List<ShippersInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShipper(dataReader));
				}
			}
			return list;
		}
		public override void SetDefalutShipper(int shipperId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET IsDefault = 0;UPDATE Hishop_Shippers SET IsDefault = 1 WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipperId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool AddExpressTemplate(string expressName, string xmlFile)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ExpressTemplates(ExpressName, XmlFile, IsUse) VALUES(@ExpressName, @XmlFile, 1)");
			this.database.AddInParameter(sqlStringCommand, "ExpressName", System.Data.DbType.String, expressName);
			this.database.AddInParameter(sqlStringCommand, "XmlFile", System.Data.DbType.String, xmlFile);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateExpressTemplate(int expressId, string expressName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET ExpressName = @ExpressName WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressName", System.Data.DbType.String, expressName);
			this.database.AddInParameter(sqlStringCommand, "ExpressId", System.Data.DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetExpressIsUse(int expressId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET IsUse = ~IsUse WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressId", System.Data.DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteExpressTemplate(int expressId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ExpressTemplates WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressId", System.Data.DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetExpressTemplates()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates");
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetIsUserExpressTemplates()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates WHERE IsUse = 1");
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PaymentType_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action == DataProviderAction.Create)
				{
					this.database.AddOutParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, 4);
				}
				else
				{
					this.database.AddInParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, paymentMode.ModeId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, paymentMode.Name);
					this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, paymentMode.Description);
					this.database.AddInParameter(storedProcCommand, "Gateway", System.Data.DbType.String, paymentMode.Gateway);
					this.database.AddInParameter(storedProcCommand, "IsUseInpour", System.Data.DbType.Boolean, paymentMode.IsUseInpour);
					this.database.AddInParameter(storedProcCommand, "IsUseInDistributor", System.Data.DbType.Boolean, paymentMode.IsUseInDistributor);
					this.database.AddInParameter(storedProcCommand, "Charge", System.Data.DbType.Currency, paymentMode.Charge);
					this.database.AddInParameter(storedProcCommand, "IsPercent", System.Data.DbType.Boolean, paymentMode.IsPercent);
					this.database.AddInParameter(storedProcCommand, "Settings", System.Data.DbType.String, paymentMode.Settings);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				PaymentModeActionStatus paymentModeActionStatus = (PaymentModeActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
				result = paymentModeActionStatus;
			}
			return result;
		}
		public override void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public override PaymentModeInfo GetPaymentMode(int modeId)
		{
			PaymentModeInfo result = new PaymentModeInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public override PaymentModeInfo GetPaymentMode(string gateway)
		{
			PaymentModeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, gateway);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public override IList<PaymentModeInfo> GetPaymentModes()
		{
			IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
		public override IList<ShippingModeInfo> GetShippingModes(string paymentGateway)
		{
			IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
			string text = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId";
			if (!string.IsNullOrEmpty(paymentGateway))
			{
				text += " WHERE Gateway = @Gateway)";
			}
			text += " Order By DisplaySequence";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			if (!string.IsNullOrEmpty(paymentGateway))
			{
				this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, paymentGateway);
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShippingMode(dataReader));
				}
			}
			return list;
		}
		public override void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_ShippingTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public override bool CreateShippingMode(ShippingModeInfo shippingMode)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Create");
			this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, shippingMode.Name);
			this.database.AddInParameter(storedProcCommand, "TemplateId", System.Data.DbType.Int32, shippingMode.TemplateId);
			this.database.AddOutParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, 4);
			this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, shippingMode.Description);
			bool result;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					this.database.ExecuteNonQuery(storedProcCommand, dbTransaction);
					if (result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0))
					{
						int num = (int)this.database.GetParameterValue(storedProcCommand, "ModeId");
						System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, num);
						StringBuilder stringBuilder = new StringBuilder();
						int num2 = 0;
						stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
						foreach (string current in shippingMode.ExpressCompany)
						{
							stringBuilder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
							this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num2, System.Data.DbType.String, current);
							num2++;
						}
						sqlStringCommand.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
						int num3 = (int)this.database.ExecuteScalar(sqlStringCommand, dbTransaction);
						if (num3 != 0)
						{
							dbTransaction.Rollback();
							result = false;
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public override bool DeleteShippingMode(int modeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_TemplateRelatedShipping Where ModeId=@ModeId;DELETE FROM Hishop_ShippingTypes Where ModeId=@ModeId;UPDATE Hishop_PurchaseOrders set ShippingModeId=0 where ShippingModeId=@ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			this.database.AddOutParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 4);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShippingMode(ShippingModeInfo shippingMode)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Update");
			this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, shippingMode.Name);
			this.database.AddInParameter(storedProcCommand, "TemplateId", System.Data.DbType.Int32, shippingMode.TemplateId);
			this.database.AddInParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, shippingMode.ModeId);
			this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, shippingMode.Description);
			bool result;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					this.database.ExecuteNonQuery(storedProcCommand, dbTransaction);
					if (result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0))
					{
						System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, shippingMode.ModeId);
						StringBuilder stringBuilder = new StringBuilder();
						int num = 0;
						stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
						foreach (string current in shippingMode.ExpressCompany)
						{
							stringBuilder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
							this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num, System.Data.DbType.String, current);
							num++;
						}
						sqlStringCommand.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
						int num2 = (int)this.database.ExecuteScalar(sqlStringCommand, dbTransaction);
						if (num2 != 0)
						{
							dbTransaction.Rollback();
							result = false;
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			ShippingModeInfo shippingModeInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId");
			if (includeDetail)
			{
				System.Data.Common.DbCommand expr_1A = sqlStringCommand;
				expr_1A.CommandText += " SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId";
			}
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shippingModeInfo = DataMapper.PopulateShippingMode(dataReader);
				}
				if (includeDetail)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						if (dataReader["ExpressCompanyName"] != DBNull.Value)
						{
							shippingModeInfo.ExpressCompany.Add((string)dataReader["ExpressCompanyName"]);
						}
					}
				}
			}
			return shippingModeInfo;
		}
		public override bool CreateShippingTemplate(ShippingModeInfo shippingMode)
		{
			bool flag = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ShippingTemplates(TemplateName,Weight,AddWeight,Price,AddPrice) VALUES(@TemplateName,@Weight,@AddWeight,@Price,@AddPrice);SELECT @@Identity");
			this.database.AddInParameter(sqlStringCommand, "TemplateName", System.Data.DbType.String, shippingMode.Name);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Int32, shippingMode.Weight);
			if (shippingMode.AddWeight.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "AddWeight", System.Data.DbType.Int32, shippingMode.AddWeight);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "AddWeight", System.Data.DbType.Int32, 0);
			}
			this.database.AddInParameter(sqlStringCommand, "Price", System.Data.DbType.Currency, shippingMode.Price);
			if (shippingMode.AddPrice.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "AddPrice", System.Data.DbType.Currency, shippingMode.AddPrice);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "AddPrice", System.Data.DbType.Currency, 0);
			}
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					object obj = this.database.ExecuteScalar(sqlStringCommand, dbTransaction);
					int num = 0;
					if (obj != null && obj != DBNull.Value)
					{
						int.TryParse(obj.ToString(), out num);
						flag = (num > 0);
					}
					if (flag)
					{
						System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand2, "TemplateId", System.Data.DbType.Int32, num);
						if (shippingMode.ModeGroup != null && shippingMode.ModeGroup.Count > 0)
						{
							StringBuilder stringBuilder = new StringBuilder();
							int num2 = 0;
							int num3 = 0;
							stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
							stringBuilder.Append(" DECLARE @GroupId Int;");
							foreach (ShippingModeGroupInfo current in shippingMode.ModeGroup)
							{
								stringBuilder.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num2).Append(",@AddPrice").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
								this.database.AddInParameter(sqlStringCommand2, "Price" + num2, System.Data.DbType.Currency, current.Price);
								this.database.AddInParameter(sqlStringCommand2, "AddPrice" + num2, System.Data.DbType.Currency, current.AddPrice);
								stringBuilder.Append("Set @GroupId =@@identity;");
								foreach (ShippingRegionInfo current2 in current.ModeRegions)
								{
									stringBuilder.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num3).Append("); SELECT @ERR=@ERR+@@ERROR;");
									this.database.AddInParameter(sqlStringCommand2, "RegionId" + num3, System.Data.DbType.Int32, current2.RegionId);
									num3++;
								}
								num2++;
							}
							sqlStringCommand2.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
							int num4 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
							if (num4 != 0)
							{
								dbTransaction.Rollback();
								flag = false;
							}
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}
		public override bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
		{
			bool result = false;
			StringBuilder stringBuilder = new StringBuilder("UPDATE Hishop_ShippingTemplates SET TemplateName=@TemplateName,Weight=@Weight,AddWeight=@AddWeight,Price=@Price,AddPrice=@AddPrice WHERE TemplateId=@TemplateId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "TemplateName", System.Data.DbType.String, shippingMode.Name);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Currency, shippingMode.Weight);
			this.database.AddInParameter(sqlStringCommand, "AddWeight", System.Data.DbType.Currency, shippingMode.AddWeight);
			this.database.AddInParameter(sqlStringCommand, "Price", System.Data.DbType.Currency, shippingMode.Price);
			this.database.AddInParameter(sqlStringCommand, "AddPrice", System.Data.DbType.Currency, shippingMode.AddPrice);
			this.database.AddInParameter(sqlStringCommand, "TemplateId", System.Data.DbType.Int32, shippingMode.TemplateId);
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction) > 0))
					{
						System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand2, "TemplateId", System.Data.DbType.Int32, shippingMode.TemplateId);
						StringBuilder stringBuilder2 = new StringBuilder();
						int num = 0;
						int num2 = 0;
						stringBuilder2.Append("DELETE Hishop_ShippingTypeGroups WHERE TemplateId=@TemplateId;");
						stringBuilder2.Append("DELETE Hishop_ShippingRegions WHERE TemplateId=@TemplateId;");
						stringBuilder2.Append("DECLARE @ERR INT; Set @ERR =0;");
						stringBuilder2.Append(" DECLARE @GroupId Int;");
						if (shippingMode.ModeGroup != null && shippingMode.ModeGroup.Count > 0)
						{
							foreach (ShippingModeGroupInfo current in shippingMode.ModeGroup)
							{
								stringBuilder2.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num).Append(",@AddPrice").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
								this.database.AddInParameter(sqlStringCommand2, "Price" + num, System.Data.DbType.Currency, current.Price);
								this.database.AddInParameter(sqlStringCommand2, "AddPrice" + num, System.Data.DbType.Currency, current.AddPrice);
								stringBuilder2.Append("Set @GroupId =@@identity;");
								foreach (ShippingRegionInfo current2 in current.ModeRegions)
								{
									stringBuilder2.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
									this.database.AddInParameter(sqlStringCommand2, "RegionId" + num2, System.Data.DbType.Int32, current2.RegionId);
									num2++;
								}
								num++;
							}
						}
						sqlStringCommand2.CommandText = stringBuilder2.Append("SELECT @ERR;").ToString();
						int num3 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
						if (num3 != 0)
						{
							dbTransaction.Rollback();
							result = false;
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public override bool DeleteShippingTemplate(int templateId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShippingTemplates Where TemplateId=@TemplateId");
			this.database.AddInParameter(sqlStringCommand, "TemplateId", System.Data.DbType.Int32, templateId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetShippingTemplates(Pagination pagin)
		{
			return DataHelper.PagingByRownumber(pagin.PageIndex, pagin.PageSize, pagin.SortBy, pagin.SortOrder, pagin.IsCount, "Hishop_ShippingTemplates", "TemplateId", "", "*");
		}
		public override System.Data.DataTable GetShippingAllTemplates()
		{
			string text = "SELECT * FROM Hishop_ShippingTemplates ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
		{
			ShippingModeInfo shippingModeInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT * FROM Hishop_ShippingTemplates Where TemplateId =@TemplateId");
			if (includeDetail)
			{
				System.Data.Common.DbCommand expr_1A = sqlStringCommand;
				expr_1A.CommandText += " SELECT GroupId,TemplateId,Price,AddPrice FROM Hishop_ShippingTypeGroups Where TemplateId =@TemplateId";
				System.Data.Common.DbCommand expr_30 = sqlStringCommand;
				expr_30.CommandText += " SELECT sr.TemplateId,sr.GroupId,sr.RegionId FROM Hishop_ShippingRegions sr Where sr.TemplateId =@TemplateId";
			}
			this.database.AddInParameter(sqlStringCommand, "TemplateId", System.Data.DbType.Int32, templateId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shippingModeInfo = DataMapper.PopulateShippingTemplate(dataReader);
				}
				if (includeDetail)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						shippingModeInfo.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(dataReader));
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
						{
							if (current.GroupId == (int)dataReader["GroupId"])
							{
								current.ModeRegions.Add(DataMapper.PopulateShippingRegion(dataReader));
							}
						}
					}
				}
			}
			return shippingModeInfo;
		}
		public override IList<string> GetExpressCompanysByMode(int modeId)
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if (dataReader["ExpressCompanyName"] != DBNull.Value)
					{
						list.Add((string)dataReader["ExpressCompanyName"]);
					}
				}
			}
			return list;
		}
		public override System.Data.DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSales_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, productSale.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, productSale.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, productSale.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildProductSaleQuery(productSale));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSalesNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildProductSaleQuery(productSale));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(select count(*) from aspnet_Members) as AllUserCounts FROM aspnet_Members  GROUP BY TopRegionId ");
			IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				UserStatisticsInfo userStatisticsInfo = null;
				while (dataReader.Read())
				{
					userStatisticsInfo = DataMapper.PopulateUserStatistics(dataReader);
					list.Add(userStatisticsInfo);
				}
				if (userStatisticsInfo != null)
				{
					totalRegionsUsers = int.Parse(userStatisticsInfo.AllUserCounts.ToString());
				}
				else
				{
					totalRegionsUsers = 0;
				}
			}
			return list;
		}
		public override OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, userOrder.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, userOrder.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, userOrder.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildUserOrderQuery(userOrder));
			this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", System.Data.DbType.Int32, 4);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfPage += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfPage += (decimal)dataReader["Profits"];
					}
				}
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
			return orderStatisticsInfo;
		}
		public override OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatisticsNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildUserOrderQuery(userOrder));
			this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", System.Data.DbType.Int32, 4);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
			return orderStatisticsInfo;
		}
		public override AdminStatisticsInfo GetStatistics()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"SELECT  (SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderStatus = 2) AS orderNumbWaitConsignment, (SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2) AS purchaseOrderNumbWaitConsignment, (select Count(LeaveId) from Hishop_LeaveComments l where (select count(replyId) from Hishop_LeaveCommentReplys where leaveId =l.leaveId)=0) as leaveComments,(select Count(ConsultationId) from Hishop_ProductConsultations where ReplyUserId is null) as productConsultations,(select Count(*) from Hishop_ManagerMessageBox where IsRead=0 and Accepter='admin' and Sernder in (select UserName from vw_aspnet_Members)) as messages, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderPriceToday, isnull((select sum(PurchaseProfit) from Hishop_PurchaseOrders where  (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and PurchaseDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as PurchaseOrderProfitToday, isnull((select sum(OrderProfit) from Hishop_Orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)  and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderProfitToday, (select count(*) from vw_aspnet_Members where CreateDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"' ) as userNewAddToday, (select count(*) from vw_aspnet_Distributors where CreateDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"' and IsApproved=1) as distroNewAddToday, isnull((select sum(balance) from vw_aspnet_Members),0) as memberBalance, isnull((select sum(balance) from vw_aspnet_Distributors),0) as distroBalance,(select count(*) from (select ProductId from Hishop_SKUs where Stock<=AlertStock group by ProductId) as a) as productAlert,(select count(PurchaseOrderId) from Hishop_PurchaseOrders where PurchaseStatus=",
				1,
				") as purchaseOrderNumbWait,(select count(*) from Hishop_BalanceDrawRequest) as memberBlancedraw,(select count(*) from Hishop_DistributorBalanceDrawRequest) as distributorBlancedraw,(select count(*) from Hishop_SiteRequest where RequestStatus=1) as distributorSiteRequest,(select count(*) from Hishop_Orders where datediff(dd,getdate(),OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as todayFinishOrder,(select count(*) from Hishop_Orders where datediff(dd,getdate()-1,OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as yesterdayFinishOrder, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,getdate()-1,OrderDate)=0),0) as orderPriceYesterDay,(select count(*) from Hishop_PurchaseOrders where datediff(dd,getdate(),PurchaseDate)=0 and (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)) as todayFinishPurchaseOrder,(select count(*) from Hishop_PurchaseOrders where datediff(dd,getdate()-1,PurchaseDate)=0 and (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)) as yesterdayFinishPurchaseOrder, isnull((select sum(PurchaseTotal)-isnull(sum(RefundAmount),0) from Hishop_PurchaseOrders where (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and datediff(dd,getdate(),PurchaseDate)=0),0) as purchaseorderPriceToDay, isnull((select sum(PurchaseTotal)-isnull(sum(RefundAmount),0) from Hishop_PurchaseOrders where (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and datediff(dd,getdate()-1,PurchaseDate)=0),0) as purchaseorderPriceYesterDay,(select count(*) from vw_aspnet_Members where datediff(dd,getdate()-1,CreateDate)=0) as userNewAddYesterToday,(select count(*) from vw_aspnet_Distributors where datediff(dd,getdate()-1,CreateDate)=0) as distroNewAddYesterToday,(select count(*) from vw_aspnet_Members) as TotalMembers,(select count(*) from vw_aspnet_Distributors) as TotalDistributors,(select count(*) from Hishop_Products where SaleStatus!=0) as TotalProducts, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,OrderDate,getdate())<=30),0) as orderPriceMonth, isnull((select sum(PurchaseTotal)-isnull(sum(RefundAmount),0) from Hishop_PurchaseOrders where (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and datediff(dd,PurchaseDate,getdate())<=30),0) as purchaseorderPriceMonth"
			}));
			AdminStatisticsInfo adminStatisticsInfo = new AdminStatisticsInfo();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					adminStatisticsInfo.OrderNumbWaitConsignment = (int)dataReader["orderNumbWaitConsignment"];
					adminStatisticsInfo.PurchaseOrderNumbWaitConsignment = (int)dataReader["purchaseOrderNumbWaitConsignment"];
					adminStatisticsInfo.LeaveComments = (int)dataReader["LeaveComments"];
					adminStatisticsInfo.ProductConsultations = (int)dataReader["ProductConsultations"];
					adminStatisticsInfo.Messages = (int)dataReader["Messages"];
					adminStatisticsInfo.PurchaseOrderProfitToday = (decimal)dataReader["PurchaseOrderProfitToday"];
					adminStatisticsInfo.OrderProfitToday = (decimal)dataReader["orderProfitToday"];
					adminStatisticsInfo.UserNewAddToday = (int)dataReader["userNewAddToday"];
					adminStatisticsInfo.DistroButorsNewAddToday = (int)dataReader["distroNewAddToday"];
					adminStatisticsInfo.MembersBalance = (decimal)dataReader["memberBalance"];
					adminStatisticsInfo.DistrosBalance = (decimal)dataReader["distroBalance"];
					adminStatisticsInfo.OrderPriceToday = (decimal)dataReader["orderPriceToday"];
					adminStatisticsInfo.ProductAlert = (int)dataReader["productAlert"];
					adminStatisticsInfo.PurchaseOrderNumbWait = (int)dataReader["purchaseOrderNumbWait"];
					adminStatisticsInfo.MemberBlancedrawRequest = (int)dataReader["memberBlancedraw"];
					adminStatisticsInfo.DistributorBlancedrawRequest = (int)dataReader["distributorBlancedraw"];
					adminStatisticsInfo.DistributorSiteRequest = (int)dataReader["distributorSiteRequest"];
					adminStatisticsInfo.TodayFinishOrder = (int)dataReader["todayFinishOrder"];
					adminStatisticsInfo.YesterdayFinishOrder = (int)dataReader["yesterdayFinishOrder"];
					adminStatisticsInfo.OrderPriceYesterDay = (decimal)dataReader["orderPriceYesterDay"];
					adminStatisticsInfo.TodayFinishPurchaseOrder = (int)dataReader["todayFinishPurchaseOrder"];
					adminStatisticsInfo.YesterdayFinishPurchaseOrder = (int)dataReader["yesterdayFinishPurchaseOrder"];
					adminStatisticsInfo.PurchaseorderPriceToDay = (decimal)dataReader["purchaseorderPriceToDay"];
					adminStatisticsInfo.PurchaseorderPriceYesterDay = (decimal)dataReader["purchaseorderPriceYesterDay"];
					adminStatisticsInfo.UserNewAddYesterToday = (int)dataReader["userNewAddYesterToday"];
					adminStatisticsInfo.DistroNewAddYesterToday = (int)dataReader["distroNewAddYesterToday"];
					adminStatisticsInfo.TotalMembers = (int)dataReader["TotalMembers"];
					adminStatisticsInfo.TotalDistributors = (int)dataReader["TotalDistributors"];
					adminStatisticsInfo.TotalProducts = (int)dataReader["TotalProducts"];
					adminStatisticsInfo.OrderPriceMonth = (decimal)dataReader["OrderPriceMonth"];
					adminStatisticsInfo.PurchaseorderPriceMonth = (decimal)dataReader["PurchaseorderPriceMonth"];
				}
			}
			return adminStatisticsInfo;
		}
		public override IList<OrderPriceStatisticsForChartInfo> GetOrderPriceStatisticsOfSevenDays(int days)
		{
			IList<OrderPriceStatisticsForChartInfo> list = new List<OrderPriceStatisticsForChartInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT isnull((SELECT sum(Amount) FROM Hishop_Orders WHERE OrderDate BETWEEN @StartDate AND @EndDate),0) AS Price");
			this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
			this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
			DateTime date = default(DateTime);
			DateTime dateTime = default(DateTime);
			date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double)(-(double)days));
			for (int i = 1; i <= days; i++)
			{
				OrderPriceStatisticsForChartInfo orderPriceStatisticsForChartInfo = new OrderPriceStatisticsForChartInfo();
				if (i > 1)
				{
					date = dateTime;
				}
				dateTime = date.AddDays(1.0);
				this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
				this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
				orderPriceStatisticsForChartInfo.Price = (decimal)this.database.ExecuteScalar(sqlStringCommand);
				orderPriceStatisticsForChartInfo.TimePoint = date.Day;
				list.Add(orderPriceStatisticsForChartInfo);
			}
			return list;
		}
		public override System.Data.DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildMemberStatisticsQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(SalesData.BuildMemberStatisticsQuery(query));
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductVisitAndBuyStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildProductVisitAndBuyStatisticsQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
			stringBuilder.Append("FROM Hishop_Products WHERE SaleCounts>0 ORDER BY BuyPercentage DESC;");
			stringBuilder.Append("SELECT COUNT(*) as TotalProductSales FROM Hishop_Products WHERE SaleCounts>0;");
			sqlStringCommand.CommandText = stringBuilder.ToString();
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					totalProductSales = (int)dataReader["TotalProductSales"];
				}
				else
				{
					totalProductSales = 0;
				}
			}
			return result;
		}
		public override DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat("orderDate >= '{0}'", query.StartDate.Value);
			}
			if (query.EndDate.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" AND ");
			}
			stringBuilder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_SaleDetails", "OrderId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}
		public override DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_SaleDetails WHERE 1=1");
			if (query.StartDate.HasValue)
			{
				System.Data.Common.DbCommand expr_2B = sqlStringCommand;
				expr_2B.CommandText += string.Format(" AND OrderDate >= '{0}'", query.StartDate);
			}
			if (query.EndDate.HasValue)
			{
				System.Data.Common.DbCommand expr_64 = sqlStringCommand;
				expr_64.CommandText += string.Format(" AND OrderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
			}
			System.Data.Common.DbCommand expr_A4 = sqlStringCommand;
			expr_A4.CommandText += string.Format("AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dbQueryResult;
		}
		public override DbQueryResult GetSaleTargets()
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			string text = string.Empty;
			text = string.Format("select (select Count(OrderId) from Hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1}  AND OrderStatus != {2}) as OrderNumb,", 1, 4, 9) + string.Format("(select ISNULL(sum(OrderTotal) - sum(RefundAmount),0) from hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}) as OrderPrice, ", 1, 4, 9) + " (select COUNT(*) from vw_aspnet_Members) as UserNumb,  (select count(*) from vw_aspnet_Members where UserID in (select userid from Hishop_orders)) as UserOrderedNumb,  ISNULL((select sum(VistiCounts) from Hishop_products),0) as ProductVisitNumb ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dbQueryResult;
		}
		public override System.Data.DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = DateTime.Now.AddDays(-7.0);
				DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
				DateTime now = DateTime.Now;
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime2);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, now);
				decimal allSalesTotal = 0m;
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				if (obj != null)
				{
					allSalesTotal = Convert.ToDecimal(obj);
				}
				System.Data.DataTable dataTable = this.CreateTable();
				for (int i = 0; i < 7; i++)
				{
					DateTime dateTime3 = DateTime.Now.AddDays((double)(-(double)i));
					decimal daySaleTotal = this.GetDaySaleTotal(dateTime3.Year, dateTime3.Month, dateTime3.Day, saleStatisticsType);
					this.InsertToTable(dataTable, dateTime3.Day, daySaleTotal, allSalesTotal);
				}
				result = dataTable;
			}
			return result;
		}
		public override decimal GetDaySaleTotal(int year, int month, int int_0, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, month, int_0);
				DateTime dateTime2 = dateTime.AddDays(1.0);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}
		public override decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, month, 1);
				DateTime dateTime2 = dateTime.AddMonths(1);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}
		public override System.Data.DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
				System.Data.DataTable dataTable = this.CreateTable();
				decimal monthSaleTotal = this.GetMonthSaleTotal(year, month, saleStatisticsType);
				int dayCount = this.GetDayCount(year, month);
				int arg_9F_0;
				if (year == DateTime.Now.Year)
				{
					if (month == DateTime.Now.Month)
					{
						arg_9F_0 = DateTime.Now.Day;
						goto IL_9F;
					}
				}
				arg_9F_0 = dayCount;
				IL_9F:
				int num = arg_9F_0;
				for (int i = 1; i <= num; i++)
				{
					DateTime dateTime = new DateTime(year, month, i);
					DateTime dateTime2 = dateTime.AddDays(1.0);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", dateTime);
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", dateTime2);
					object obj = this.database.ExecuteScalar(sqlStringCommand);
					decimal salesTotal = (obj == null) ? 0m : Convert.ToDecimal(obj);
					this.InsertToTable(dataTable, i, salesTotal, monthSaleTotal);
				}
				result = dataTable;
			}
			return result;
		}
		private int GetDayCount(int year, int month)
		{
			int result;
			if (month == 2)
			{
				if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
				{
					result = 29;
				}
				else
				{
					result = 28;
				}
			}
			else
			{
				if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
				{
					result = 31;
				}
				else
				{
					result = 30;
				}
			}
			return result;
		}
		public override decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, 1, 1);
				DateTime dateTime2 = dateTime.AddYears(1);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}
		public override System.Data.DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
				System.Data.DataTable dataTable = this.CreateTable();
				int num = (year == DateTime.Now.Year) ? DateTime.Now.Month : 12;
				for (int i = 1; i <= num; i++)
				{
					DateTime dateTime = new DateTime(year, i, 1);
					DateTime dateTime2 = dateTime.AddMonths(1);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", dateTime);
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", dateTime2);
					object obj = this.database.ExecuteScalar(sqlStringCommand);
					decimal salesTotal = (obj == null) ? 0m : Convert.ToDecimal(obj);
					decimal yearSaleTotal = this.GetYearSaleTotal(year, saleStatisticsType);
					this.InsertToTable(dataTable, i, salesTotal, yearSaleTotal);
				}
				result = dataTable;
			}
			return result;
		}
		private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (saleStatisticsType)
			{
			case SaleStatisticsType.SaleCounts:
				stringBuilder.Append("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				break;
			case SaleStatisticsType.SaleTotal:
				stringBuilder.Append("SELECT Isnull(SUM(OrderTotal),0)");
				stringBuilder.Append(" FROM Hishop_orders WHERE  (OrderDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				break;
			case SaleStatisticsType.Profits:
				stringBuilder.Append("SELECT IsNull(SUM(OrderProfit),0) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				break;
			}
			return stringBuilder.ToString();
		}
		private void InsertToTable(System.Data.DataTable table, int date, decimal salesTotal, decimal allSalesTotal)
		{
			System.Data.DataRow dataRow = table.NewRow();
			dataRow["Date"] = date;
			dataRow["SaleTotal"] = salesTotal;
			if (allSalesTotal != 0m)
			{
				dataRow["Percentage"] = salesTotal / allSalesTotal * 100m;
			}
			else
			{
				dataRow["Percentage"] = 0;
			}
			dataRow["Lenth"] = (decimal)dataRow["Percentage"] * 4m;
			table.Rows.Add(dataRow);
		}
		private System.Data.DataTable CreateTable()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.DataColumn column = new System.Data.DataColumn("Date", typeof(int));
			System.Data.DataColumn column2 = new System.Data.DataColumn("SaleTotal", typeof(decimal));
			System.Data.DataColumn column3 = new System.Data.DataColumn("Percentage", typeof(decimal));
			System.Data.DataColumn column4 = new System.Data.DataColumn("Lenth", typeof(decimal));
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			return dataTable;
		}
		public override IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
		{
			IList<UserStatisticsForDate> list = new List<UserStatisticsForDate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate) AS UserAdd ");
			this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
			this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
			DateTime date = default(DateTime);
			DateTime dateTime = default(DateTime);
			if (days.HasValue)
			{
				date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double)(-(double)days.Value));
			}
			else
			{
				if (year.HasValue && month.HasValue)
				{
					date = new DateTime(year.Value, month.Value, 1);
				}
				else
				{
					if (year.HasValue && !month.HasValue)
					{
						date = new DateTime(year.Value, 1, 1);
					}
				}
			}
			if (days.HasValue)
			{
				for (int i = 1; i <= days; i++)
				{
					UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
					if (i > 1)
					{
						date = dateTime;
					}
					dateTime = date.AddDays(1.0);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
					userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
					userStatisticsForDate.TimePoint = date.Day;
					list.Add(userStatisticsForDate);
				}
			}
			else
			{
				if (year.HasValue && month.HasValue)
				{
					int num = DateTime.DaysInMonth(year.Value, month.Value);
					for (int i = 1; i <= num; i++)
					{
						UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
						if (i > 1)
						{
							date = dateTime;
						}
						dateTime = date.AddDays(1.0);
						this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
						this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
						userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
						userStatisticsForDate.TimePoint = i;
						list.Add(userStatisticsForDate);
					}
				}
				else
				{
					if (year.HasValue && !month.HasValue)
					{
						int num2 = 12;
						for (int i = 1; i <= num2; i++)
						{
							UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
							if (i > 1)
							{
								date = dateTime;
							}
							dateTime = date.AddMonths(1);
							this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
							this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
							userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
							userStatisticsForDate.TimePoint = i;
							list.Add(userStatisticsForDate);
						}
					}
				}
			}
			return list;
		}
		public override decimal GetAddUserTotal(int year)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate)  AS UserAdd");
			DateTime dateTime = new DateTime(year, 1, 1);
			DateTime dateTime2 = dateTime.AddYears(1);
			this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
			this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return (obj == null) ? 0m : Convert.ToDecimal(obj);
		}
		public override OrderInfo GetOrderInfo(string orderId)
		{
			OrderInfo orderInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId; SELECT  * FROM Hishop_OrderGifts Where OrderId = @OrderId; SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.Gifts.Add(DataMapper.PopulateOrderGift(dataReader));
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.LineItems.Add((string)dataReader["SkuId"], DataMapper.PopulateLineItem(dataReader));
				}
			}
			return orderInfo;
		}
		public override IList<LineItemInfo> GetLineItemInfo(string orderId)
		{
			List<LineItemInfo> list = new List<LineItemInfo>();
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						list.Add(DataMapper.PopulateLineItem(dataReader));
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list;
		}
		public override System.Data.DataTable GetRecentlyOrders(out int number)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 OrderId, OrderDate, UserId, Username, Wangwang, RealName, ShipTo, OrderTotal,ISNULL(GroupBuyId,0) as GroupBuyId,ISNULL(GroupBuyStatus,0) as GroupBuyStatus, PaymentType,ManagerMark, OrderStatus, RefundStatus,ManagerRemark FROM Hishop_Orders ORDER BY OrderDate DESC");
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_Orders WHERE  OrderStatus=2");
			number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			return result;
		}
		public override void UpdatePayOrderStock(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool UpdateRefundOrderStock(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override DbQueryResult GetOrders(OrderQuery query)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Orders_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildOrdersQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalOrders", System.Data.DbType.Int32, 4);
			DbQueryResult dbQueryResult = new DbQueryResult();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dbQueryResult.TotalRecords = (int)this.database.GetParameterValue(storedProcCommand, "TotalOrders");
			return dbQueryResult;
		}
		public override System.Data.DataTable GetSendGoodsOrders(string orderIds)
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId, OrderDate,RefundStatus, ShipTo, OrderTotal, OrderStatus,ShippingRegion,Address,ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,ShipOrderNumber,ExpressCompanyAbb," + string.Format(" ExpressCompanyName FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND OrderId IN ({0}) order by OrderDate desc", orderIds));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override int DeleteOrders(string orderIds)
		{
			string text = string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int ConfirmPay(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET PayDate = @PayDate, OrderStatus = @OrderStatus ,OrderPoint=@OrderPoint WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderPoint", System.Data.DbType.Int32, order.Points);
			this.database.AddInParameter(sqlStringCommand, "PayDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool ConfirmOrderFinish(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET FinishDate = @FinishDate, OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "FinishDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 5);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int SendGoods(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, OrderStatus = @OrderStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName, ExpressCompanyAbb = @ExpressCompanyAbb WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, order.ShipOrderNumber);
			this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, order.RealShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, order.RealModeName);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
			this.database.AddInParameter(sqlStringCommand, "ShippingDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, order.ExpressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, order.ExpressCompanyAbb);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool UpdateOrderAmount(OrderInfo order, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderTotal = @OrderTotal, OrderProfit=@OrderProfit, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount, OrderPoint=@OrderPoint, Amount=@Amount,OrderCostPrice=@OrderCostPrice WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderTotal", System.Data.DbType.Currency, order.GetTotal());
			this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Currency, order.AdjustedFreight);
			this.database.AddInParameter(sqlStringCommand, "PayCharge", System.Data.DbType.Currency, order.PayCharge);
			this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", System.Data.DbType.Currency, order.GetCostPrice());
			this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", System.Data.DbType.Currency, order.AdjustedDiscount);
			this.database.AddInParameter(sqlStringCommand, "OrderPoint", System.Data.DbType.Int32, Convert.ToInt32((order.GetTotal() - order.AdjustedFreight - order.PayCharge - order.Tax) / HiContext.Current.SiteSettings.PointsRate));
			this.database.AddInParameter(sqlStringCommand, "OrderProfit", System.Data.DbType.Currency, order.GetProfit());
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, order.GetAmount());
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool CloseTransaction(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderStatus=@OrderStatus,CloseReason=@CloseReason WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(sqlStringCommand, "CloseReason", System.Data.DbType.String, order.CloseReason);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateOrderShippingMode(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "ShippingModeId", System.Data.DbType.Int32, order.ShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "ModeName", System.Data.DbType.String, order.ModeName);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateOrderPaymentType(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", System.Data.DbType.Int32, order.PaymentTypeId);
			this.database.AddInParameter(sqlStringCommand, "PaymentType", System.Data.DbType.String, order.PaymentType);
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, order.Gateway);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool SaveShippingAddress(OrderInfo order)
		{
			bool result;
			if (order == null)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE OrderId = @OrderId");
				this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.String, order.RegionId);
				this.database.AddInParameter(sqlStringCommand, "ShippingRegion", System.Data.DbType.String, order.ShippingRegion);
				this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, order.Address);
				this.database.AddInParameter(sqlStringCommand, "ZipCode", System.Data.DbType.String, order.ZipCode);
				this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, order.ShipTo);
				this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, order.TelPhone);
				this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, order.CellPhone);
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool SaveOrderRemark(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE OrderId=@OrderId");
			if (order.ManagerMark.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, (int)order.ManagerMark.Value);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, DBNull.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "ManagerRemark", System.Data.DbType.String, order.ManagerRemark);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
			this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, realShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, realModeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, expressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, expressCompanyAbb);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetOrderPrinted(string orderId, bool isPrinted)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET IsPrinted=@isPrinted WHERE OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "isPrinted", System.Data.DbType.Boolean, isPrinted);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool EditOrderShipNumber(string orderId, string shipNumber)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, shipNumber);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		private static string BuildProductSaleQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
			stringBuilder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
			stringBuilder.AppendFormat(" FROM Hishop_OrderItems o  WHERE 0=0 ", new object[0]);
			stringBuilder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			stringBuilder.Append(" GROUP BY ProductId HAVING ProductId IN");
			stringBuilder.Append(" (SELECT ProductId FROM Hishop_Products)");
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildRegionsUserQuery(Pagination page)
		{
			if (null == page)
			{
				throw new ArgumentNullException("page");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT r.RegionId, r.RegionName, SUM(au.UserCount) AS Usercounts,");
			stringBuilder.Append(" (SELECT (SELECT SUM(COUNT) FROM vw_aspnet_Members)) AS AllUserCounts ");
			stringBuilder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r ");
			stringBuilder.Append(" WHERE (r.AreaId IS NOT NULL) AND ((au.path LIKE r.path + LTRIM(RTRIM(STR(r.RegionId))) + ',%') OR au.RegionId = r.RegionId)");
			stringBuilder.Append(" group by r.RegionId, r.RegionName ");
			stringBuilder.Append(" UNION SELECT 0, '0', sum(au.Usercount) AS Usercounts,");
			stringBuilder.Append(" (SELECT (SELECT count(*) FROM vw_aspnet_Members)) AS AllUserCounts ");
			stringBuilder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r  ");
			stringBuilder.Append(" WHERE au.regionid IS NULL OR au.regionid = 0 group by r.RegionId, r.RegionName");
			if (!string.IsNullOrEmpty(page.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(page.SortBy), page.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildUserOrderQuery(UserOrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
			string result;
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				result = stringBuilder.ToString();
			}
			else
			{
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (!string.IsNullOrEmpty(query.SortBy))
				{
					stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
		private static string BuildOrdersQuery(OrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE 1 = 1 ", new object[0]);
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
					stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[]
					{
						1,
						4,
						9,
						DateTime.Now.AddMonths(-3)
					});
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
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildMemberStatisticsQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT UserId, UserName ");
			if (query.StartDate.HasValue || query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and userId = vw_aspnet_Members.UserId) as SaleTotals");
				stringBuilder.AppendFormat(",(select Count(OrderId) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and userId = vw_aspnet_Members.UserId) as OrderCount ");
			}
			else
			{
				stringBuilder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
			}
			stringBuilder.Append(" from vw_aspnet_Members where Expenditure > 0");
			if (!query.StartDate.HasValue && !query.EndDate.HasValue)
			{
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
			stringBuilder.Append(" FROM Hishop_products where SaleCounts>0");
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		public override bool DeleteOrderGift(string orderId, int giftId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public override bool DeleteLineItem(string skuId, string orderId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override int GetSkuStock(string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
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
		public override LineItemInfo GetLineItemInfo(string skuId, string orderId)
		{
			LineItemInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderItems WHERE SkuId=@SkuId AND OrderId=@OrderId");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(sqlStringCommand, "orderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateLineItem(dataReader);
				}
			}
			return result;
		}
		public override bool UpdateLineItem(string orderId, LineItemInfo lineItem, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, lineItem.SkuId);
			this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", System.Data.DbType.Int32, lineItem.ShipmentQuantity);
			this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", System.Data.DbType.Currency, lineItem.ItemAdjustedPrice);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, lineItem.Quantity);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override DbQueryResult GetOrderGifts(OrderGiftQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select top {0} * from Hishop_OrderGifts where OrderId=@OrderId", query.PageSize);
			if (query.PageIndex == 1)
			{
				stringBuilder.Append(" ORDER BY GiftId ASC");
			}
			else
			{
				stringBuilder.AppendFormat(" and GiftId > (select max(GiftId) from (select top {0} GiftId from Hishop_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(GiftId) as Total from Hishop_OrderGifts where OrderId=@OrderId", new object[0]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, query.OrderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}
		public override DbQueryResult GetGifts(GiftQuery query)
		{
			string filter = null;
			if (!string.IsNullOrEmpty(query.Name))
			{
				filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
		}
		public override bool ClearOrderGifts(string orderId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public override bool AddOrderGift(string orderId, OrderGiftInfo gift, int quantity, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_OrderGifts where OrderId=@OrderId AND GiftId=@GiftId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, gift.GiftId);
			bool result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("update Hishop_OrderGifts set Quantity=@Quantity where OrderId=@OrderId AND GiftId=@GiftId");
					this.database.AddInParameter(sqlStringCommand2, "OrderId", System.Data.DbType.String, orderId);
					this.database.AddInParameter(sqlStringCommand2, "GiftId", System.Data.DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand2, "Quantity", System.Data.DbType.Int32, (int)dataReader["Quantity"] + quantity);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2) == 1);
					}
				}
				else
				{
					System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("INSERT INTO Hishop_OrderGifts(OrderId,GiftId,GiftName,CostPrice,ThumbnailsUrl,Quantity,PromoType) VALUES(@OrderId,@GiftId,@GiftName,@CostPrice,@ThumbnailsUrl,@Quantity,@PromoType)");
					this.database.AddInParameter(sqlStringCommand3, "OrderId", System.Data.DbType.String, orderId);
					this.database.AddInParameter(sqlStringCommand3, "GiftId", System.Data.DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand3, "GiftName", System.Data.DbType.String, gift.GiftName);
					this.database.AddInParameter(sqlStringCommand3, "CostPrice", System.Data.DbType.Currency, gift.CostPrice);
					this.database.AddInParameter(sqlStringCommand3, "ThumbnailsUrl", System.Data.DbType.String, gift.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand3, "Quantity", System.Data.DbType.Int32, quantity);
					this.database.AddInParameter(sqlStringCommand3, "PromoType", System.Data.DbType.Int16, gift.PromoteType);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3) == 1);
					}
				}
			}
			return result;
		}
		public override IList<GiftInfo> GetGiftList(GiftQuery query)
		{
			IList<GiftInfo> list = new List<GiftInfo>();
			string text = string.Format("SELECT * FROM Hishop_Gifts WHERE [Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateGift(dataReader));
				}
			}
			return list;
		}
		public override OrderGiftInfo GetOrderGift(int giftId, string orderId)
		{
			OrderGiftInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateOrderGift(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.PurchaseOrderId))
			{
				stringBuilder.AppendFormat("PurchaseOrderId = '{0}'", query.PurchaseOrderId);
			}
			else
			{
				stringBuilder.Append(" 1=1");
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
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_PurchaseOrders", "PurchaseOrderId", stringBuilder.ToString(), "OrderId, PurchaseOrderId, PurchaseDate,RefundStatus, ShipTo, OrderTotal, PurchaseTotal,AdjustedDiscount, PaymentType,Gateway,PurchaseStatus, Distributorname,DistributorWangwang,ManagerMark,ManagerRemark,DistributorId,ISNULL(IsPrinted,0) IsPrinted,ShipOrderNumber");
		}
		public override System.Data.DataTable GetSendGoodsPurchaseOrders(string purchaseOrderIds)
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId, PurchaseOrderId, PurchaseDate,RefundStatus, ShipTo, OrderTotal, PurchaseTotal, PurchaseStatus,Distributorname,ShippingRegion,Address, ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,ShipOrderNumber,ExpressCompanyAbb,ExpressCompanyName FROM Hishop_PurchaseOrders" + string.Format(" WHERE (PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest')) AND PurchaseOrderId IN ({0}) order by PurchaseDate desc", purchaseOrderIds));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetRecentlyPurchaseOrders(out int number)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 * FROM Hishop_PurchaseOrders ORDER BY PurchaseDate DESC ");
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PurchaseOrders WHERE  PurchaseStatus=2 ");
			number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			return result;
		}
		public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
		{
			PurchaseOrderInfo purchaseOrderInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId SELECT  * FROM Hishop_PurchaseOrderGifts Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderItems Where PurchaseOrderId = @PurchaseOrderId ");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					purchaseOrderInfo = DataMapper.PopulatePurchaseOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					purchaseOrderInfo.PurchaseOrderGifts.Add(DataMapper.PopulatePurchaseOrderGift(dataReader));
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					purchaseOrderInfo.PurchaseOrderItems.Add(DataMapper.PopulatePurchaseOrderItem(dataReader));
				}
			}
			return purchaseOrderInfo;
		}
		public override int DeletePurchaseOrders(string purchaseOrderIds)
		{
			string text = string.Format("DELETE FROM Hishop_PurchaseOrders WHERE PurchaseOrderId IN({0})", purchaseOrderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,CloseReason=@CloseReason WHERE PurchaseOrderId = @PurchaseOrderId ");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "CloseReason", System.Data.DbType.String, purchaseOrder.CloseReason);
			this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 4);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdatePurchaseOrderShippingMode(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName WHERE PurchaseOrderId = @PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "ShippingModeId", System.Data.DbType.Int32, purchaseOrder.ShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "ModeName", System.Data.DbType.String, purchaseOrder.ModeName);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool SavePurchaseOrderShippingAddress(PurchaseOrderInfo purchaseOrder)
		{
			bool result;
			if (purchaseOrder == null)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE PurchaseOrderId = @PurchaseOrderId");
				this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.String, purchaseOrder.RegionId);
				this.database.AddInParameter(sqlStringCommand, "ShippingRegion", System.Data.DbType.String, purchaseOrder.ShippingRegion);
				this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, purchaseOrder.Address);
				this.database.AddInParameter(sqlStringCommand, "ZipCode", System.Data.DbType.String, purchaseOrder.ZipCode);
				this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, purchaseOrder.ShipTo);
				this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, purchaseOrder.TelPhone);
				this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, purchaseOrder.CellPhone);
				this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool SavePurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE PurchaseOrderId=@PurchaseOrderId");
			if (purchaseOrder.ManagerMark.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, (int)purchaseOrder.ManagerMark.Value);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, DBNull.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "ManagerRemark", System.Data.DbType.String, purchaseOrder.ManagerRemark);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool SendPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, PurchaseStatus = @PurchaseStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName , ExpressCompanyAbb = @ExpressCompanyAbb WHERE PurchaseOrderId = @PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, purchaseOrder.ShipOrderNumber);
			this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, purchaseOrder.RealShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, purchaseOrder.RealModeName);
			this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 3);
			this.database.AddInParameter(sqlStringCommand, "ShippingDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, purchaseOrder.ExpressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, purchaseOrder.ExpressCompanyAbb);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET  PurchaseStatus = @PurchaseStatus, FinishDate=@FinishDate WHERE PurchaseOrderId = @PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 5);
			this.database.AddInParameter(sqlStringCommand, "FinishDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override void UpdateRefundSubmitPurchaseOrderStock(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId)");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) - @refundAmount, OrderNumber = ISNULL(OrderNumber,0) - @refundNum WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "refundAmount", System.Data.DbType.Decimal, refundAmount);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			if (isAllRefund)
			{
				this.database.AddInParameter(sqlStringCommand, "refundNum", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "refundNum", System.Data.DbType.Int32, 0);
			}
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool UpdatePurchaseOrderAmount(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseTotal=@PurchaseTotal, PurchaseProfit=@PurchaseProfit, AdjustedDiscount=@AdjustedDiscount WHERE PurchaseOrderId=@PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", System.Data.DbType.Currency, purchaseOrder.GetPurchaseTotal());
			this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", System.Data.DbType.Currency, purchaseOrder.GetPurchaseProfit());
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", System.Data.DbType.Currency, purchaseOrder.AdjustedDiscount);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override void UpdateProductStock(string purchaseOrderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId)");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void UpdateDistributorAccount(decimal expenditure, int distributorId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET Expenditure=Expenditure+@expenditureAdd, PurchaseOrder = PurchaseOrder + 1 WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributorId);
			this.database.AddInParameter(sqlStringCommand, "expenditureAdd", System.Data.DbType.Decimal, expenditure);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool ConfirmPayPurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PayDate = @PayDate, PurchaseStatus=@PurchaseStatus WHERE PurchaseOrderId = @PurchaseOrderId ");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "PayDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 2);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO Hishop_PurchaseOrderItems (PurchaseOrderId,SkuId, ProductId,SKU,CostPrice,Quantity,ItemListPrice,ItemPurchasePrice,ItemDescription,ItemHomeSiteDescription,ThumbnailsUrl,Weight,SKUContent)");
			stringBuilder.Append("VALUES(@PurchaseOrderId,@SkuId, @ProductId,@SKU,@CostPrice,@Quantity,@ItemListPrice,@ItemPurchasePrice,@ItemDescription,@ItemHomeSiteDescription,@ThumbnailsUrl,@Weight,@SKUContent);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, item.SkuId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, item.ProductId);
			this.database.AddInParameter(sqlStringCommand, "SKU", System.Data.DbType.String, item.SKU);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, item.Quantity);
			this.database.AddInParameter(sqlStringCommand, "ItemListPrice", System.Data.DbType.Currency, item.ItemListPrice);
			this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice", System.Data.DbType.Currency, item.ItemPurchasePrice);
			this.database.AddInParameter(sqlStringCommand, "CostPrice", System.Data.DbType.Currency, item.CostPrice);
			this.database.AddInParameter(sqlStringCommand, "ItemDescription", System.Data.DbType.String, item.ItemDescription);
			this.database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription", System.Data.DbType.String, item.ItemDescription);
			this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl", System.Data.DbType.String, item.ThumbnailsUrl);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Int32, item.ItemWeight);
			this.database.AddInParameter(sqlStringCommand, "SKUContent", System.Data.DbType.String, item.SKUContent);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override bool UpdatePurchaseOrderQuantity(string POrderId, string SkuId, int Quantity)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrderItems SET Quantity=@Quantity WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, Quantity);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, SkuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int GetCurrentPOrderItemQuantity(string POrderId, string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Quantity FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != DBNull.Value && obj != null)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override bool DeletePurchaseOrderItem(string POrderId, string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Weight=@Weight,PurchaseProfit=@PurchaseProfit,PurchaseTotal=@PurchaseTotal,AdjustedFreight=@AdjustedFreight WHERE PurchaseOrderId=@PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Int32, purchaseOrder.Weight);
			this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", System.Data.DbType.Decimal, purchaseOrder.GetPurchaseProfit());
			this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", System.Data.DbType.Decimal, purchaseOrder.GetPurchaseTotal());
			this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Decimal, purchaseOrder.AdjustedFreight);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool AddMemberPoint(UserPointInfo point)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, point.OrderId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, point.UserId);
			this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, point.TradeDate);
			this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, (int)point.TradeType);
			this.database.AddInParameter(sqlStringCommand, "Increased", System.Data.DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Reduced", System.Data.DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, point.Points);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, point.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int GetHistoryPoint(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM Hishop_PointDetails WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override bool UpdateUserAccount(decimal orderTotal, int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1 WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "OrderPrice", System.Data.DbType.Decimal, orderTotal);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems)
		{
			bool result;
			if (lineItems.Count <= 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				foreach (LineItemInfo current in lineItems.Values)
				{
					stringBuilder.Append("UPDATE Hishop_Products SET SaleCounts = SaleCounts + @SaleCounts").Append(num).Append(", ShowSaleCounts = ShowSaleCounts + @SaleCounts").Append(num).Append(" WHERE ProductId=@ProductId").Append(num).Append(";");
					this.database.AddInParameter(sqlStringCommand, "SaleCounts" + num, System.Data.DbType.Int32, current.Quantity);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + num, System.Data.DbType.Int32, current.ProductId);
					num++;
				}
				sqlStringCommand.CommandText = stringBuilder.ToString().Remove(stringBuilder.Length - 1);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override bool ChangeMemberGrade(int userId, int gradId, int points)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM aspnet_MemberGrades Order by Point Desc ");
			bool result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read() && (int)dataReader["GradeId"] != gradId)
				{
					if ((int)dataReader["Point"] <= points)
					{
						result = this.UpdateUserRank(userId, (int)dataReader["GradeId"]);
						return result;
					}
				}
				result = true;
			}
			return result;
		}
		private bool UpdateUserRank(int userId, int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetPurchaseOrderShippingMode(string purchaseOrderIds, int realShippingModeId, string realModeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_PurchaseOrders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest')) AND PurchaseOrderId IN ({0})", purchaseOrderIds));
			this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, realShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, realModeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetPurchaseOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_PurchaseOrders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest')) AND PurchaseOrderId IN ({0})", purchaseOrderIds));
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, expressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, expressCompanyAbb);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetPurchaseOrderShipNumber(string purchaseOrderId, string shipNumber)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber=@ShipOrderNumber WHERE PurchaseOrderId =@PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, shipNumber);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool EditPurchaseOrderShipNumber(string purchaseOrderId, string orderId, string shipNumber)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber=@ShipOrderNumber WHERE PurchaseOrderId =@PurchaseOrderId");
			if (!string.IsNullOrEmpty(orderId))
			{
				System.Data.Common.DbCommand expr_1A = sqlStringCommand;
				expr_1A.CommandText += " UPDATE distro_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId";
			}
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, shipNumber);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			if (!string.IsNullOrEmpty(orderId))
			{
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			}
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetPurchaseOrderPrinted(string purchaseOrderIds, bool isPrinted)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET IsPrinted=@isPrinted WHERE PurchaseOrderId =@PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderIds);
			this.database.AddInParameter(sqlStringCommand, "isPrinted", System.Data.DbType.Boolean, isPrinted);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataSet GetPurchaseOrdersAndLines(string purchaseOrderIds)
		{
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT * FROM Hishop_PurchaseOrders WHERE PurchaseStatus > 0 AND PurchaseStatus < 4 AND PurchaseOrderId IN ({0}) order by PurchaseDate desc ", purchaseOrderIds);
			stringBuilder.AppendFormat(";SELECT * FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId IN ({0});", purchaseOrderIds);
			stringBuilder.AppendFormat("SELECT * FROM Hishop_PurchaseOrderGifts WHERE PurchaseOrderId IN ({0});", purchaseOrderIds);
			stringBuilder.Append("SELECT * FROM Hishop_Shippers ORDER BY DistributorUserId, IsDefault DESC");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override System.Data.DataSet GetTradeOrders(OrderQuery query, out int records)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_Orders_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildOrdersQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalOrders", System.Data.DbType.Int32, 4);
			System.Data.DataSet dataSet2;
			dataSet = (dataSet2 = this.database.ExecuteDataSet(storedProcCommand));
			try
			{
				dataSet.Relations.Add("OrderRelation", dataSet.Tables[0].Columns["OrderId"], dataSet.Tables[1].Columns["OrderId"]);
			}
			finally
			{
				if (dataSet2 != null)
				{
					((IDisposable)dataSet2).Dispose();
				}
			}
			records = (int)this.database.GetParameterValue(storedProcCommand, "TotalOrders");
			return dataSet;
		}
		public override System.Data.DataSet GetTradeOrders(string orderId)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT OrderId,0 as SellerUid,Username,EmailAddress,ShipTo,ShippingRegion,Address,ZipCode,CellPhone,TelPhone,Remark,ManagerMark,ManagerRemark,(select sum(Quantity) from Hishop_OrderItems where Hishop_OrderItems.OrderId=p.OrderId) as Nums,OrderTotal,OrderTotal,AdjustedFreight,DiscountValue,AdjustedDiscount,PayDate,ShippingDate,ReFundStatus,OrderStatus FROM Hishop_Orders as p Where OrderId in (",
				orderId,
				") order by OrderId; SELECT 0 as Tid,OrderId,ProductId,ItemDescription,SKU,SKUContent,Quantity,ItemListPrice,ItemAdjustedPrice,'0.00' as DiscountFee,'0.00' as Fee,'-1' as RefundStatus,'-1' as [Types],'-1' as [Status] FROM Hishop_OrderItems Where OrderId in (",
				orderId,
				")  order by OrderId"
			}));
			System.Data.DataSet dataSet2;
			dataSet = (dataSet2 = this.database.ExecuteDataSet(sqlStringCommand));
			try
			{
				dataSet.Relations.Add("OrderDetailsRelation", dataSet.Tables[0].Columns["OrderId"], dataSet.Tables[1].Columns["OrderId"]);
			}
			finally
			{
				if (dataSet2 != null)
				{
					((IDisposable)dataSet2).Dispose();
				}
			}
			return dataSet;
		}
		public override System.Data.DataSet GetAPIPurchaseOrders(PurchaseOrderQuery query, out int records)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_PurchaseOrder_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, this.BuildPurchaOrdersQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalPurchaseOrders", System.Data.DbType.Int32, 4);
			System.Data.DataSet dataSet2;
			dataSet = (dataSet2 = this.database.ExecuteDataSet(storedProcCommand));
			try
			{
				dataSet.Relations.Add("PurchaseOrderRelation", dataSet.Tables[0].Columns["PurchaseOrderId"], dataSet.Tables[1].Columns["PurchaseOrderId"]);
			}
			finally
			{
				if (dataSet2 != null)
				{
					((IDisposable)dataSet2).Dispose();
				}
			}
			records = (int)this.database.GetParameterValue(storedProcCommand, "TotalPurchaseOrders");
			return dataSet;
		}
		public string BuildPurchaOrdersQuery(PurchaseOrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE 1 = 1 ", new object[0]);
			if (!string.IsNullOrEmpty(query.PurchaseOrderId))
			{
				stringBuilder.AppendFormat("AND PurchaseOrderId = '{0}'", query.PurchaseOrderId);
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
				if (query.PurchaseStatus != OrderStatus.All)
				{
					stringBuilder.AppendFormat(" AND PurchaseStatus ={0}", Convert.ToInt32(query.PurchaseStatus));
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',PurchaseDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',PurchaseDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
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
		public override ShippingModeInfo GetShippingModeByCompany(string companyname)
		{
			ShippingModeInfo result = new ShippingModeInfo();
			string text = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId AND st.ModeId IN (SELECT ModeId FROM Hishop_TemplateRelatedShipping WHERE ExpressCompanyName='" + DataHelper.CleanSearchString(companyname) + "')";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShippingMode(dataReader);
				}
			}
			return result;
		}
		public override System.Data.DataSet GetOrdersAndLines(string orderIds)
		{
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT * FROM Hishop_Orders WHERE OrderStatus > 0 AND OrderStatus < 4 AND OrderId IN ({0}) order by OrderDate desc ", orderIds);
			stringBuilder.AppendFormat(" SELECT * FROM Hishop_OrderItems WHERE OrderId IN ({0});", orderIds);
			stringBuilder.AppendFormat(" SELECT * FROM Hishop_OrderGifts WHERE OrderId IN ({0});", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override System.Data.DataSet GetOrderGoods(string orderIds)
		{
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
			stringBuilder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
			stringBuilder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override System.Data.DataSet GetProductGoods(string orderIds)
		{
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
			stringBuilder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
			stringBuilder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override System.Data.DataSet GetPurchaseOrderGoods(string purchaseorderIds)
		{
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT PurchaseOrderId, ItemHomeSiteDescription AS ProductName, SKU, SKUContent, Quantity as ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = po.SkuId) + Quantity AS Stock, (SELECT Remark FROM Hishop_PurchaseOrders WHERE PurchaseOrderId = po.PurchaseOrderId) AS Remark");
			stringBuilder.Append(" FROM Hishop_PurchaseOrderItems po WHERE PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.AppendFormat(" AND PurchaseOrderId IN ({0}) ORDER BY PurchaseOrderId;", purchaseorderIds);
			stringBuilder.AppendFormat("SELECT PurchaseOrderId AS GiftPurchaseOrderId,GiftName,Quantity FROM dbo.Hishop_PurchaseOrderGifts WHERE PurchaseOrderId IN({0}) AND PurchaseOrderId IN(SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", purchaseorderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override System.Data.DataSet GetPurchaseProductGoods(string purchaseorderIds)
		{
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ItemHomeSiteDescription AS ProductName, SKU, SKUContent, sum(Quantity) as ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = po.SkuId) + sum(Quantity) AS Stock FROM Hishop_PurchaseOrderItems po");
			stringBuilder.Append(" WHERE PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.AppendFormat(" AND PurchaseOrderId in ({0}) GROUP BY ItemHomeSiteDescription, SkuId, SKU, SKUContent;", purchaseorderIds);
			stringBuilder.AppendFormat("SELECT PurchaseOrderId AS GiftPurchaseOrderId,GiftName,Quantity FROM dbo.Hishop_PurchaseOrderGifts WHERE PurchaseOrderId IN({0}) AND PurchaseOrderId IN(SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", purchaseorderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
			stringBuilder.Append(" update Hishop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
			if (refundType == 1 && accept)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				if (orderInfo != null)
				{
					Member member = Users.GetUser(orderInfo.UserId, false) as Member;
					decimal num = orderInfo.GetTotal();
					decimal num2 = member.Balance + num;
					if (orderInfo.GroupBuyStatus != GroupBuyStatus.Failed)
					{
						num -= orderInfo.NeedPrice;
						num2 -= orderInfo.NeedPrice;
					}
					stringBuilder.Append("insert into Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark)");
					stringBuilder.AppendFormat("values({0},'{1}',{2},{3},{4},{5},'{6}')", new object[]
					{
						member.UserId,
						member.Username,
						"getdate()",
						5,
						num,
						num2,
						"订单" + orderId + "退款"
					});
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 9);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 2);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, Operator);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void GetRefundType(string orderId, out int refundType, out string remark)
		{
			refundType = 0;
			remark = "";
			string text = "select RefundType,RefundRemark from Hishop_OrderRefund where HandleStatus=0 and OrderId='" + orderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				refundType = (int)dataReader["RefundType"];
				remark = (string)dataReader["RefundRemark"];
			}
		}
		public override bool CheckReturn(string orderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
			stringBuilder.Append(" update Hishop_OrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney where HandleStatus=0 and OrderId = @OrderId;");
			if (refundType == 1 && accept)
			{
				stringBuilder.Append(" insert into Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
				stringBuilder.AppendFormat(",Balance,Remark) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
				stringBuilder.Append("@RefundMoney as Income,isnull((select top 1 Balance from Hishop_BalanceDetails b");
				stringBuilder.AppendFormat(" where b.UserId=a.UserId order by JournalNumber desc),0)+@RefundMoney as Balance,'订单{0}退货' as Remark ", orderId);
				stringBuilder.Append("from Hishop_Orders a where OrderId = @OrderId;");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 10);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, Operator);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "RefundMoney", System.Data.DbType.Decimal, refundMoney);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
		{
			refundType = 0;
			remark = "";
			string text = "select RefundType,Comments from Hishop_OrderReturns where HandleStatus=0 and OrderId='" + orderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				refundType = (int)dataReader["RefundType"];
				remark = (string)dataReader["Comments"];
			}
		}
		public override bool CheckReplace(string orderId, string adminRemark, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
			stringBuilder.Append(" update Hishop_OrderReplace set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override string GetReplaceComments(string orderId)
		{
			string text = "select Comments from Hishop_OrderReplace where HandleStatus=0 and OrderId='" + orderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj == null || obj is DBNull)
			{
				result = "";
			}
			else
			{
				result = obj.ToString();
			}
			return result;
		}
		public override DbQueryResult GetRefundApplys(RefundApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderRefund", "RefundId", stringBuilder.ToString(), "*");
		}
		public override bool DelRefundApply(int refundId)
		{
			string text = string.Format("DELETE FROM Hishop_OrderRefund WHERE RefundId={0} and HandleStatus >0 ", refundId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReturns", "ReturnsId", stringBuilder.ToString(), "*");
		}
		public override bool DelReturnsApply(int returnsId)
		{
			string text = string.Format("DELETE FROM Hishop_OrderReturns WHERE ReturnsId={0} and HandleStatus >0 ", returnsId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReplace", "ReplaceId", stringBuilder.ToString(), "*");
		}
		public override bool DelReplaceApply(int replaceId)
		{
			string text = string.Format("DELETE FROM Hishop_OrderReplace WHERE ReplaceId={0} and HandleStatus >0 ", replaceId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SaveDebitNote(DebitNote note)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" insert into Hishop_OrderDebitNote(NoteId,OrderId,Operator,Remark) values(@NoteId,@OrderId,@Operator,@Remark)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, note.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetAllDebitNote(DebitNoteQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderDebitNote", "NoteId", stringBuilder.ToString(), "*");
		}
		public override bool DelDebitNote(string noteId)
		{
			string text = string.Format("DELETE FROM Hishop_OrderDebitNote WHERE NoteId='{0}'", noteId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SaveSendNote(SendNote note)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" insert into Hishop_OrderSendNote(NoteId,OrderId,Operator,Remark) values(@NoteId,@OrderId,@Operator,@Remark)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, note.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetAllSendNote(RefundApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderSendNote", "OrderId", stringBuilder.ToString(), "*");
		}
		public override bool DelSendNote(string noteId)
		{
			string text = string.Format("DELETE FROM Hishop_OrderSendNote WHERE NoteId='{0}'", noteId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CheckPurchaseRefund(string purchaseOrderId, string Operator, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
			stringBuilder.Append(" update Hishop_PurchaseOrderRefund set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and PurchaseOrderId = @PurchaseOrderId;");
			if (refundType == 1 && accept)
			{
				stringBuilder.Append(" insert into Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
				stringBuilder.AppendFormat(",Balance,Remark) select DistributorId as UserId,Distributorname as Username,getdate() as TradeDate,{0} as TradeType,", 5);
				stringBuilder.Append("PurchaseTotal as Income,isnull((select top 1 Balance from Hishop_DistributorBalanceDetails b");
				stringBuilder.AppendFormat(" where b.UserId=a.DistributorId order by JournalNumber desc),0)+PurchaseTotal as Balance,'采购单{0}退款' as Remark ", purchaseOrderId);
				stringBuilder.Append("from Hishop_PurchaseOrders a where PurchaseOrderId = @PurchaseOrderId;");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 9);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 2);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, Operator);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void GetPurchaseRefundType(string purchaseOrderId, out int refundType, out string remark)
		{
			refundType = 0;
			remark = "";
			string text = "select RefundType,RefundRemark from Hishop_PurchaseOrderRefund where HandleStatus=0 and PurchaseOrderId='" + purchaseOrderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				refundType = (int)dataReader["RefundType"];
				remark = (string)dataReader["RefundRemark"];
			}
		}
		public override bool CheckPurchaseReturn(string purchaseOrderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
			stringBuilder.Append(" update Hishop_PurchaseOrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,RefundMoney=@RefundMoney,HandleTime=@HandleTime where HandleStatus=0 and PurchaseOrderId = @PurchaseOrderId;");
			if (refundType == 1 && accept)
			{
				stringBuilder.Append(" insert into Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
				stringBuilder.AppendFormat(",Balance,Remark) select DistributorId as UserId,Distributorname as Username,getdate() as TradeDate,{0} as TradeType,", 7);
				stringBuilder.Append("@RefundMoney as Income,isnull((select top 1 Balance from Hishop_DistributorBalanceDetails b");
				stringBuilder.AppendFormat(" where b.UserId=a.DistributorId order by JournalNumber desc),0)+@RefundMoney as Balance,'采购单{0}退货' as Remark ", purchaseOrderId);
				stringBuilder.Append("from Hishop_PurchaseOrders a where PurchaseOrderId = @PurchaseOrderId;");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 10);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "RefundMoney", System.Data.DbType.Decimal, refundMoney);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, Operator);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void GetPurchaseRefundTypeFromReturn(string purchaseOrderId, out int refundType, out string remark)
		{
			refundType = 0;
			remark = "";
			string text = "select RefundType,Comments from Hishop_PurchaseOrderReturns where HandleStatus=0 and PurchaseOrderId='" + purchaseOrderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				refundType = (int)dataReader["RefundType"];
				remark = (string)dataReader["Comments"];
			}
		}
		public override bool CheckPurchaseReplace(string purchaseOrderId, string adminRemark, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
			stringBuilder.Append(" update Hishop_PurchaseOrderReplace set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and PurchaseOrderId = @PurchaseOrderId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override string GetPurchaseReplaceComments(string purchaseOrderId)
		{
			string text = "select Comments from Hishop_PurchaseOrderReplace where HandleStatus=0 and PurchaseOrderId='" + purchaseOrderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj == null || obj is DBNull)
			{
				result = "";
			}
			else
			{
				result = obj.ToString();
			}
			return result;
		}
		public override DbQueryResult GetPurchaseRefundApplys(RefundApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and PurchaseOrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseOrderRefund", "RefundId", stringBuilder.ToString(), "*");
		}
		public override bool DelPurchaseRefundApply(int refundId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseOrderRefund WHERE RefundId={0} and HandleStatus >0 ", refundId));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetPurchaseReturnsApplys(ReturnsApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and PurchaseOrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseOrderReturns", "ReturnsId", stringBuilder.ToString(), "*");
		}
		public override bool DelPurchaseReturnsApply(int returnId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseOrderReturns WHERE ReturnsId={0} and HandleStatus >0 ", returnId));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetPurchaseReplaceApplys(ReplaceApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and PurchaseOrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseOrderReplace", "ReplaceId", stringBuilder.ToString(), "*");
		}
		public override bool DelPurchaseReplaceApply(int replaceId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseOrderReplace WHERE ReplaceId={0} and HandleStatus >0 ", replaceId));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SavePurchaseDebitNote(PurchaseDebitNote note)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Hishop_PurchaseDebitNote(NoteId,PurchaseOrderId,Operator,Remark) values(@NoteId,@PurchaseOrderId,@Operator,@Remark)");
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, note.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetAllPurchaseDebitNote(DebitNoteQuery query)
		{
			string filter = string.Empty;
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				filter = string.Format("PurchaseOrderId = '{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseDebitNote", "NoteId", filter, "*");
		}
		public override bool DelPurchaseDebitNote(string noteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseDebitNote WHERE NoteId='{0}'", noteId));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SavePurchaseSendNote(SendNote note)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Hishop_PurchaseSendNote(NoteId,PurchaseOrderId,Operator,Remark) values(@NoteId,@PurchaseOrderId,@Operator,@Remark)");
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, note.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetAllPurchaseSendNote(RefundApplyQuery query)
		{
			string filter = string.Empty;
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				filter = string.Format("PurchaseOrderId = '{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseSendNote", "NoteId", filter, "*");
		}
		public override bool DelPurchaseSendNote(string noteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseSendNote WHERE NoteId='{0}'", noteId));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
