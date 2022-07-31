using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace algoritmai
{
    class Program
    {
        static Int64 sk = 0;
        static void Main(string[] args)
        {
            int[] n = new int[500];
            
            for (int i = 0; i < 5; i++)
            {
                int t = 100;
                

                
                Stopwatch watch = Stopwatch.StartNew();
                
                watch.Start();
                F1(ref n, 0, n.Length);
                watch.Stop();

                TimeSpan ts = watch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine("Runtime " + elapsedTime);
                Console.WriteLine(sk);
                sk = 0;
                Array.Resize(ref n, n.Length + t);

                Console.WriteLine(n.Length);

            }
            
           
            
            

            

        }
        //T(n)=2∗T(n/2)+n^3
        public static void F1(ref int[] n,int strInd,int endInd)
        {
            
            sk++;
            int a = endInd - strInd;
            if (a < 2)
            {

                return;

            }
            sk++;

          
            F1(ref n,strInd,strInd + (endInd-strInd) / 2);
            F1(ref n, endInd + (endInd - strInd) / 2, endInd);
            sk += 2;

            for (int j = 0; j <endInd; j++)
            {
                sk++;
                for (int k = 0; k < endInd; k++)
                {
                    sk++;
                    for (int m = 0; m < endInd; m++)
                    {
                        sk++;
                        n[m] += 1;
                    }
                }
            }
            
            
        }
        //T(n)=T(n/7)+ T(n/8)+ n^2
        public static void F2(ref int[] n, int strInd, int endInd)
        {
            int a = endInd - strInd;
            sk++;
            if (a < 8)
            {

                return;

            }
            sk++;
            F2(ref n, strInd, strInd + (endInd - strInd) / 7);
            F2(ref n, endInd + (endInd - strInd) / 8, endInd);
            sk++;
            sk++;
            for (int j = 0; j < endInd; j++)
            {
                sk++;
                for (int k = 0; k < endInd; k++)
                {
                    sk++;

                    n[k] += 1;
                    
                }
            }

        }


        //T(n)=T(n−5)+ T(n−6)+ n
        public static void F3(ref int[] n, int strInd, int endInd)
        {
            int a = endInd - strInd;
            sk++;
            if (a < 6)
            {

                return;

            }
            sk++;
            F2(ref n, strInd, strInd + (endInd - strInd) - 5);
            F2(ref n, endInd + (endInd - strInd) - 6, endInd);
            sk++;
            sk++;
            for (int j = 0; j < endInd; j++)
            {
                sk++;
                n[j] += 1;
            }
        }




    }
}
