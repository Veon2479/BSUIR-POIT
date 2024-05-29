using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Xml;

namespace _01
{
	public class Field
	{

		private int _pointsCount = 100000;
		double prior1;
		double prior2;
		int start1 = 100, end1 = 740;
		int start2 = -100, end2 = 540;
		int coeff = 440000;
		double coeff1, coeff2;
		const int xOffset = 600;
		private Random _random;

		public Field(double prior1, double prior2)
		{
			_random = new Random();
			this.prior1 = prior1;
			this.prior2 = prior2;
			coeff1 = coeff*prior1;
			coeff2 = coeff*prior2;
		}

		public void Calculate (ref Bitmap bmp, Graphics g)
		{
			var points1 = new int[_pointsCount];
			var points2 = new int[_pointsCount];

			double mx1 = 0, mx2 = 0;
			for (int i = 0; i < _pointsCount; i++)
			{
				points1[i] = _random.Next(start1, end1);
				points2[i] = _random.Next(start2, end2);
				mx1 = mx1 + points1[i];
				mx2 = mx2 + points2[i];
			}
			mx1 = mx1/(double)_pointsCount;
			mx2 = mx2/(double)_pointsCount;

			double sd1 = 0, sd2 = 0;
			double temp;
			for (int i = 0; i < _pointsCount; i++)
			{
				temp = points1[i] - mx1;
				sd1 = sd1 + temp * temp;
				temp = points2[i] - mx2;
				sd2 = sd2 + temp * temp;
			}
			sd1 = Math.Sqrt(sd1/(double)_pointsCount);
			sd2 = Math.Sqrt(sd2/(double)_pointsCount);

			 var dens1 = new double[_pointsCount];
			 var dens2 = new double[_pointsCount];

			dens1[0] = Math.Exp(-0.5 * Math.Pow((- xOffset - mx1) / sd1, 2)) / 
						(double)(sd1 * Math.Sqrt(2 * Math.PI)) * prior1;
			dens2[0] = Math.Exp(-0.5 * Math.Pow((- xOffset - mx2) / sd2, 2)) / 
						(double)(sd2 * Math.Sqrt(2 * Math.PI)) * prior2;
	
			var bluePen = new Pen(Color.Blue, 4);
			var redPen = new Pen(Color.Red, 4);
			int xCenter = 0;
			bool isDivergence = false;
			double p1 = 0, p2 = 0;
			for (int i = 1; i < _pointsCount; i++)
			{
				dens1[i] = Math.Exp(-0.5 * Math.Pow((i - xOffset - mx1) / sd1, 2)) / 
							(double)(sd1 * Math.Sqrt(2 * Math.PI)) * prior1;
				dens2[i] = Math.Exp(-0.5 * Math.Pow((i - xOffset - mx2) / sd2, 2)) / 
							(double)(sd2 * Math.Sqrt(2 * Math.PI)) * prior2;
			
				g.DrawLine(bluePen, new Point(i - 1, bmp.Height - (int)(dens1[i - 1] * coeff)),
									new Point(i , bmp.Height - (int)(dens1[i] * coeff)));
				g.DrawLine(redPen, new Point(i - 1, bmp.Height - (int)(dens2[i - 1] * coeff)),
									new Point(i, bmp.Height - (int)(dens2[i] * coeff)));

/*				if (Math.Abs((int)(dens1[i] * coeff) - (int)(dens2[i] * coeff)) >  bmp.Height/200 && isDivergence)
					g.DrawLine(new Pen(Color.Green), new Point(i,0), new Point(i, bmp.Height));
				if (Math.Abs((int)(dens1[i] * coeff) - (int)(dens2[i] * coeff)) < 3 && isDivergence)
					g.DrawLine(new Pen(Color.White), new Point(i,0), new Point(i, bmp.Height/2));
*/
				if (Math.Abs((dens1[i] * coeff) - (dens2[i] * coeff)) > bmp.Height/200)
					isDivergence= true;
				if (Math.Abs((int)(dens1[i] * coeff) - (int)(dens2[i] * coeff)) < 3 &&//Math.Abs(dens1[i - 1] * coeff - dens2[i - 1] * coeff)  &&
					isDivergence && 
					xCenter == 0)
					xCenter = i;//(int)(i * bmp.Width / (double)_pointsCount);

				if (xCenter == 0)
					p1 = p1 + dens2[i];
				else
					p2 = p2 + dens1[i];
			}

			var blackPen = new Pen(Color.Black, 4);
			//g.DrawLine(blackPen, new Point(xCenter, bmp.Height/2), new Point(xCenter, bmp.Height));
			g.DrawLine(blackPen, new Point(xCenter, 0), new Point(xCenter, bmp.Height));
			
			//g.DrawLine(blackPen, new Point(0, 0), new Point(xCenter, bmp.Height));

			var font = new Font("Times New Roman", 14);
			var blueBrush = new SolidBrush(Color.Blue);
			var redBrush = new SolidBrush(Color.Red);
			var blackBrush = new SolidBrush(Color.Black);
			g.DrawString("P(C2)p(Xm/C2)", font, blueBrush, new Point(0, bmp.Height/30));
			g.DrawString("P(C1)p(Xm/C1)", font, redBrush, new Point(0, 0));
			g.DrawString($"P  = {p1+p2}", font, blackBrush, new Point(0, 2*bmp.Height/30));
			g.DrawString($"P(Wrong Accept)  = {p1}", font, blackBrush, new Point(0, 3*bmp.Height/30));
			g.DrawString($"P(Wrong Decline) = {p2}", font, blackBrush, new Point(0, 4*bmp.Height/30));
		}
	}
}
