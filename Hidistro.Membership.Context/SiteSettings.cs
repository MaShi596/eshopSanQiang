using System.Xml;
using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Membership.Context
{
	public class SiteSettings
	{
		private string string_0 = "";
		private string string_1 = "";
		private int int_0 = 3;
		private int int_1 = 5;
		private System.DateTime? nullable_0 = null;
		private int int_2 = 2;
		private bool bool_0;
		private string string_2 = "";
		private string string_3 = "";
		private string string_4 = "";
		private string string_5 = "";
		private bool bool_1;
		private bool bool_2;
		private bool bool_3;
		private string string_6 = "";
		private decimal decimal_0;
		private string string_7 = "";
		private string string_8 = "";
		private int int_3;
		private string string_9 = "";
		private System.DateTime? nullable_1 = null;
		private string string_10 = "";
		private string string_11 = "";
		private string string_12 = "";
		private int int_4;
		private decimal decimal_1;
		private string string_13 = "";
		private int? nullable_2 = null;
		private string string_14 = "";
		private string string_15 = "";
		private string string_16 = "";
		private bool bool_4;
		private System.DateTime? nullable_3 = null;
		private string string_17 = "";
		private int int_5 = -1;
		private string string_18 = "";
		private string string_19 = "";
		private string string_20 = "";
		private bool bool_5;
		private string string_21 = "";
		private string string_22 = "";
		private string string_23 = "";
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_24;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_25;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_26;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_27;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_28;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_29;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_30;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_31;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_32;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private bool bool_6;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private bool bool_7;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_33;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_34;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_6;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_7;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_8;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_35;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_36;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_37;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_38;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_39;
		public string AssistantIv
		{
			get
			{
				return this.string_0;
			}
			set
			{
				this.string_0 = value;
			}
		}
		public string AssistantKey
		{
			get
			{
				return this.string_1;
			}
			set
			{
				this.string_1 = value;
			}
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "过期几天自动关闭订单的天数必须在1-90之间")]
		public int CloseOrderDays
		{
			get
			{
				return this.int_0;
			}
			set
			{
				this.int_0 = value;
			}
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "过期几天自动关闭采购单的天数必须在1-90之间")]
		public int ClosePurchaseOrderDays
		{
			get
			{
				return this.int_1;
			}
			set
			{
				this.int_1 = value;
			}
		}
		public System.DateTime? CreateDate
		{
			get
			{
				return this.nullable_0;
			}
			set
			{
				this.nullable_0 = value;
			}
		}
		public int DecimalLength
		{
			get
			{
				return this.int_2;
			}
			set
			{
				this.int_2 = value;
			}
		}
		public string DefaultProductImage
		{
			get;
			set;
		}
		public string DefaultProductThumbnail1
		{
			get;
			set;
		}
		public string DefaultProductThumbnail2
		{
			get;
			set;
		}
		public string DefaultProductThumbnail3
		{
			get;
			set;
		}
		public string DefaultProductThumbnail4
		{
			get;
			set;
		}
		public string DefaultProductThumbnail5
		{
			get;
			set;
		}
		public string DefaultProductThumbnail6
		{
			get;
			set;
		}
		public string DefaultProductThumbnail7
		{
			get;
			set;
		}
		public string DefaultProductThumbnail8
		{
			get;
			set;
		}
		public bool Disabled
		{
			get;
			set;
		}
		public bool IsOpenEtao
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				this.bool_0 = value;
			}
		}
		public bool IsOpenSiteSale
		{
			get;
			set;
		}
		public string DistributorRequestInstruction
		{
			get;
			set;
		}
		public string DistributorRequestProtocols
		{
			get;
			set;
		}
		public bool EmailEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings) && this.EmailSender.Trim().Length > 0 && this.EmailSettings.Trim().Length > 0;
			}
		}
		public string EmailSender
		{
			get
			{
				return this.string_2;
			}
			set
			{
				this.string_2 = value;
			}
		}
		public string EmailSettings
		{
			get
			{
				return this.string_3;
			}
			set
			{
				this.string_3 = value;
			}
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "发货几天自动完成订单的天数必须在1-90之间")]
		public int FinishOrderDays
		{
			get;
			set;
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "发货几天自动完成采购单的天数必须在1-90之间")]
		public int FinishPurchaseOrderDays
		{
			get;
			set;
		}
		public string Footer
		{
			get
			{
				return this.string_4;
			}
			set
			{
				this.string_4 = value;
			}
		}
		[StringLengthValidator(0, 4000, Ruleset = "ValMasterSettings", MessageTemplate = "网页客服代码长度限制在4000个字符以内")]
		public string HtmlOnlineServiceCode
		{
			get
			{
				return this.string_5;
			}
			set
			{
				this.string_5 = value;
			}
		}
		public bool IsDistributorSettings
		{
			get
			{
				return this.UserId.HasValue;
			}
		}
		public bool IsShowCountDown
		{
			get
			{
				return this.bool_1;
			}
			set
			{
				this.bool_1 = value;
			}
		}
		public bool IsShowGroupBuy
		{
			get
			{
				return this.bool_2;
			}
			set
			{
				this.bool_2 = value;
			}
		}
		public bool IsShowOnlineGift
		{
			get
			{
				return this.bool_3;
			}
			set
			{
				this.bool_3 = value;
			}
		}
		public string LogoUrl
		{
			get
			{
				return this.string_6;
			}
			set
			{
				this.string_6 = value;
			}
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "最近几天内订单的天数必须在1-90之间")]
		public int OrderShowDays
		{
			get;
			set;
		}
		[RangeValidator(typeof(decimal), "0.1", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "几元一积分必须在0.1-10000000之间")]
		public decimal PointsRate
		{
			get
			{
				return this.decimal_0;
			}
			set
			{
				this.decimal_0 = value;
			}
		}
		public string RecordCode
		{
			get
			{
				return this.string_7;
			}
			set
			{
				this.string_7 = value;
			}
		}
		public string RecordCode2
		{
			get
			{
				return this.string_8;
			}
			set
			{
				this.string_8 = value;
			}
		}
		public int ReferralDeduct
		{
			get
			{
				return this.int_3;
			}
			set
			{
				this.int_3 = value;
			}
		}
		public string RegisterAgreement
		{
			get
			{
				return this.string_9;
			}
			set
			{
				this.string_9 = value;
			}
		}
		public System.DateTime? RequestDate
		{
			get
			{
				return this.nullable_1;
			}
			set
			{
				this.nullable_1 = value;
			}
		}
		[HtmlCoding, StringLengthValidator(0, 100, Ruleset = "ValMasterSettings", MessageTemplate = "店铺描述META_DESCRIPTION的长度限制在100字符以内")]
		public string SearchMetaDescription
		{
			get;
			set;
		}
		[StringLengthValidator(0, 100, Ruleset = "ValMasterSettings", MessageTemplate = "搜索关键字META_KEYWORDS的长度限制在100字符以内")]
		public string SearchMetaKeywords
		{
			get;
			set;
		}
		[StringLengthValidator(0, 60, Ruleset = "ValMasterSettings", MessageTemplate = "简单介绍TITLE的长度限制在60字符以内")]
		public string SiteDescription
		{
			get;
			set;
		}
		[StringLengthValidator(1, 60, Ruleset = "ValMasterSettings", MessageTemplate = "店铺名称为必填项，长度限制在60字符以内")]
		public string SiteName
		{
			get;
			set;
		}
		[StringLengthValidator(1, 128, Ruleset = "ValMasterSettings", MessageTemplate = "店铺域名必须控制在128个字符以内")]
		public string SiteUrl
		{
			get;
			set;
		}
		public string SiteUrl2
		{
			get
			{
				return this.string_10;
			}
			set
			{
				this.string_10 = value;
			}
		}
		public bool SMSEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings) && this.SMSSender.Trim().Length > 0 && this.SMSSettings.Trim().Length > 0;
			}
		}
		public string SMSSender
		{
			get
			{
				return this.string_11;
			}
			set
			{
				this.string_11 = value;
			}
		}
		public string SMSSettings
		{
			get
			{
				return this.string_12;
			}
			set
			{
				this.string_12 = value;
			}
		}
		public int TaobaoShippingType
		{
			get
			{
				return this.int_4;
			}
			set
			{
				this.int_4 = value;
			}
		}
		[RangeValidator(typeof(decimal), "0", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "税率必须在0-100之间")]
		public decimal TaxRate
		{
			get
			{
				return this.decimal_1;
			}
			set
			{
				this.decimal_1 = value;
			}
		}
		public string Theme
		{
			get
			{
				return this.string_13;
			}
			set
			{
				this.string_13 = value;
			}
		}
		public int? UserId
		{
			get
			{
				return this.nullable_2;
			}
			set
			{
				this.nullable_2 = value;
			}
		}
		[StringLengthValidator(0, 10, Ruleset = "ValMasterSettings", MessageTemplate = "“您的价”重命名的长度限制在10字符以内")]
		public string YourPriceName
		{
			get
			{
				return this.string_14;
			}
			set
			{
				this.string_14 = value;
			}
		}
		public string CnzzUsername
		{
			get
			{
				return this.string_15;
			}
			set
			{
				this.string_15 = value;
			}
		}
		public string CnzzPassword
		{
			get
			{
				return this.string_16;
			}
			set
			{
				this.string_16 = value;
			}
		}
		public bool EnabledCnzz
		{
			get
			{
				return this.bool_4;
			}
			set
			{
				this.bool_4 = value;
			}
		}
		public System.DateTime? EtaoApplyTime
		{
			get
			{
				return this.nullable_3;
			}
			set
			{
				this.nullable_3 = value;
			}
		}
		public string EtaoID
		{
			get
			{
				return this.string_17;
			}
			set
			{
				this.string_17 = value;
			}
		}
		public int EtaoStatus
		{
			get
			{
				return this.int_5;
			}
			set
			{
				this.int_5 = value;
			}
		}
		public string SiteMapTime
		{
			get
			{
				return this.string_18;
			}
			set
			{
				this.string_18 = value;
			}
		}
		public string SiteMapNum
		{
			get
			{
				return this.string_19;
			}
			set
			{
				this.string_19 = value;
			}
		}
		public string CheckCode
		{
			get
			{
				return this.string_20;
			}
			set
			{
				this.string_20 = value;
			}
		}
		public bool EnableMobileClient
		{
			get
			{
				return this.bool_5;
			}
			set
			{
				this.bool_5 = value;
			}
		}
		public string MobileClientSpread
		{
			get
			{
				return this.string_21;
			}
			set
			{
				this.string_21 = value;
			}
		}
		public string CellPhoneToken
		{
			get
			{
				return this.string_22;
			}
			set
			{
				this.string_22 = value;
			}
		}
		public string CellPhoneUserCode
		{
			get
			{
				return this.string_23;
			}
			set
			{
				this.string_23 = value;
			}
		}
		public SiteSettings(string siteUrl, int? distributorUserId)
		{
			this.SiteUrl = siteUrl;
			this.UserId = distributorUserId;
			this.Disabled = false;
			this.SiteDescription = "全亚洲最快的香港云主机 www.vps123.cc";
			this.Theme = "default";
			this.SiteName = "分销子站";
			this.LogoUrl = "/utility/pics/logo.jpg";
			this.DefaultProductImage = "/utility/pics/none.gif";
			this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
			this.DecimalLength = 2;
			this.IsOpenSiteSale = true;
			this.IsShowGroupBuy = true;
			this.IsShowCountDown = true;
			this.IsShowOnlineGift = true;
			this.PointsRate = 1m;
			this.OrderShowDays = 7;
			this.CloseOrderDays = 3;
			this.ClosePurchaseOrderDays = 5;
			this.FinishOrderDays = 7;
			this.FinishPurchaseOrderDays = 10;
			this.HtmlOnlineServiceCode = "";
			this.CnzzUsername = "";
			this.CnzzPassword = "";
			this.EnabledCnzz = false;
			this.SiteMapNum = "";
			this.SiteMapTime = "";
			this.CheckCode = "";
		}
		public static SiteSettings FromXml(XmlDocument xmlDocument_0)
		{
			XmlNode xmlNode = xmlDocument_0.SelectSingleNode("Settings");
			return new SiteSettings(xmlNode.SelectSingleNode("SiteUrl").InnerText, null)
			{
				AssistantIv = xmlNode.SelectSingleNode("AssistantIv").InnerText,
				AssistantKey = xmlNode.SelectSingleNode("AssistantKey").InnerText,
				DecimalLength = int.Parse(xmlNode.SelectSingleNode("DecimalLength").InnerText),
				DefaultProductImage = xmlNode.SelectSingleNode("DefaultProductImage").InnerText,
				DefaultProductThumbnail1 = xmlNode.SelectSingleNode("DefaultProductThumbnail1").InnerText,
				DefaultProductThumbnail2 = xmlNode.SelectSingleNode("DefaultProductThumbnail2").InnerText,
				DefaultProductThumbnail3 = xmlNode.SelectSingleNode("DefaultProductThumbnail3").InnerText,
				DefaultProductThumbnail4 = xmlNode.SelectSingleNode("DefaultProductThumbnail4").InnerText,
				DefaultProductThumbnail5 = xmlNode.SelectSingleNode("DefaultProductThumbnail5").InnerText,
				DefaultProductThumbnail6 = xmlNode.SelectSingleNode("DefaultProductThumbnail6").InnerText,
				DefaultProductThumbnail7 = xmlNode.SelectSingleNode("DefaultProductThumbnail7").InnerText,
				DefaultProductThumbnail8 = xmlNode.SelectSingleNode("DefaultProductThumbnail8").InnerText,
				IsOpenSiteSale = bool.Parse(xmlNode.SelectSingleNode("IsOpenSiteSale").InnerText),
				Disabled = bool.Parse(xmlNode.SelectSingleNode("Disabled").InnerText),
				ReferralDeduct = int.Parse(xmlNode.SelectSingleNode("ReferralDeduct").InnerText),
				Footer = xmlNode.SelectSingleNode("Footer").InnerText,
				RegisterAgreement = xmlNode.SelectSingleNode("RegisterAgreement").InnerText,
				HtmlOnlineServiceCode = xmlNode.SelectSingleNode("HtmlOnlineServiceCode").InnerText,
				LogoUrl = xmlNode.SelectSingleNode("LogoUrl").InnerText,
				OrderShowDays = int.Parse(xmlNode.SelectSingleNode("OrderShowDays").InnerText),
				CloseOrderDays = int.Parse(xmlNode.SelectSingleNode("CloseOrderDays").InnerText),
				ClosePurchaseOrderDays = int.Parse(xmlNode.SelectSingleNode("ClosePurchaseOrderDays").InnerText),
				FinishOrderDays = int.Parse(xmlNode.SelectSingleNode("FinishOrderDays").InnerText),
				FinishPurchaseOrderDays = int.Parse(xmlNode.SelectSingleNode("FinishPurchaseOrderDays").InnerText),
				PointsRate = decimal.Parse(xmlNode.SelectSingleNode("PointsRate").InnerText),
				SearchMetaDescription = xmlNode.SelectSingleNode("SearchMetaDescription").InnerText,
				SearchMetaKeywords = xmlNode.SelectSingleNode("SearchMetaKeywords").InnerText,
				SiteDescription = xmlNode.SelectSingleNode("SiteDescription").InnerText,
				SiteName = xmlNode.SelectSingleNode("SiteName").InnerText,
				SiteUrl = xmlNode.SelectSingleNode("SiteUrl").InnerText,
				UserId = null,
				Theme = xmlNode.SelectSingleNode("Theme").InnerText,
				YourPriceName = xmlNode.SelectSingleNode("YourPriceName").InnerText,
				DistributorRequestInstruction = xmlNode.SelectSingleNode("DistributorRequestInstruction").InnerText,
				DistributorRequestProtocols = xmlNode.SelectSingleNode("DistributorRequestProtocols").InnerText,
				EmailSender = xmlNode.SelectSingleNode("EmailSender").InnerText,
				EmailSettings = xmlNode.SelectSingleNode("EmailSettings").InnerText,
				SMSSender = xmlNode.SelectSingleNode("SMSSender").InnerText,
				SMSSettings = xmlNode.SelectSingleNode("SMSSettings").InnerText,
				CnzzUsername = xmlNode.SelectSingleNode("CnzzUsername").InnerText,
				CnzzPassword = xmlNode.SelectSingleNode("CnzzPassword").InnerText,
				EnabledCnzz = bool.Parse(xmlNode.SelectSingleNode("EnabledCnzz").InnerText),
				SiteMapTime = xmlNode.SelectSingleNode("SiteMapTime").InnerText,
				SiteMapNum = xmlNode.SelectSingleNode("SiteMapNum").InnerText,
				CheckCode = xmlNode.SelectSingleNode("CheckCode").InnerText,
				TaobaoShippingType = int.Parse(xmlNode.SelectSingleNode("TaobaoShippingType").InnerText),
				TaxRate = decimal.Parse(xmlNode.SelectSingleNode("TaxRate").InnerText)
			};
		}
		private static void smethod_0(XmlDocument xmlDocument_0, XmlNode xmlNode_0, string string_40, string string_41)
		{
			XmlNode xmlNode = xmlNode_0.SelectSingleNode(string_40);
			if (xmlNode == null)
			{
				xmlNode = xmlDocument_0.CreateElement(string_40);
				xmlNode_0.AppendChild(xmlNode);
			}
			xmlNode.InnerText = string_41;
		}
		public void WriteToXml(XmlDocument xmlDocument_0)
		{
			XmlNode xmlNode_ = xmlDocument_0.SelectSingleNode("Settings");
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SiteUrl", this.SiteUrl);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "AssistantIv", this.AssistantIv);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "AssistantKey", this.AssistantKey);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DecimalLength", this.DecimalLength.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductImage", this.DefaultProductImage);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "IsOpenSiteSale", this.IsOpenSiteSale ? "true" : "false");
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "Disabled", this.Disabled ? "true" : "false");
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "ReferralDeduct", this.ReferralDeduct.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "Footer", this.Footer);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "RegisterAgreement", this.RegisterAgreement);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "HtmlOnlineServiceCode", this.HtmlOnlineServiceCode);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "LogoUrl", this.LogoUrl);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "OrderShowDays", this.OrderShowDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "CloseOrderDays", this.CloseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "ClosePurchaseOrderDays", this.ClosePurchaseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "FinishOrderDays", this.FinishOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "FinishPurchaseOrderDays", this.FinishPurchaseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "TaxRate", this.TaxRate.ToString());
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "PointsRate", this.PointsRate.ToString("F"));
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SearchMetaDescription", this.SearchMetaDescription);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SearchMetaKeywords", this.SearchMetaKeywords);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SiteDescription", this.SiteDescription);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SiteName", this.SiteName);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "Theme", this.Theme);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "YourPriceName", this.YourPriceName);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DistributorRequestInstruction", this.DistributorRequestInstruction);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "DistributorRequestProtocols", this.DistributorRequestProtocols);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "EmailSender", this.EmailSender);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "EmailSettings", this.EmailSettings);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SMSSender", this.SMSSender);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SMSSettings", this.SMSSettings);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "CnzzUsername", this.CnzzUsername);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "CnzzPassword", this.CnzzPassword);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "EnabledCnzz", this.EnabledCnzz.ToString());
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SiteMapTime", this.SiteMapTime);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "SiteMapNum", this.SiteMapNum);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "CheckCode", this.CheckCode);
			SiteSettings.smethod_0(xmlDocument_0, xmlNode_, "TaobaoShippingType", this.TaobaoShippingType.ToString());
		}
	}
}
