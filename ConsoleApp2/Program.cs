using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream file = new FileStream("result.bmp", FileMode.Create, FileAccess.Write))
            {
                //Sudaromas bmp formato monochrominis 1000x1000 taškų paveikslėlis
                file.Write(
                new byte[62]
                {
                    //Antraštė
                    0x42, 0x4d,
                    0x3e, 0xf4, 0x1, 0x0,
                    0x0, 0x0, 0x0, 0x0,
                    0x3e, 0x0, 0x0, 0x0,
                    //Antraštės informacija
                    0x28, 0x0, 0x0, 0x0,
                    0xe8, 0x3, 0x0, 0x0,
                    0xe8, 0x3, 0x0, 0x0,
                    0x1, 0x0,
                    0x1, 0x0,
                    0x0, 0x0, 0x0, 0x0,
                    0x0, 0xf4, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0,
                    //Spalvų lentelė 
                    0xff, 0xff, 0xff, 0x0,
                    0x0, 0x0, 0x0, 0x0
                });
                //Suskaičiuojame bmp paveikslėlio eilutės duomenų kiekį baitais (4 kartotinis) 1000 bit / 32 bit = 31,25 tokiu atveju eilutei reikės 32 baitų po 4 
                int l = 128;
                //Apibrėžiame taškų masyvą. Masyvo pirmo taško spalvą atitiks masyvo pirmo bito reikšmė
                var t = new byte[1000 * l];
                //Paišome kvadratą 128x128 taškų, kurio kairys apatinis kampas sutampa su bmp paveikslėlio apatiniu kairiu kampu.
                int triang_lenumber = l / 6;
                int max = 8;
                int deph = 3;
                Stack<int> limiter = new Stack<int>();
                limiter.Push(0);
                t = TriangleRecursive(l, ref t, triang_lenumber, 1000, max, 0.33, 0.33, ref limiter, 0, 0, deph);
                file.Write(t);
                file.Close();
            }
        }

        public static byte[] TriangleRecursive(int line_length, ref byte[] matrix, int deduction, int scale, int limit, double modx, double mody, ref Stack<int> limiter, int addx, int addy, int depth)
        {
            limiter.Push(limiter.Peek() + 1);
            if (limiter.Peek() >= depth)
            {
                limiter.Pop();
                return matrix;
            }
            int ls = line_length;
            int triang_lenumber = deduction;
            int count = 0;
            for (int i = ((int)(scale * mody)) + addy; i < ((int)(scale * (mody * 2))) + addy; i++)
            {
                for (int j = i * ls + ((int)(ls * modx)) + addx + triang_lenumber; j < i * ls + ((int)(ls * (2 * modx))) + addx - triang_lenumber; j++)
                {
                    matrix[j] = 0xFF;
                }
                if (count != limit)
                    count++;
                else
                {
                    count = 0;
                    triang_lenumber--;
                }
            }
            if (limiter.Peek() < depth)
            {

                //limiter.Push(limiter.Peek() + 1);
                matrix = TriangleRecursive(ls, ref matrix, deduction / 2, scale, limit, modx / 2, mody * mody, ref limiter, addx, addy, depth);
            }
            if (limiter.Peek() < depth)
            {

                //limiter.Push(limiter.Peek() + 1);
                matrix = TriangleRecursive(ls, ref matrix, deduction / 2, scale, limit, modx / 2, mody * mody, ref limiter, ls / 2, addy, depth);
            }
            if (limiter.Peek() < depth)
            {

                //limiter.Push(limiter.Peek() + 1);
                matrix = TriangleRecursive(ls, ref matrix, deduction / 2, scale, limit, modx / 2, mody * mody, ref limiter, addx, scale * 2 / 3, depth);
            }
            if (limiter.Peek() < depth)
            {

                //limiter.Push(limiter.Peek() + 1);
                matrix = TriangleRecursive(ls, ref matrix, deduction / 2, scale, limit, modx / 2, mody * mody, ref limiter, ls / 2, scale * 2 / 3, depth);
            }

            return matrix;
        }
    }
}
