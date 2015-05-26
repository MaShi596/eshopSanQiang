using System;
using System.Runtime.Serialization;
using System.Text;
namespace Hidistro.Entities.Sales
{
	[DataContract]
	public class PurchaseOrderTaobaoInfo
	{
		[DataMember]
		public string created
		{
			get;
			set;
		}
		[DataMember]
		public string expire_time
		{
			get;
			set;
		}
		[DataMember]
		public string isPart
		{
			get;
			set;
		}
		[DataMember]
		public string is_delivery
		{
			get;
			set;
		}
		[DataMember]
		public string logi_name
		{
			get;
			set;
		}
		[DataMember]
		public string login_no
		{
			get;
			set;
		}
		[DataMember]
		public string order_id
		{
			get;
			set;
		}
		[DataMember]
		public string status
		{
			get;
			set;
		}
		[DataMember]
		public string time
		{
			get;
			set;
		}
		public PurchaseOrderTaobaoInfo()
		{
			this.created = "false";
			this.expire_time = "0";
			this.isPart = "false";
			this.is_delivery = "false";
			this.logi_name = "";
			this.login_no = "";
			this.order_id = "";
			this.status = "未下单";
			this.time = "";
		}
		public string ToJson()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"order_id\":\"");
			stringBuilder.Append(this.order_id);
			stringBuilder.Append("\",\"created\":\"");
			stringBuilder.Append(this.created);
			stringBuilder.Append("\",\"expire_time\":\"");
			stringBuilder.Append(this.expire_time);
			stringBuilder.Append("\",\"isPart\":\"");
			stringBuilder.Append(this.isPart);
			stringBuilder.Append("\",\"is_delivery\":\"");
			stringBuilder.Append(this.is_delivery);
			stringBuilder.Append("\",\"logi_name\":\"");
			stringBuilder.Append(this.logi_name);
			stringBuilder.Append("\",\"login_no\":\"");
			stringBuilder.Append(this.login_no);
			stringBuilder.Append("\",\"status\":\"");
			stringBuilder.Append(this.status);
			stringBuilder.Append("\",\"time\":\"");
			stringBuilder.Append(this.time);
			stringBuilder.Append("\"}");
			return stringBuilder.ToString();
		}
	}
}
