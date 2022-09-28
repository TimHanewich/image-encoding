using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace ImageEncoding
{
    public class ImageEncoder
    {

        public int MaxDimension {get; set;} //max width/height per image


        public ImageEncoder()
        {
            MaxDimension = 100;
        }
        
        //Returns an image string (png)
        public MemoryStream[] Encode(Stream input)
        {

            //Convert the # of pixels needed
            int byte_count = Convert.ToInt32(input.Length);
            int pixels_needed = Convert.ToInt32(Math.Ceiling(Convert.ToSingle(byte_count / 3)));
            
            //Count the # of images that we need
            int pixels_per_image = MaxDimension * MaxDimension;
            int images_needed = Convert.ToInt32(Math.Ceiling(Convert.ToSingle(Convert.ToSingle(pixels_needed) / Convert.ToSingle(pixels_per_image))));
            
            //Refactor to make it neater
            float pixels_needed_per_image = Convert.ToSingle(pixels_needed) / Convert.ToSingle(images_needed);
            int UseDimension = Convert.ToInt32(Math.Ceiling(Math.Sqrt(pixels_needed_per_image)));

            //Set up stream
            input.Seek(0, SeekOrigin.Begin);

            //Go!
            List<MemoryStream> ToReturn = new List<MemoryStream>();
            Bitmap WorkingOn = new Bitmap(UseDimension, UseDimension);
            int OnX = 0;
            int OnY = 0;
            while (true)
            {

                
                
                bool terminate = false;

                //Collect 3 bytes
                List<int> ThisPixel = new List<int>();
                while (ThisPixel.Count < 3)
                {
                    int nb = input.ReadByte();
                    if (nb == -1)
                    {
                        terminate = true;
                        break;
                    }
                    ThisPixel.Add(nb);
                }

                //Determine the "A-code". The "A" value of the ARGB value indicates something about the data transcribed there.
                //0 = dead cell. Just ignore
                //1 = partial transmission. Only the first byte, R, counts.
                //2 = partial transmission. Only the first two bytes, R and G count.
                //3 = data transcribed normally. All bytes count (R, G, and B)
                int A_code = 3; 
                if (ThisPixel.Count == 0)
                {
                    A_code = 0;
                }
                else if (ThisPixel.Count == 1)
                {
                    A_code = 1;
                }
                else if (ThisPixel.Count == 2)
                {
                    A_code = 2;
                }
                else if (ThisPixel.Count == 3)
                {
                    A_code = 3;
                }

                //If it does not have 4 bytes, make it up, ONLY if there is data there. If there is NOT data there, break altogether
                if (ThisPixel.Count > 0)
                {
                    while (ThisPixel.Count < 3)
                    {
                        ThisPixel.Add(0);
                    }
                }
                
                //Set the pixel, if there is data
                if (ThisPixel.Count > 0)
                {
                    WorkingOn.SetPixel(OnX, OnY, Color.FromArgb(A_code, ThisPixel[0], ThisPixel[1], ThisPixel[2]));
                }

                //Increment the OnX, OnY. or set an entire new image
                if (OnX == (UseDimension - 1)) //We are in the last column and need to step down
                {
                    if (OnY == (UseDimension - 1)) //We are in the last row and need to create a new image entirely
                    {
                        //Save the image
                        MemoryStream ThisImage = new MemoryStream();
                        WorkingOn.Save(ThisImage, ImageFormat.Png);
                        ThisImage.Seek(0, SeekOrigin.Begin);
                        ToReturn.Add(ThisImage);

                        //Create a new working on
                        WorkingOn = new Bitmap(UseDimension, UseDimension);

                        //Reset OnX and OnY
                        OnX = 0;
                        OnY = 0;
                    }
                    else //There is at least 1 more row to go, so step down
                    {
                        OnX = 0;
                        OnY = OnY + 1;
                    }
                }
                else
                {
                    OnX = OnX + 1;
                }

                //If we found it was time to terminate, do it now
                if (terminate)
                {
                    
                    //Do the one final image write (save what we are working on right now to the ToReturn list)
                    MemoryStream ThisImage = new MemoryStream();
                    WorkingOn.Save(ThisImage, ImageFormat.Png);
                    ThisImage.Seek(0, SeekOrigin.Begin);
                    ToReturn.Add(ThisImage);

                    break;
                }
            }


            return ToReturn.ToArray();
        }

    }
}