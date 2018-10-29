using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class InversionChooser : Chooser
{
  private static readonly List<string> _choices = new List<string> 
  {
    "No Inversion",
    "Invert X-Axis",
    "Invert Y-Axis",
    "Invert X and Y Axis"
  };

  private static readonly string defaultChoice = _choices[0];

  private readonly int _entityMaxX;
  private readonly int _entityMaxY;

  public InversionChooser(int x, int y, Entity entity) : base(x, y, _choices, defaultChoice)
  {
    _entityMaxX = entity.Cells.Select(cell => cell.X).Max();
    _entityMaxY = entity.Cells.Select(cell => cell.Y).Max();
  }

  public Func<Point, Point> SelectedInversion 
  {
    get
    {
      var selectedIndex = Selection;
      switch (selectedIndex)
      {
        case 0:
          return x => x;
        case 1:
          return InvertX;
        case 2:
          return InvertY;
        case 3:
          return InvertXY;
      }
      throw new ArgumentOutOfRangeException();
    }
  }

  private Point InvertXY(Point arg)
  {
    return InvertY(InvertX(arg));
  }

  private Point InvertY(Point arg)
  {
    return new Point(arg.X, _entityMaxY - arg.Y);
  }

  private Point InvertX(Point arg)
  {
    return new Point(_entityMaxX - arg.X, arg.Y);
  }
}

