using Newtonsoft.Json;
using System;
using System.Data;
namespace Hidistro.UI.Web
{
	public class ConvertTojson : JsonConverter
	{
		public override bool CanConvert(System.Type objectType)
		{
			return typeof(System.Data.DataTable).IsAssignableFrom(objectType);
		}
		public override object ReadJson(JsonReader reader, System.Type objectType)
		{
			throw new System.NotImplementedException();
		}
		public override void WriteJson(JsonWriter writer, object value)
		{
			System.Data.DataTable dataTable = (System.Data.DataTable)value;
			writer.WriteStartArray();
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				writer.WriteStartObject();
				foreach (System.Data.DataColumn dataColumn in dataTable.Columns)
				{
					writer.WritePropertyName(dataColumn.ColumnName);
					writer.WriteValue(dataRow[dataColumn].ToString());
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}
	}
}
