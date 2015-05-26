using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.SaleSystem.Tags;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Hidistro.UI.Web.Handler
{
	public class DesigAdvert : AdminPage, System.Web.IHttpHandler
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
				string a;
				if ((a = this.modeId) != null)
				{
					if (!(a == "editeadvertslide"))
					{
						if (!(a == "editeadvertimage"))
						{
							if (!(a == "editelogo"))
							{
								if (a == "editeadvertcustom")
								{
									string text = context.Request.Form["Param"];
									if (text != "")
									{
										JavaScriptObject advertcustomobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text);
										if (this.CheckAdvertCustom(advertcustomobject) && this.UpdateAdvertCustom(advertcustomobject))
										{
											var AnonymousType = new
											{
												AdCustom = new Common_CustomAd
												{
													AdId = System.Convert.ToInt32(this.elementId)
												}.RendHtml()
											};
                                            this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType));
										}
									}
								}
							}
							else
							{
								string text2 = context.Request.Form["Param"];
								if (text2 != "")
								{
									JavaScriptObject javaScriptObject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text2);
									if (this.CheckLogo(javaScriptObject) && this.UpdateLogo(javaScriptObject))
									{
										Common_Logo common_Logo = new Common_Logo();
										var AnonymousType2 = new
										{
											LogoUrl = common_Logo.RendHtml()
										};
										this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType2));
									}
								}
							}
						}
						else
						{
							string text3 = context.Request.Form["Param"];
							if (text3 != "")
							{
								JavaScriptObject advertimageobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text3);
								if (this.CheckAdvertImage(advertimageobject) && this.UpdateAdvertImage(advertimageobject))
								{
									var AnonymousType3 = new
									{
										AdImage = new Common_ImageAd
										{
											AdId = System.Convert.ToInt32(this.elementId)
										}.RendHtml()
									};
									this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType3));
								}
							}
						}
					}
					else
					{
						string text4 = context.Request.Form["Param"];
						if (text4 != "")
						{
							JavaScriptObject avdvertobject = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text4);
							if (this.CheckAdvertSlide(avdvertobject) && this.UpdateAdvertSlide(avdvertobject))
							{
								var AnonymousType4 = new
								{
									AdSlide = new Common_SlideAd
									{
										AdId = System.Convert.ToInt32(this.elementId)
									}.RendHtml()
								};
								this.message = string.Format(this.resultformat, "true", JavaScriptConvert.SerializeObject(AnonymousType4));
							}
						}
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
		private bool CheckAdvertSlide(JavaScriptObject avdvertobject)
		{
			if (string.IsNullOrEmpty(avdvertobject["Image1"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image2"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image3"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image4"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image5"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image6"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image7"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image8"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image9"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image10"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
				return false;
			}
			int num = 0;
			for (int i = 1; i <= 10; i++)
			{
				if (!string.IsNullOrEmpty(avdvertobject["Image" + i].ToString()))
				{
					num++;
				}
			}
			if (num < 3)
			{
				this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(avdvertobject["Id"].ToString()))
			{
				if (avdvertobject["Id"].ToString().Split(new char[]
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
		private bool UpdateAdvertSlide(JavaScriptObject avdvertobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改轮播广告失败\"");
			this.elementId = avdvertobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			avdvertobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(avdvertobject);
			return TagsHelper.UpdateAdNode((int)System.Convert.ToInt16(this.elementId), "slide", xmlNodeString);
		}
		private bool CheckAdvertImage(JavaScriptObject advertimageobject)
		{
			if (string.IsNullOrEmpty(advertimageobject["Image"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择广告图片！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(advertimageobject["Id"].ToString()))
			{
				if (advertimageobject["Id"].ToString().Split(new char[]
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
		private bool UpdateAdvertImage(JavaScriptObject advertimageobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改单张广告图片失败\"");
			this.elementId = advertimageobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			advertimageobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertimageobject);
			return TagsHelper.UpdateAdNode((int)System.Convert.ToInt16(this.elementId), "image", xmlNodeString);
		}
		private bool CheckAdvertCustom(JavaScriptObject advertcustomobject)
		{
			if (string.IsNullOrEmpty(advertcustomobject["Html"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"自定义内容不允许为空！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(advertcustomobject["Id"].ToString()))
			{
				if (advertcustomobject["Id"].ToString().Split(new char[]
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
		private bool UpdateAdvertCustom(JavaScriptObject advertcustomobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"自定义编辑失败\"");
			this.elementId = advertcustomobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
			advertcustomobject["Id"] = this.elementId;
			System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertcustomobject);
			if (xmlNodeString.Keys.Contains("Html"))
			{
				xmlNodeString["Html"] = Globals.HtmlDecode(xmlNodeString["Html"]);
			}
			return TagsHelper.UpdateAdNode((int)System.Convert.ToInt16(this.elementId), "custom", xmlNodeString);
		}
		private bool CheckLogo(JavaScriptObject logoobject)
		{
			if (string.IsNullOrEmpty(logoobject["LogoUrl"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请上传Logo图片！\"");
				return false;
			}
			return true;
		}
		private bool UpdateLogo(JavaScriptObject advertimageobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改Logo图片失败\"");
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
			siteSettings.LogoUrl = advertimageobject["LogoUrl"].ToString();
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			return true;
		}
		private bool ValidationSettings(Hidistro.Membership.Context.SiteSettings setting, ref string errors)
		{
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.SiteSettings>(setting, new string[]
			{
				"ValMasterSettings"
			});
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					errors += Formatter.FormatErrorMessage(current.Message);
				}
			}
			return validationResults.IsValid;
		}
		public System.Collections.Generic.Dictionary<string, string> GetXmlNodeString(JavaScriptObject scriptobject)
		{
			return scriptobject.ToDictionary((System.Collections.Generic.KeyValuePair<string, object> s) => s.Key, (System.Collections.Generic.KeyValuePair<string, object> s) => Globals.HtmlEncode(s.Value.ToString()));
		}
	}
}
