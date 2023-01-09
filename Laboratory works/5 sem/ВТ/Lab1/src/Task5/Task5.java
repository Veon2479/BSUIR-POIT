package Task5;

import java.util.Random;

public class Task5 {

    public static void main(String[] args) {
        int[] nums = generateNumbers(5);
        for (int i = 0; i < nums.length; i++)
            System.out.print(nums[i] + " ");
        System.out.println("\nNumbers to delete: " + getCount(nums));

    }

    private static int[] generateNumbers(int N)
    {
        int[] res = new int[N];
        Random rn = new Random();
        for (int i = 0; i < N; i++)
            res[i] = N + rn.nextInt() % N;
        return res;
    }

    private static int getCount(int[] nums)
    {
        int[] mask = new int[nums.length];
        for (int i = 0; i < nums.length; i++)
            mask[i] = 1;
        int max = 0;
        for (int i = 0; i < nums.length; i++)
        {
            for (int j = i + 1; j < nums.length; j++)
            {
                if (nums[j] > nums[i] && mask[j] <= mask[i])
                {
                    mask[j] = mask[i] + 1;
                }

            }

            if (mask[i] > max)
                max = mask[i];
        }

        return nums.length - max;
    }

}
