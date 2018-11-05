using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class AddEntityDialog: Toplevel
{
  private readonly RotationChooser _rotationChooser;
  private readonly InversionChooser _invertChooser;
  private readonly Entity _entity;

  private readonly BoardView _demoBoard;

  public AddEntityDialog(Entity entity)
  {
    _entity = entity;

    var rect = CalculateRect(entity.Size);
    var win = new Window(rect, $"Insert {entity.Name}");
    Add(win);

    var previewSize = PreviewRect(entity.Size);
    var demoFrame = new FrameView(previewSize, "Preview");
    _demoBoard = new BoardView(0, 0, previewSize.Width - 2, previewSize.Height - 2);
    _demoBoard.Focus = new Point(2, 2);
    demoFrame.Add(_demoBoard);

    var closeBtn = new Button(3, rect.Height - 3, "Add", true);
    closeBtn.Clicked += Close;

    var cancelBtn = new Button(closeBtn.Frame.Right + 3, rect.Height - 3, "Cancel");
    cancelBtn.Clicked += Cancel;

    var rotationLabel = new Label(1, 1, "Rotation");
    _rotationChooser = new RotationChooser(rotationLabel.Frame.Right + 3, 1, entity);
    _rotationChooser.Changed += Redraw;

    var invertLabel = new Label(1, 3, "Inversion");
    _invertChooser = new InversionChooser(invertLabel.Frame.Right + 2, 3, entity);
    _invertChooser.Changed += Redraw;

    var nameLabel = new Label(1, 5, $"Name: {entity.Name}");

    var descOffset = 7;
    if (!string.IsNullOrEmpty(entity.FullName) && !entity.FullName.Equals(entity.Name))
    {
      descOffset = 9;
      var fullNameLabel = new Label(1, 7, $"Full Name: {entity.FullName}");
      win.Add(fullNameLabel);
    }
    
    var descSize = previewSize.X - 1;
    var labels =
      SplitLines(descSize, entity.Description)
      .Select((text, lineNum) => new Label(1, descOffset+lineNum, text));

    win.Add(rotationLabel);
    win.Add(_rotationChooser);
    win.Add(invertLabel);
    win.Add(_invertChooser);
    win.Add(nameLabel);
    foreach (var label in labels)
    {
      win.Add(label);
    }
    win.Add(demoFrame);
    win.Add(closeBtn);
    win.Add(cancelBtn);

    Redraw();
  }

  private IEnumerable<string> SplitLines(int descSize, string description)
  {
    var remaining = description;
    while (!string.IsNullOrEmpty(remaining))
    {
      int? lastspace = null;
      for (var i = 0; true; ++i)
      {
        if (i == remaining.Length)
        {
          yield return remaining;
          remaining = "";
          break;
        }

        if (i == descSize)
        {
          var breakAt = lastspace ?? descSize;
          var skipSpace = lastspace.HasValue ? 1 : 0;
          yield return remaining.Substring(0, breakAt);
          remaining = remaining.Substring(breakAt + skipSpace);
          break;
        }

        if (remaining[i] == ' ')
        {
          lastspace = i;
        }

        if (remaining[i] == '\n')
        {
          yield return remaining.Substring(0, i);
          remaining = remaining.Substring(i+1);
          break;
        }
      }
    }
  }

  private void Redraw()
  {
    _demoBoard.ClearBoard();
    foreach (var cell in Cells)
    {
      _demoBoard.Add(cell.X, cell.Y);
    }
    SetNeedsDisplay();
  }

  public bool Success { get; private set; } = false;

  public IEnumerable<Point> Cells => 
    _entity.Cells
      .Select(x => _rotationChooser.SelectedTransform(x))
      .Select(x => _invertChooser.SelectedInversion(x));

  private void Cancel()
  {
    Running = false;
  }

  private void Close()
  {
    Success = true;
    Running = false;
  }

  private static int GetViewSize(Rect size)
  {
    var maxWidth = Math.Max(size.Width, 10);
    var maxHeight = Math.Max(size.Height, 10);

    var viewSize = Math.Max(maxWidth, maxHeight);
    viewSize += 6;
    return viewSize;
  }

  private static Rect PreviewRect(Rect size)
  {
    var viewSize = GetViewSize(size);

    var x = 38;
    var y = 0;
    return new Rect(x, y, viewSize, viewSize);
  }

  private static Rect CalculateRect(Rect size)
  {
    var viewSize = GetViewSize(size);

    var width = 40 + viewSize;
    var height = 4 + viewSize;
    var x = (Application.Top.Frame.Width - width) / 2;
    var y = (Application.Top.Frame.Height - height) / 2; 

    return new Rect(x, y, width, height);
  }
}