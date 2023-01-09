package Task1;

import java.text.MessageFormat;

public class Task1 {

    public static void main(String args[])
    {
        typeRes(1, 2);
        typeRes(0.5f, 100);
        typeRes( 0.000001f, 2);
    }

    private static void typeRes(float x, float y)
    {
        System.out.println(MessageFormat.format("f({0}, {1}) = {2}", x, y, func(x, y) ));
    }

    private static float func(float x, float y)
    {
        return (float)( x + ( (1 + Math.pow(Math.sin(x+y), 2) ) / ( 2 + Math.abs( x - (2 * x / ( 1 + Math.pow(x*y, 2) ) ) ) )) );
    }

}
