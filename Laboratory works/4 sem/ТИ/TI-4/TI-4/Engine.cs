using System;
using System.Net.Http.Headers;

namespace TI_4;

public class Engine
{
    //interface part

    public enum ErrCodes
    {
        None, ErrParameters, ErrAnotherH, ErrAnotherK
    }
    
    public ErrCodes ErrState => _errState;
    public int Hash => _hash;
    public int R => _r;
    public int S => _s;

    public int W => _w;
    public int U1 => _u1;
    public int U2 => _u2;
    public int V => _v;
    
    //private part
    private ErrCodes _errState = ErrCodes.None;

    private int _q, _p, _h, _x, _k;
    private int _hash;
    private int _g, _y, _r, _s;
    private int _w, _u1, _u2, _v;


    public void ComputeEDS(string text, int q, int p, int h, int x, int k)
    {
        SetParameters(q, p, h, x, k);
        if (_errState == ErrCodes.None)
        {
            ComputeG();
            if (_errState == ErrCodes.None)
            {
                ComputePublicKey();
                ComputeHash( text, q );
                ComputeR();
                ComputeS();
            }
        }
    }

    public bool CheckEDS(string text, int q, int p, int h, int x, int r, int s)
    {
        bool RESULT = false;
        SetParameters(q, p, h, x, r, s);
        
        //prepare data
        ComputeG();
        if (_errState == ErrCodes.None)
        {
            ComputeY();
        
            this._w = FastPow(this._s, this._q - 2, this._q);
            ComputeHash(text, this._q);
            this._u1 = (this._hash * this._w) % this._q;
            this._u2 = (this._r * this._w) % q;
            this._v = ((FastPow(this._g, this._u1, this._p) * 
                        FastPow(this._y, this._u2, this._p)) % 
                       this._p) % this._q;
        
            if (this._v == this._r)
                RESULT = true;
        }
        
        return RESULT;
    }
    
    private void SetParameters(int q, int p, int h, int x, int k)
    {
        ResetState();
        this._errState = Validate(q, p, h, x, k);
        this._q = q;
        this._p = p;
        this._h = h;
        this._x = x;
        this._k = k;
    }

    private void SetParameters(int q, int p, int h, int x, int r, int s)
    {
        ResetState();
        this._errState = Validate(q, p, h, x, r, s);
        this._q = q;
        this._p = p;
        this._h = h;
        this._x = x;
        this._r = r;
        this._s = s;
    }
    private void ResetState()
    {
        this._q = 0;
        this._p = 0;
        this._h = 0;
        this._x = 0;
        this._k = 0;
        this._g = 0;
        this._y = 0;
        this._r = 0;
        this._s = 0;
        this._hash = 0;
        this._w = 0;
        this._v = 0;
        this._u1 = 0;
        this._u2 = 0;
        this._errState = ErrCodes.None;
    }
    
    //validates parameters before any computing
    private ErrCodes ValidatePrimes(int q, int p)
    {
        bool RESULT = IsPrime(q);
        RESULT &= IsPrime(p);
        RESULT &= ((p - 1) % q == 0);
        RESULT &= ((p > 0) && (q > 0));
        if (RESULT == false)
            return ErrCodes.ErrParameters;
        else
            return ErrCodes.None;
    }
    private ErrCodes Validate(int q, int p, int h, int x, int k)
    {
        bool RESULT = (ValidatePrimes(q, p) == ErrCodes.None);
        RESULT &= ( (h > 1) && (h < p-1) );
        RESULT &= ((x > 0) && (x < q));
        RESULT &= ((k > 0) && (k < q));
        if (RESULT == false)
            return ErrCodes.ErrParameters;
        else
            return ErrCodes.None;
    }

    private ErrCodes Validate(int q, int p, int h, int x, int r, int s)
    {
        bool RESULT = (ValidatePrimes(q, p) == ErrCodes.None);
        RESULT &= ( (h > 1) && (h < p-1) );
        RESULT &= ((x > 0) && (x < q));
        RESULT &= (r >= 0);
        RESULT &= (s >= 0);
        if (RESULT == false)
            return ErrCodes.ErrParameters;
        else
            return ErrCodes.None;
    }
    
    private void ComputePublicKey()
    {
        this._y = FastPow(this._g, this._x, this._p);
    }

    private void ComputeG()
    {
        this._g = FastPow(this._h, (this._p - 1) / this._q, this._p);
        if (this._g <= 1)
            _errState = ErrCodes.ErrAnotherH;
    }
    
    private void ComputeR()
    {
        this._r = FastPow(this._g, this._k, this._p) % this._q;
        if (this._r == 0)
            this._errState = ErrCodes.ErrAnotherK;
    }

    private void ComputeS()
    {
        this._s = ( FastPow(this._k, this._q - 2, this._q) * (this._hash + this._x * this._r) ) % this._q;
        if (this._s == 0)
            this._errState = ErrCodes.ErrAnotherK;
    }

    private void ComputeY()
    {
        this._y = FastPow(this._g, this._x, this._p);
    }
    private bool IsPrime(int num)
    {
        bool RESULT = true;
        int i = 2;
        while ( RESULT && i < Math.Round( Math.Sqrt( num ) ) )
        {
            if (num % i == 0)
                RESULT = false;
            i++;
        }
        return RESULT;
    }

    //computing num^deg by modulo mod
    private int FastPow(int num, int deg, int mod )
    {
        int RESULT = 1;
        while (deg != 0)
        {
            while (deg % 2 == 0)
            {
                deg /= 2;
                num = (num * num) % mod;
            }
            deg--;
            RESULT = (RESULT * num) % mod;
        }
        return RESULT;
    }
    
    //compute hash of message
    public void ComputeHash( string text, int mod )
    {
        Console.WriteLine("Text is:-"+text+"-end");
        int RESULT = 100 % mod;
        for (int i = 0; i < text.Length - 1; i++)
        {
            RESULT = ( (int) Math.Pow((RESULT + text[i]), 2) ) % mod;
        }
        this._hash = RESULT;
    }
    
}