using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;

using Microsoft.Win32;

using JabbR.Models;

namespace JabbR
{
	public partial class GetUserImage : System.Web.UI.Page
	{
		private const string pictureDirPath = @"\\Security\Picture IDs\Photos";

		private static Dictionary<string, Image> _cachedImages;

		static GetUserImage()
		{
			_cachedImages = new Dictionary<string, Image>();
		}

		/// <summary>
		/// Handles the Load event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(Request.QueryString["id"]))
				return;

			string employeeId = Request.QueryString["id"];
			bool error = false;
			Image thumbnail = null;

			if (_cachedImages.ContainsKey(employeeId))
			{
				thumbnail = _cachedImages[employeeId];
			}
			else
			{
				using (var context = new DirectoryContext())
				{
					//var wic = WindowsIdentity.Impersonate(System.IntPtr.Zero);

					var pictureDir = new DirectoryInfo(pictureDirPath);

					var pictureMatches = pictureDir.GetFiles("*" + employeeId + ".jpg", SearchOption.TopDirectoryOnly);

					if (pictureDir.Exists && pictureMatches.Any())
					{
						var pictureFile = pictureMatches.First();

						thumbnail = GenerateThumbnail(GetFileBytes(pictureFile));
						_cachedImages[employeeId] = thumbnail;
					}
					else
					{
						error = true;
					}

					//wic.Undo();
				}
			}

			if (!error && thumbnail != null)
			{
				var codec = ImageCodecInfo.GetImageEncoders().Where(ici => ici.FormatDescription == "JPEG").FirstOrDefault();

				if (codec != null)
				{
					var codecParams = new EncoderParameters(1);
					codecParams.Param[0] = new EncoderParameter(Encoder.Quality, 76L);

					Response.ContentType = "image/jpeg";
					Response.AppendHeader("Content-Disposition", "filename=\"thumb-" + employeeId + ".jpg\"");

					thumbnail.Save(Response.OutputStream, codec, codecParams);
				}
			}

			if (error)
			{
				//Response.ContentType = "image/png";
				//Response.AppendHeader("Content-Disposition", "filename=\"invalid-attachment.png\"");

				// Must use an intermediate MemoryStream here
				// Without it, i.e. trying to save directly to the output like so:
				// Properties.Resources.invalid_attachment.Save(Response.OutputStream, ImageFormat.Png);
				// generates a generic GDI+ error.  It only happens when trying to save PNGs... no idea why
				// See:  http://www.hanselman.com/blog/TheWeeklySourceCode50ALittleOnAGenericErrorOccurredInGDIAndTroubleGeneratingImagesOnWithASPNET.aspx
				//imageStream = new MemoryStream();
				//Properties.Resources.invalid_attachment.Save(imageStream, ImageFormat.Png);
				//imageStream.WriteTo(Response.OutputStream);

				//imageStream.Close();
				//imageStream.Dispose();
			}

			Response.End();
		}

		private Image GenerateThumbnail(byte[] imageData)
		{
			if (imageData == null)
				return null;

			Image thumbnail = null;

			try
			{
				// Open memory stream, load byte array into it, and convert it into a stream
				MemoryStream imageStream = new MemoryStream(imageData);
				//Image.GetThumbnailImageAbort dummyCallBack = new Image.GetThumbnailImageAbort(ThumbnailCallback);
				Image image = Image.FromStream(imageStream);

				// Get scaled thumbnail dimensions 
				int[] thumbDimensions = ScaleImageResolution(image.Width, image.Height);

				thumbnail = new Bitmap(thumbDimensions[0], thumbDimensions[1]);
				
				var g = Graphics.FromImage(thumbnail);
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

				// Make thumbnail from original image
				//thumbnail = image.GetThumbnailImage(thumbDimensions[0], thumbDimensions[1], dummyCallBack, IntPtr.Zero);
				g.DrawImage(image, 0, 0, thumbDimensions[0], thumbDimensions[1]);
			}
			catch
			{
				thumbnail = null;
			}

			return thumbnail;
		}
		private int[] ScaleImageResolution(int OldWidth, int OldHeight)
		{
			// Larger thumbnail dimension
			const double thumbSize = 32.0d;
			// Index 0 is Width, Index 1 is Height
			int[] newSizes = new int[2];

			if (OldHeight > OldWidth)
			{
				newSizes[1] = (int)thumbSize;
				newSizes[0] = (int)(OldWidth * thumbSize / OldHeight);
			}
			else
			{
				newSizes[0] = (int)thumbSize;
				newSizes[1] = (int)(OldHeight * thumbSize / OldWidth);
			}
			return newSizes;
		}
		private bool ThumbnailCallback()
		{
			return false;
		}
		private byte[] GetFileBytes(FileInfo file)
		{
			byte[] binaryData = null;

			if (file.Exists)
			{
				BinaryReader dataReader = new BinaryReader(file.OpenRead());

				binaryData = dataReader.ReadBytes((int)file.Length);

				// This will implicitly close the underlying FileStream, too
				dataReader.Close();
			}

			return binaryData;
		}
	}
}