package Task7;

import java.util.Random;

public class Task7 {

    public static void main(String[] args) {
        int[] nums = generateNumbers(20);
        shellSort(nums);
        for (int i = 0; i < nums.length; i++)
            System.out.print(nums[i] + " ");
    }

    public static void shellSort(int[] array) {
        int h = 1;

        while (h <= array.length / 3) {
            h = h * 3 + 1;
        }

        while (h > 0) {
            for (int outer = h; outer < array.length; outer++) {
                int tmp = array[outer];
                int inner = outer;

                while (inner > h - 1 && array[inner - h] > tmp) {
                    array[inner] = array[inner - h];
                    inner -= h;
                }

                array[inner] = tmp;
            }

            h = (h - 1) / 3;
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
