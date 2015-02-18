using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Common.ExtensionMethods
{
    public static class ImageExtensions
    {
        public const string TEMP_COMPRESSED_FOLDER = "COMPRESSED_IMAGES";

        public static string COMPRESSED_IMAGE_FILE_PATH = Path.GetTempPath() + "CompressedPassportImage.bmp";

        public static BitmapSource ToBitmapSource(this MediaTypeNames.Image source)
        {
            var bitmap = new Bitmap(source);

            var bitSrc = bitmap.ToBitmapSource();

            bitmap.Dispose();

            return bitSrc;
        }

        public static BitmapSource CreateThumbnails(Bitmap bitmapImage, string path)
        {
            using (var thumbnail = bitmapImage.GetThumbnailImage(320/*width*/, 450/*height*/, null, IntPtr.Zero))
            {
                thumbnail.Save(path);
                return ToBitmapSource(thumbnail);
            }
        }




        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            var hBitmap = source.GetHbitmap();

            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
               // NativeMethods.DeleteObject(hBitmap);
            }
        }

        /// <summary>
        /// FxCop requires all Marshalled functions to be in a class called NativeMethods.
        /// </summary>
        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }


        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        //public static BitmapImage ConvertBitmap2BitmapImage(this Bitmap bitmap)
        //{
        //    byte[] bytes = null;
        //    try
        //    {
        //        bytes = (byte[])TypeDescriptor.GetConverter(bitmap).ConvertTo(bitmap, typeof(byte[]));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    BitmapImage biImg = new BitmapImage();
        //    if (bytes != null)
        //    {
        //        MemoryStream ms = new MemoryStream(bytes);
        //        biImg.BeginInit();
        //        biImg.StreamSource = ms;
        //        biImg.EndInit();
        //    }
        //    return biImg;
        //}

        public static Bitmap ConvertBitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bmp = new Bitmap(outStream);
                return bmp;
            }
        }

        public static void DeleteImages(List<string> imgPaths)
        {
            string tempPath = System.IO.Path.GetTempPath();
            foreach (var path in imgPaths)
            {
                if (File.Exists(tempPath + path))
                    File.Delete(tempPath + path);
            }
            imgPaths.Clear();
        }


        public static Bitmap ImageSourceToBitmap(BitmapSource source)
        {

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(outStream);
                outStream.Seek(0, SeekOrigin.Begin);
                return new Bitmap(outStream);
            }
        }

        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }


        public static byte[] ToPhotographByteArray(this Bitmap image, ImageFormat format)
        {


            Bitmap cloneImage = DeepCopyBitmap(image);
            using (MemoryStream ms = new MemoryStream())
            {
                cloneImage.Save(ms, format);
                return ms.ToArray();
            }


        }

        public static Bitmap DeepCopyBitmap(Bitmap oldImage)
        {
            BitmapData oldBitmapData =
                oldImage.LockBits(
                    new Rectangle(new System.Drawing.Point(), oldImage.Size),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);

            int byteCount = oldBitmapData.Stride * oldImage.Height;
            byte[] bitmapBytes = new byte[byteCount];
            Marshal.Copy(oldBitmapData.Scan0, bitmapBytes, 0, byteCount);
            oldImage.UnlockBits(oldBitmapData);

            Bitmap newImage = new Bitmap(oldImage.Width, oldImage.Height);

            BitmapData newBitmapData =
                newImage.LockBits(
                    new Rectangle(new System.Drawing.Point(), newImage.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
            Marshal.Copy(bitmapBytes, 0, newBitmapData.Scan0, bitmapBytes.Length);
            newImage.UnlockBits(newBitmapData);

            return newImage;
        }



        public static byte[] ToByteArray(this Bitmap image, ImageFormat format)
        {
            try
            {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, format);
                        return ms.ToArray();
                    }
            }
            catch( Exception ex)
            {
 
            }
        return new byte[1];        }


        public static Bitmap BytetoBitmap(string base64String,ImageFormat format,string filepath)
        {
            Bitmap img;

            byte[] decodedData = Decode(base64String);
            using (MemoryStream ms = new MemoryStream(decodedData))
            {
                img = (Bitmap)Image.FromStream(ms);
                img.Save(filepath, format);
                
            }
            return img;
        }

       
        public static  byte[] Decode(string returnvalue)
        {
            byte[] getbytes = System.Convert.FromBase64String(returnvalue);
            // byte[] getbytes = UTF8Encoding.UTF8.GetBytes(returnvalue);
            string test = System.Text.Encoding.UTF8.GetString(getbytes);
            byte[] final = Convert.FromBase64String(test);
            return final;

        }

        public static Bitmap ToBitmap(this byte[] imageString)
        {
            if (imageString.IsCollectionValid())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Read(imageString, 0, imageString.Length);
                    using (Image img = Image.FromStream(new MemoryStream(imageString)))
                    {
                        Bitmap bitImage = new Bitmap(img);
                        return bitImage;
                    }
                }
            }
            return null;
        }

        //TODO:Implementation to be changed
        public static void CompressBitmapImage(string[] imagesPath, string[] compressedImagesPath)
        {
            int imageCode = 0;
            foreach (var path in imagesPath)
            {
                using (Bitmap bmp = new Bitmap(path))
                {
                    ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 25L);
                    if (!Directory.Exists(System.IO.Path.GetTempPath() + TEMP_COMPRESSED_FOLDER))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + TEMP_COMPRESSED_FOLDER);
                    }
                    bmp.Save(compressedImagesPath[imageCode], jpegCodec, encoderParams);
                }
                imageCode++;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        //TODO:Implementation to be changed
    }
}
