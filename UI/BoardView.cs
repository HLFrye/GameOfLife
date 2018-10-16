using System;
using Terminal.Gui;
using LifeInterface;
using LifeLib;
using System.Diagnostics;

public class BoardView: View 
{
    private Point _focus;
    private readonly ILifeGameInterface _game;

    public BoardView(int x, int y, int width, int height)
    : base(new Rect(x, y, width, height))
    {
        _focus = new Point(1, 1);
        _game = new Life.LifeGame();
    }

    public event Action Updated = delegate { };

    public override bool CanFocus { get; set; } = false;

    public override bool ProcessKey(KeyEvent keyEvent)
    {
        switch (keyEvent.Key)
        {
            case Key.CursorUp:
                if (_focus.Y > 1)
                {
                    _focus.Y -= 1; 
                }
                SetNeedsDisplay();
                return true;
            case Key.CursorDown:
                if (_focus.Y < (Frame.Bottom - 1))
                {
                    _focus.Y += 1;
                }
                SetNeedsDisplay();
                return true;
            case Key.CursorLeft: 
                if (_focus.X > 1)
                {
                    _focus.X -= 1;
                }
                SetNeedsDisplay();
                return true;
            case Key.CursorRight:
                if (_focus.X < (Frame.Right - 1))
                {
                    _focus.X += 1;
                }
                SetNeedsDisplay();
                return true;
            case Key.Space:
                Toggle(_focus.X, _focus.Y);
                SetNeedsDisplay();
                return true;
            default: 
                return false;
        }
    }
 
  public TimeSpan LastUpdate { get; private set; } = new TimeSpan();

  public int NumGenerations { get; private set; }

  internal int GetCellCount()
  {
      // TODO
      return _game.CellCount;
  }

  public void Update()
    {
        var watch = new Stopwatch();
        watch.Start();
        _game.Update();
        watch.Stop();
        LastUpdate = watch.Elapsed;
        NumGenerations++;

        Updated();
        SetNeedsDisplay();
    }

    public void Add(int x, int y)
    {
        _game.Add(x, y);
    }

    public void Remove(int x, int y)
    {
        _game.Remove(x, y);
    }

    private void Toggle(int x, int y)
    {
        if (_game.Get(x, y))
        {
            _game.Remove(x, y);
        }
        else
        {
            _game.Add(x, y);
        }
    }

    public override void Redraw(Rect region)
    {
        Clear();
        for (var y = region.Top; y < region.Bottom; ++y)
        {
            for (var x = region.Left; x < region.Right; ++x) 
            {
                if (HasFocus && _focus.X == x && _focus.Y == y)
                {
                    Driver.SetAttribute(ColorScheme.Focus);
                }
                else
                {
                    Driver.SetAttribute(ColorScheme.Normal);
                }
                Move(x, y);
                var draw = _game.Get(x, y) ? 'x' : ' ';
                Driver.AddRune(draw);
            }
        }
    }

    internal void TakeFocus(Window win)
    {
        CanFocus = true;
        win.SetFocus(this);
    }
}
