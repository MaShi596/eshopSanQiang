using Hidistro.Membership.Context;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
namespace Hidistro.UI.Web
{
	public class VerifyCodeImage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				base.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
				string text = Hidistro.Membership.Context.HiContext.Current.CreateVerifyCode(4);
				int num = 45;
				int num2 = text.Length * 20;
				Bitmap bitmap = new Bitmap(num2 - 3, 27);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.Clear(Color.AliceBlue);
				graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, bitmap.Width - 1, bitmap.Height - 3);
				System.Random random = new System.Random();
				Pen pen = new Pen(Color.LightGray, 0f);
				for (int i = 0; i < 50; i++)
				{
					int x = random.Next(0, bitmap.Width);
					int y = random.Next(0, bitmap.Height);
					graphics.DrawRectangle(pen, x, y, 1, 1);
				}
				char[] array = text.ToCharArray();
				StringFormat stringFormat = new StringFormat(StringFormatFlags.NoClip);
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				Color[] array2 = new Color[]
				{
					Color.Black,
					Color.Red,
					Color.DarkBlue,
					Color.Green,
					Color.Brown,
					Color.DarkCyan,
					Color.Purple,
					Color.DarkGreen
				};
				for (int j = 0; j < array.Length; j++)
				{
					int num3 = random.Next(7);
					random.Next(4);
					Font font = new Font("Microsoft Sans Serif", 17f, FontStyle.Bold);
					Brush brush = new SolidBrush(array2[num3]);
					Point point = new Point(14, 11);
					float num4 = (float)random.Next(-num, num);
					graphics.TranslateTransform((float)point.X, (float)point.Y);
					graphics.RotateTransform(num4);
					graphics.DrawString(array[j].ToString(), font, brush, 1f, 1f, stringFormat);
					graphics.RotateTransform(-num4);
					graphics.TranslateTransform(2f, (float)(-(float)point.Y));
				}
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				bitmap.Save(memoryStream, ImageFormat.Gif);
				base.Response.ClearContent();
				base.Response.ContentType = "image/gif";
				base.Response.BinaryWrite(memoryStream.ToArray());
				graphics.Dispose();
				bitmap.Dispose();
			}
			catch
			{
			}
		}
	}
}
