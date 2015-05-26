using Hidistro.Core;
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
namespace Hidistro.ControlPanel.Promotions
{
	public static class PromoteHelper
	{
		public static System.Data.DataTable GetPromotions(bool isProductPromote, bool isWholesale)
		{
			return PromotionsProvider.Instance().GetPromotions(isProductPromote, isWholesale);
		}
		public static PromotionInfo GetPromotion(int activityId)
		{
			return PromotionsProvider.Instance().GetPromotion(activityId);
		}
		public static IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
		{
			return PromotionsProvider.Instance().GetPromoteMemberGrades(activityId);
		}
		public static System.Data.DataTable GetPromotionProducts(int activityId)
		{
			return PromotionsProvider.Instance().GetPromotionProducts(activityId);
		}
		public static DbQueryResult GetCouponsList(CouponItemInfoQuery couponquery)
		{
			return PromotionsProvider.Instance().GetCouponsList(couponquery);
		}
		public static bool AddPromotionProducts(int activityId, string productIds)
		{
			return PromotionsProvider.Instance().AddPromotionProducts(activityId, productIds);
		}
		public static bool DeletePromotionProducts(int activityId, int? productId)
		{
			return PromotionsProvider.Instance().DeletePromotionProducts(activityId, productId);
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
					int num = PromotionsProvider.Instance().AddPromotion(promotion, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = -1;
					}
					else
					{
						if (!PromotionsProvider.Instance().AddPromotionMemberGrades(num, promotion.MemberGradeIds, dbTransaction))
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
					if (!PromotionsProvider.Instance().EditPromotion(promotion, dbTransaction))
					{
						dbTransaction.Rollback();
						result = -1;
					}
					else
					{
						if (!PromotionsProvider.Instance().AddPromotionMemberGrades(promotion.ActivityId, promotion.MemberGradeIds, dbTransaction))
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
			return PromotionsProvider.Instance().DeletePromotion(activityId);
		}
		public static IList<Member> GetMemdersByNames(IList<string> names)
		{
			IList<Member> list = new List<Member>();
			foreach (string current in names)
			{
				IUser user = Users.GetUser(0, current, false, false);
				if (user != null && user.UserRole == UserRole.Member)
				{
					list.Add(user as Member);
				}
			}
			return list;
		}
		public static IList<Member> GetMembersByRank(int? gradeId)
		{
			return PromotionsProvider.Instance().GetMembersByRank(gradeId);
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
					int num = PromotionsProvider.Instance().AddBundlingProduct(bundlingInfo, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!PromotionsProvider.Instance().AddBundlingProductItems(num, bundlingInfo.BundlingItemInfos, dbTransaction))
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
					if (!PromotionsProvider.Instance().UpdateBundlingProduct(bundlingInfo, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!PromotionsProvider.Instance().DeleteBundlingByID(bundlingInfo.BundlingID, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!PromotionsProvider.Instance().AddBundlingProductItems(bundlingInfo.BundlingID, bundlingInfo.BundlingItemInfos, dbTransaction))
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
			return PromotionsProvider.Instance().GetBundlingInfo(bundlingID);
		}
		public static bool DeleteBundlingProduct(int bundlingID)
		{
			return PromotionsProvider.Instance().DeleteBundlingProduct(bundlingID);
		}
		public static DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
		{
			return PromotionsProvider.Instance().GetBundlingProducts(query);
		}
		public static string GetPriceByProductId(int productId)
		{
			return PromotionsProvider.Instance().GetPriceByProductId(productId);
		}
		public static bool AddGroupBuy(GroupBuyInfo groupBuy)
		{
			Globals.EntityCoding(groupBuy, true);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					int num = PromotionsProvider.Instance().AddGroupBuy(groupBuy, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!PromotionsProvider.Instance().AddGroupBuyCondition(num, groupBuy.GroupBuyConditions, dbTransaction))
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
			return PromotionsProvider.Instance().ProductGroupBuyExist(productId);
		}
		public static bool DeleteGroupBuy(int groupBuyId)
		{
			return PromotionsProvider.Instance().DeleteGroupBuy(groupBuyId);
		}
		public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
		{
			Globals.EntityCoding(groupBuy, true);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!PromotionsProvider.Instance().UpdateGroupBuy(groupBuy, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!PromotionsProvider.Instance().DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!PromotionsProvider.Instance().AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTransaction))
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
			return PromotionsProvider.Instance().GetGroupBuy(groupBuyId);
		}
		public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
		{
			return PromotionsProvider.Instance().GetGroupBuyList(query);
		}
		public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
		{
			return PromotionsProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
		}
		public static void SwapGroupBuySequence(int groupBuyId, int displaySequence)
		{
			PromotionsProvider.Instance().SwapGroupBuySequence(groupBuyId, displaySequence);
		}
		public static int GetOrderCount(int groupBuyId)
		{
			return PromotionsProvider.Instance().GetOrderCount(groupBuyId);
		}
		public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
		{
			return PromotionsProvider.Instance().SetGroupBuyStatus(groupBuyId, status);
		}
		public static bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			return PromotionsProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
		}
		public static DbQueryResult GetCountDownList(GroupBuyQuery query)
		{
			return PromotionsProvider.Instance().GetCountDownList(query);
		}
		public static void SwapCountDownSequence(int countDownId, int displaySequence)
		{
			PromotionsProvider.Instance().SwapCountDownSequence(countDownId, displaySequence);
		}
		public static bool DeleteCountDown(int countDownId)
		{
			return PromotionsProvider.Instance().DeleteCountDown(countDownId);
		}
		public static bool AddCountDown(CountDownInfo countDownInfo)
		{
			return PromotionsProvider.Instance().AddCountDown(countDownInfo);
		}
		public static bool UpdateCountDown(CountDownInfo countDownInfo)
		{
			return PromotionsProvider.Instance().UpdateCountDown(countDownInfo);
		}
		public static bool ProductCountDownExist(int productId)
		{
			return PromotionsProvider.Instance().ProductCountDownExist(productId);
		}
		public static CountDownInfo GetCountDownInfo(int countDownId)
		{
			return PromotionsProvider.Instance().GetCountDownInfo(countDownId);
		}
	}
}
