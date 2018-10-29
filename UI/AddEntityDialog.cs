using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class AddEntityDialog: Toplevel
{
  private readonly RotationChooser _rotationChooser;
  private readonly InversionChooser _invertChooser;
  private readonly Entity _entity;

  public AddEntityDialog(Entity entity)
  {
    _entity = entity;

    var rect = CalculateRect(entity.Size);
    var win = new Window(rect, $"Insert {entity.Name}");
    Add(win);

    var demoFrame = new FrameView(PreviewRect(entity.Size), "Preview");
    var demoBoard = new BoardView(0, 0, demoFrame.Frame.Width - 2, demoFrame.Frame.Height - 2);
    demoFrame.Add(demoBoard);
    foreach (var cell in entity.Cells)
    {
      demoBoard.Add(cell.X, cell.Y);
    }


    var closeBtn = new Button(3, rect.Height - 3, "Add", true);
    closeBtn.Clicked += Close;

    var cancelBtn = new Button(closeBtn.Frame.Right + 3, rect.Height - 3, "Cancel");
    cancelBtn.Clicked += Cancel;

    var rotationLabel = new Label(1, 1, "Rotation");
    _rotationChooser = new RotationChooser(rotationLabel.Frame.Right + 3, 1);

    var invertLabel = new Label(1, 3, "Inversion");
    _invertChooser = new InversionChooser(invertLabel.Frame.Right + 2, 3, entity);
    
    win.Add(rotationLabel);
    win.Add(_rotationChooser);
    win.Add(invertLabel);
    win.Add(_invertChooser);
    win.Add(demoFrame);
    win.Add(closeBtn);
    win.Add(cancelBtn);
  }

  private Rect PreviewRect(Rect size)
  {
    var maxWidth = Math.Max(size.Width, 10);
    var maxHeight = Math.Max(size.Height, 10);

    var viewSize = Math.Max(maxWidth, maxHeight);

    var x = 38;
    var y = 0;
    return new Rect(x, y, viewSize, viewSize);
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

  private Rect CalculateRect(Rect size)
  {
    var maxWidth = Math.Max(size.Width, 10);
    var maxHeight = Math.Max(size.Height, 10);

    var viewSize = Math.Max(maxWidth, maxHeight);

    var width = 40 + viewSize;
    var height = 4 + viewSize;
    var x = (Application.Top.Frame.Width - width) / 2;
    var y = (Application.Top.Frame.Height - height) / 2; 

    return new Rect(x, y, width, height);
  }
}