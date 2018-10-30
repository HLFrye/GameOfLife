using System;
using Terminal.Gui;
using LifeInterface;
using LifeLib;
using System.Diagnostics;

public class BoardView: View 
{
    public enum InputMode
    {
        Editing,
        Moving
    }

    private InputMode _inputMode = InputMode.Editing;

    private Point _offset;
    private ILifeGameInterface _game;

    public BoardView(int x, int y, int width, int height)
    : base(new Rect(x, y, width, height))
    {
        _offset = new Point(0, 0);
        Focus = new Point(1, 1);
        _game = new Life.LifeGame();
    }

    public Point Focus { get; set; }

    public event Action Updated = delegate { };

    public override bool CanFocus { get; set; } = false;

    public override bool ProcessKey(KeyEvent keyEvent)
    {
        switch (_inputMode)
        {
            case InputMode.Editing:
                return EditModeProcessKey(keyEvent);
            case InputMode.Moving:
                return MoveModeProcessKey(keyEvent);
        }
        return false;
    }

    private bool MoveModeProcessKey(KeyEvent keyEvent)
    {
        switch (keyEvent.Key)
        {
            case Key.CursorUp:
                _offset.Y -= 1;
                SetNeedsDisplay();
                return true;
            case Key.CursorDown:
                _offset.Y += 1;
                SetNeedsDisplay();
                return true;
            case Key.CursorLeft: 
                _offset.X -= 1;
                SetNeedsDisplay();
                return true;
            case Key.CursorRight:
                _offset.X += 1;
                SetNeedsDisplay();
                return true;
            default: 
                return false;
        }
    }

    private void MoveFocus(int xdelta, int ydelta)
    {
        var pt = Focus;
        pt.X += xdelta;
        pt.Y += ydelta;
        Focus = pt;
    }

    private bool EditModeProcessKey(KeyEvent keyEvent)
    {
        switch (keyEvent.Key)
        {
            case Key.CursorUp:
                if (Focus.Y > 1)
                {
                    MoveFocus(0, -1);
                }
                SetNeedsDisplay();
                return true;
            case Key.CursorDown:
                if (Focus.Y < (Frame.Bottom - 1))
                {
                    MoveFocus(0, 1);
                }
                SetNeedsDisplay();
                return true;
            case Key.CursorLeft: 
                if (Focus.X > 1)
                {
                    MoveFocus(-1, 0);
                }
                SetNeedsDisplay();
                return true;
            case Key.CursorRight:
                if (Focus.X < (Frame.Right - 1))
                {
                    MoveFocus(1, 0);
                }
                SetNeedsDisplay();
                return true;
            case Key.Space:
                Toggle(Focus.X + _offset.X, Focus.Y + _offset.Y);
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
        _game.Add(x + Focus.X, y + Focus.Y);
    }

    public void Remove(int x, int y)
    {
        _game.Remove(x + Focus.X, y + Focus.Y);
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
                if (HasFocus && _inputMode == InputMode.Editing && Focus.X == x && Focus.Y == y)
                {
                    Driver.SetAttribute(ColorScheme.Focus);
                }
                else
                {
                    Driver.SetAttribute(ColorScheme.Normal);
                }
                Move(x, y);
                var draw = _game.Get(x + _offset.X, y + _offset.Y) ? 'x' : ' ';
                Driver.AddRune(draw);
            }
        }
    }

    public void ClearBoard()
    {
        _game = new Life.LifeGame();
        SetNeedsDisplay();
    }

    internal void BeginEditMode(Window win)
    {
        try
        {
            CanFocus = true;
            _inputMode = InputMode.Editing;
            win.SetFocus(this);
        }
        finally
        {
            CanFocus = false;
        }
    }

    internal void BeginMoveMode(Window win)
    {
        try
        {
            CanFocus = true;
            _inputMode = InputMode.Moving;
            win.SetFocus(this);
        }
        finally
        {
            CanFocus = false;
        }
    }
}
