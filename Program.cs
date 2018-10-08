using System;
using System.Threading;
using Terminal.Gui;

delegate bool GetChar(int x, int y);

class BitView: View 
{
    private readonly GetChar _charFunc;
    private Point _focus;

    public BitView(int x, int y, int width, int height, GetChar charFunc)
        : base(new Rect(x, y, width, height))
        {
            _charFunc = charFunc;
            _focus = new Point(10, 10);
        }

    public override bool CanFocus => true;

    public override bool ProcessKey(KeyEvent keyEvent)
    {
        switch (keyEvent.Key)
        {
            case Key.CursorUp:
                if (_focus.Y > 1)
                {
                    _focus.Y -= 1; 
                }
                return true;
            case Key.CursorDown:
                if (_focus.Y < (Frame.Bottom - 1))
                {
                    _focus.Y += 1;
                }
                return true;
            case Key.CursorLeft: 
                if (_focus.X > 1)
                {
                    _focus.X -= 1;
                }
                return true;
            case Key.CursorRight:
                if (_focus.X < (Frame.Right - 1))
                {
                    _focus.X += 1;
                }
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
                if (_focus.X == x && _focus.Y == y)
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
}

class Demo {
    static void NewFile()
    {

    }

    static void Close()
    {

    }

    static bool Quit()
    {
        return true;
    }

    static Rect CalcGameRect(Rect parent)
    {
        return new Rect(
            parent.Left + 10, 
            parent.Top + 10, 
            parent.Width - 20, 
            parent.Height - 20
        );
    }

    static void Main ()
    {
        Application.Init ();
        var top = Application.Top;

	// Creates the top-level window to show
       var win = new Window (new Rect (0, 1, top.Frame.Width, top.Frame.Height-1), "MyApp");
       top.Add (win);

	// Creates a menubar, the item "New" has a help menu.
        // var menu = new MenuBar (new MenuBarItem [] {
        //     new MenuBarItem ("_File", new MenuItem [] {
        //         new MenuItem ("_New", "Creates new file", NewFile),
        //         new MenuItem ("_Close", "", () => Close ()),
        //         new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
        //     }),
        //     new MenuBarItem ("_Edit", new MenuItem [] {
        //         new MenuItem ("_Copy", "", null),
        //         new MenuItem ("C_ut", "", null),
        //         new MenuItem ("_Paste", "", null)
        //     })
        // });
        // top.Add (menu);

        var gameRect = CalcGameRect(top.Frame);

	// Add some controls
	// win.Add (
    //         new Label (3, 2, "Login: "),
    //         new TextField (14, 2, 40, ""),
    //         new Label (3, 4, "Password: "),
    //         new TextField (14, 4, 40, "") { Secret = true },
    //         new CheckBox (3, 6, "Remember me"),
    //         new RadioGroup (3, 8, new [] { "_Personal", "_Company" }),
    //         new Button (3, 14, "Ok"),
    //         new Button (10, 14, "Cancel"),
    //         new Label (3, 18, "Press ESC and 9 to activate the menubar"));

        var editBtn = new Button(gameRect.X, gameRect.Bottom + 3, "Edit");
        var startBtn = new Button(editBtn.Frame.Right + 3, gameRect.Bottom + 3, "Start");
        var changeSpeedBtn = new Button(startBtn.Frame.Right + 3, gameRect.Bottom + 3, "Change Speed");

        win.Add(
            editBtn,
            startBtn,
            changeSpeedBtn
        );

        var rng = new Random();
        var frame = new FrameView(gameRect, "Life");
        var gameView = new BitView(0, 0, gameRect.Width - 2, gameRect.Height - 2, (x, y) => (rng.Next() % 2) == 0);
        frame.Add(gameView);
        win.Add(frame);        

        var autoEvent = new AutoResetEvent(false);
        var started = true;
        startBtn.Clicked += () => 
        {
            started = !started;
        };

        var stateTimer = new Timer(x => 
        {
            if (started)
            {
                gameView.SetNeedsDisplay();
                Application.Refresh();
            }
        }, 
        autoEvent, 1000, 500);

        Application.Run ();

    }
}