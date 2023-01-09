package Task6;

import java.util.Random;

public class Task6 {

    public static void main(String[] args) {
        int[] nums = generateNumbers(10);
        for (int i = 0; i < nums.length; i++)
        {
            for (int j = 0; j < nums.length; j++)
            {
                System.out.print(nums[ (i + j) % nums.length] + " ");
            }
            System.out.println();
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
}
