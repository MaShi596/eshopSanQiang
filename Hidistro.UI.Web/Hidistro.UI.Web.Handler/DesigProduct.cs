using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
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
	public class DesigProduct : AdminPage, System.Web.IHttpHandler
	{
		private string message = "";
		private string modeId = "";
		private string elementId = "";
		private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";
		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public new void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				this.modeId = context.Request.Form["ModelId"];
				string key;
				switch (key = this.modeId)
				{
				case "simpleview":
					this.message = string.Format(this.resultformat, "true", this.GetSimpleProductView());
					break;
				case "editesimple":
				{
					string text = context.Request.Form["Param"];
					if (text != "")
					{
						JavaScriptObject simpleobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text);
						if (this.CheckSimpleProduct(simpleobject) && this.UpdateSimpleProduct(simpleobject))
						{
							var AnonymousType = new
							{
								Simple = new Common_SubjectProduct_Simple
								{
									SubjectId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType));
						}
					}
					break;
				}
				case "producttabview":
				{
					string categorys = this.GetCategorys();
					string productBrand = this.GetProductBrand();
					string productTags = this.GetProductTags();
					string arg = string.Concat(new string[]
					{
						"{\"Categorys\":",
						categorys,
						",\"Brands\":",
						productBrand,
						",\"Tags\":",
						productTags,
						"}"
					});
					this.message = string.Format(this.resultformat, "true", arg);
					break;
				}
				case "editeproducttab":
				{
					string text2 = context.Request.Form["Param"];
					if (text2 != "")
					{
						JavaScriptObject javaScriptObject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text2);
						if (this.CheckProductTab(javaScriptObject) && this.UpdateProductTab(javaScriptObject))
						{
							var AnonymousType2 = new
							{
								ProductTab = new Common_SubjectProduct_Tab
								{
									SubjectId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType2));
						}
					}
					break;
				}
				case "productfloorview":
				{
					string categorys = this.GetCategorys();
					string productBrand = this.GetProductBrand();
					string productTags = this.GetProductTags();
					string arg2 = string.Concat(new string[]
					{
						"{\"Categorys\":",
						categorys,
						",\"Brands\":",
						productBrand,
						",\"Tags\":",
						productTags,
						"}"
					});
					this.message = string.Format(this.resultformat, "true", arg2);
					break;
				}
				case "editeproductfloor":
				{
					string text3 = context.Request.Form["Param"];
					if (text3 != "")
					{
						JavaScriptObject floorobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text3);
						if (this.CheckProductFloor(floorobject) && this.UpdateProductFloor(floorobject))
						{
							var AnonymousType3 = new
							{
								ProductFloor = new Common_SubjectProduct_Floor
								{
									SubjectId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType3));
						}
					}
					break;
				}
				case "productgroupview":
				{
					string categorys = this.GetCategorys();
					string arg3 = "{\"Categorys\":" + categorys + "}";
					this.message = string.Format(this.resultformat, "true", arg3);
					break;
				}
				case "editeproductgroup":
				{
					string text4 = context.Request.Form["Param"];
					if (text4 != "")
					{
						JavaScriptObject groupobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text4);
						if (this.CheckProductGroup(groupobject) && this.UpdateProductGroup(groupobject))
						{
							var AnonymousType4 = new
							{
								ProductGroup = new Common_SubjectProduct_Group
								{
									SubjectId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
                            this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType4));
						}
					}
					break;
				}
				case "producttopview":
				{
					string categorys = this.GetCategorys();
					string arg4 = "{\"Categorys\":" + categorys + "}";
					this.message = string.Format(this.resultformat, "true", arg4);
					break;
				}
				case "editeproducttop":
				{
					string text5 = context.Request.Form["Param"];
					if (text5 != "")
					{
						JavaScriptObject topobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text5);
						if (this.CheckProductTop(topobject) && this.UpdateProductTop(topobject))
						{
							var AnonymousType5 = new
							{
								ProductTop = new Common_SubjectProduct_Top
								{
									SubjectId = System.Convert.ToInt32(this.elementId)
								}.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType5));
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
			return scriptobject.ToDictionary((System.Collections.Generic.KeyValuePair<string, object> s) => s.Key, (System.Collections.Generic.KeyValuePair<string, object> s) => Globals.HtmlEncode(DataHelper.CleanSearchString(s.Value.ToString())));
		}
		private string GetSimpleProductView()
		{
			string text = "";
			string text2 = "";
			string productBrand = this.GetProductBrand();
			string categorys = this.GetCategorys();
			string productTags = this.GetProductTags();
			System.Collections.Generic.IList<ProductTypeInfo> productTypeList = this.GetProductTypeList();
			System.Collections.Generic.IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(AttributeUseageMode.MultiView);
			if (productTypeList != null)
			{
				text = JavaScriptConvert.SerializeObject(productTypeList);
			}
			if (attributes.Count > 0)
			{
				text2 = JavaScriptConvert.SerializeObject(attributes);
			}
			return string.Concat(new string[]
			{
				"{\"Categorys\":",
				categorys,
				",\"Brands\":",
				productBrand,
				",\"Tags\":",
				productTags,
				",\"ProductTypes\":",
				text,
				",\"Attributes\":",
				text2,
				"}"
			});
		}
		private bool CheckSimpleProduct(JavaScriptObject simpleobject)
		{
			if (string.IsNullOrEmpty(simpleobject["MaxNum"].ToString()) || System.Convert.ToInt16(simpleobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt16(simpleobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(simpleobject["ImageSize"].ToString()) && System.Convert.ToInt16(simpleobject["ImageSize"].ToString()) > 0)
			{
				if (!string.IsNullOrEmpty(simpleobject["SubjectId"].ToString()))
				{
					if (simpleobject["SubjectId"].ToString().Split(new char[]
					{
						'_'
					}).Length == 2)
					{
						return true;
					}
				}
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
			return false;
		}
		private bool UpdateSimpleProduct(JavaScriptObject simpleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改商品列表失败\"");
			this.elementId = simpleobject["SubjectId"].ToString().Split(new char[]
			{
				'_'
			})[1];
			simpleobject["SubjectId"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(simpleobject);
			return TagsHelper.UpdateProductNode((int)System.Convert.ToInt16(this.elementId), "simple", xmlNodeString);
		}
		private bool CheckProductTab(JavaScriptObject tabobject)
		{
			if (string.IsNullOrEmpty(tabobject["MaxNum"].ToString()) || System.Convert.ToInt16(tabobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt16(tabobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(tabobject["ImageSize"].ToString()) && System.Convert.ToInt16(tabobject["ImageSize"].ToString()) > 0)
			{
				if (!string.IsNullOrEmpty(tabobject["SubjectId"].ToString()))
				{
					if (tabobject["SubjectId"].ToString().Split(new char[]
					{
						'_'
					}).Length == 2)
					{
						return true;
					}
				}
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
			return false;
		}
		private bool UpdateProductTab(JavaScriptObject producttabobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改商品选项卡失败\"");
			this.elementId = producttabobject["SubjectId"].ToString().Split(new char[]
			{
				'_'
			})[1];
			producttabobject["SubjectId"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(producttabobject);
			return TagsHelper.UpdateProductNode((int)System.Convert.ToInt16(this.elementId), "tab", xmlNodeString);
		}
		private bool CheckProductFloor(JavaScriptObject floorobject)
		{
			if (string.IsNullOrEmpty(floorobject["MaxNum"].ToString()) || System.Convert.ToInt16(floorobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt16(floorobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(floorobject["SubCategoryNum"].ToString()) && (System.Convert.ToInt16(floorobject["SubCategoryNum"].ToString()) < 0 || System.Convert.ToInt16(floorobject["MaxNum"].ToString()) > 100))
			{
				this.message = string.Format(this.resultformat, "false", "\"子类显示数量必须为0~100的正整数！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(floorobject["ImageSize"].ToString()) && System.Convert.ToInt16(floorobject["ImageSize"].ToString()) > 0)
			{
				if (!string.IsNullOrEmpty(floorobject["SubjectId"].ToString()))
				{
					if (floorobject["SubjectId"].ToString().Split(new char[]
					{
						'_'
					}).Length == 2)
					{
						if (string.IsNullOrEmpty(floorobject["Title"].ToString()) && string.IsNullOrEmpty(floorobject["ImageTitle"].ToString()))
						{
							this.message = string.Format(this.resultformat, "false", "\"请上传标题图片或输入楼层标题\"");
							return false;
						}
						return true;
					}
				}
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
			return false;
		}
		private bool UpdateProductFloor(JavaScriptObject floorobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改商品楼层失败\"");
			this.elementId = floorobject["SubjectId"].ToString().Split(new char[]
			{
				'_'
			})[1];
			floorobject["SubjectId"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(floorobject);
			return TagsHelper.UpdateProductNode((int)System.Convert.ToInt16(this.elementId), "floor", xmlNodeString);
		}
		private bool CheckProductGroup(JavaScriptObject groupobject)
		{
			if (string.IsNullOrEmpty(groupobject["Title"].ToString()) && string.IsNullOrEmpty(groupobject["ImageTitle"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请上传标题图片或输入栏目标题\"");
				return false;
			}
			if (string.IsNullOrEmpty(groupobject["MaxNum"].ToString()) || System.Convert.ToInt16(groupobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt16(groupobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(groupobject["SubCategoryNum"].ToString()) || System.Convert.ToInt16(groupobject["SubCategoryNum"].ToString()) < 0 || System.Convert.ToInt16(groupobject["SubCategoryNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"子类商品数量必须为1~100！\"");
				return false;
			}
			if (string.IsNullOrEmpty(groupobject["BrandNum"].ToString()) || System.Convert.ToInt16(groupobject["BrandNum"].ToString()) < 0 || System.Convert.ToInt16(groupobject["BrandNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"品牌显示数量必须1~`100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(groupobject["HotKeywordNum"].ToString()) || System.Convert.ToInt16(groupobject["HotKeywordNum"].ToString()) < 0 || System.Convert.ToInt16(groupobject["HotKeywordNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"热门关键字显示数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(groupobject["ImageSize"].ToString()) || System.Convert.ToInt16(groupobject["ImageSize"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品图片规格不允许为空！\"");
				return false;
			}
			if (string.IsNullOrEmpty(groupobject["SaleTopNum"].ToString()) || System.Convert.ToInt16(groupobject["SaleTopNum"].ToString()) < 0 || System.Convert.ToInt16(groupobject["SaleTopNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"销售排行显示数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(groupobject["ImageNum"].ToString()) || System.Convert.ToInt16(groupobject["ImageNum"].ToString()) < 0 || System.Convert.ToInt16(groupobject["ImageNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"排行榜图片显示数量必须为1~100的正整数！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(groupobject["TopImageSize"].ToString()) && System.Convert.ToInt16(groupobject["TopImageSize"].ToString()) > 0)
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"排行榜图片图片规格不允许为空！\"");
			return false;
		}
		private bool UpdateProductGroup(JavaScriptObject groupobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改组合商品失败\"");
			this.elementId = groupobject["SubjectId"].ToString().Split(new char[]
			{
				'_'
			})[1];
			groupobject["SubjectId"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(groupobject);
			return TagsHelper.UpdateProductNode((int)System.Convert.ToInt16(this.elementId), "group", xmlNodeString);
		}
		private bool CheckProductTop(JavaScriptObject topobject)
		{
			if (string.IsNullOrEmpty(topobject["MaxNum"].ToString()) || System.Convert.ToInt16(topobject["MaxNum"].ToString()) <= 0 || System.Convert.ToInt16(topobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(topobject["ImageNum"].ToString()) || System.Convert.ToInt16(topobject["ImageNum"].ToString()) < 0 || System.Convert.ToInt16(topobject["ImageNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"图片显示数量必须为1~100的正整数！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(topobject["ImageSize"].ToString()) && System.Convert.ToInt16(topobject["ImageSize"].ToString()) > 0)
			{
				return true;
			}
			this.message = string.Format(this.resultformat, "false", "\"商品图片规格不允许为空！\"");
			return false;
		}
		private bool UpdateProductTop(JavaScriptObject topobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改销售排行榜失败\"");
			this.elementId = topobject["SubjectId"].ToString().Split(new char[]
			{
				'_'
			})[1];
			topobject["SubjectId"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(topobject);
			return TagsHelper.UpdateProductNode((int)System.Convert.ToInt16(this.elementId), "top", xmlNodeString);
		}
		private string GetProductBrand()
		{
			string result = "";
			int index = 2;
			System.Data.DataTable dataTable = ControlProvider.Instance().GetBrandCategories().Copy();
			if (dataTable != null)
			{
				do
				{
					dataTable.Columns.RemoveAt(index);
				}
				while (dataTable.Columns.Count > 2);
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
		private string GetProductTags()
		{
			string result = "";
			System.Data.DataTable tags = CatalogHelper.GetTags();
			if (tags != null)
			{
				result = JavaScriptConvert.SerializeObject(tags, new JsonConverter[]
				{
					new ConvertTojson()
				});
			}
			return result;
		}
		private System.Collections.Generic.IList<ProductTypeInfo> GetProductTypeList()
		{
			return ControlProvider.Instance().GetProductTypes();
		}
		public string GetXmlPath(string xmlname)
		{
			if (xmlname != "")
			{
				return Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/config/" + xmlname + ".xml");
			}
			return null;
		}
	}
}
