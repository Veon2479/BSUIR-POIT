package Task9;

import java.util.ArrayList;
import java.util.List;
import Task9.Ball;
import Task9.Colour;

public class Urn {
    private List<Ball> balls;

    public Urn()
    {
        balls = new ArrayList<>();
    }
    public void AddBall(Ball ball)
    {
        balls.add(ball);
    }

    public int GetUrnWeight()
    {
        int res = 0;
        for (Ball ball : balls)
            res += ball.Weight;
        return res;
    }

    public int CountColouredBalls(Colour colour)
    {
        int res = 0;
        for (Ball ball : balls)
            if (ball.Colour == colour)
                res++;
        return res;
    }

}
