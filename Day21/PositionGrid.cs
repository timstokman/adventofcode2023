namespace Day21;

public record PositionGrid(int X, int Y, int XGrid, int YGrid)
{
     public IEnumerable<PositionGrid> Surrounding(int width, int height)
     {
          if (X == 0)
          {
               yield return this with { X = width - 1, XGrid = XGrid - 1 };
          }
          else
          {
               yield return this with { X = X - 1 };
          }

          if (X == width - 1)
          {
               yield return this with { X = 0, XGrid = XGrid + 1 };
          }
          else
          {
               yield return this with { X = X + 1 };
          }
          
          if (Y == 0)
          {
               yield return this with { Y = height - 1, YGrid = YGrid - 1 };
          }
          else
          {
               yield return this with { Y = Y - 1 };
          }

          if (Y == height - 1)
          {
               yield return this with { Y = 0, YGrid = YGrid + 1 };
          }
          else
          {
               yield return this with { Y = Y + 1 };
          }
     }
}