using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class RotationChooser: Chooser
{
  private static readonly Dictionary<string, Func<Point, Point>> _transforms = new Dictionary<string, Func<Point, Point>>
  {
    {DefaultSelection, x => x},
    {"90 degrees CW", Rotate(90)},
    {"180 degrees CW", Rotate(180)},
    {"270 degrees CW", Rotate(270)},
  };

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

  private static Func<Point, Point> Rotate(int angle)
  {
    return point => 
    {
      var newX = point.X * IntCos(angle) - point.Y * IntSin(angle);
      var newY = point.X * IntSin(angle) + point.Y * IntCos(angle);

      var newPt = new Point(newX, newY);
      return newPt;
    };
  }

  private const string DefaultSelection = "No Rotation";

  private string _selection = DefaultSelection;

  public RotationChooser(int x, int y): base(x, y, _transforms.Keys.ToList(), DefaultSelection)
  {
  }

  public Func<Point, Point> SelectedTransform => _transforms[_selection];
}