using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.Subsites.Promotions
{
	public static class SubsitePromoteHelper
	{
		public static string GetPriceByProductId(int productId)
		{
			return SubsitePromotionsProvider.Instance().GetPriceByProductId(productId);
		}
		public static bool AddGroupBuy(GroupBuyInfo groupBuy)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					int num = SubsitePromotionsProvider.Instance().AddGroupBuy(groupBuy, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!SubsitePromotionsProvider.Instance().AddGroupBuyCondition(num, groupBuy.GroupBuyConditions, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							dbTransaction.Commit();
							result = true;
						}
					}
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static bool ProductGroupBuyExist(int productId)
		{
			return SubsitePromotionsProvider.Instance().ProductGroupBuyExist(productId);
		}
		public static bool DeleteGroupBuy(int groupBuyId)
		{
			return SubsitePromotionsProvider.Instance().DeleteGroupBuy(groupBuyId);
		}
		public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsitePromotionsProvider.Instance().UpdateGroupBuy(groupBuy, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!SubsitePromotionsProvider.Instance().DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!SubsitePromotionsProvider.Instance().AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTransaction))
							{
								dbTransaction.Rollback();
								result = false;
							}
							else
							{
								dbTransaction.Commit();
								result = true;
							}
						}
					}
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			return SubsitePromotionsProvider.Instance().GetGroupBuy(groupBuyId);
		}
		public static DbQueryResult GetCouponsList(CouponItemInfoQuery couponquery)
		{
			return SubsitePromotionsProvider.Instance().GetCouponsList(couponquery);
		}
		public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
		{
			return SubsitePromotionsProvider.Instance().GetGroupBuyList(query);
		}
		public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
		{
			return SubsitePromotionsProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
		}
		public static void SwapGroupBuySequence(int groupBuyId, int displaySequence)
		{
			SubsitePromotionsProvider.Instance().SwapGroupBuySequence(groupBuyId, displaySequence);
		}
		public static int GetOrderCount(int groupBuyId)
		{
			return SubsitePromotionsProvider.Instance().GetOrderCount(groupBuyId);
		}
		public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
		{
			return SubsitePromotionsProvider.Instance().SetGroupBuyStatus(groupBuyId, status);
		}
		public static bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			return SubsitePromotionsProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
		}
		public static System.Data.DataTable GetPromotions(bool isProductPromote)
		{
			return SubsitePromotionsProvider.Instance().GetPromotions(isProductPromote);
		}
		public static IList<Member> GetMembersByRank(int? gradeId)
		{
			return SubsitePromotionsProvider.Instance().GetMembersByRank(gradeId);
		}
		public static IList<Member> GetMemdersByNames(IList<string> names)
		{
			IList<Member> list = new List<Member>();
			foreach (string current in names)
			{
				IUser user = Users.GetUser(0, current, false, false);
				if (user != null && user.UserRole == UserRole.Underling)
				{
					list.Add(user as Member);
				}
			}
			return list;
		}
		public static PromotionInfo GetPromotion(int activityId)
		{
			return SubsitePromotionsProvider.Instance().GetPromotion(activityId);
		}
		public static IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
		{
			return SubsitePromotionsProvider.Instance().GetPromoteMemberGrades(activityId);
		}
		public static System.Data.DataTable GetPromotionProducts(int activityId)
		{
			return SubsitePromotionsProvider.Instance().GetPromotionProducts(activityId);
		}
		public static bool AddPromotionProducts(int activityId, string productIds)
		{
			return SubsitePromotionsProvider.Instance().AddPromotionProducts(activityId, productIds);
		}
		public static bool DeletePromotionProducts(int activityId, int? productId)
		{
			return SubsitePromotionsProvider.Instance().DeletePromotionProducts(activityId, productId);
		}
		public static int AddPromotion(PromotionInfo promotion)
		{
			Database database = DatabaseFactory.CreateDatabase();
			int result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					int num = SubsitePromotionsProvider.Instance().AddPromotion(promotion, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = -1;
					}
					else
					{
						if (!SubsitePromotionsProvider.Instance().AddPromotionMemberGrades(num, promotion.MemberGradeIds, dbTransaction))
						{
							dbTransaction.Rollback();
							result = -2;
						}
						else
						{
							dbTransaction.Commit();
							result = num;
						}
					}
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					result = 0;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static int EditPromotion(PromotionInfo promotion)
		{
			Database database = DatabaseFactory.CreateDatabase();
			int result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsitePromotionsProvider.Instance().EditPromotion(promotion, dbTransaction))
					{
						dbTransaction.Rollback();
						result = -1;
					}
					else
					{
						if (!SubsitePromotionsProvider.Instance().AddPromotionMemberGrades(promotion.ActivityId, promotion.MemberGradeIds, dbTransaction))
						{
							dbTransaction.Rollback();
							result = -2;
						}
						else
						{
							dbTransaction.Commit();
							result = 1;
						}
					}
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					result = 0;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static bool DeletePromotion(int activityId)
		{
			return SubsitePromotionsProvider.Instance().DeletePromotion(activityId);
		}
		public static bool AddBundlingProduct(BundlingInfo bundlingInfo)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					int num = SubsitePromotionsProvider.Instance().AddBundlingProduct(bundlingInfo, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!SubsitePromotionsProvider.Instance().AddBundlingProductItems(num, bundlingInfo.BundlingItemInfos, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							dbTransaction.Commit();
							result = true;
						}
					}
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static bool UpdateBundlingProduct(BundlingInfo bundlingInfo)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsitePromotionsProvider.Instance().UpdateBundlingProduct(bundlingInfo, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!SubsitePromotionsProvider.Instance().DeleteBundlingByID(bundlingInfo.BundlingID, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!SubsitePromotionsProvider.Instance().AddBundlingProductItems(bundlingInfo.BundlingID, bundlingInfo.BundlingItemInfos, dbTransaction))
							{
								dbTransaction.Rollback();
								result = false;
							}
							else
							{
								dbTransaction.Commit();
								result = true;
							}
						}
					}
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static BundlingInfo GetBundlingInfo(int bundlingID)
		{
			return SubsitePromotionsProvider.Instance().GetBundlingInfo(bundlingID);
		}
		public static bool DeleteBundlingProduct(int bundlingID)
		{
			return SubsitePromotionsProvider.Instance().DeleteBundlingProduct(bundlingID);
		}
		public static DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
		{
			return SubsitePromotionsProvider.Instance().GetBundlingProducts(query);
		}
		public static DbQueryResult GetCountDownList(GroupBuyQuery query)
		{
			return SubsitePromotionsProvider.Instance().GetCountDownList(query);
		}
		public static void SwapCountDownSequence(int countDownId, int displaySequence)
		{
			SubsitePromotionsProvider.Instance().SwapCountDownSequence(countDownId, displaySequence);
		}
		public static bool DeleteCountDown(int countDownId)
		{
			return SubsitePromotionsProvider.Instance().DeleteCountDown(countDownId);
		}
		public static bool AddCountDown(CountDownInfo countDownInfo)
		{
			return SubsitePromotionsProvider.Instance().AddCountDown(countDownInfo);
		}
		public static bool UpdateCountDown(CountDownInfo countDownInfo)
		{
			return SubsitePromotionsProvider.Instance().UpdateCountDown(countDownInfo);
		}
		public static bool ProductCountDownExist(int productId)
		{
			return SubsitePromotionsProvider.Instance().ProductCountDownExist(productId);
		}
		public static CountDownInfo GetCountDownInfo(int countDownId)
		{
			return SubsitePromotionsProvider.Instance().GetCountDownInfo(countDownId);
		}
	}
}
