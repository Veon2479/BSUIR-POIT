package Task3;

import static java.lang.String.format;

public class Task3 {

    public static void main(String[] args) {


        WriteTable(1, 10, 0.3f);
    }

    private static void WriteTable(float a, float b, float h)
    {
        while (a < b + 1e-4)
        {
            System.out.println(format("%.4f", a) + ' ' + format("%.4f", Math.tan(a)));
            a += h;
        }
    }

}
