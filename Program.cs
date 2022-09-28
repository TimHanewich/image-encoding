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

            //Read from
            if (0 == 1)
            {
                Stream reading = System.IO.File.OpenRead(@"C:\Users\timh\Downloads\test_t.png");
                Bitmap bm = new Bitmap(reading);
                Color c = bm.GetPixel(0, 0);
                Console.WriteLine("A: " + c.A.ToString());
                Console.WriteLine("R: " + c.R.ToString());
                Console.WriteLine("G: " + c.G.ToString());
                Console.WriteLine("B: " + c.B.ToString());
                Console.ReadLine();
            }
            


            //Save to
            if (0 == 1)
            {
                Bitmap bmtest = new Bitmap(10, 10);
                bmtest.SetPixel(0, 0, Color.FromArgb(255, 100, 100, 100));
                Stream tst = System.IO.File.OpenWrite(@"C:\Users\timh\Downloads\test_t.png");
                bmtest.Save(tst, ImageFormat.Png);
                tst.Close();
                Console.WriteLine("Saved!");
                Console.ReadLine();
            }
            


            


            //Encode
            if (0 == 1)
            {
                ImageEncoder ie = new ImageEncoder();
                Stream te = System.IO.File.OpenRead(@"C:\Users\timh\Downloads\WIN_20220928_14_49_00_Pro.jpg");
                MemoryStream[] imgs = ie.Encode(te);
                for (int t = 0; t < imgs.Length; t++)
                {
                    MemoryStream msts = imgs[t];
                    string path = Path.Combine(@"C:\Users\timh\Downloads\drops", t.ToString() + ".png");
                    Stream twt = System.IO.File.Create(path);
                    msts.CopyTo(twt);
                    twt.Close();
                }
            }
            
            //Decode
            if (0 == 0)
            {
                string[] paths = System.IO.Directory.GetFiles(@"C:\Users\timh\Downloads\drops");
                List<Stream> ToPass = new List<Stream>();
                foreach (string path in paths)
                {
                    Stream s = System.IO.File.OpenRead(path);
                    ToPass.Add(s);
                }

                MemoryStream ms = ImageEncoder.Decode(ToPass.ToArray());
                Console.WriteLine(ms.Length.ToString());
            }
            
        }
    }
}