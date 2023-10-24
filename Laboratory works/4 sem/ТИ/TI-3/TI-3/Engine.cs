using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using Avalonia.Collections;

namespace TI_3;

public class Engine
{
    public enum StateCodes
    {
        ScOk, ScErrPrime, ScErrModulo, ScErrKey
    }

    public StateCodes State { get; private set; } = StateCodes.ScOk;
    
    public bool Encrypt( in byte[] plain, out byte[] ciph, ulong p, ulong q, ulong ks )
    {
        State = StateCodes.ScOk;
        bool result = CheckPrimes(p, q);
        ciph = Array.Empty<byte>();
        if (result)
        {
            ulong fr = (p - 1) * (q - 1);
            result &= CheckSecretKey(ks, fr);
            
            if (result)
            {
                ulong r = p * q;
                ulong kp = ( ExtEuclidean(fr, ks).y + fr ) % fr;
                ushort[] tmp = new ushort[plain.Length];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = (ushort) FastPow(plain[i], kp, r);
                }
                
                ciph = new byte[plain.Length * 2];
                Buffer.BlockCopy(tmp, 0, ciph, 0, ciph.Length);
            }
            
        }
        return result;
    }

    public bool Decrypt(in byte[] ciph, out byte[] plain, ulong r, ulong ks)
    {
        State = StateCodes.ScOk;
        bool result = CheckModulo(r);
        plain = Array.Empty<byte>();
        if (result)
        {
            ushort[] tmp = new ushort[ciph.Length / 2];
            Buffer.BlockCopy(ciph, 0, tmp, 0, ciph.Length);
            plain = new byte[ciph.Length / 2];
            
            for ( int i = 0; i < tmp.Length; i++)
            {
                plain[i] = (byte) FastPow((ulong) tmp[i], ks, r);
            }

        }
        return result;
    }
    
    //computing num^deg by modulo mod
    private ulong FastPow(ulong num, ulong deg, ulong mod )
    {
        ulong result = 1;
        while (deg != 0)
        {
            while (deg % 2 == 0)
            {
                deg /= 2;
                num = (num * num) % mod;
            }
            deg--;
            result = (result * num) % mod;
        }
        return result;
    }
    private bool CheckPrimes(ulong p, ulong q)
    {
        bool result = (p > 1) && (q > 1);
        ulong min = (p > q) ? q : p;
        ulong i = 2;
        while (result && i <= Math.Sqrt(min))
        {
            result &= (p % i != 0) & (q % i != 0);
            i++;
        }
        ulong max = (p > q) ? p : q;
        while (result && i <= Math.Sqrt(max))
        {
            result &= (max % i != 0);
            i++;
        }

        if (!result)
            State = StateCodes.ScErrPrime;
        return result & CheckModulo( p * q );
    }

    private bool CheckModulo(ulong r)
    {
        bool result = r > 1;
        result &= (r <= 0xFF_FF);
        result &= (r > 0xFF);
        if (!result)
            State = StateCodes.ScErrModulo;
        return result;
    }

    private bool CheckSecretKey(ulong ks, ulong fr)
    {
        bool result = ks > 1;
        ulong gcd = ExtEuclidean(ks, fr).d;
        result &= ks < fr;
        result &= (1 == gcd);
        if (!result)
            State = StateCodes.ScErrKey;
        return result;
    }

    /*private (ulong d, ulong x, ulong y) ExtEuclidean(ulong a, ulong b)
    {
    //this **** was given by lector(????), i'm sure, it's not working at all
        (ulong itemOld, ulong itemNew) dTuple = (a, b);
        (ulong itemOld, ulong itemNew) xTuple = (1, 0);
        (ulong itemOld, ulong itemNew) yTuple = (0, 1);
        ulong q = 0;
        while (dTuple.itemNew > 1)
        {
            q = dTuple.itemOld / dTuple.itemNew;
            dTuple = (dTuple.itemNew, dTuple.itemOld % dTuple.itemNew);
            xTuple = (xTuple.itemNew, xTuple.itemOld - q * xTuple.itemNew);
            yTuple = (yTuple.itemNew, yTuple.itemOld - q * yTuple.itemNew);
        }
        Console.WriteLine(dTuple + " " + yTuple + " " + xTuple);
        return (dTuple.itemNew, xTuple.itemNew, yTuple.itemNew);
    }*/

    private (ulong d, ulong x, ulong y) ExtEuclidean(ulong a, ulong b)
    {
        (ulong d, ulong x, ulong y) resTuple = (0, 0, 0);
        if (a == 0)
        {
            resTuple.x = 0;
            resTuple.y = 1;
            resTuple.d = b;
        }
        else
        {
            var tmpTuple = ExtEuclidean(b % a, a);
            resTuple.d = tmpTuple.d;
            resTuple.x = tmpTuple.y - (b / a) * tmpTuple.x;
            resTuple.y = tmpTuple.x;
        }
        return resTuple;
    }

}