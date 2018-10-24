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

  private static Transform Rotate(int v)
  {
    var angle = v * Math.PI/180;
    return point => 
    {
      var newX = (int)(point.X * Math.Cos(angle) - point.Y * Math.Sin(angle));
      var newY = (int)(point.X * Math.Sin(angle) + point.X * Math.Cos(angle));

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
    var width = keys.Max(x => x.Length);
  

    var winRect = new Rect(Frame.Left + 1, Frame.Top + 1, width + 2, height + 3);
    var win = new Window(winRect, "Select");
    tl.Add(win);

    var list = new ListView(keys);
    list.SelectedItem = keys.IndexOf(_selection);

    var btn = new Button("Ok", true);
    btn.Clicked += () => tl.Running = false;

    win.Add(list);
    win.Add(btn);
    Application.Run(tl);
    _selection = keys[list.SelectedItem];
  }
}