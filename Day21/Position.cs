namespace Day21;

public record Position(int X, int Y)
{
     public IEnumerable<Position> Surrounding(int width, int height)
     {
          if (X != 0)
          {
               yield return this with { X = X - 1 };
          }

          if (X != width - 1)
          {
               yield return this with { X = X + 1 };
          }

          if (Y != 0)
          {
               yield return this with { Y = Y - 1 };
          }

          if (Y != height - 1)
          {
               yield return this with { Y = Y + 1 };
          }
     }

     public PositionGrid Grid
          => new PositionGrid(X, Y, 0, 0);
}