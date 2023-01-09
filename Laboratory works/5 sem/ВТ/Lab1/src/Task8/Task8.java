package Task8;

import java.util.Random;

public class Task8 {


    public static void main(String[] args) {
        int N = 3;
        int[] a = generateNumbers(N), b = generateNumbers(N);
        shellSort(a);
        shellSort(b);
        System.out.print("A: ");
        for (int i = 0; i < N; i++)
            System.out.print(a[i] + " ");
        System.out.println();
        System.out.print("B: ");
        for (int i = 0; i < N; i++)
            System.out.print(b[i] + " ");
        System.out.println();
        int i = 0;
        int j = 0;
        System.out.print("Positions: ");
        int[] res = new int[2 * N];
        while (j < b.length)
        {
            if (i - j < N && a[i - j] < b[j])
            {
                res[i] = a[i - j];
            }
            else
            {
                System.out.print(i + " ");
                res[i] = b[j];
                j++;
            }
            i++;

        }

    }
//   1 3 3 3 4

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
