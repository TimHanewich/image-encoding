using System;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace ImageEncoding
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            //Encode
            ImageEncoder ie = new ImageEncoder();
            ie.MaxDimension = 8000;
            Stream te = System.IO.File.OpenRead(@"C:\Users\timh\Downloads\halo.mkv");
            MemoryStream[] imgs = ie.Encode(te);
            for (int t = 0; t < imgs.Length; t++)
            {
                MemoryStream msts = imgs[t];
                string path = Path.Combine(@"C:\Users\timh\Downloads\drops", t.ToString() + ".png");
                Stream twt = System.IO.File.Create(path);
                msts.CopyTo(twt);
                twt.Close();
            }
            
            //Decode
            // string[] paths = System.IO.Directory.GetFiles(@"C:\Users\timh\Downloads\drops");
            // List<Stream> ToPass = new List<Stream>();
            // foreach (string path in ImageEncoder.SortPathsByNumber(paths))
            // {
            //     Stream s = System.IO.File.OpenRead(path);
            //     Console.WriteLine(path);
            //     ToPass.Add(s);
            // }

            // MemoryStream ms = ImageEncoder.Decode(ToPass.ToArray());
            // ms.Seek(0, SeekOrigin.Begin);
            
            // Stream pwt = System.IO.File.OpenWrite(@"C:\Users\timh\Downloads\spearsD.mp3");
            // ms.CopyTo(pwt);
            // pwt.Close();
            
        }
    }
}