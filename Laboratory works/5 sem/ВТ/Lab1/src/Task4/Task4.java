package Task4;

import java.util.Random;

public class Task4 {

    public static void main(String[] args) {

        int[] arr = generateNumbers(100);
        for (int i = 0; i < arr.length; i++)
        {
            if (isPrime(arr[i]))
                System.out.println(i + " - " + arr[i]);
        }

    }

    private static int[] generateNumbers(int N)
    {
        int[] res = new int[N];
        Random rn = new Random();
        for (int i = 0; i < N; i++)
            res[i] = N + rn.nextInt() % N;
        return res;
    }

    private static boolean isPrime(int a)
    {
        boolean res = true;
        if (a <= 1)
            res = false;
        int i = 2;
        while (res && i <= Math.sqrt(a) )
        {
            if (a % i == 0)
                res = false;
            i++;
        }
        return res;
    }
}
