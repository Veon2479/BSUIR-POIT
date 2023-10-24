using System;
using System.Collections.Generic;
using System.Numerics;

namespace TI_2;

public class Engine
{
    private readonly int _size;
    private readonly List<Vector2> _masks = new();
    private readonly ulong _mask = 0xFFFF_FFFF_FFFF_FFFF;
    private readonly ulong _byteMask = 0xFF;

    public Engine(List<int> degrees, int size)
    {
        _size = size;
        //_masks.Add(1);
        foreach (int deg in degrees)
        {
            _masks.Add(new Vector2((ulong) 1 << (deg - 1), deg - 1));
        }

        _mask <<= _size;
        _mask = ~_mask;

        _byteMask <<= _size - 8;
    }
    
    public byte[] Convert(in byte[] src, ulong key, out byte[] keyBytes)
    {
        byte[] dest = new byte[src.Length];
        keyBytes = new byte[src.Length];
        
        key &= _mask;
        ulong newBit = 0;
        byte keyByte = 0;
        for (int i = 0; i < src.Length; i++)
        {
            keyByte = (byte) ((key & _byteMask) >> (_size - 8));
            dest[i] = (byte) (src[i] ^ keyByte);
            keyBytes[i] = keyByte;
            
            for (int j = 0; j < 8; j++)
            {
                newBit = 0;
                foreach (var mask in _masks)
                {
                    newBit ^= ( ((ulong) mask.X & key) >> ( (int) mask.Y));
                }
                key <<= 1;
                key |= newBit;
                key &= _mask; //not necessary!
            }
            

        }
        
        return dest;
    }
}