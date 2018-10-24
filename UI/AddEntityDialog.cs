using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

public class AddEntityDialog: Toplevel
{
  private readonly RotationChooser _rotationChooser;
  private readonly Entity _entity;

  public AddEntityDialog(Entity entity)
  {
    _entity = entity;

    var rect = CalculateRect(entity.Size);
    var win = new Window(rect, $"Insert {entity.Name}");
    Add(win);

    var demoFrame = new FrameView(PreviewRect(entity.Size), "Preview");

    var closeBtn = new Button(3, rect.Height - 3, "Add", true);
    closeBtn.Clicked += Close;

    var cancelBtn = new Button(closeBtn.Frame.Right + 3, rect.Height - 3, "Cancel");
    cancelBtn.Clicked += Cancel;

    var rotationLabel = new Label(1, 1, "Rotation");
    _rotationChooser = new RotationChooser(rotationLabel.Frame.Right + 3, 1);

    var invertField = new CheckBox(1, 3, "Invert");
    
    win.Add(rotationLabel);
    win.Add(_rotationChooser);
    win.Add(invertField);
    win.Add(demoFrame);
    win.Add(closeBtn);
    win.Add(cancelBtn);
  }

  private Action ChangeRotation()
  {
    throw new NotImplementedException();
  }

  private Rect PreviewRect(Rect size)
  {
    var viewWidth = Math.Max(size.Width, 10);
    var viewHeight = Math.Max(size.Height, 10);

    var x = 38;
    var y = 0;
    return new Rect(x, y, viewWidth, viewHeight);
  }

  public bool Success { get; private set; } = false;

  public IEnumerable<Point> Cells => _entity.Cells.Select(x => _rotationChooser.SelectedTransform(x));

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
    var viewWidth = Math.Max(size.Width, 10);
    var viewHeight = Math.Max(size.Height, 10);

    var width = 40 + viewWidth;
    var height = 4 + viewHeight;
    var x = (Application.Top.Frame.Width - width) / 2;
    var y = (Application.Top.Frame.Height - height) / 2; 

    return new Rect(x, y, width, height);
  }
}