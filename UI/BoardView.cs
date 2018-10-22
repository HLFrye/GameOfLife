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

    private Point _focus;
    private Point _offset;
    private readonly ILifeGameInterface _game;

    public BoardView(int x, int y, int width, int height)
    : base(new Rect(x, y, width, height))
    {
        _offset = new Point(0, 0);
        _focus = new Point(1, 1);
        _game = new Life.LifeGame();
    }

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

    private bool EditModeProcessKey(KeyEvent keyEvent)
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
                Toggle(_focus.X + _offset.X, _focus.Y + _offset.Y);
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
        _game.Add(x + _focus.X, y + _focus.Y);
    }

    public void Remove(int x, int y)
    {
        _game.Remove(x + _focus.X, y + _focus.Y);
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
                if (HasFocus && _inputMode == InputMode.Editing && _focus.X == x && _focus.Y == y)
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
