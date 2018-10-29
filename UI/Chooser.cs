using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class Chooser: Button 
{
  private readonly IList<string> _choices;
  public Chooser(int x, int y, IList<string> choices, string defaultChoice)
    :base(x, y, defaultChoice)
  {
    if (!choices.Contains(defaultChoice))
    {
      throw new ArgumentOutOfRangeException($"{nameof(defaultChoice)} must be a key in {nameof(choices)}");
    }

    Selection = choices.IndexOf(defaultChoice);
    _choices = choices;
    Clicked += ShowChoiceDialog;
  }
  
  public int Selection { get; private set; }

  private void ShowChoiceDialog()
  {
    var tl = new Toplevel();
    var keys = _choices;
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

    var list = new ListView((IList)keys);
    list.SelectedItem = Selection;

    var btn = new Button((width - 8) / 2, height, "Ok", true);
    btn.Clicked += () => tl.Running = false;

    win.Add(list);
    win.Add(btn);
    Application.Run(tl);
    Selection = list.SelectedItem;
    Text = _choices[Selection];
    SetNeedsDisplay();
  }
}