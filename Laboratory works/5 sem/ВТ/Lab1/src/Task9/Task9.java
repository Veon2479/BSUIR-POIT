package Task9;

import static Task9.Colour.*;
import Task9.Urn;
import Task9.Ball;

public class Task9 {

    public static void main(String[] args) {
        Urn urn = new Urn();
        urn.AddBall(new Ball(Blue, 25));
        urn.AddBall(new Ball(Black, 25));
        urn.AddBall(new Ball(Red, 1));
        urn.AddBall(new Ball(Blue, 25));
        urn.AddBall(new Ball(Blue, 70));
        urn.AddBall(new Ball(White, 25));
        urn.AddBall(new Ball(Blue, 34));
        urn.AddBall(new Ball(Blue, 25));
        urn.AddBall(new Ball(Blue, 1000));
        urn.AddBall(new Ball(Black, 25));
        urn.AddBall(new Ball(Blue, 25));

        System.out.println("Urn weight is: " + urn.GetUrnWeight());
        System.out.println("Blue balls: " + urn.CountColouredBalls(Blue));
    }
}
