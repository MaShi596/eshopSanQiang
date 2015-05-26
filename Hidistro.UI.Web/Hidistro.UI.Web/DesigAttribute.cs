using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Comments;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.UI.Web
{
    public static class DesigAttribute
    {
        // Fields
        private static string _pagename;
        public static string DesigPageName;
        public static string SourcePageName = "";

        // Properties
        public static string DesigPagePath
        {
            get
            {
                return Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/" + DesigPageName);
            }
        }

        public static string PageName
        {
            get
            {
                return _pagename;
            }
            set
            {
                _pagename = value;
                if (_pagename != "")
                {
                    switch (_pagename)
                    {
                        case "default":
                            DesigPageName = "Skin-Desig_Templete.html";
                            SourcePageName = "Default.aspx";
                            return;

                        case "login":
                            DesigPageName = "Skin-Desig_login.html";
                            SourcePageName = "Login.aspx";
                            return;

                        case "brand":
                            DesigPageName = "Skin-Desig_Brand.html";
                            SourcePageName = "Brand.aspx";
                            return;

                        case "branddetail":
                            {
                                DesigPageName = "Skin-Desig_BrandDetails.html";
                                SourcePageName = "BrandDetails.aspx";
                                DataTable brandCategories = CategoryBrowser.GetBrandCategories(0, 1);
                                if (brandCategories.Rows.Count <= 0)
                                {
                                    break;
                                }
                                SourcePageName = "BrandDetails.aspx?brandId=" + brandCategories.Rows[0]["BrandId"].ToString();
                                brandCategories.Dispose();
                                return;
                            }
                        case "product":
                            DesigPageName = "Skin-Desig_SubCategory.html";
                            SourcePageName = "SubCategory.aspx";
                            return;

                        case "productdetail":
                            {
                                DesigPageName = "Skin-Desig_ProductDetails.html";
                                SourcePageName = "ProductDetails.aspx";
                                SubjectListQuery query = new SubjectListQuery
                                {
                                    MaxNum = 1
                                };
                                DataTable subjectList = ProductBrowser.GetSubjectList(query);
                                if (subjectList.Rows.Count <= 0)
                                {
                                    break;
                                }
                                SourcePageName = "ProductDetails.aspx?productId=" + subjectList.Rows[0]["ProductId"].ToString();
                                subjectList.Dispose();
                                return;
                            }
                        case "article":
                            DesigPageName = "Skin-Desig_Articles.html";
                            SourcePageName = "Articles.aspx";
                            return;

                        case "articledetail":
                            {
                                DesigPageName = "Skin-Desig_ArticleDetails.html";
                                SourcePageName = "ArticleDetails.aspx";
                                IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(0, 1);
                                if (articleList.Count <= 0)
                                {
                                    break;
                                }
                                SourcePageName = "ArticleDetails.aspx?articleId=" + articleList[0].ArticleId.ToString();
                                return;
                            }
                        case "cuountdown":
                            DesigPageName = "Skin-Desig_CountDownProducts.html";
                            SourcePageName = "CountDownProducts.aspx";
                            return;

                        case "cuountdowndetail":
                            {
                                DesigPageName = "Skin-Desig_CountDownProductsDetails.html";
                                SourcePageName = "CountDownProductsDetails.aspx";
                                DataTable counDownProducList = ProductBrowser.GetCounDownProducList(1);
                                if (counDownProducList.Rows.Count <= 0)
                                {
                                    break;
                                }
                                SourcePageName = "CountDownProductsDetails.aspx?productId=" + counDownProducList.Rows[0]["ProductId"].ToString();
                                counDownProducList.Dispose();
                                return;
                            }
                        case "groupbuy":
                            DesigPageName = "Skin-Desig_GroupBuyProducts.html";
                            SourcePageName = "GroupBuyProducts.aspx";
                            return;

                        case "groupbuydetail":
                            {
                                DesigPageName = "Skin-Desig_CountDownProductsDetails.html";
                                SourcePageName = "GroupBuyProductDetails.aspx";
                                DataTable groupByProductList = ProductBrowser.GetGroupByProductList(1);
                                if (groupByProductList.Rows.Count <= 0)
                                {
                                    break;
                                }
                                SourcePageName = "GroupBuyProductDetails.aspx?productId=" + groupByProductList.Rows[0]["ProductId"].ToString();
                                groupByProductList.Dispose();
                                return;
                            }
                        case "help":
                            DesigPageName = "Skin-Desig_Helps.html";
                            SourcePageName = "Helps.aspx";
                            return;

                        case "helpdetail":
                            {
                                DesigPageName = "Skin-Desig_HelpDetails.html";
                                SourcePageName = "HelpDetails.aspx";
                                DataSet helps = CommentBrowser.GetHelps();
                                if ((helps.Tables.Count <= 0) || (helps.Tables[1].Rows.Count <= 0))
                                {
                                    break;
                                }
                                SourcePageName = "HelpDetails.aspx?helpId=" + helps.Tables[1].Rows[0]["HelpId"].ToString();
                                helps.Dispose();
                                return;
                            }
                        case "gift":
                            DesigPageName = "Skin-Desig_OnlineGifts.html";
                            SourcePageName = "OnlineGifts.aspx";
                            return;

                        case "giftdetail":
                            {
                                DesigPageName = "Skin-Desig_GiftDetails.html";
                                SourcePageName = "GiftDetails.aspx";
                                IList<GiftInfo> gifts = ProductBrowser.GetGifts(1);
                                if (gifts.Count <= 0)
                                {
                                    break;
                                }
                                SourcePageName = "GiftDetails.aspx?giftId=" + gifts[0].GiftId.ToString();
                                return;
                            }
                        case "shopcart":
                            DesigPageName = "Skin-Desig_ShoppingCart.html";
                            SourcePageName = "ShoppingCart.aspx";
                            break;

                        default:
                            return;
                    }
                }
            }
        }

        public static string SourcePagePath
        {
            get
            {
                return (HiContext.Current.HostPath + "/" + SourcePageName);
            }
        }
    }
}
