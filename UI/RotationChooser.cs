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

    if (maxX % 2 == 0) maxX+=2;
    if (maxY % 2 == 0) maxY+=2;

    var centerPoint = new Point(maxX / 2, maxY / 2);

    return point => 
    {
      var newX = 
        point.X * IntCos(angle) - 
        point.Y * IntSin(angle) +
        -centerPoint.X * IntCos(angle) + centerPoint.X + centerPoint.Y * IntSin(angle);

      var newY = 
        point.X * IntSin(angle) + 
        point.Y * IntCos(angle) +
        -centerPoint.X * IntSin(angle) - centerPoint.Y * IntCos(angle) + centerPoint.Y;

      var newPt = new Point(newX, newY);
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