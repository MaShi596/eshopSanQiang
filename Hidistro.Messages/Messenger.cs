using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hishop.Plugins;
using System;
using System.Net.Mail;
using System.Text;
namespace Hidistro.Messages
{
	public static class Messenger
	{
		internal static bool SendMail(System.Net.Mail.MailMessage email, EmailSender sender)
		{
			string text;
			return Messenger.SendMail(email, sender, out text);
		}
		internal static bool SendMail(System.Net.Mail.MailMessage email, EmailSender sender, out string string_0)
		{
			bool result;
			try
			{
				string_0 = "";
				result = sender.Send(email, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding));
			}
			catch (System.Exception ex)
			{
				string_0 = ex.Message;
				result = false;
			}
			return result;
		}
		public static SendStatus SendMail(string subject, string body, string emailTo, SiteSettings settings, out string string_0)
		{
			string_0 = "";
			SendStatus result;
			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body) || string.IsNullOrEmpty(emailTo) || subject.Trim().Length == 0 || body.Trim().Length == 0 || emailTo.Trim().Length == 0)
			{
				result = SendStatus.RequireMsg;
			}
			else
			{
				if (settings == null || !settings.EmailEnabled)
				{
					result = SendStatus.NoProvider;
				}
				else
				{
					EmailSender emailSender = Messenger.CreateEmailSender(settings, out string_0);
					if (emailSender == null)
					{
						result = SendStatus.ConfigError;
					}
					else
					{
						System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
						{
							IsBodyHtml = true,
							Priority = System.Net.Mail.MailPriority.High,
							Body = body.Trim(),
							Subject = subject.Trim()
						};
						mailMessage.To.Add(emailTo);
						result = (Messenger.SendMail(mailMessage, emailSender, out string_0) ? SendStatus.Success : SendStatus.Fail);
					}
				}
			}
			return result;
		}
		public static SendStatus SendMail(string subject, string body, string[] string_0, string[] string_1, SiteSettings settings, out string string_2)
		{
			string_2 = "";
			SendStatus result;
			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body) || subject.Trim().Length == 0 || body.Trim().Length == 0 || ((string_0 == null || string_0.Length == 0) && (string_1 == null || string_1.Length == 0)))
			{
				result = SendStatus.RequireMsg;
			}
			else
			{
				if (settings == null || !settings.EmailEnabled)
				{
					result = SendStatus.NoProvider;
				}
				else
				{
					EmailSender emailSender = Messenger.CreateEmailSender(settings, out string_2);
					if (emailSender == null)
					{
						result = SendStatus.ConfigError;
					}
					else
					{
						System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
						{
							IsBodyHtml = true,
							Priority = System.Net.Mail.MailPriority.High,
							Body = body.Trim(),
							Subject = subject.Trim()
						};
						if (string_0 != null && string_0.Length > 0)
						{
							for (int i = 0; i < string_0.Length; i++)
							{
								string addresses = string_0[i];
								mailMessage.CC.Add(addresses);
							}
						}
						if (string_1 != null && string_1.Length > 0)
						{
							for (int i = 0; i < string_1.Length; i++)
							{
								string addresses = string_1[i];
								mailMessage.Bcc.Add(addresses);
							}
						}
						result = (Messenger.SendMail(mailMessage, emailSender, out string_2) ? SendStatus.Success : SendStatus.Fail);
					}
				}
			}
			return result;
		}
		internal static EmailSender CreateEmailSender(SiteSettings settings)
		{
			string text;
			return Messenger.CreateEmailSender(settings, out text);
		}
		internal static EmailSender CreateEmailSender(SiteSettings settings, out string string_0)
		{
			EmailSender result;
			try
			{
				string_0 = "";
				if (!settings.EmailEnabled)
				{
					result = null;
				}
				else
				{
					result = EmailSender.CreateInstance(settings.EmailSender, HiCryptographer.Decrypt(settings.EmailSettings));
				}
			}
			catch (System.Exception ex)
			{
				string_0 = ex.Message;
				result = null;
			}
			return result;
		}
		public static SendStatus SendSMS(string phoneNumber, string message, SiteSettings settings, out string string_0)
		{
			string_0 = "";
			SendStatus result;
			if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message) || phoneNumber.Trim().Length == 0 || message.Trim().Length == 0)
			{
				result = SendStatus.RequireMsg;
			}
			else
			{
				if (settings == null || !settings.SMSEnabled)
				{
					result = SendStatus.NoProvider;
				}
				else
				{
					SMSSender sMSSender = Messenger.CreateSMSSender(settings, out string_0);
					if (sMSSender == null)
					{
						result = SendStatus.ConfigError;
					}
					else
					{
						result = (sMSSender.Send(phoneNumber, message, out string_0) ? SendStatus.Success : SendStatus.Fail);
					}
				}
			}
			return result;
		}
		public static SendStatus SendSMS(string[] phoneNumbers, string message, SiteSettings settings, out string string_0)
		{
			string_0 = "";
			SendStatus result;
			if (phoneNumbers == null || string.IsNullOrEmpty(message) || phoneNumbers.Length == 0 || message.Trim().Length == 0)
			{
				result = SendStatus.RequireMsg;
			}
			else
			{
				if (settings == null || !settings.SMSEnabled)
				{
					result = SendStatus.NoProvider;
				}
				else
				{
					SMSSender sMSSender = Messenger.CreateSMSSender(settings, out string_0);
					if (sMSSender == null)
					{
						result = SendStatus.ConfigError;
					}
					else
					{
						result = (sMSSender.Send(phoneNumbers, message, out string_0) ? SendStatus.Success : SendStatus.Fail);
					}
				}
			}
			return result;
		}
		internal static SMSSender CreateSMSSender(SiteSettings settings)
		{
			string text;
			return Messenger.CreateSMSSender(settings, out text);
		}
		internal static SMSSender CreateSMSSender(SiteSettings settings, out string string_0)
		{
			SMSSender result;
			try
			{
				string_0 = "";
				if (!settings.SMSEnabled)
				{
					result = null;
				}
				else
				{
					result = SMSSender.CreateInstance(settings.SMSSender, HiCryptographer.Decrypt(settings.SMSSettings));
				}
			}
			catch (System.Exception ex)
			{
				string_0 = ex.Message;
				result = null;
			}
			return result;
		}
		public static SendStatus SendInnerMessage(SiteSettings settings, string subject, string message, string sendto)
		{
			SendStatus result;
			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message) || subject.Trim().Length == 0 || message.Trim().Length == 0)
			{
				result = SendStatus.RequireMsg;
			}
			else
			{
				if (settings == null)
				{
					result = SendStatus.NoProvider;
				}
				else
				{
					if (settings.IsDistributorSettings)
					{
						IUser user = Users.GetUser(settings.UserId.Value);
						result = (InnerMessageProvider.Instance().SendDistributorMessage(subject, message, user.Username, sendto) ? SendStatus.Success : SendStatus.Fail);
					}
					else
					{
						result = (InnerMessageProvider.Instance().SendMessage(subject, message, sendto) ? SendStatus.Success : SendStatus.Fail);
					}
				}
			}
			return result;
		}
		public static void AcceptRequest(IUser user)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("AcceptDistributorRequest");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, null, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void UserRegister(IUser user, string createPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("NewUserAccountCreated");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, createPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void UserPasswordChanged(IUser user, string changedPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedPassword");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, changedPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void UserPasswordForgotten(IUser user, string resetPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("ForgottenPassword");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, resetPassword, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, true, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void UserDealPasswordChanged(IUser user, string changedDealPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedDealPassword");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericUserMessages(siteSettings, user.Username, user.Email, null, changedDealPassword, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void OrderCreated(OrderInfo order, IUser user)
		{
			if (order != null && user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderCreated");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, order.GetTotal(), order.Remark, order.ModeName, order.ShipTo, order.Address, order.ZipCode, order.TelPhone, order.CellPhone, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void OrderPayment(IUser user, string orderId, decimal amount)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderPayment");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, orderId, amount, null, null, null, null, null, null, null, null, null, 0m, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void OrderShipping(OrderInfo order, IUser user)
		{
			if (order != null && user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderShipping");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, order.OrderId, order.GetTotal(), order.Remark, order.RealModeName, order.ShipTo, order.Address, order.ZipCode, order.TelPhone, order.CellPhone, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void OrderRefund(IUser user, string orderId, decimal amount)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderRefund");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, orderId, 0m, null, null, null, null, null, null, null, null, null, amount, null, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		public static void OrderClosed(IUser user, string orderId, string reason)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderClosed");
				if (template != null)
				{
					System.Net.Mail.MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					Messenger.GenericOrderMessages(siteSettings, user.Username, user.Email, orderId, 0m, null, null, null, null, null, null, null, null, null, 0m, reason, template, out email, out smsMessage, out innerSubject, out innerMessage);
					Messenger.Send(template, siteSettings, user, false, email, innerSubject, innerMessage, smsMessage);
				}
			}
		}
		private static void Send(MessageTemplate template, SiteSettings settings, IUser user, bool sendFirst, System.Net.Mail.MailMessage email, string innerSubject, string innerMessage, string smsMessage)
		{
			if (template.SendEmail && email != null)
			{
				if (sendFirst)
				{
					EmailSender emailSender = Messenger.CreateEmailSender(settings);
					if (emailSender == null || !Messenger.SendMail(email, emailSender))
					{
						Emails.EnqueuEmail(email, settings);
					}
				}
				else
				{
					Emails.EnqueuEmail(email, settings);
				}
			}
			if (template.SendSMS)
			{
				string userCellPhone = Messenger.GetUserCellPhone(user);
				if (!string.IsNullOrEmpty(userCellPhone))
				{
					string text;
					Messenger.SendSMS(userCellPhone, smsMessage, settings, out text);
				}
			}
			if (template.SendInnerMessage)
			{
				Messenger.SendInnerMessage(settings, innerSubject, innerMessage, user.Username);
			}
		}
		private static string GetUserCellPhone(IUser user)
		{
			string result;
			if (user == null)
			{
				result = null;
			}
			else
			{
				if (user is Member)
				{
					result = ((Member)user).CellPhone;
				}
				else
				{
					if (user is Distributor)
					{
						result = ((Distributor)user).CellPhone;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}
		private static void GenericUserMessages(SiteSettings settings, string username, string userEmail, string password, string dealPassword, MessageTemplate template, out System.Net.Mail.MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			string text;
			innerMessage = (text = null);
			innerSubject = text;
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericUserEmail(template, settings, username, userEmail, password, dealPassword);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericUserMessageFormatter(settings, template.SMSBody, username, userEmail, password, dealPassword);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageSubject, username, userEmail, password, dealPassword);
					innerMessage = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageBody, username, userEmail, password, dealPassword);
				}
			}
		}
		private static System.Net.Mail.MailMessage GenericUserEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string password, string dealPassword)
		{
			System.Net.Mail.MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			System.Net.Mail.MailMessage result;
			if (emailTemplate == null)
			{
				result = null;
			}
			else
			{
				emailTemplate.Subject = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Subject, username, userEmail, password, dealPassword);
				emailTemplate.Body = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Body, username, userEmail, password, dealPassword);
				result = emailTemplate;
			}
			return result;
		}
		private static string GenericUserMessageFormatter(SiteSettings settings, string stringToFormat, string username, string userEmail, string password, string dealPassword)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username.Trim());
			stringToFormat = stringToFormat.Replace("$Email$", userEmail.Trim());
			stringToFormat = stringToFormat.Replace("$Password$", password);
			stringToFormat = stringToFormat.Replace("$DealPassword$", dealPassword);
			return stringToFormat;
		}
		private static void GenericOrderMessages(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason, MessageTemplate template, out System.Net.Mail.MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			string text;
			innerMessage = (text = null);
			innerSubject = text;
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericOrderEmail(template, settings, username, userEmail, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.SMSBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
					innerMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
			}
		}
		private static System.Net.Mail.MailMessage GenericOrderEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
		{
			System.Net.Mail.MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			System.Net.Mail.MailMessage result;
			if (emailTemplate == null)
			{
				result = null;
			}
			else
			{
				emailTemplate.Subject = Messenger.GenericOrderMessageFormatter(settings, username, emailTemplate.Subject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				emailTemplate.Body = Messenger.GenericOrderMessageFormatter(settings, username, emailTemplate.Body, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				result = emailTemplate;
			}
			return result;
		}
		private static string GenericOrderMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username);
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
			stringToFormat = stringToFormat.Replace("$Total$", total.ToString("F"));
			stringToFormat = stringToFormat.Replace("$Memo$", memo);
			stringToFormat = stringToFormat.Replace("$Shipping_Type$", shippingType);
			stringToFormat = stringToFormat.Replace("$Shipping_Name$", shippingName);
			stringToFormat = stringToFormat.Replace("$Shipping_Addr$", shippingAddress);
			stringToFormat = stringToFormat.Replace("$Shipping_Zip$", shippingZip);
			stringToFormat = stringToFormat.Replace("$Shipping_Phone$", shippingPhone);
			stringToFormat = stringToFormat.Replace("$Shipping_Cell$", shippingCell);
			stringToFormat = stringToFormat.Replace("$Shipping_Email$", shippingEmail);
			stringToFormat = stringToFormat.Replace("$Shipping_Billno$", shippingBillno);
			stringToFormat = stringToFormat.Replace("$RefundMoney$", refundMoney.ToString("F"));
			stringToFormat = stringToFormat.Replace("$CloseReason$", closeReason);
			return stringToFormat;
		}
	}
}
