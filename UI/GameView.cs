using System;
using Terminal.Gui;
using LifeInterface;

class GameView: View 
{
    private readonly Func<int, int, bool> _charFunc;
    private Point _focus;

    private readonly ILifeGameInterface _game;

    public GameView(int x, int y, int width, int height)
        : base(new Rect(x, y, width, height))
        {
            var rng = new Random();
            _charFunc = (a, b) => (rng.Next() % 2) == 0;
            _focus = new Point(1, 1);
            _game = null;
        }

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
            default: 
                return false;
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
                var draw = _charFunc(x, y) ? 'x' : ' ';
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