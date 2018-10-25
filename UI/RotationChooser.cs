using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class RotationChooser: Button
{
  public delegate Point Transform(Point point);

  private readonly Dictionary<string, Transform> _transforms = new Dictionary<string, Transform>
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

  private static Transform Rotate(int angle)
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

  public RotationChooser(int x, int y): base(x, y, DefaultSelection)
  {
    Clicked += ShowChooseRotationDialog;
  }

  public Transform SelectedTransform => _transforms[_selection];

  private void ShowChooseRotationDialog()
  {
    var tl = new Toplevel();
    var keys = _transforms.Keys.ToList();
    var height = keys.Count;
    var width = keys.Max(x => x.Length) + 1;
  
    var winX = 0;
    var winY = 0;
    View view = this;
    while (view != null)
    {
      winX += view.Frame.Location.X;
      winY += view.Frame.Location.Y;
      
      view = view.SuperView;
    }

    var winRect = new Rect(winX, winY, width + 2, height + 3);
    var win = new Window(winRect, "Choose");
    tl.Add(win);

    var list = new ListView(keys);
    list.SelectedItem = keys.IndexOf(_selection);

    var btn = new Button((width - 8) / 2, height, "Ok", true);
    btn.Clicked += () => tl.Running = false;

    win.Add(list);
    win.Add(btn);
    Application.Run(tl);
    _selection = keys[list.SelectedItem];
    Text = _selection;
    SetNeedsDisplay();
  }
}