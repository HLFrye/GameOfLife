using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class RotationChooser: Chooser
{
  private static readonly List<string> _choices = new List<string>
  {
    "No Rotation",
    "90 degrees CW", 
    "180 degrees CW",
    "270 degrees CW",
  };

  private readonly Entity _entity;

  private static int IntCos(int degrees)
  {
    switch (degrees)
    {
      case 0:
        return 1;
      case 90:
      case 270:
        return 0;
      case 180:
        return -1;
    }
    throw new NotImplementedException();
  }

  private static int IntSin(int degrees)
  {
    switch (degrees)
    {
      case 0:
      case 180:
        return 0;
      case 90:
        return 1;
      case 270:
        return -1;
    }
    throw new NotImplementedException();
  }

  private Func<Point, Point> Rotate(int angle)
  {
    var maxX = _entity.Cells.Max(x => x.X);
    var maxY = _entity.Cells.Max(x => x.Y);

    // if (maxX % 2 == 0) maxX+=2;
    // if (maxY % 2 == 0) maxY+=2;

    var centerX = maxX / 2.0;
    var centerY = maxY / 2.0;

    return point => 
    {
      var newX = Math.Ceiling(
        point.X * IntCos(angle) - 
        point.Y * IntSin(angle) +
        -centerX * IntCos(angle) + centerX + centerY * IntSin(angle));

      var newY = Math.Ceiling(
        point.X * IntSin(angle) + 
        point.Y * IntCos(angle) +
        -centerX * IntSin(angle) - centerY * IntCos(angle) + centerY);

      var newPt = new Point((int)newX, (int)newY);
      return newPt;
    };
  }

  public RotationChooser(int x, int y, Entity entity): base(x, y, _choices, _choices[0])
  {
    _entity = entity;
  }

  public Func<Point, Point> SelectedTransform 
  {
    get 
    {
      switch (Selection)
      {
        case 0:
          return x => x;
        case 1:
          return Rotate(90);
        case 2:
          return Rotate(180);
        case 3: 
          return Rotate(270);
      }
      throw new ArgumentOutOfRangeException();
    }
  } 
}