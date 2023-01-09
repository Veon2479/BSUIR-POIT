package Task2;

public class Task2
{
    public static void main(String[] args)
    {
        System.out.println(isInside(0, 0));
        System.out.println(isInside(0, 10));
        System.out.println(isInside(10, 10));
    }

    private static boolean isInside(int x, int y)
    {
        boolean res = true;

        if (y > 0)
        {
            if ( y > 5 || x > 4 || x < -4 )
                res = false;
        }
        else
        {
            if ( y < -3 || x > 6 || x < -6 )
                res = false;
        }

        return res;
    }
}
