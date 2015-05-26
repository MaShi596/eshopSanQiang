using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
namespace Hidistro.UI.Web
{
	public class PIC
	{
		private Bitmap _outBmp;
		public Bitmap OutBMP
		{
			get
			{
				return this._outBmp;
			}
		}
		private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
		{
			double num = System.Convert.ToDouble(width);
			double num2 = System.Convert.ToDouble(height);
			double num3 = System.Convert.ToDouble(maxWidth);
			double num4 = System.Convert.ToDouble(maxHeight);
			double num5;
			double num6;
			if (num < num3 && num2 < num4)
			{
				num5 = num;
				num6 = num2;
			}
			else
			{
				if (num / num2 > num3 / num4)
				{
					num5 = (double)maxWidth;
					num6 = num5 * num2 / num;
				}
				else
				{
					num6 = (double)maxHeight;
					num5 = num6 * num / num2;
				}
			}
			return new Size(System.Convert.ToInt32(num5), System.Convert.ToInt32(num6));
		}
		public static void SendSmallImage(string fileName, string newFile, int maxHeight, int maxWidth)
		{
            Image image = null;
            Bitmap bitmap = null;
            Graphics graphics = null;
            try
            {
                image = Image.FromFile(fileName);
                ImageFormat rawFormat = image.RawFormat;
                Size size = NewSize(maxWidth, maxHeight, image.Width, image.Height);
                bitmap = new Bitmap(size.Width, size.Height);
                graphics = Graphics.FromImage(bitmap);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] numArray = new long[] { 100L };
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, numArray);
                encoderParams.Param[0] = parameter;
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                int index = 0;
                while (index < imageEncoders.Length)
                {
                    if (imageEncoders[index].FormatDescription.Equals("JPEG"))
                    {
                        goto Label_00F1;
                    }
                    index++;
                }
                goto Label_00F8;
            Label_00F1:
                encoder = imageEncoders[index];
            Label_00F8:
                if (encoder != null)
                {
                    bitmap.Save(newFile, encoder, encoderParams);
                }
                else
                {
                    bitmap.Save(newFile, rawFormat);
                }
            }
            catch
            {
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }
		}
		public void Dispose()
		{
			if (this._outBmp != null)
			{
				this._outBmp.Dispose();
				this._outBmp = null;
			}
		}
		public void SendSmallImage(string fileName, int maxHeight, int maxWidth)
		{
			Image image = null;
			this._outBmp = null;
			Graphics graphics = null;
			try
			{
				image = Image.FromFile(fileName);
				ImageFormat arg_18_0 = image.RawFormat;
				Size size = PIC.NewSize(maxWidth, maxHeight, image.Width, image.Height);
				this._outBmp = new Bitmap(size.Width, size.Height);
				graphics = Graphics.FromImage(this._outBmp);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			catch
			{
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
			}
		}
		public System.IO.MemoryStream AddImageSignPic(Image img, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
		{
			Graphics graphics = null;
			Image image = null;
			ImageAttributes imageAttributes = null;
			System.IO.MemoryStream result;
			try
			{
				graphics = Graphics.FromImage(img);
				image = new Bitmap(watermarkFilename);
				imageAttributes = new ImageAttributes();
				ColorMap[] map = new ColorMap[]
				{
					new ColorMap
					{
						OldColor = Color.FromArgb(255, 0, 255, 0),
						NewColor = Color.FromArgb(0, 0, 0, 0)
					}
				};
				imageAttributes.SetRemapTable(map, ColorAdjustType.Bitmap);
				float num = 0.5f;
				if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
				{
					num = (float)watermarkTransparency / 10f;
				}
				float[][] array = new float[5][];
				float[][] arg_A1_0 = array;
				int arg_A1_1 = 0;
				float[] array2 = new float[5];
				array2[0] = 1f;
				arg_A1_0[arg_A1_1] = array2;
				float[][] arg_B8_0 = array;
				int arg_B8_1 = 1;
				float[] array3 = new float[5];
				array3[1] = 1f;
				arg_B8_0[arg_B8_1] = array3;
				float[][] arg_CF_0 = array;
				int arg_CF_1 = 2;
				float[] array4 = new float[5];
				array4[2] = 1f;
				arg_CF_0[arg_CF_1] = array4;
				float[][] arg_E3_0 = array;
				int arg_E3_1 = 3;
				float[] array5 = new float[5];
				array5[3] = num;
				arg_E3_0[arg_E3_1] = array5;
				array[4] = new float[]
				{
					0f,
					0f,
					0f,
					0f,
					1f
				};
				float[][] newColorMatrix = array;
				ColorMatrix newColorMatrix2 = new ColorMatrix(newColorMatrix);
				imageAttributes.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
				int x = 0;
				int y = 0;
				switch (watermarkStatus)
				{
				case 1:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.01f);
					break;
				case 2:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.01f);
					break;
				case 3:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.01f);
					break;
				case 4:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 5:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 6:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 7:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				case 8:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				case 9:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				}
				graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo imageCodecInfo = null;
				ImageCodecInfo[] array6 = imageEncoders;
				for (int i = 0; i < array6.Length; i++)
				{
					ImageCodecInfo imageCodecInfo2 = array6[i];
					if (imageCodecInfo2.MimeType.IndexOf("jpeg") > -1)
					{
						imageCodecInfo = imageCodecInfo2;
					}
				}
				EncoderParameters encoderParameters = new EncoderParameters();
				long[] array7 = new long[1];
				if (quality < 0 || quality > 100)
				{
					quality = 80;
				}
				array7[0] = (long)quality;
				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, array7);
				encoderParameters.Param[0] = encoderParameter;
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				if (imageCodecInfo != null)
				{
					img.Save(memoryStream, imageCodecInfo, encoderParameters);
				}
				result = memoryStream;
			}
			catch
			{
				System.IO.MemoryStream memoryStream = null;
				result = memoryStream;
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (img != null)
				{
					img.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (imageAttributes != null)
				{
					imageAttributes.Dispose();
				}
			}
			return result;
		}
	}
}
