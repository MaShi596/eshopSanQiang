using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Comments;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
namespace Hidistro.UI.Web.Handler
{
	public class DesigComents : System.Web.IHttpHandler
	{
		private string message = "";
		private string modeId = "";
		private string elementId = "";
		private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				this.modeId = context.Request.Form["ModelId"];
				string key;
				switch (key = this.modeId)
				{
				case "commentarticleview":
				{
					string arg = this.GetMainArticleCategories();
					this.message = string.Format(this.resultformat, "true", arg);
					break;
				}
				case "commentCategory":
				{
					string arg = this.GetCategorys();
					this.message = string.Format(this.resultformat, "true", arg);
					break;
				}
				case "editecommentarticle":
				{
					string text = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(text))
					{
						JavaScriptObject articleobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text);
						if (this.CheckCommentArticle(articleobject) && this.UpdateCommentArticle(articleobject))
						{
							var AnonymousType = new
							{
								ComArticle = new Common_SubjectArticle
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
                            this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType));
						}
					}
					break;
				}
				case "editecommentcategory":
				{
					string text2 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(text2))
					{
						JavaScriptObject categoryobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text2);
						if (this.CheckCommentCategory(categoryobject) && this.UpdateCommentCategory(categoryobject))
						{
							var AnonymousTypea = new
							{
								ComCategory = new Common_SubjectCategory
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousTypea));
						}
					}
					break;
				}
				case "editecommentbrand":
				{
					string text3 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(text3))
					{
						JavaScriptObject javaScriptObject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text3);
						if (this.CheckCommentBrand(javaScriptObject) && this.UpdateCommentBrand(javaScriptObject))
						{
							var AnonymousTypeb = new
							{
								ComBrand = new Common_SubjectBrand
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousTypeb));
						}
					}
					break;
				}
				case "editecommentkeyword":
				{
					string text4 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(text4))
					{
						JavaScriptObject keywordobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text4);
						if (this.CheckCommentKeyWord(keywordobject) && this.UpdateCommentKeyWord(keywordobject))
						{
							var AnonymousTypea2 = new
							{
								ComCategory = new Common_SubjectKeyword
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousTypea2));
						}
					}
					break;
				}
				case "editecommentattribute":
				{
					string text5 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(text5))
					{
						JavaScriptObject attributeobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text5);
						if (this.CheckCommentAttribute(attributeobject) && this.UpdateCommentAttribute(attributeobject))
						{
							var AnonymousTypec = new
							{
								ComAttribute = new Common_SubjectAttribute
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousTypec));
						}
					}
					break;
				}
				case "editecommenttitle":
				{
					string text6 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(text6))
					{
						JavaScriptObject titleobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text6);
						if (this.CheckCommentTitle(titleobject) && this.UpdateCommentTitle(titleobject))
						{
							var AnonymousTyped = new
							{
								ComTitle = new Common_SubjectTitle
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousTyped));
						}
					}
					break;
				}
				case "editecommentmorelink":
				{
					string text7 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(text7))
					{
						JavaScriptObject morelinkobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text7);
						if (this.CheckCommentMorelink(morelinkobject) && this.UpdateMorelink(morelinkobject))
						{
							var AnonymousTypee = new
							{
								ComMoreLink = new Common_SubjectMoreLink
								{
									CommentId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousTypee));
						}
					}
					break;
				}
				}
			}
			catch (System.Exception ex)
			{
				this.message = "{\"success\":false,\"Result\":\"未知错误:" + ex.Message + "\"}";
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}
		public System.Collections.Generic.Dictionary<string, string> GetXmlNodeString(JavaScriptObject scriptobject)
		{
			return scriptobject.ToDictionary((System.Collections.Generic.KeyValuePair<string, object> s) => s.Key, (System.Collections.Generic.KeyValuePair<string, object> s) => Globals.HtmlEncode(s.Value.ToString()));
		}
		private bool CheckCommentArticle(JavaScriptObject articleobject)
		{
			if (string.IsNullOrEmpty(articleobject["Title"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入文字标题!\"");
				return false;
			}
			if (!string.IsNullOrEmpty(articleobject["MaxNum"].ToString()) && System.Convert.ToInt16(articleobject["MaxNum"].ToString()) > 0 && System.Convert.ToInt16(articleobject["MaxNum"].ToString()) <= 100)
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
			return false;
		}
		private bool UpdateCommentArticle(JavaScriptObject articleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改文章标签列表失败\"");
			this.elementId = articleobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			articleobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(articleobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt16(this.elementId), "article", xmlNodeString);
		}
		private bool CheckCommentCategory(JavaScriptObject categoryobject)
		{
			if (string.IsNullOrEmpty(categoryobject["CategoryId"].ToString()) || System.Convert.ToInt16(categoryobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
				return false;
			}
			if (!string.IsNullOrEmpty(categoryobject["MaxNum"].ToString()) && System.Convert.ToInt16(categoryobject["MaxNum"].ToString()) > 0)
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
			return false;
		}
		private bool UpdateCommentCategory(JavaScriptObject categoryobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改分类标签列表失败\"");
			this.elementId = categoryobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			categoryobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(categoryobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt16(this.elementId), "category", xmlNodeString);
		}
		private bool CheckCommentAttribute(JavaScriptObject attributeobject)
		{
			if (string.IsNullOrEmpty(attributeobject["CategoryId"].ToString()) || System.Convert.ToInt16(attributeobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
				return false;
			}
			if (!string.IsNullOrEmpty(attributeobject["MaxNum"].ToString()) && System.Convert.ToInt16(attributeobject["MaxNum"].ToString()) > 0)
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
			return false;
		}
		private bool UpdateCommentAttribute(JavaScriptObject attributeobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改属性标签列表失败\"");
			this.elementId = attributeobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			attributeobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt16(this.elementId), "attribute", xmlNodeString);
		}
		private bool CheckCommentBrand(JavaScriptObject brandobject)
		{
			if (!string.IsNullOrEmpty(brandobject["CategoryId"].ToString()) && System.Convert.ToInt16(brandobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(brandobject["IsShowLogo"].ToString()) || string.IsNullOrEmpty(brandobject["IsShowTitle"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"参数格式不正确!\"");
				return false;
			}
			if (!string.IsNullOrEmpty(brandobject["MaxNum"].ToString()) && System.Convert.ToInt16(brandobject["MaxNum"].ToString()) > 0 && System.Convert.ToInt16(brandobject["MaxNum"].ToString()) <= 100)
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"显示数量必须为0~100的正整数！\"");
			return false;
		}
		private bool UpdateCommentBrand(JavaScriptObject attributeobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = attributeobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			attributeobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt16(this.elementId), "brand", xmlNodeString);
		}
		private bool CheckCommentKeyWord(JavaScriptObject keywordobject)
		{
			if (!string.IsNullOrEmpty(keywordobject["CategoryId"].ToString()) && System.Convert.ToInt16(keywordobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(keywordobject["MaxNum"].ToString()) && System.Convert.ToInt16(keywordobject["MaxNum"].ToString()) > 0 && System.Convert.ToInt16(keywordobject["MaxNum"].ToString()) <= 100)
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"显示数量必须为1~100的正整数！\"");
			return false;
		}
		private bool UpdateCommentKeyWord(JavaScriptObject keywordobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = keywordobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			keywordobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(keywordobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt16(this.elementId), "keyword", xmlNodeString);
		}
		private bool CheckCommentMorelink(JavaScriptObject morelinkobject)
		{
			if (!string.IsNullOrEmpty(morelinkobject["CategoryId"].ToString()) && System.Convert.ToInt16(morelinkobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(morelinkobject["Title"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入链接标题！\"");
				return false;
			}
			return true;
		}
		private bool UpdateMorelink(JavaScriptObject morelinkobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = morelinkobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			morelinkobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(morelinkobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt16(this.elementId), "morelink", xmlNodeString);
		}
		private bool CheckCommentTitle(JavaScriptObject titleobject)
		{
			if (!string.IsNullOrEmpty(titleobject["Title"].ToString()) && !string.IsNullOrEmpty(titleobject["ImageTitle"].ToString()))
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"请输入标题或上传图片！\"");
			return false;
		}
		private bool UpdateCommentTitle(JavaScriptObject titleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = titleobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			titleobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(titleobject);
			return TagsHelper.UpdateCommentNode((int)System.Convert.ToInt16(this.elementId), "title", xmlNodeString);
		}
		private string GetCategorys()
		{
			string result = "";
			string[] propertyName = new string[]
			{
				"CategoryId",
				"Name",
				"Depth"
			};
			System.Data.DataTable dataTable;
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				dataTable = this.ConvertListToDataTable<CategoryInfo>(SubsiteCatalogHelper.GetSequenceCategories(), propertyName);
			}
			else
			{
				dataTable = this.ConvertListToDataTable<CategoryInfo>(CatalogHelper.GetSequenceCategories(), propertyName);
			}
			if (dataTable != null)
			{
				result = JavaScriptConvert.SerializeObject(dataTable, new JsonConverter[]
				{
					new ConvertTojson()
				});
			}
			return result;
		}
		private string GetMainArticleCategories()
		{
			System.Collections.Generic.IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
			return JavaScriptConvert.SerializeObject(articleMainCategories);
		}
		private System.Data.DataTable ConvertListToDataTable<T>(System.Collections.Generic.IList<T> list, params string[] propertyName)
		{
			System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
			if (propertyName != null)
			{
				list2.AddRange(propertyName);
			}
			System.Data.DataTable dataTable = new System.Data.DataTable();
			if (list.Count > 0)
			{
				T t = list[0];
				System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties();
				System.Reflection.PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					System.Reflection.PropertyInfo propertyInfo = array[i];
					if (list2.Count == 0)
					{
						dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
					}
					else
					{
						if (list2.Contains(propertyInfo.Name))
						{
							dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
						}
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
					System.Reflection.PropertyInfo[] array2 = properties;
					for (int k = 0; k < array2.Length; k++)
					{
						System.Reflection.PropertyInfo propertyInfo2 = array2[k];
						if (list2.Count == 0)
						{
							object value = propertyInfo2.GetValue(list[j], null);
							arrayList.Add(value);
						}
						else
						{
							if (list2.Contains(propertyInfo2.Name))
							{
								object value2 = propertyInfo2.GetValue(list[j], null);
								arrayList.Add(value2);
							}
						}
					}
					object[] values = arrayList.ToArray();
					dataTable.LoadDataRow(values, true);
				}
			}
			return dataTable;
		}
	}
}
