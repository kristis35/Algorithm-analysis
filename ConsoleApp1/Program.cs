using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
	internal class Renderer
	{
		enum GenerationMask
		{
			None = 0,
			Top = 1 << 0,
			Left = 1 << 1,
			Bottom = 1 << 2,
			Right = 1 << 3,
			Mask_All = Top | Left | Right | Bottom
		}

		static void SimpleShape(Renderer render, double X, double Y, double Size, GenerationMask Mask)
		{
			double Portion = Size / 5;

			if ((Mask & GenerationMask.Top) != 0)
				render.DrawLine(X - Portion * 0.5, Y + Portion * 2.5, X + Portion * 0.5, Y + Portion * 2.5, 0.5, 0); // Upper square top line

			render.DrawLine(X - Portion * 0.5, Y + Portion * 1.5, X - Portion * 0.5, Y + Portion * 2.5, 0.5, 0); // Upper square left line
			render.DrawLine(X + Portion * 0.5, Y + Portion * 1.5, X + Portion * 0.5, Y + Portion * 2.5, 0.5, 0); // Upper square right line

			render.DrawLine(X - Portion * 1.5, Y + Portion * 1.5, X - Portion * 0.5, Y + Portion * 1.5, 0.5, 0); // Upper mid left square top line
			render.DrawLine(X - Portion * 1.5, Y + Portion * 1.5, X - Portion * 1.5, Y + Portion * 0.5, 0.5, 0); // Upper mid left square left line

			render.DrawLine(X + Portion * 0.5, Y + Portion * 1.5, X + Portion * 1.5, Y + Portion * 1.5, 0.5, 0); // Upper mid right square top line
			render.DrawLine(X + Portion * 1.5, Y + Portion * 1.5, X + Portion * 1.5, Y + Portion * 0.5, 0.5, 0); // Upper mid right square right line

			render.DrawLine(X + Portion * 1.5, Y + Portion * 0.5, X + Portion * 2.5, Y + Portion * 0.5, 0.5, 0); // Mid right square top line

			if ((Mask & GenerationMask.Right) != 0)
				render.DrawLine(X + Portion * 2.5, Y + Portion * 0.5, X + Portion * 2.5, Y - Portion * 0.5, 0.5, 0); // Mid right square right line

			render.DrawLine(X + Portion * 2.5, Y - Portion * 0.5, X + Portion * 1.5, Y - Portion * 0.5, 0.5, 0); // Mid right square bottom line

			render.DrawLine(X - Portion * 1.5, Y + Portion * 0.5, X - Portion * 2.5, Y + Portion * 0.5, 0.5, 0); // Mid left square top line

			if ((Mask & GenerationMask.Left) != 0)
				render.DrawLine(X - Portion * 2.5, Y + Portion * 0.5, X - Portion * 2.5, Y - Portion * 0.5, 0.5, 0); // Mid left square left line

			render.DrawLine(X - Portion * 2.5, Y - Portion * 0.5, X - Portion * 1.5, Y - Portion * 0.5, 0.5, 0); // Mid left square bottom line

			render.DrawLine(X - Portion * 1.5, Y - Portion * 0.5, X - Portion * 1.5, Y - Portion * 1.5, 0.5, 0); // Lower mid left square left line
			render.DrawLine(X - Portion * 1.5, Y - Portion * 1.5, X - Portion * 0.5, Y - Portion * 1.5, 0.5, 0); // Lower mid left square bottom line

			render.DrawLine(X + Portion * 1.5, Y - Portion * 0.5, X + Portion * 1.5, Y - Portion * 1.5, 0.5, 0); // Lower mid right square right line
			render.DrawLine(X + Portion * 0.5, Y - Portion * 1.5, X + Portion * 1.5, Y - Portion * 1.5, 0.5, 0); // Lower mid right square bottom line

			render.DrawLine(X - Portion * 0.5, Y - Portion * 1.5, X - Portion * 0.5, Y - Portion * 2.5, 0.5, 0); // Lower square left line

			if ((Mask & GenerationMask.Bottom) != 0)
				render.DrawLine(X - Portion * 0.5, Y - Portion * 2.5, X + Portion * 0.5, Y - Portion * 2.5, 0.5, 0); // Lower square bottom line

			render.DrawLine(X + Portion * 0.5, Y - Portion * 1.5, X + Portion * 0.5, Y - Portion * 2.5, 0.5, 0); // Lower square left line
		}

		static void Recursive(Renderer render, double X, double Y, double Size, uint ParentTo, GenerationMask ConnectionMask = GenerationMask.Mask_All)
		{
			if (ParentTo == 0)
			{
				SimpleShape(render, X, Y, Size, ConnectionMask);
				return;
			}

			double CurrentIteration = (1 << (byte)ParentTo) * 5d + ((1 << (byte)ParentTo) - 1) * 3d;
			double LastIteration = (1 << (byte)(ParentTo - 1)) * 5d + ((1 << (byte)(ParentTo - 1)) - 1) * 3d;

			double Distance = Size * ((LastIteration / 2d + 1.5d) / CurrentIteration);
			double NewSize = Size * (LastIteration / CurrentIteration);

			SimpleShape(render, X, Y, Size * (5 / CurrentIteration), GenerationMask.None);

			if ((ConnectionMask & GenerationMask.Right) == 0)
				Recursive(render, X + Distance, Y, NewSize, ParentTo - 1, GenerationMask.Top | GenerationMask.Bottom);
			else
				Recursive(render, X + Distance, Y, NewSize, ParentTo - 1, GenerationMask.Mask_All & ~GenerationMask.Left);

			if ((ConnectionMask & GenerationMask.Left) == 0)
				Recursive(render, X - Distance, Y, NewSize, ParentTo - 1, GenerationMask.Top | GenerationMask.Bottom);
			else
				Recursive(render, X - Distance, Y, NewSize, ParentTo - 1, GenerationMask.Mask_All & ~GenerationMask.Right);

			if ((ConnectionMask & GenerationMask.Top) == 0)
				Recursive(render, X, Y + Distance, NewSize, ParentTo - 1, GenerationMask.Right | GenerationMask.Left);
			else
				Recursive(render, X, Y + Distance, NewSize, ParentTo - 1, GenerationMask.Mask_All & ~GenerationMask.Bottom);

			if ((ConnectionMask & GenerationMask.Bottom) == 0)
				Recursive(render, X, Y - Distance, NewSize, ParentTo - 1, GenerationMask.Right | GenerationMask.Left);
			else
				Recursive(render, X, Y - Distance, NewSize, ParentTo - 1, GenerationMask.Mask_All & ~GenerationMask.Top);
		}

		public void DrawLine(double X0, double Y0, double X1, double Y1, double Precision = 0.5, uint Color = 0)
		{
			double Length = Math.Sqrt(Math.Pow(X0 - X1, 2) + Math.Pow(Y0 - Y1, 2));

			double XStep = (X1 - X0) / (Length / Precision);
			double YStep = (Y1 - Y0) / (Length / Precision);

			double XRun = X0;
			double YRun = Y0;
			for (double i = 0; i < Length; i += Precision)
			{
				XRun += XStep;
				YRun += YStep;

				SetPixel(XRun, YRun, Color);
			}
		}

		private void SetPixel(double X, double Y, uint Color)
		{
			int Pixel = GetPixel(X, Y);
			if (Pixel < 0)
				return;

			Buffer[Pixel] = Color;
		}

		private int GetPixel(double X, double Y)
		{
			int Pixel = ((int)Math.Round(Y) * Width) + (int)Math.Round(X);
			if (Pixel > Buffer.Length)
				return -1;

			if (X < 0)
				return -1;
			else if (X > Width)
				return -1;

			return Pixel;
		}

		private readonly uint[] Buffer;
		private readonly ushort Width;
		private readonly ushort Height;
		private readonly string OutputName;


		static void Main(string[] args)
		{

			





		}



	}

	

}

