using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using System;
using System.Data;
namespace Hidistro.Entities
{
	public static class DataMapper
	{
		public static ArticleInfo PopulateArticle(IDataRecord reader)
		{
			ArticleInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ArticleInfo articleInfo = new ArticleInfo();
				articleInfo.ArticleId = (int)reader["ArticleId"];
				articleInfo.CategoryId = (int)reader["CategoryId"];
				articleInfo.Title = (string)reader["Title"];
				if (reader["Meta_Description"] != System.DBNull.Value)
				{
					articleInfo.MetaDescription = (string)reader["Meta_Description"];
				}
				if (reader["Meta_Keywords"] != System.DBNull.Value)
				{
					articleInfo.MetaKeywords = (string)reader["Meta_Keywords"];
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					articleInfo.Description = (string)reader["Description"];
				}
				if (reader["IconUrl"] != System.DBNull.Value)
				{
					articleInfo.IconUrl = (string)reader["IconUrl"];
				}
				articleInfo.Content = (string)reader["Content"];
				articleInfo.AddedDate = (System.DateTime)reader["AddedDate"];
				articleInfo.IsRelease = (bool)reader["IsRelease"];
				result = articleInfo;
			}
			return result;
		}
		public static ArticleCategoryInfo PopulateArticleCategory(IDataRecord reader)
		{
			ArticleCategoryInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ArticleCategoryInfo articleCategoryInfo = new ArticleCategoryInfo();
				articleCategoryInfo.CategoryId = (int)reader["CategoryId"];
				articleCategoryInfo.Name = (string)reader["Name"];
				articleCategoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
				if (reader["IconUrl"] != System.DBNull.Value)
				{
					articleCategoryInfo.IconUrl = (string)reader["IconUrl"];
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					articleCategoryInfo.Description = (string)reader["Description"];
				}
				result = articleCategoryInfo;
			}
			return result;
		}
		public static HelpInfo PopulateHelp(IDataReader reader)
		{
			HelpInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				HelpInfo helpInfo = new HelpInfo();
				helpInfo.CategoryId = (int)reader["CategoryId"];
				helpInfo.HelpId = (int)reader["HelpId"];
				helpInfo.Title = (string)reader["Title"];
				if (reader["Meta_Description"] != System.DBNull.Value)
				{
					helpInfo.MetaDescription = (string)reader["Meta_Description"];
				}
				if (reader["Meta_Keywords"] != System.DBNull.Value)
				{
					helpInfo.MetaKeywords = (string)reader["Meta_Keywords"];
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					helpInfo.Description = (string)reader["Description"];
				}
				helpInfo.Content = (string)reader["Content"];
				helpInfo.AddedDate = (System.DateTime)reader["AddedDate"];
				helpInfo.IsShowFooter = (bool)reader["IsShowFooter"];
				result = helpInfo;
			}
			return result;
		}
		public static HelpCategoryInfo PopulateHelpCategory(IDataReader reader)
		{
			HelpCategoryInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
				helpCategoryInfo.CategoryId = new int?((int)reader["CategoryId"]);
				helpCategoryInfo.Name = (string)reader["Name"];
				helpCategoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
				if (reader["IconUrl"] != System.DBNull.Value)
				{
					helpCategoryInfo.IconUrl = (string)reader["IconUrl"];
				}
				if (reader["IndexChar"] != System.DBNull.Value)
				{
					helpCategoryInfo.IndexChar = (string)reader["IndexChar"];
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					helpCategoryInfo.Description = (string)reader["Description"];
				}
				helpCategoryInfo.IsShowFooter = (bool)reader["IsShowFooter"];
				result = helpCategoryInfo;
			}
			return result;
		}
		public static AfficheInfo PopulateAffiche(IDataReader reader)
		{
			AfficheInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				AfficheInfo afficheInfo = new AfficheInfo();
				afficheInfo.AfficheId = (int)reader["AfficheId"];
				if (reader["Title"] != System.DBNull.Value)
				{
					afficheInfo.Title = (string)reader["Title"];
				}
				afficheInfo.Content = (string)reader["Content"];
				afficheInfo.AddedDate = (System.DateTime)reader["AddedDate"];
				result = afficheInfo;
			}
			return result;
		}
		public static LeaveCommentInfo PopulateLeaveComment(IDataReader reader)
		{
			LeaveCommentInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				LeaveCommentInfo leaveCommentInfo = new LeaveCommentInfo();
				leaveCommentInfo.LeaveId = (long)reader["LeaveId"];
				if (reader["UserId"] != System.DBNull.Value)
				{
					leaveCommentInfo.UserId = new int?((int)reader["UserId"]);
				}
				if (reader["UserName"] != System.DBNull.Value)
				{
					leaveCommentInfo.UserName = (string)reader["UserName"];
				}
				leaveCommentInfo.Title = (string)reader["Title"];
				leaveCommentInfo.PublishContent = (string)reader["PublishContent"];
				leaveCommentInfo.PublishDate = (System.DateTime)reader["PublishDate"];
				leaveCommentInfo.LastDate = (System.DateTime)reader["LastDate"];
				result = leaveCommentInfo;
			}
			return result;
		}
		public static LeaveCommentReplyInfo PopulateLeaveCommentReply(IDataReader reader)
		{
			LeaveCommentReplyInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new LeaveCommentReplyInfo
				{
					LeaveId = (long)reader["LeaveId"],
					ReplyId = (long)reader["ReplyId"],
					UserId = (int)reader["UserId"],
					ReplyContent = (string)reader["ReplyContent"],
					ReplyDate = (System.DateTime)reader["ReplyDate"]
				};
			}
			return result;
		}
		public static ProductConsultationInfo PopulateProductConsultation(IDataRecord reader)
		{
			ProductConsultationInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ProductConsultationInfo productConsultationInfo = new ProductConsultationInfo();
				productConsultationInfo.ConsultationId = (int)reader["ConsultationId"];
				productConsultationInfo.ProductId = (int)reader["ProductId"];
				productConsultationInfo.UserId = (int)reader["UserId"];
				productConsultationInfo.UserName = (string)reader["UserName"];
				productConsultationInfo.ConsultationText = (string)reader["ConsultationText"];
				productConsultationInfo.ConsultationDate = (System.DateTime)reader["ConsultationDate"];
				productConsultationInfo.UserEmail = (string)reader["UserEmail"];
				if (System.DBNull.Value != reader["ReplyText"])
				{
					productConsultationInfo.ReplyText = (string)reader["ReplyText"];
				}
				if (System.DBNull.Value != reader["ReplyDate"])
				{
					productConsultationInfo.ReplyDate = new System.DateTime?((System.DateTime)reader["ReplyDate"]);
				}
				if (System.DBNull.Value != reader["ReplyUserId"])
				{
					productConsultationInfo.ReplyUserId = new int?((int)reader["ReplyUserId"]);
				}
				result = productConsultationInfo;
			}
			return result;
		}
		public static ProductReviewInfo PopulateProductReview(IDataRecord reader)
		{
			ProductReviewInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new ProductReviewInfo
				{
					ReviewId = (long)reader["ReviewId"],
					ProductId = (int)reader["ProductId"],
					UserId = (int)reader["UserId"],
					ReviewText = (string)reader["ReviewText"],
					UserName = (string)reader["UserName"],
					UserEmail = (string)reader["UserEmail"],
					ReviewDate = (System.DateTime)reader["ReviewDate"]
				};
			}
			return result;
		}
		public static ProductInfo PopulateProduct(IDataReader reader)
		{
			ProductInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				ProductInfo productInfo = new ProductInfo();
				productInfo.CategoryId = (int)reader["CategoryId"];
				productInfo.ProductId = (int)reader["ProductId"];
				if (System.DBNull.Value != reader["TypeId"])
				{
					productInfo.TypeId = new int?((int)reader["TypeId"]);
				}
				productInfo.ProductName = (string)reader["ProductName"];
				if (System.DBNull.Value != reader["ProductCode"])
				{
					productInfo.ProductCode = (string)reader["ProductCode"];
				}
				if (System.DBNull.Value != reader["ShortDescription"])
				{
					productInfo.ShortDescription = (string)reader["ShortDescription"];
				}
				if (System.DBNull.Value != reader["Unit"])
				{
					productInfo.Unit = (string)reader["Unit"];
				}
				if (System.DBNull.Value != reader["Description"])
				{
					productInfo.Description = (string)reader["Description"];
				}
				if (System.DBNull.Value != reader["Title"])
				{
					productInfo.Title = (string)reader["Title"];
				}
				if (System.DBNull.Value != reader["Meta_Description"])
				{
					productInfo.MetaDescription = (string)reader["Meta_Description"];
				}
				if (System.DBNull.Value != reader["Meta_Keywords"])
				{
					productInfo.MetaKeywords = (string)reader["Meta_Keywords"];
				}
				productInfo.SaleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
				productInfo.AddedDate = (System.DateTime)reader["AddedDate"];
				productInfo.VistiCounts = (int)reader["VistiCounts"];
				productInfo.SaleCounts = (int)reader["SaleCounts"];
				productInfo.ShowSaleCounts = (int)reader["ShowSaleCounts"];
				productInfo.DisplaySequence = (int)reader["DisplaySequence"];
				if (System.DBNull.Value != reader["ImageUrl1"])
				{
					productInfo.ImageUrl1 = (string)reader["ImageUrl1"];
				}
				if (System.DBNull.Value != reader["ImageUrl2"])
				{
					productInfo.ImageUrl2 = (string)reader["ImageUrl2"];
				}
				if (System.DBNull.Value != reader["ImageUrl3"])
				{
					productInfo.ImageUrl3 = (string)reader["ImageUrl3"];
				}
				if (System.DBNull.Value != reader["ImageUrl4"])
				{
					productInfo.ImageUrl4 = (string)reader["ImageUrl4"];
				}
				if (System.DBNull.Value != reader["ImageUrl5"])
				{
					productInfo.ImageUrl5 = (string)reader["ImageUrl5"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl40"])
				{
					productInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl60"])
				{
					productInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl100"])
				{
					productInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl160"])
				{
					productInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl180"])
				{
					productInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl220"])
				{
					productInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl310"])
				{
					productInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl410"])
				{
					productInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
				}
				productInfo.LineId = (int)reader["LineId"];
				if (System.DBNull.Value != reader["MarketPrice"])
				{
					productInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
				}
				productInfo.LowestSalePrice = (decimal)reader["LowestSalePrice"];
				productInfo.PenetrationStatus = (PenetrationStatus)((short)reader["PenetrationStatus"]);
				if (System.DBNull.Value != reader["BrandId"])
				{
					productInfo.BrandId = new int?((int)reader["BrandId"]);
				}
				if (reader["MainCategoryPath"] != System.DBNull.Value)
				{
					productInfo.MainCategoryPath = (string)reader["MainCategoryPath"];
				}
				if (reader["ExtendCategoryPath"] != System.DBNull.Value)
				{
					productInfo.ExtendCategoryPath = (string)reader["ExtendCategoryPath"];
				}
				productInfo.HasSKU = (bool)reader["HasSKU"];
				if (reader["TaobaoProductId"] != System.DBNull.Value)
				{
					productInfo.TaobaoProductId = (long)reader["TaobaoProductId"];
				}
				result = productInfo;
			}
			return result;
		}
		public static ProductInfo PopulateSubProduct(IDataReader reader)
		{
			ProductInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				ProductInfo productInfo = new ProductInfo();
				productInfo.CategoryId = (int)reader["CategoryId"];
				productInfo.ProductId = (int)reader["ProductId"];
				if (System.DBNull.Value != reader["TypeId"])
				{
					productInfo.TypeId = new int?((int)reader["TypeId"]);
				}
				productInfo.ProductName = (string)reader["ProductName"];
				if (System.DBNull.Value != reader["ProductCode"])
				{
					productInfo.ProductCode = (string)reader["ProductCode"];
				}
				if (System.DBNull.Value != reader["ShortDescription"])
				{
					productInfo.ShortDescription = (string)reader["ShortDescription"];
				}
				if (System.DBNull.Value != reader["Description"])
				{
					productInfo.Description = (string)reader["Description"];
				}
				if (System.DBNull.Value != reader["Title"])
				{
					productInfo.Title = (string)reader["Title"];
				}
				if (System.DBNull.Value != reader["Meta_Description"])
				{
					productInfo.MetaDescription = (string)reader["Meta_Description"];
				}
				if (System.DBNull.Value != reader["Meta_Keywords"])
				{
					productInfo.MetaKeywords = (string)reader["Meta_Keywords"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl40"])
				{
					productInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl60"])
				{
					productInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl100"])
				{
					productInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl160"])
				{
					productInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl180"])
				{
					productInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl220"])
				{
					productInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl310"])
				{
					productInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl410"])
				{
					productInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
				}
				if (System.DBNull.Value != reader["MarketPrice"])
				{
					productInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
				}
				productInfo.LowestSalePrice = (decimal)reader["LowestSalePrice"];
				if (System.DBNull.Value != reader["BrandId"])
				{
					productInfo.BrandId = new int?((int)reader["BrandId"]);
				}
				productInfo.SaleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
				productInfo.PenetrationStatus = (PenetrationStatus)((short)reader["PenetrationStatus"]);
				productInfo.VistiCounts = (int)reader["VistiCounts"];
				productInfo.DisplaySequence = (int)reader["DisplaySequence"];
				productInfo.LineId = (int)reader["LineId"];
				result = productInfo;
			}
			return result;
		}
		public static CategoryInfo ConvertDataRowToProductCategory(DataRow dataRow_0)
		{
			CategoryInfo categoryInfo = new CategoryInfo();
			categoryInfo.CategoryId = (int)dataRow_0["CategoryId"];
			categoryInfo.Name = (string)dataRow_0["Name"];
			categoryInfo.DisplaySequence = (int)dataRow_0["DisplaySequence"];
			if (dataRow_0["ParentCategoryId"] != System.DBNull.Value)
			{
				categoryInfo.ParentCategoryId = new int?((int)dataRow_0["ParentCategoryId"]);
			}
			if (dataRow_0["Icon"] != System.DBNull.Value)
			{
				categoryInfo.Icon = (string)dataRow_0["Icon"];
			}
			categoryInfo.Depth = (int)dataRow_0["Depth"];
			categoryInfo.Path = (string)dataRow_0["Path"];
			if (dataRow_0["RewriteName"] != System.DBNull.Value)
			{
				categoryInfo.RewriteName = (string)dataRow_0["RewriteName"];
			}
			if (dataRow_0["Theme"] != System.DBNull.Value)
			{
				categoryInfo.Theme = (string)dataRow_0["Theme"];
			}
			categoryInfo.HasChildren = (bool)dataRow_0["HasChildren"];
			return categoryInfo;
		}
		public static ProductTypeInfo PopulateType(IDataReader reader)
		{
			ProductTypeInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				ProductTypeInfo productTypeInfo = new ProductTypeInfo();
				productTypeInfo.TypeId = (int)reader["TypeId"];
				productTypeInfo.TypeName = (string)reader["TypeName"];
				if (reader["Remark"] != System.DBNull.Value)
				{
					productTypeInfo.Remark = (string)reader["Remark"];
				}
				result = productTypeInfo;
			}
			return result;
		}
		public static SKUItem PopulateSKU(IDataReader reader)
		{
			SKUItem result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				SKUItem sKUItem = new SKUItem();
				sKUItem.SkuId = (string)reader["SkuId"];
				sKUItem.ProductId = (int)reader["ProductId"];
				if (reader["SKU"] != System.DBNull.Value)
				{
					sKUItem.SKU = (string)reader["SKU"];
				}
				if (reader["Weight"] != System.DBNull.Value)
				{
					sKUItem.Weight = (decimal)reader["Weight"];
				}
				sKUItem.Stock = (int)reader["Stock"];
				sKUItem.AlertStock = (int)reader["AlertStock"];
				if (reader["CostPrice"] != System.DBNull.Value)
				{
					sKUItem.CostPrice = (decimal)reader["CostPrice"];
				}
				sKUItem.SalePrice = (decimal)reader["SalePrice"];
				sKUItem.PurchasePrice = (decimal)reader["PurchasePrice"];
				result = sKUItem;
			}
			return result;
		}
		public static AttributeValueInfo PopulateAttributeValue(IDataReader reader)
		{
			AttributeValueInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				result = new AttributeValueInfo
				{
					ValueId = (int)reader["ValueId"],
					AttributeId = (int)reader["AttributeId"],
					ValueStr = (string)reader["ValueStr"]
				};
			}
			return result;
		}
		public static ProductLineInfo PopulateProductLine(IDataRecord reader)
		{
			ProductLineInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ProductLineInfo productLineInfo = new ProductLineInfo
				{
					LineId = (int)reader["LineId"],
					Name = (string)reader["Name"]
				};
				if (reader["SupplierName"] != System.DBNull.Value)
				{
					productLineInfo.SupplierName = (string)reader["SupplierName"];
				}
				result = productLineInfo;
			}
			return result;
		}
		public static CategoryInfo PopulateProductCategory(IDataRecord reader)
		{
			CategoryInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				CategoryInfo categoryInfo = new CategoryInfo();
				categoryInfo.CategoryId = (int)reader["CategoryId"];
				categoryInfo.Name = (string)reader["Name"];
				categoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
				if (reader["AssociatedProductType"] != System.DBNull.Value)
				{
					categoryInfo.AssociatedProductType = new int?((int)reader["AssociatedProductType"]);
				}
				if (reader["Meta_Description"] != System.DBNull.Value)
				{
					categoryInfo.MetaDescription = (string)reader["Meta_Description"];
				}
				if (reader["Meta_Keywords"] != System.DBNull.Value)
				{
					categoryInfo.MetaKeywords = (string)reader["Meta_Keywords"];
				}
				if (reader["Notes1"] != System.DBNull.Value)
				{
					categoryInfo.Notes1 = (string)reader["Notes1"];
				}
				if (reader["Notes2"] != System.DBNull.Value)
				{
					categoryInfo.Notes2 = (string)reader["Notes2"];
				}
				if (reader["Notes3"] != System.DBNull.Value)
				{
					categoryInfo.Notes3 = (string)reader["Notes3"];
				}
				if (reader["Notes4"] != System.DBNull.Value)
				{
					categoryInfo.Notes4 = (string)reader["Notes4"];
				}
				if (reader["Notes5"] != System.DBNull.Value)
				{
					categoryInfo.Notes5 = (string)reader["Notes5"];
				}
				if (reader["Icon"] != System.DBNull.Value)
				{
					categoryInfo.Icon = (string)reader["Icon"];
				}
				if (reader["ParentCategoryId"] != System.DBNull.Value)
				{
					categoryInfo.ParentCategoryId = new int?((int)reader["ParentCategoryId"]);
				}
				categoryInfo.Depth = (int)reader["Depth"];
				categoryInfo.Path = (string)reader["Path"];
				if (reader["RewriteName"] != System.DBNull.Value)
				{
					categoryInfo.RewriteName = (string)reader["RewriteName"];
				}
				if (reader["SKUPrefix"] != System.DBNull.Value)
				{
					categoryInfo.SKUPrefix = (string)reader["SKUPrefix"];
				}
				if (reader["Theme"] != System.DBNull.Value)
				{
					categoryInfo.Theme = (string)reader["Theme"];
				}
				categoryInfo.HasChildren = (bool)reader["HasChildren"];
				result = categoryInfo;
			}
			return result;
		}
		public static BrandCategoryInfo PopulateBrandCategory(IDataRecord reader)
		{
			BrandCategoryInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
				brandCategoryInfo.BrandId = (int)reader["BrandId"];
				brandCategoryInfo.BrandName = (string)reader["BrandName"];
				if (reader["DisplaySequence"] != System.DBNull.Value)
				{
					brandCategoryInfo.DisplaySequence = (int)reader["DisplaySequence"];
				}
				if (reader["Logo"] != System.DBNull.Value)
				{
					brandCategoryInfo.Logo = (string)reader["Logo"];
				}
				if (reader["CompanyUrl"] != System.DBNull.Value)
				{
					brandCategoryInfo.CompanyUrl = (string)reader["CompanyUrl"];
				}
				if (reader["RewriteName"] != System.DBNull.Value)
				{
					brandCategoryInfo.RewriteName = (string)reader["RewriteName"];
				}
				if (reader["MetaKeywords"] != System.DBNull.Value)
				{
					brandCategoryInfo.MetaKeywords = (string)reader["MetaKeywords"];
				}
				if (reader["MetaDescription"] != System.DBNull.Value)
				{
					brandCategoryInfo.MetaDescription = (string)reader["MetaDescription"];
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					brandCategoryInfo.Description = (string)reader["Description"];
				}
				if (reader["Theme"] != System.DBNull.Value)
				{
					brandCategoryInfo.Theme = (string)reader["Theme"];
				}
				result = brandCategoryInfo;
			}
			return result;
		}
		public static InpourRequestInfo PopulateInpourRequest(IDataReader reader)
		{
			InpourRequestInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new InpourRequestInfo
				{
					InpourId = (string)reader["InpourId"],
					TradeDate = (System.DateTime)reader["TradeDate"],
					UserId = (int)reader["UserId"],
					PaymentId = (int)reader["PaymentId"],
					InpourBlance = (decimal)reader["InpourBlance"]
				};
			}
			return result;
		}
		public static AccountSummaryInfo PopulateAccountSummary(IDataRecord reader)
		{
			AccountSummaryInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				AccountSummaryInfo accountSummaryInfo = new AccountSummaryInfo();
				if (reader["AccountAmount"] != System.DBNull.Value)
				{
					accountSummaryInfo.AccountAmount = (decimal)reader["AccountAmount"];
				}
				if (reader["FreezeBalance"] != System.DBNull.Value)
				{
					accountSummaryInfo.FreezeBalance = (decimal)reader["FreezeBalance"];
				}
				accountSummaryInfo.UseableBalance = accountSummaryInfo.AccountAmount - accountSummaryInfo.FreezeBalance;
				result = accountSummaryInfo;
			}
			return result;
		}
		public static CouponInfo PopulateCoupon(IDataReader reader)
		{
			CouponInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				CouponInfo couponInfo = new CouponInfo();
				couponInfo.CouponId = (int)reader["CouponId"];
				couponInfo.Name = (string)reader["Name"];
				couponInfo.StartTime = (System.DateTime)reader["StartTime"];
				couponInfo.ClosingTime = (System.DateTime)reader["ClosingTime"];
				if (reader["Description"] != System.DBNull.Value)
				{
					couponInfo.Description = (string)reader["Description"];
				}
				if (reader["Amount"] != System.DBNull.Value)
				{
					couponInfo.Amount = new decimal?((decimal)reader["Amount"]);
				}
				couponInfo.DiscountValue = (decimal)reader["DiscountValue"];
				couponInfo.SentCount = (int)reader["SentCount"];
				couponInfo.UsedCount = (int)reader["UsedCount"];
				couponInfo.NeedPoint = (int)reader["NeedPoint"];
				result = couponInfo;
			}
			return result;
		}
		public static GroupBuyInfo PopulateGroupBuy(IDataReader reader)
		{
			GroupBuyInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				GroupBuyInfo groupBuyInfo = new GroupBuyInfo();
				groupBuyInfo.GroupBuyId = (int)reader["GroupBuyId"];
				groupBuyInfo.ProductId = (int)reader["ProductId"];
				if (System.DBNull.Value != reader["NeedPrice"])
				{
					groupBuyInfo.NeedPrice = (decimal)reader["NeedPrice"];
				}
				groupBuyInfo.MaxCount = (int)reader["MaxCount"];
				groupBuyInfo.StartDate = (System.DateTime)reader["StartDate"];
				groupBuyInfo.EndDate = (System.DateTime)reader["EndDate"];
				if (System.DBNull.Value != reader["Content"])
				{
					groupBuyInfo.Content = (string)reader["Content"];
				}
				groupBuyInfo.Status = (GroupBuyStatus)((int)reader["Status"]);
				result = groupBuyInfo;
			}
			return result;
		}
		public static BundlingInfo PopulateBindInfo(IDataReader reader)
		{
			BundlingInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				BundlingInfo bundlingInfo = new BundlingInfo();
				bundlingInfo.BundlingID = (int)reader["BundlingID"];
				bundlingInfo.Name = (string)reader["Name"];
				if (System.DBNull.Value != reader["price"])
				{
					bundlingInfo.Price = (decimal)reader["price"];
				}
				bundlingInfo.Num = (int)reader["Num"];
				bundlingInfo.AddTime = (System.DateTime)reader["AddTime"];
				if (System.DBNull.Value != reader["ShortDescription"])
				{
					bundlingInfo.ShortDescription = (string)reader["ShortDescription"];
				}
				bundlingInfo.SaleStatus = (int)reader["SaleStatus"];
				if (System.DBNull.Value != reader["DisplaySequence"])
				{
					bundlingInfo.DisplaySequence = (int)reader["DisplaySequence"];
				}
				result = bundlingInfo;
			}
			return result;
		}
		public static CountDownInfo PopulateCountDown(IDataReader reader)
		{
			CountDownInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				CountDownInfo countDownInfo = new CountDownInfo();
				countDownInfo.CountDownId = (int)reader["CountDownId"];
				countDownInfo.ProductId = (int)reader["ProductId"];
				countDownInfo.StartDate = (System.DateTime)reader["StartDate"];
				countDownInfo.EndDate = (System.DateTime)reader["EndDate"];
				if (System.DBNull.Value != reader["Content"])
				{
					countDownInfo.Content = (string)reader["Content"];
				}
				if (System.DBNull.Value != reader["CountDownPrice"])
				{
					countDownInfo.CountDownPrice = (decimal)reader["CountDownPrice"];
				}
				if (System.DBNull.Value != reader["MaxCount"])
				{
					countDownInfo.MaxCount = (int)reader["MaxCount"];
				}
				result = countDownInfo;
			}
			return result;
		}
		public static CouponItemInfo PopulateCouponItem(IDataReader reader)
		{
			CouponItemInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				CouponItemInfo couponItemInfo = new CouponItemInfo();
				couponItemInfo.CouponId = (int)reader["CouponId"];
				couponItemInfo.ClaimCode = (string)reader["ClaimCode"];
				couponItemInfo.GenerateTime = (System.DateTime)reader["GenerateTime"];
				if (reader["UserId"] != System.DBNull.Value)
				{
					couponItemInfo.UserId = new int?((int)reader["UserId"]);
				}
				if (reader["EmailAddress"] != System.DBNull.Value)
				{
					couponItemInfo.EmailAddress = (string)reader["EmailAddress"];
				}
				result = couponItemInfo;
			}
			return result;
		}
		public static GiftInfo PopulateGift(IDataReader reader)
		{
			GiftInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				GiftInfo giftInfo = new GiftInfo();
				giftInfo.GiftId = (int)reader["GiftId"];
				giftInfo.Name = ((System.DBNull.Value == reader["Name"]) ? null : ((string)reader["Name"]));
				giftInfo.ShortDescription = ((System.DBNull.Value == reader["ShortDescription"]) ? null : ((string)reader["ShortDescription"]));
				giftInfo.Unit = ((System.DBNull.Value == reader["Unit"]) ? null : ((string)reader["Unit"]));
				giftInfo.LongDescription = ((System.DBNull.Value == reader["LongDescription"]) ? null : ((string)reader["LongDescription"]));
				giftInfo.Title = ((System.DBNull.Value == reader["Title"]) ? null : ((string)reader["Title"]));
				giftInfo.Meta_Description = ((System.DBNull.Value == reader["Meta_Description"]) ? null : ((string)reader["Meta_Description"]));
				giftInfo.Meta_Keywords = ((System.DBNull.Value == reader["Meta_Keywords"]) ? null : ((string)reader["Meta_Keywords"]));
				if (System.DBNull.Value != reader["CostPrice"])
				{
					giftInfo.CostPrice = new decimal?((decimal)reader["CostPrice"]);
				}
				if (System.DBNull.Value != reader["ImageUrl"])
				{
					giftInfo.ImageUrl = (string)reader["ImageUrl"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl40"])
				{
					giftInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl60"])
				{
					giftInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl100"])
				{
					giftInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl160"])
				{
					giftInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl180"])
				{
					giftInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl220"])
				{
					giftInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl310"])
				{
					giftInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
				}
				if (System.DBNull.Value != reader["ThumbnailUrl410"])
				{
					giftInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
				}
				if (System.DBNull.Value != reader["PurchasePrice"])
				{
					giftInfo.PurchasePrice = (decimal)reader["PurchasePrice"];
				}
				if (System.DBNull.Value != reader["MarketPrice"])
				{
					giftInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
				}
				giftInfo.NeedPoint = (int)reader["NeedPoint"];
				giftInfo.IsDownLoad = (bool)reader["IsDownLoad"];
				giftInfo.IsPromotion = (bool)reader["IsPromotion"];
				result = giftInfo;
			}
			return result;
		}
		public static PromotionInfo PopulatePromote(IDataRecord reader)
		{
			PromotionInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				PromotionInfo promotionInfo = new PromotionInfo();
				promotionInfo.ActivityId = (int)reader["ActivityId"];
				promotionInfo.Name = (string)reader["Name"];
				promotionInfo.PromoteType = (PromoteType)reader["PromoteType"];
				promotionInfo.Condition = (decimal)reader["Condition"];
				promotionInfo.DiscountValue = (decimal)reader["DiscountValue"];
				promotionInfo.StartDate = (System.DateTime)reader["StartDate"];
				promotionInfo.EndDate = (System.DateTime)reader["EndDate"];
				if (System.DBNull.Value != reader["Description"])
				{
					promotionInfo.Description = (string)reader["Description"];
				}
				result = promotionInfo;
			}
			return result;
		}
		public static CountDownInfo PopulateCountDown(IDataRecord reader)
		{
			CountDownInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				CountDownInfo countDownInfo = new CountDownInfo();
				countDownInfo.CountDownId = (int)reader["CountDownId"];
				countDownInfo.ProductId = (int)reader["ProductId"];
				if (System.DBNull.Value != reader["CountDownPrice"])
				{
					countDownInfo.CountDownPrice = (decimal)reader["CountDownPrice"];
				}
				countDownInfo.StartDate = (System.DateTime)reader["StartDate"];
				countDownInfo.EndDate = (System.DateTime)reader["EndDate"];
				if (System.DBNull.Value != reader["Content"])
				{
					countDownInfo.Content = (string)reader["Content"];
				}
				countDownInfo.DisplaySequence = (int)reader["DisplaySequence"];
				if (System.DBNull.Value != reader["MaxCount"])
				{
					countDownInfo.MaxCount = (int)reader["MaxCount"];
				}
				result = countDownInfo;
			}
			return result;
		}
		public static ShippersInfo PopulateShipper(IDataRecord reader)
		{
			ShippersInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippersInfo shippersInfo = new ShippersInfo();
				shippersInfo.ShipperId = (int)reader["ShipperId"];
				shippersInfo.DistributorUserId = (int)reader["DistributorUserId"];
				shippersInfo.IsDefault = (bool)reader["IsDefault"];
				shippersInfo.ShipperTag = (string)reader["ShipperTag"];
				shippersInfo.ShipperName = (string)reader["ShipperName"];
				shippersInfo.RegionId = (int)reader["RegionId"];
				shippersInfo.Address = (string)reader["Address"];
				if (reader["CellPhone"] != System.DBNull.Value)
				{
					shippersInfo.CellPhone = (string)reader["CellPhone"];
				}
				if (reader["TelPhone"] != System.DBNull.Value)
				{
					shippersInfo.TelPhone = (string)reader["TelPhone"];
				}
				if (reader["Zipcode"] != System.DBNull.Value)
				{
					shippersInfo.Zipcode = (string)reader["Zipcode"];
				}
				if (reader["Remark"] != System.DBNull.Value)
				{
					shippersInfo.Remark = (string)reader["Remark"];
				}
				result = shippersInfo;
			}
			return result;
		}
		public static PaymentModeInfo PopulatePayment(IDataRecord reader)
		{
			PaymentModeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				PaymentModeInfo paymentModeInfo = new PaymentModeInfo
				{
					ModeId = (int)reader["ModeId"],
					Name = (string)reader["Name"],
					DisplaySequence = (int)reader["DisplaySequence"],
					IsUseInpour = (bool)reader["IsUseInpour"],
					Charge = (decimal)reader["Charge"],
					IsPercent = (bool)reader["IsPercent"]
				};
				try
				{
					paymentModeInfo.IsUseInDistributor = (bool)reader["IsUseInDistributor"];
				}
				catch
				{
					paymentModeInfo.IsUseInDistributor = false;
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					paymentModeInfo.Description = (string)reader["Description"];
				}
				if (reader["Gateway"] != System.DBNull.Value)
				{
					paymentModeInfo.Gateway = (string)reader["Gateway"];
				}
				if (reader["Settings"] != System.DBNull.Value)
				{
					paymentModeInfo.Settings = (string)reader["Settings"];
				}
				result = paymentModeInfo;
			}
			return result;
		}
		public static ShippingModeInfo PopulateShippingMode(IDataRecord reader)
		{
			ShippingModeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
				if (reader["ModeId"] != System.DBNull.Value)
				{
					shippingModeInfo.ModeId = (int)reader["ModeId"];
				}
				if (reader["TemplateId"] != System.DBNull.Value)
				{
					shippingModeInfo.TemplateId = (int)reader["TemplateId"];
				}
				shippingModeInfo.Name = (string)reader["Name"];
				shippingModeInfo.TemplateName = (string)reader["TemplateName"];
				if (reader["Weight"] != System.DBNull.Value)
				{
					shippingModeInfo.Weight = (decimal)reader["Weight"];
				}
				if (System.DBNull.Value != reader["AddWeight"])
				{
					shippingModeInfo.AddWeight = new decimal?((decimal)reader["AddWeight"]);
				}
				if (reader["Price"] != System.DBNull.Value)
				{
					shippingModeInfo.Price = (decimal)reader["Price"];
				}
				if (System.DBNull.Value != reader["AddPrice"])
				{
					shippingModeInfo.AddPrice = new decimal?((decimal)reader["AddPrice"]);
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					shippingModeInfo.Description = (string)reader["Description"];
				}
				shippingModeInfo.DisplaySequence = (int)reader["DisplaySequence"];
				result = shippingModeInfo;
			}
			return result;
		}
		public static ShippingModeInfo PopulateShippingTemplate(IDataRecord reader)
		{
			ShippingModeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
				if (reader["TemplateId"] != System.DBNull.Value)
				{
					shippingModeInfo.TemplateId = (int)reader["TemplateId"];
				}
				shippingModeInfo.Name = (string)reader["TemplateName"];
				shippingModeInfo.Weight = (decimal)reader["Weight"];
				if (System.DBNull.Value != reader["AddWeight"])
				{
					shippingModeInfo.AddWeight = new decimal?((decimal)reader["AddWeight"]);
				}
				shippingModeInfo.Price = (decimal)reader["Price"];
				if (System.DBNull.Value != reader["AddPrice"])
				{
					shippingModeInfo.AddPrice = new decimal?((decimal)reader["AddPrice"]);
				}
				result = shippingModeInfo;
			}
			return result;
		}
		public static ShippingModeGroupInfo PopulateShippingModeGroup(IDataRecord reader)
		{
			ShippingModeGroupInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingModeGroupInfo shippingModeGroupInfo = new ShippingModeGroupInfo();
				shippingModeGroupInfo.TemplateId = (int)reader["TemplateId"];
				shippingModeGroupInfo.GroupId = (int)reader["GroupId"];
				shippingModeGroupInfo.Price = (decimal)reader["Price"];
				if (System.DBNull.Value != reader["AddPrice"])
				{
					shippingModeGroupInfo.AddPrice = (decimal)reader["AddPrice"];
				}
				result = shippingModeGroupInfo;
			}
			return result;
		}
		public static ShippingRegionInfo PopulateShippingRegion(IDataRecord reader)
		{
			ShippingRegionInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new ShippingRegionInfo
				{
					TemplateId = (int)reader["TemplateId"],
					GroupId = (int)reader["GroupId"],
					RegionId = (int)reader["RegionId"]
				};
			}
			return result;
		}
		public static OrderInfo PopulateOrder(IDataRecord reader)
		{
			OrderInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				OrderInfo orderInfo = new OrderInfo();
				orderInfo.OrderId = (string)reader["OrderId"];
				if (System.DBNull.Value != reader["GatewayOrderId"])
				{
					orderInfo.GatewayOrderId = (string)reader["GatewayOrderId"];
				}
				if (System.DBNull.Value != reader["Remark"])
				{
					orderInfo.Remark = (string)reader["Remark"];
				}
				if (System.DBNull.Value != reader["ManagerMark"])
				{
					orderInfo.ManagerMark = new OrderMark?((OrderMark)reader["ManagerMark"]);
				}
				if (System.DBNull.Value != reader["ManagerRemark"])
				{
					orderInfo.ManagerRemark = (string)reader["ManagerRemark"];
				}
				if (System.DBNull.Value != reader["AdjustedDiscount"])
				{
					orderInfo.AdjustedDiscount = (decimal)reader["AdjustedDiscount"];
				}
				if (System.DBNull.Value != reader["OrderStatus"])
				{
					orderInfo.OrderStatus = (OrderStatus)reader["OrderStatus"];
				}
				if (System.DBNull.Value != reader["CloseReason"])
				{
					orderInfo.CloseReason = (string)reader["CloseReason"];
				}
				if (System.DBNull.Value != reader["OrderPoint"])
				{
					orderInfo.Points = (int)reader["OrderPoint"];
				}
				orderInfo.OrderDate = (System.DateTime)reader["OrderDate"];
				if (System.DBNull.Value != reader["PayDate"])
				{
					orderInfo.PayDate = (System.DateTime)reader["PayDate"];
				}
				if (System.DBNull.Value != reader["ShippingDate"])
				{
					orderInfo.ShippingDate = (System.DateTime)reader["ShippingDate"];
				}
				if (System.DBNull.Value != reader["FinishDate"])
				{
					orderInfo.FinishDate = (System.DateTime)reader["FinishDate"];
				}
				orderInfo.UserId = (int)reader["UserId"];
				orderInfo.Username = (string)reader["Username"];
				if (System.DBNull.Value != reader["EmailAddress"])
				{
					orderInfo.EmailAddress = (string)reader["EmailAddress"];
				}
				if (System.DBNull.Value != reader["RealName"])
				{
					orderInfo.RealName = (string)reader["RealName"];
				}
				if (System.DBNull.Value != reader["QQ"])
				{
					orderInfo.QQ = (string)reader["QQ"];
				}
				if (System.DBNull.Value != reader["Wangwang"])
				{
					orderInfo.Wangwang = (string)reader["Wangwang"];
				}
				if (System.DBNull.Value != reader["MSN"])
				{
					orderInfo.MSN = (string)reader["MSN"];
				}
				if (System.DBNull.Value != reader["ShippingRegion"])
				{
					orderInfo.ShippingRegion = (string)reader["ShippingRegion"];
				}
				if (System.DBNull.Value != reader["Address"])
				{
					orderInfo.Address = (string)reader["Address"];
				}
				if (System.DBNull.Value != reader["ZipCode"])
				{
					orderInfo.ZipCode = (string)reader["ZipCode"];
				}
				if (System.DBNull.Value != reader["ShipTo"])
				{
					orderInfo.ShipTo = (string)reader["ShipTo"];
				}
				if (System.DBNull.Value != reader["TelPhone"])
				{
					orderInfo.TelPhone = (string)reader["TelPhone"];
				}
				if (System.DBNull.Value != reader["CellPhone"])
				{
					orderInfo.CellPhone = (string)reader["CellPhone"];
				}
				if (System.DBNull.Value != reader["ShipToDate"])
				{
					orderInfo.ShipToDate = (string)reader["ShipToDate"];
				}
				if (System.DBNull.Value != reader["ShippingModeId"])
				{
					orderInfo.ShippingModeId = (int)reader["ShippingModeId"];
				}
				if (System.DBNull.Value != reader["ModeName"])
				{
					orderInfo.ModeName = (string)reader["ModeName"];
				}
				if (System.DBNull.Value != reader["RealShippingModeId"])
				{
					orderInfo.RealShippingModeId = (int)reader["RealShippingModeId"];
				}
				if (System.DBNull.Value != reader["RealModeName"])
				{
					orderInfo.RealModeName = (string)reader["RealModeName"];
				}
				if (System.DBNull.Value != reader["RegionId"])
				{
					orderInfo.RegionId = (int)reader["RegionId"];
				}
				if (System.DBNull.Value != reader["Freight"])
				{
					orderInfo.Freight = (decimal)reader["Freight"];
				}
				if (System.DBNull.Value != reader["AdjustedFreight"])
				{
					orderInfo.AdjustedFreight = (decimal)reader["AdjustedFreight"];
				}
				if (System.DBNull.Value != reader["ShipOrderNumber"])
				{
					orderInfo.ShipOrderNumber = (string)reader["ShipOrderNumber"];
				}
				if (System.DBNull.Value != reader["ExpressCompanyName"])
				{
					orderInfo.ExpressCompanyName = (string)reader["ExpressCompanyName"];
				}
				if (System.DBNull.Value != reader["ExpressCompanyAbb"])
				{
					orderInfo.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
				}
				if (System.DBNull.Value != reader["PaymentTypeId"])
				{
					orderInfo.PaymentTypeId = (int)reader["PaymentTypeId"];
				}
				if (System.DBNull.Value != reader["PaymentType"])
				{
					orderInfo.PaymentType = (string)reader["PaymentType"];
				}
				if (System.DBNull.Value != reader["PayCharge"])
				{
					orderInfo.PayCharge = (decimal)reader["PayCharge"];
				}
				if (System.DBNull.Value != reader["RefundStatus"])
				{
					orderInfo.RefundStatus = (RefundStatus)reader["RefundStatus"];
				}
				if (System.DBNull.Value != reader["RefundAmount"])
				{
					orderInfo.RefundAmount = (decimal)reader["RefundAmount"];
				}
				if (System.DBNull.Value != reader["RefundRemark"])
				{
					orderInfo.RefundRemark = (string)reader["RefundRemark"];
				}
				if (System.DBNull.Value != reader["Gateway"])
				{
					orderInfo.Gateway = (string)reader["Gateway"];
				}
				if (System.DBNull.Value != reader["ReducedPromotionId"])
				{
					orderInfo.ReducedPromotionId = (int)reader["ReducedPromotionId"];
				}
				if (System.DBNull.Value != reader["ReducedPromotionName"])
				{
					orderInfo.ReducedPromotionName = (string)reader["ReducedPromotionName"];
				}
				if (System.DBNull.Value != reader["ReducedPromotionAmount"])
				{
					orderInfo.ReducedPromotionAmount = (decimal)reader["ReducedPromotionAmount"];
				}
				if (System.DBNull.Value != reader["IsReduced"])
				{
					orderInfo.IsReduced = (bool)reader["IsReduced"];
				}
				if (System.DBNull.Value != reader["SentTimesPointPromotionId"])
				{
					orderInfo.SentTimesPointPromotionId = (int)reader["SentTimesPointPromotionId"];
				}
				if (System.DBNull.Value != reader["SentTimesPointPromotionName"])
				{
					orderInfo.SentTimesPointPromotionName = (string)reader["SentTimesPointPromotionName"];
				}
				if (System.DBNull.Value != reader["IsSendTimesPoint"])
				{
					orderInfo.IsSendTimesPoint = (bool)reader["IsSendTimesPoint"];
				}
				if (System.DBNull.Value != reader["TimesPoint"])
				{
					orderInfo.TimesPoint = (decimal)reader["TimesPoint"];
				}
				if (System.DBNull.Value != reader["FreightFreePromotionId"])
				{
					orderInfo.FreightFreePromotionId = (int)reader["FreightFreePromotionId"];
				}
				if (System.DBNull.Value != reader["FreightFreePromotionName"])
				{
					orderInfo.FreightFreePromotionName = (string)reader["FreightFreePromotionName"];
				}
				if (System.DBNull.Value != reader["IsFreightFree"])
				{
					orderInfo.IsFreightFree = (bool)reader["IsFreightFree"];
				}
				if (System.DBNull.Value != reader["CouponName"])
				{
					orderInfo.CouponName = (string)reader["CouponName"];
				}
				if (System.DBNull.Value != reader["CouponCode"])
				{
					orderInfo.CouponCode = (string)reader["CouponCode"];
				}
				if (System.DBNull.Value != reader["CouponAmount"])
				{
					orderInfo.CouponAmount = (decimal)reader["CouponAmount"];
				}
				if (System.DBNull.Value != reader["CouponValue"])
				{
					orderInfo.CouponValue = (decimal)reader["CouponValue"];
				}
				if (System.DBNull.Value != reader["GroupBuyId"])
				{
					orderInfo.GroupBuyId = (int)reader["GroupBuyId"];
				}
				if (System.DBNull.Value != reader["CountDownBuyId"])
				{
					orderInfo.CountDownBuyId = (int)reader["CountDownBuyId"];
				}
				if (System.DBNull.Value != reader["Bundlingid"])
				{
					orderInfo.BundlingID = (int)reader["Bundlingid"];
				}
				if (System.DBNull.Value != reader["BundlingPrice"])
				{
					orderInfo.BundlingPrice = (decimal)reader["BundlingPrice"];
				}
				if (System.DBNull.Value != reader["NeedPrice"])
				{
					orderInfo.NeedPrice = (decimal)reader["NeedPrice"];
				}
				if (System.DBNull.Value != reader["GroupBuyStatus"])
				{
					orderInfo.GroupBuyStatus = (GroupBuyStatus)reader["GroupBuyStatus"];
				}
				if (System.DBNull.Value != reader["Tax"])
				{
					orderInfo.Tax = (decimal)reader["Tax"];
				}
				else
				{
					orderInfo.Tax = 0m;
				}
				if (System.DBNull.Value != reader["InvoiceTitle"])
				{
					orderInfo.InvoiceTitle = (string)reader["InvoiceTitle"];
				}
				else
				{
					orderInfo.InvoiceTitle = "";
				}
				result = orderInfo;
			}
			return result;
		}
		public static OrderGiftInfo PopulateOrderGift(IDataRecord reader)
		{
			OrderGiftInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				OrderGiftInfo orderGiftInfo = new OrderGiftInfo
				{
					OrderId = (string)reader["OrderId"],
					GiftId = (int)reader["GiftId"],
					GiftName = (string)reader["GiftName"],
					CostPrice = (reader["CostPrice"] == System.DBNull.Value) ? 0m : ((decimal)reader["CostPrice"]),
					ThumbnailsUrl = (reader["ThumbnailsUrl"] == System.DBNull.Value) ? string.Empty : ((string)reader["ThumbnailsUrl"]),
					Quantity = (reader["Quantity"] == System.DBNull.Value) ? 0 : ((int)reader["Quantity"]),
					PromoteType = (int)reader["PromoType"]
				};
				result = orderGiftInfo;
			}
			return result;
		}
		public static LineItemInfo PopulateLineItem(IDataRecord reader)
		{
			LineItemInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				LineItemInfo lineItemInfo = new LineItemInfo();
				lineItemInfo.SkuId = (string)reader["SkuId"];
				lineItemInfo.ProductId = (int)reader["ProductId"];
				if (reader["SKU"] != System.DBNull.Value)
				{
					lineItemInfo.SKU = (string)reader["SKU"];
				}
				lineItemInfo.Quantity = (int)reader["Quantity"];
				lineItemInfo.ShipmentQuantity = (int)reader["ShipmentQuantity"];
				lineItemInfo.ItemCostPrice = (decimal)reader["CostPrice"];
				lineItemInfo.ItemListPrice = (decimal)reader["ItemListPrice"];
				lineItemInfo.ItemAdjustedPrice = (decimal)reader["ItemAdjustedPrice"];
				lineItemInfo.ItemDescription = (string)reader["ItemDescription"];
				if (reader["ThumbnailsUrl"] != System.DBNull.Value)
				{
					lineItemInfo.ThumbnailsUrl = (string)reader["ThumbnailsUrl"];
				}
				lineItemInfo.ItemWeight = (decimal)reader["Weight"];
				if (System.DBNull.Value != reader["SKUContent"])
				{
					lineItemInfo.SKUContent = (string)reader["SKUContent"];
				}
				if (System.DBNull.Value != reader["PromotionId"])
				{
					lineItemInfo.PromotionId = (int)reader["PromotionId"];
				}
				if (System.DBNull.Value != reader["PromotionName"])
				{
					lineItemInfo.PromotionName = (string)reader["PromotionName"];
				}
				result = lineItemInfo;
			}
			return result;
		}
		public static PurchaseOrderInfo PopulatePurchaseOrder(IDataReader reader)
		{
			PurchaseOrderInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
				purchaseOrderInfo.PurchaseOrderId = (string)reader["PurchaseOrderId"];
				if (System.DBNull.Value != reader["OrderId"])
				{
					purchaseOrderInfo.OrderId = (string)reader["OrderId"];
				}
				if (System.DBNull.Value != reader["ManagerMark"])
				{
					purchaseOrderInfo.ManagerMark = new OrderMark?((OrderMark)reader["ManagerMark"]);
				}
				if (System.DBNull.Value != reader["Remark"])
				{
					purchaseOrderInfo.Remark = (string)reader["Remark"];
				}
				if (System.DBNull.Value != reader["ManagerRemark"])
				{
					purchaseOrderInfo.ManagerRemark = (string)reader["ManagerRemark"];
				}
				if (System.DBNull.Value != reader["AdjustedDiscount"])
				{
					purchaseOrderInfo.AdjustedDiscount = (decimal)reader["AdjustedDiscount"];
				}
				if (System.DBNull.Value != reader["PurchaseStatus"])
				{
					purchaseOrderInfo.PurchaseStatus = (OrderStatus)reader["PurchaseStatus"];
				}
				if (System.DBNull.Value != reader["CloseReason"])
				{
					purchaseOrderInfo.CloseReason = (string)reader["CloseReason"];
				}
				purchaseOrderInfo.PurchaseDate = (System.DateTime)reader["PurchaseDate"];
				if (System.DBNull.Value != reader["PayDate"])
				{
					purchaseOrderInfo.PayDate = (System.DateTime)reader["PayDate"];
				}
				if (System.DBNull.Value != reader["ShippingDate"])
				{
					purchaseOrderInfo.ShippingDate = (System.DateTime)reader["ShippingDate"];
				}
				if (System.DBNull.Value != reader["FinishDate"])
				{
					purchaseOrderInfo.FinishDate = (System.DateTime)reader["FinishDate"];
				}
				purchaseOrderInfo.DistributorId = (int)reader["DistributorId"];
				purchaseOrderInfo.Distributorname = (string)reader["Distributorname"];
				if (System.DBNull.Value != reader["DistributorEmail"])
				{
					purchaseOrderInfo.DistributorEmail = (string)reader["DistributorEmail"];
				}
				if (System.DBNull.Value != reader["DistributorRealName"])
				{
					purchaseOrderInfo.DistributorRealName = (string)reader["DistributorRealName"];
				}
				if (System.DBNull.Value != reader["DistributorQQ"])
				{
					purchaseOrderInfo.DistributorQQ = (string)reader["DistributorQQ"];
				}
				if (System.DBNull.Value != reader["DistributorWangwang"])
				{
					purchaseOrderInfo.DistributorWangwang = (string)reader["DistributorWangwang"];
				}
				if (System.DBNull.Value != reader["DistributorMSN"])
				{
					purchaseOrderInfo.DistributorMSN = (string)reader["DistributorMSN"];
				}
				if (System.DBNull.Value != reader["ShippingRegion"])
				{
					purchaseOrderInfo.ShippingRegion = (string)reader["ShippingRegion"];
				}
				if (System.DBNull.Value != reader["Address"])
				{
					purchaseOrderInfo.Address = (string)reader["Address"];
				}
				if (System.DBNull.Value != reader["ZipCode"])
				{
					purchaseOrderInfo.ZipCode = (string)reader["ZipCode"];
				}
				if (System.DBNull.Value != reader["ShipTo"])
				{
					purchaseOrderInfo.ShipTo = (string)reader["ShipTo"];
				}
				if (System.DBNull.Value != reader["TelPhone"])
				{
					purchaseOrderInfo.TelPhone = (string)reader["TelPhone"];
				}
				if (System.DBNull.Value != reader["CellPhone"])
				{
					purchaseOrderInfo.CellPhone = (string)reader["CellPhone"];
				}
				if (System.DBNull.Value != reader["ShipToDate"])
				{
					purchaseOrderInfo.ShipToDate = (string)reader["ShipToDate"];
				}
				if (System.DBNull.Value != reader["ShippingModeId"])
				{
					purchaseOrderInfo.ShippingModeId = (int)reader["ShippingModeId"];
				}
				if (System.DBNull.Value != reader["ModeName"])
				{
					purchaseOrderInfo.ModeName = (string)reader["ModeName"];
				}
				if (System.DBNull.Value != reader["RealShippingModeId"])
				{
					purchaseOrderInfo.RealShippingModeId = (int)reader["RealShippingModeId"];
				}
				if (System.DBNull.Value != reader["RealModeName"])
				{
					purchaseOrderInfo.RealModeName = (string)reader["RealModeName"];
				}
				if (System.DBNull.Value != reader["RegionId"])
				{
					purchaseOrderInfo.RegionId = (int)reader["RegionId"];
				}
				if (System.DBNull.Value != reader["Freight"])
				{
					purchaseOrderInfo.Freight = (decimal)reader["Freight"];
				}
				if (System.DBNull.Value != reader["AdjustedFreight"])
				{
					purchaseOrderInfo.AdjustedFreight = (decimal)reader["AdjustedFreight"];
				}
				if (System.DBNull.Value != reader["ShipOrderNumber"])
				{
					purchaseOrderInfo.ShipOrderNumber = (string)reader["ShipOrderNumber"];
				}
				if (System.DBNull.Value != reader["Weight"])
				{
					purchaseOrderInfo.Weight = (decimal)reader["Weight"];
				}
				if (System.DBNull.Value != reader["RefundStatus"])
				{
					purchaseOrderInfo.RefundStatus = (RefundStatus)reader["RefundStatus"];
				}
				if (System.DBNull.Value != reader["RefundAmount"])
				{
					purchaseOrderInfo.RefundAmount = (decimal)reader["RefundAmount"];
				}
				if (System.DBNull.Value != reader["RefundRemark"])
				{
					purchaseOrderInfo.RefundRemark = (string)reader["RefundRemark"];
				}
				if (System.DBNull.Value != reader["OrderTotal"])
				{
					purchaseOrderInfo.OrderTotal = (decimal)reader["OrderTotal"];
				}
				if (System.DBNull.Value != reader["ExpressCompanyName"])
				{
					purchaseOrderInfo.ExpressCompanyName = (string)reader["ExpressCompanyName"];
				}
				if (System.DBNull.Value != reader["ExpressCompanyAbb"])
				{
					purchaseOrderInfo.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
				}
				if (System.DBNull.Value != reader["PaymentTypeId"])
				{
					purchaseOrderInfo.PaymentTypeId = (int)reader["PaymentTypeId"];
				}
				if (System.DBNull.Value != reader["PaymentType"])
				{
					purchaseOrderInfo.PaymentType = (string)reader["PaymentType"];
				}
				if (System.DBNull.Value != reader["Gateway"])
				{
					purchaseOrderInfo.Gateway = (string)reader["Gateway"];
				}
				if (System.DBNull.Value != reader["TaobaoOrderId"])
				{
					purchaseOrderInfo.TaobaoOrderId = (string)reader["TaobaoOrderId"];
				}
				if (System.DBNull.Value != reader["Tax"])
				{
					purchaseOrderInfo.Tax = (decimal)reader["Tax"];
				}
				if (System.DBNull.Value != reader["InvoiceTitle"])
				{
					purchaseOrderInfo.InvoiceTitle = (string)reader["InvoiceTitle"];
				}
				result = purchaseOrderInfo;
			}
			return result;
		}
		public static PurchaseOrderItemInfo PopulatePurchaseOrderItem(IDataReader reader)
		{
			PurchaseOrderItemInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo();
				purchaseOrderItemInfo.PurchaseOrderId = (string)reader["PurchaseOrderId"];
				purchaseOrderItemInfo.SkuId = (string)reader["SkuId"];
				purchaseOrderItemInfo.ProductId = (int)reader["ProductId"];
				if (System.DBNull.Value != reader["SKU"])
				{
					purchaseOrderItemInfo.SKU = (string)reader["SKU"];
				}
				purchaseOrderItemInfo.Quantity = (int)reader["Quantity"];
				purchaseOrderItemInfo.ItemCostPrice = (decimal)reader["CostPrice"];
				purchaseOrderItemInfo.ItemListPrice = (decimal)reader["ItemListPrice"];
				purchaseOrderItemInfo.ItemPurchasePrice = (decimal)reader["ItemPurchasePrice"];
				purchaseOrderItemInfo.ItemDescription = (string)reader["ItemDescription"];
				purchaseOrderItemInfo.ItemHomeSiteDescription = (string)reader["ItemHomeSiteDescription"];
				if (reader["ThumbnailsUrl"] != System.DBNull.Value)
				{
					purchaseOrderItemInfo.ThumbnailsUrl = (string)reader["ThumbnailsUrl"];
				}
				purchaseOrderItemInfo.ItemWeight = ((reader["Weight"] == System.DBNull.Value) ? 0m : ((decimal)reader["Weight"]));
				if (reader["SKUContent"] != System.DBNull.Value)
				{
					purchaseOrderItemInfo.SKUContent = (string)reader["SKUContent"];
				}
				result = purchaseOrderItemInfo;
			}
			return result;
		}
		public static PurchaseOrderGiftInfo PopulatePurchaseOrderGift(IDataReader reader)
		{
			PurchaseOrderGiftInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new PurchaseOrderGiftInfo
				{
					PurchaseOrderId = (string)reader["PurchaseOrderId"],
					GiftId = (int)reader["GiftId"],
					GiftName = (string)reader["GiftName"],
					CostPrice = (reader["CostPrice"] == System.DBNull.Value) ? 0m : ((decimal)reader["CostPrice"]),
					PurchasePrice = (reader["PurchasePrice"] == System.DBNull.Value) ? 0m : ((decimal)reader["PurchasePrice"]),
					ThumbnailsUrl = (reader["ThumbnailsUrl"] == System.DBNull.Value) ? string.Empty : ((string)reader["ThumbnailsUrl"]),
					Quantity = (reader["Quantity"] == System.DBNull.Value) ? 0 : ((int)reader["Quantity"])
				};
			}
			return result;
		}
		public static UserStatisticsInfo PopulateUserStatistics(IDataRecord reader)
		{
			UserStatisticsInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				UserStatisticsInfo userStatisticsInfo = new UserStatisticsInfo();
				if (reader["RegionId"] != System.DBNull.Value)
				{
					userStatisticsInfo.RegionId = (long)((int)reader["RegionId"]);
				}
				if (reader["Usercounts"] != System.DBNull.Value)
				{
					userStatisticsInfo.Usercounts = (int)reader["Usercounts"];
				}
				if (reader["AllUserCounts"] != System.DBNull.Value)
				{
					userStatisticsInfo.AllUserCounts = (int)reader["AllUserCounts"];
				}
				result = userStatisticsInfo;
			}
			return result;
		}
		public static StatisticsInfo PopulateStatistics(IDataRecord reader)
		{
			StatisticsInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				StatisticsInfo statisticsInfo = new StatisticsInfo();
				statisticsInfo.OrderNumbWaitConsignment = (int)reader["orderNumbWaitConsignment"];
				statisticsInfo.ApplyRequestWaitDispose = (int)reader["ApplyRequestWaitDispose"];
				statisticsInfo.ProductNumStokWarning = (int)reader["ProductNumStokWarning"];
				statisticsInfo.PurchaseOrderNumbWaitConsignment = (int)reader["purchaseOrderNumbWaitConsignment"];
				statisticsInfo.LeaveComments = (int)reader["LeaveComments"];
				statisticsInfo.ProductConsultations = (int)reader["ProductConsultations"];
				statisticsInfo.Messages = (int)reader["Messages"];
				statisticsInfo.OrderNumbToday = (int)reader["OrderNumbToday"];
				statisticsInfo.OrderPriceToday = (decimal)reader["OrderPriceToday"];
				statisticsInfo.OrderProfitToday = (decimal)reader["OrderProfitToday"];
				statisticsInfo.UserNewAddToday = (int)reader["UserNewAddToday"];
				statisticsInfo.DistroButorsNewAddToday = (int)reader["AgentNewAddToday"];
				statisticsInfo.UserNumbBirthdayToday = (int)reader["userNumbBirthdayToday"];
				statisticsInfo.OrderNumbYesterday = (int)reader["OrderNumbYesterday"];
				statisticsInfo.OrderPriceYesterday = (decimal)reader["OrderPriceYesterday"];
				statisticsInfo.OrderProfitYesterday = (decimal)reader["OrderProfitYesterday"];
				statisticsInfo.UserNumb = (int)reader["UserNumb"];
				statisticsInfo.DistroButorsNumb = (int)reader["AgentNumb"];
				statisticsInfo.Balance = (decimal)reader["memberBalance"];
				statisticsInfo.BalanceDrawRequested = (decimal)reader["BalanceDrawRequested"];
				statisticsInfo.ProductNumbOnSale = (int)reader["ProductNumbOnSale"];
				statisticsInfo.ProductNumbInStock = (int)reader["ProductNumbInStock"];
				statisticsInfo.ProductAlert = (int)reader["ProductAlert"];
				if (reader["authorizeProductCount"] != System.DBNull.Value)
				{
					statisticsInfo.AuthorizeProductCount = (int)reader["authorizeProductCount"];
				}
				if (reader["arealdyPaidNum"] != System.DBNull.Value)
				{
					statisticsInfo.AlreadyPaidOrdersNum = (int)reader["arealdyPaidNum"];
				}
				if (reader["arealdyPaidTotal"] != System.DBNull.Value)
				{
					statisticsInfo.AreadyPaidOrdersAmount = (decimal)reader["arealdyPaidTotal"];
				}
				result = statisticsInfo;
			}
			return result;
		}
		public static FriendlyLinksInfo PopulateFriendlyLink(IDataRecord reader)
		{
			FriendlyLinksInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				FriendlyLinksInfo friendlyLinksInfo = new FriendlyLinksInfo();
				friendlyLinksInfo.LinkId = new int?((int)reader["LinkId"]);
				friendlyLinksInfo.Visible = (bool)reader["Visible"];
				friendlyLinksInfo.DisplaySequence = (int)reader["DisplaySequence"];
				if (reader["ImageUrl"] != System.DBNull.Value)
				{
					friendlyLinksInfo.ImageUrl = (string)reader["ImageUrl"];
				}
				if (reader["Title"] != System.DBNull.Value)
				{
					friendlyLinksInfo.Title = (string)reader["Title"];
				}
				if (reader["LinkUrl"] != System.DBNull.Value)
				{
					friendlyLinksInfo.LinkUrl = (string)reader["LinkUrl"];
				}
				result = friendlyLinksInfo;
			}
			return result;
		}
		public static VoteInfo PopulateVote(IDataRecord reader)
		{
			return new VoteInfo
			{
				VoteId = (long)reader["VoteId"],
				VoteName = (string)reader["VoteName"],
				IsBackup = (bool)reader["IsBackup"],
				MaxCheck = (int)reader["MaxCheck"],
				VoteCounts = (int)reader["VoteCounts"]
			};
		}
		public static VoteItemInfo PopulateVoteItem(IDataRecord reader)
		{
			return new VoteItemInfo
			{
				VoteId = (long)reader["VoteId"],
				VoteItemId = (long)reader["VoteItemId"],
				VoteItemName = (string)reader["VoteItemName"],
				ItemCount = (int)reader["ItemCount"]
			};
		}
		public static MessageBoxInfo PopulateMessageBox(IDataReader reader)
		{
			MessageBoxInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new MessageBoxInfo
				{
					MessageId = (long)reader["MessageId"],
					Accepter = (string)reader["Accepter"],
					Sernder = (string)reader["Sernder"],
					IsRead = (bool)reader["IsRead"],
					ContentId = (long)reader["ContentId"],
					Title = (string)reader["Title"],
					Content = (string)reader["Content"],
					Date = (System.DateTime)reader["Date"]
				};
			}
			return result;
		}
		public static MemberGradeInfo PopulateMemberGrade(IDataReader reader)
		{
			MemberGradeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeInfo();
				memberGradeInfo.GradeId = (int)reader["GradeId"];
				memberGradeInfo.Name = (string)reader["Name"];
				if (reader["Description"] != System.DBNull.Value)
				{
					memberGradeInfo.Description = (string)reader["Description"];
				}
				memberGradeInfo.Points = (int)reader["Points"];
				memberGradeInfo.IsDefault = (bool)reader["IsDefault"];
				memberGradeInfo.Discount = (int)reader["Discount"];
				result = memberGradeInfo;
			}
			return result;
		}
		public static ShippingAddressInfo PopulateShippingAddress(IDataRecord reader)
		{
			ShippingAddressInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
				shippingAddressInfo.ShippingId = (int)reader["ShippingId"];
				shippingAddressInfo.ShipTo = (string)reader["ShipTo"];
				shippingAddressInfo.RegionId = (int)reader["RegionId"];
				shippingAddressInfo.UserId = (int)reader["UserId"];
				shippingAddressInfo.Address = (string)reader["Address"];
				shippingAddressInfo.Zipcode = (string)reader["Zipcode"];
				if (reader["TelPhone"] != System.DBNull.Value)
				{
					shippingAddressInfo.TelPhone = (string)reader["TelPhone"];
				}
				if (reader["CellPhone"] != System.DBNull.Value)
				{
					shippingAddressInfo.CellPhone = (string)reader["CellPhone"];
				}
				result = shippingAddressInfo;
			}
			return result;
		}
		public static MemberClientSet PopulateMemberClientSet(IDataReader reader)
		{
			MemberClientSet result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				MemberClientSet memberClientSet = new MemberClientSet();
				memberClientSet.ClientTypeId = (int)reader["ClientTypeId"];
				if (System.DateTime.Compare((System.DateTime)reader["StartTime"], System.Convert.ToDateTime("1900-01-01")) != 0)
				{
					memberClientSet.StartTime = new System.DateTime?((System.DateTime)reader["StartTime"]);
				}
				if (System.DateTime.Compare((System.DateTime)reader["EndTime"], System.Convert.ToDateTime("1900-01-01")) != 0)
				{
					memberClientSet.EndTime = new System.DateTime?((System.DateTime)reader["EndTime"]);
				}
				memberClientSet.LastDay = (int)reader["LastDay"];
				if (reader["ClientChar"] != System.DBNull.Value)
				{
					memberClientSet.ClientChar = (string)reader["ClientChar"];
				}
				memberClientSet.ClientValue = (decimal)reader["ClientValue"];
				result = memberClientSet;
			}
			return result;
		}
		public static ShoppingCartItemInfo PopulateCartItem(IDataReader reader)
		{
			ShoppingCartItemInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
				shoppingCartItemInfo.SkuId = (string)reader["SkuId"];
				shoppingCartItemInfo.ProductId = (int)reader["ProductId"];
				shoppingCartItemInfo.SKU = (string)reader["SKU"];
				shoppingCartItemInfo.Name = (string)reader["Name"];
				shoppingCartItemInfo.MemberPrice = (decimal)reader["MemberPrice"];
				shoppingCartItemInfo.Quantity = (int)reader["Quantity"];
				shoppingCartItemInfo.Weight = (decimal)reader["Weight"];
				if (reader["SKUContent"] != System.DBNull.Value)
				{
					shoppingCartItemInfo.SkuContent = (string)reader["SKUContent"];
				}
				if (reader["ThumbnailUrl40"] != System.DBNull.Value)
				{
					shoppingCartItemInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
				}
				if (reader["ThumbnailUrl60"] != System.DBNull.Value)
				{
					shoppingCartItemInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
				}
				if (reader["ThumbnailUrl100"] != System.DBNull.Value)
				{
					shoppingCartItemInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
				}
				result = shoppingCartItemInfo;
			}
			return result;
		}
		public static ShoppingCartGiftInfo PopulateGiftCartItem(IDataReader reader)
		{
			ShoppingCartGiftInfo shoppingCartGiftInfo = new ShoppingCartGiftInfo();
			shoppingCartGiftInfo.UserId = (int)reader["UserId"];
			shoppingCartGiftInfo.GiftId = (int)reader["GiftId"];
			shoppingCartGiftInfo.Name = (string)reader["Name"];
			if (reader["CostPrice"] != System.DBNull.Value)
			{
				shoppingCartGiftInfo.CostPrice = (decimal)reader["CostPrice"];
			}
			shoppingCartGiftInfo.PurchasePrice = (decimal)reader["PurchasePrice"];
			shoppingCartGiftInfo.NeedPoint = (int)reader["NeedPoint"];
			if (reader["ThumbnailUrl40"] != System.DBNull.Value)
			{
				shoppingCartGiftInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
			}
			if (reader["ThumbnailUrl60"] != System.DBNull.Value)
			{
				shoppingCartGiftInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
			}
			if (reader["ThumbnailUrl100"] != System.DBNull.Value)
			{
				shoppingCartGiftInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
			}
			if (reader["PromoType"] != System.DBNull.Value)
			{
				shoppingCartGiftInfo.PromoType = (int)reader["PromoType"];
			}
			return shoppingCartGiftInfo;
		}
		public static PurchaseShoppingCartItemInfo PopulatePurchaseShoppingCartItemInfo(IDataReader reader)
		{
			PurchaseShoppingCartItemInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				PurchaseShoppingCartItemInfo purchaseShoppingCartItemInfo = new PurchaseShoppingCartItemInfo();
				purchaseShoppingCartItemInfo.SkuId = (string)reader["SkuId"];
				purchaseShoppingCartItemInfo.SKUContent = ((reader["SKUContent"] != System.DBNull.Value) ? ((string)reader["SKUContent"]) : string.Empty);
				purchaseShoppingCartItemInfo.SKU = ((reader["SKU"] != System.DBNull.Value) ? ((string)reader["SKU"]) : string.Empty);
				purchaseShoppingCartItemInfo.ProductId = (int)reader["ProductId"];
				purchaseShoppingCartItemInfo.ThumbnailsUrl = ((reader["ThumbnailsUrl"] != System.DBNull.Value) ? ((string)reader["ThumbnailsUrl"]) : string.Empty);
				purchaseShoppingCartItemInfo.Quantity = (int)reader["Quantity"];
				purchaseShoppingCartItemInfo.ItemDescription = (string)reader["ItemDescription"];
				purchaseShoppingCartItemInfo.ItemWeight = (decimal)reader["Weight"];
				purchaseShoppingCartItemInfo.ItemListPrice = (decimal)reader["ItemListPrice"];
				purchaseShoppingCartItemInfo.ItemPurchasePrice = (decimal)reader["ItemPurchasePrice"];
				if (System.DBNull.Value != reader["CostPrice"])
				{
					purchaseShoppingCartItemInfo.CostPrice = (decimal)reader["CostPrice"];
				}
				result = purchaseShoppingCartItemInfo;
			}
			return result;
		}
		public static SiteRequestInfo PopulSiteRequest(IDataReader reader)
		{
			SiteRequestInfo siteRequestInfo = new SiteRequestInfo();
			siteRequestInfo.RequestId = (int)reader["RequestId"];
			siteRequestInfo.UserId = (int)reader["UserId"];
			siteRequestInfo.FirstSiteUrl = (string)reader["FirstSiteUrl"];
			siteRequestInfo.RequestTime = (System.DateTime)reader["RequestTime"];
			siteRequestInfo.RequestStatus = (SiteRequestStatus)reader["RequestStatus"];
			if (reader["RefuseReason"] != System.DBNull.Value)
			{
				siteRequestInfo.RefuseReason = (string)reader["RefuseReason"];
			}
			return siteRequestInfo;
		}
		public static DistributorGradeInfo PopulDistributorGrade(IDataReader reader)
		{
			DistributorGradeInfo distributorGradeInfo = new DistributorGradeInfo();
			distributorGradeInfo.GradeId = (int)reader["GradeId"];
			distributorGradeInfo.Discount = (int)reader["Discount"];
			distributorGradeInfo.Name = (string)reader["Name"];
			if (reader["Description"] != System.DBNull.Value)
			{
				distributorGradeInfo.Description = (string)reader["Description"];
			}
			return distributorGradeInfo;
		}
	}
}
