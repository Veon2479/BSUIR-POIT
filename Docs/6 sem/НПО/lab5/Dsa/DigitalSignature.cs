using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsa
{
    public struct Xyd
    {
        public long x;
        public long y;
        public long d;
    }

    internal class DigitalSignature
    {
        /// <summary>
        /// input parameter
        /// </summary>
        public long p;
        public long q;
        private long k;
        private long x;
        public long h;
        /// <summary>
        /// counted
        /// </summary>
        public long y;
        public long g;
        public static bool IsPrime(long a)
        {
            if (a == 2)
                return true;
            if (a % 2 == 0 || a == 1)
                return false;
            long b = 3;
            while (a % b != 0)
            {
                b += 2;
            }
            return a == b;
        }
        public string CheckCheckParameters(string yText, string pText, string qText, string hText)
        {
            string Message = ParseParametersCheck(yText, pText, qText, hText);
            if(Message == "")
            {
                Message = CheckMinamal();
            }
            return Message;
        }

        public string CheckSignParameters(string kText, string xText, string pText, string qText, string hText)
        {
            string Message = ParseParametersSign(kText, xText, pText, qText, hText);
            if (Message == "")
            {
                Message = CheckSign();
            }
            return Message;
        }
        /// <summary>
        /// assigns fields of class
        /// </summary>
        /// <param name="pText"></param>
        /// <param name="qText"></param>
        /// <param name="kText"></param>
        /// <param name="hText"></param>
        /// <param name="xText"></param>
        /// <returns>an empty string if all parameters are valid</returns>
        /// 

        private string ParseParametersMinimal(string pText, string qText, string hText)
        {
            string Message = "";
            if (!Int64.TryParse(pText, out p))
            {
                Message = "p не числовое значение";
            }
            else if (!Int64.TryParse(qText, out q))
            {
                Message = "q не числовое значение";
            }
            else if (!Int64.TryParse(hText, out h))
            {
                Message = "h не числовое значение";
            }
            return Message;
        }

        private string ParseParametersCheck(string yText, string pText, string qText, string hText)
        {
            string Message = ParseParametersMinimal(pText, qText, hText);
            if(Message == "")
            {
                if(!Int64.TryParse(yText, out y))
                {
                    Message = "y не числовое значение";
                }
            }
            return Message;
        }
        private string ParseParametersSign(string kText, string xText, string pText, string qText, string hText)
        {
            string Message = ParseParametersMinimal(pText, qText, hText);
            if (Message == "")
            {
                if (!Int64.TryParse(kText, out k))
                {
                    Message = "k не числовое значение";
                }
                else if (!Int64.TryParse(xText, out x))
                {
                    Message = "x не числовое значение";
                }
            }
            return Message;
        }

        private string CheckMinamal()
        {
            string Message = "";

            bool correct = IsPrime(q);
            if (!correct)
            {
                Message = "q не простое число";
            }
            else if(!(correct = IsPrime(p)))
            {
                Message = "p не простое число";
            }
            else if (!(correct = (p - 1) % q == 0))
            {
                Message = "q не является делителем (p - 1)";
            }
            else if (!(correct = h > 1 && h < (p - 1)))
            {
                Message = "h не в интервале (1, p - 1)";
            }
            else
            {
                this.g = FastExponentiation(h, (p - 1) / q, p);
                if (!(correct = g > 1))
                {
                    Message = "g <= 1";
                }
            }
            return Message;
        }

        private string CheckSign()
        {
            string Message = CheckMinamal();
            bool correct;
            if(Message == "")
            {
                if (!(correct = x > 0 && x < q))
                {
                    Message = "x не в интервале (0, q)";
                }
                else if (!(correct = k > 0 && k < q))
                {
                    Message = "k не в интервале (0, q)";
                }
                else
                {
                    y = FastExponentiation(this.g, x, p);
                }
            }
            return Message;
        }

        public long HashCalculate(byte[] byteMessage)
        {
            long hash = 100;
            foreach(byte b in byteMessage)
            {
                hash = (hash + b) * (hash + b) % this.q;
            }
            return hash;
        }
        public (long r, long s, long hash) SignHash(string plainText)
        {
            long hash = HashCalculate(Encoding.UTF8.GetBytes(plainText));
            long r = FastExponentiation(g, k, p) % q;
            long s = (FastExponentiation(k, q -2, q) * (hash + x*r)) % q;
            return (r, s, hash);
        }
        public string AddSign(long r, long s)
        {
            return String.Concat("\r\n", r.ToString(), "\t", s.ToString());
        }
        private (long r, long s) ExtractSign(ref string plainText)
        {
            long r = 0;
            long s = 0;
            string sString;
            string rString;
            int start = plainText.LastIndexOf("\r\n", StringComparison.Ordinal);
            if(start != -1)
            {
                string rs = plainText.Substring(start + 2, plainText.Length - start - 2);
                int sStart = rs.IndexOf("\t", StringComparison.Ordinal) + 1;
                if(sStart != -1 && sStart != 0)
                {
                    rString  = rs.Substring(0, sStart - 1);
                    sString = rs.Substring(sStart, rs.Length - sStart);
                    long.TryParse(rString, out r);
                    long.TryParse(sString, out s);
                    plainText = plainText.Remove(start, plainText.Length - start);
                }
            }
            return (r, s);
        }
        public (long r, long s, long v, long w, long hash) CountCheckSign(string plainText)
        {
            (long r, long s) = ExtractSign(ref plainText);
            long w = 0;
            long hash = 0;
            long v = 0;
            if (r != 0 && s != 0)
            {
                w = FastExponentiation(s, q - 2, q);
                hash = HashCalculate(Encoding.UTF8.GetBytes(plainText));
                long u1 = hash * w % q;
                long u2 = r * w % q;
                v = ((FastExponentiation(g, u1, p) * FastExponentiation(y, u2, p)) % p) % q;
            }
            return (r, s, v, w, hash);
        }
        public static Xyd ExtendedEuclidian(long a, long b)
        {
            Xyd xyd0 = new Xyd();
            Xyd xyd1 = new Xyd();

            xyd0.d = a;
            xyd1.d = b;

            xyd0.x = xyd1.y = 0;
            xyd0.y = xyd1.x = 1;

            long q;
            long d2;
            long x2;
            long y2;
            while (xyd1.d > 1)
            {
                q = xyd0.d / xyd1.d;
                d2 = xyd0.d % xyd1.d;

                x2 = xyd0.x - q * (xyd1.x);
                y2 = xyd0.y - q * (xyd1.y);

                xyd0.d = xyd1.d;
                xyd1.d = d2;

                xyd0.x = xyd1.x;
                xyd1.x = x2;

                xyd0.y = xyd1.y;
                xyd1.y = y2;
            }
            return xyd1;
        }
        public static long FastExponentiation(long baseNumber, long exponent, long modulus)
        {
            long a = baseNumber;
            long z = exponent;
            long result = 1;

            while (z != 0)
            {
                while (z % 2 == 0)
                {
                    z = z / 2;
                    a = (a * a) % modulus;
                }
                z--;
                result = (result * a) % modulus;
            }
            return result;
        }

    }
}
