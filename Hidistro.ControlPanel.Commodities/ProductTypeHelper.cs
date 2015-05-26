using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
namespace Hidistro.ControlPanel.Commodities
{
	public sealed class ProductTypeHelper
	{
		private ProductTypeHelper()
		{
		}
		public static DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return ProductProvider.Instance().GetProductTypes(query);
		}
		public static IList<ProductTypeInfo> GetProductTypes()
		{
			return ProductProvider.Instance().GetProductTypes();
		}
		public static ProductTypeInfo GetProductType(int typeId)
		{
			return ProductProvider.Instance().GetProductType(typeId);
		}
		public static System.Data.DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			return ProductProvider.Instance().GetBrandCategoriesByTypeId(typeId);
		}
		public static int GetTypeId(string typeName)
		{
			int typeId = ProductProvider.Instance().GetTypeId(typeName);
			int result;
			if (typeId > 0)
			{
				result = typeId;
			}
			else
			{
				ProductTypeInfo productTypeInfo = new ProductTypeInfo();
				productTypeInfo.TypeName = typeName;
				result = ProductProvider.Instance().AddProductType(productTypeInfo);
			}
			return result;
		}
		public static int AddProductType(ProductTypeInfo productType)
		{
			int result;
			if (productType == null)
			{
				result = 0;
			}
			else
			{
				Globals.EntityCoding(productType, true);
				int num = ProductProvider.Instance().AddProductType(productType);
				if (num > 0)
				{
					if (productType.Brands.Count > 0)
					{
						ProductProvider.Instance().AddProductTypeBrands(num, productType.Brands);
					}
					EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "创建了一个新的商品类型:”{0}”", new object[]
					{
						productType.TypeName
					}));
				}
				result = num;
			}
			return result;
		}
		public static bool UpdateProductType(ProductTypeInfo productType)
		{
			bool result;
			if (productType == null)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(productType, true);
				bool flag;
				if (flag = ProductProvider.Instance().UpdateProductType(productType))
				{
					if (ProductProvider.Instance().DeleteProductTypeBrands(productType.TypeId))
					{
						ProductProvider.Instance().AddProductTypeBrands(productType.TypeId, productType.Brands);
					}
					EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的商品类型", new object[]
					{
						productType.TypeId
					}));
				}
				result = flag;
			}
			return result;
		}
		public static bool DeleteProductType(int typeId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
			bool result;
			if (result = ProductProvider.Instance().DeleteProducType(typeId))
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的商品类型", new object[]
				{
					typeId
				}));
			}
			return result;
		}
		public static AttributeInfo GetAttribute(int attributeId)
		{
			return ProductProvider.Instance().GetAttribute(attributeId);
		}
		public static bool AddAttribute(AttributeInfo attribute)
		{
			return ProductProvider.Instance().AddAttribute(attribute);
		}
		public static int GetSpecificationId(int typeId, string specificationName)
		{
			int specificationId = ProductProvider.Instance().GetSpecificationId(typeId, specificationName);
			int result;
			if (specificationId > 0)
			{
				result = specificationId;
			}
			else
			{
				AttributeInfo attributeInfo = new AttributeInfo();
				attributeInfo.TypeId = typeId;
				attributeInfo.UsageMode = AttributeUseageMode.Choose;
				attributeInfo.UseAttributeImage = false;
				attributeInfo.AttributeName = specificationName;
				result = ProductProvider.Instance().AddAttributeName(attributeInfo);
			}
			return result;
		}
		public static bool AddAttributeName(AttributeInfo attribute)
		{
			return ProductProvider.Instance().AddAttributeName(attribute) > 0;
		}
		public static bool UpdateAttribute(AttributeInfo attribute)
		{
			return ProductProvider.Instance().UpdateAttribute(attribute);
		}
		public static bool UpdateAttributeName(AttributeInfo attribute)
		{
			return ProductProvider.Instance().UpdateAttributeName(attribute);
		}
		public static bool DeleteAttribute(int attriubteId)
		{
			return ProductProvider.Instance().DeleteAttribute(attriubteId);
		}
		public static void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
		{
			ProductProvider.Instance().SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
		}
		public static IList<AttributeInfo> GetAttributes(int typeId)
		{
			return ProductProvider.Instance().GetAttributes(typeId);
		}
		public static IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
		{
			return ProductProvider.Instance().GetAttributes(attributeUseageMode);
		}
		public static IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
		{
			return ProductProvider.Instance().GetAttributes(typeId, attributeUseageMode);
		}
		public static bool UpdateSpecification(AttributeInfo attribute)
		{
			return ProductProvider.Instance().UpdateSpecification(attribute);
		}
		public static int AddAttributeValue(AttributeValueInfo attributeValue)
		{
			return ProductProvider.Instance().AddAttributeValue(attributeValue);
		}
		public static int GetSpecificationValueId(int attributeId, string valueStr)
		{
			int specificationValueId = ProductProvider.Instance().GetSpecificationValueId(attributeId, valueStr);
			int result;
			if (specificationValueId > 0)
			{
				result = specificationValueId;
			}
			else
			{
				AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
				attributeValueInfo.AttributeId = attributeId;
				attributeValueInfo.ValueStr = valueStr;
				result = ProductProvider.Instance().AddAttributeValue(attributeValueInfo);
			}
			return result;
		}
		public static bool ClearAttributeValue(int attributeId)
		{
			return ProductProvider.Instance().ClearAttributeValue(attributeId);
		}
		public static bool DeleteAttribute(int attributeId, int valueId)
		{
			return ProductProvider.Instance().DeleteAttribute(attributeId, valueId);
		}
		public static bool DeleteAttributeValue(int attributeValueId)
		{
			return ProductProvider.Instance().DeleteAttributeValue(attributeValueId);
		}
		public static bool UpdateAttributeValue(int attributeId, int valueId, string newValue)
		{
			return ProductProvider.Instance().UpdateAttributeValue(attributeId, valueId, newValue);
		}
		public static bool UpdateSku(AttributeValueInfo attributeValue)
		{
			return ProductProvider.Instance().UpdateSku(attributeValue);
		}
		public static void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			ProductProvider.Instance().SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}
		public static AttributeValueInfo GetAttributeValueInfo(int valueId)
		{
			return ProductProvider.Instance().GetAttributeValueInfo(valueId);
		}
		public static string UploadSKUImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/sku/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
	}
}
