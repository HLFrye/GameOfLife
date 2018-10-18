using System;
using System.Runtime.InteropServices;
using System.Timers;
using Terminal.Gui;

class GameOfLifeApplication 
{
    static PlatformID _platform = Environment.OSVersion.Platform;
 
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
            parent.Left, 
            parent.Top + 2, 
            parent.Width - 30, 
            parent.Height - 12
        );
    }

    static void Main ()
    {
        Application.Init ();
        var top = Application.Top;

	// Creates the top-level window to show
        var win = new Window (new Rect (0, 1, top.Frame.Width, top.Frame.Height-1), "Conway's Game of Life");
        top.Add (win);

        var gameRect = CalcGameRect(top.Frame);

        var editBtn = new Button(gameRect.X, gameRect.Bottom + 3, "Edit");
        var startBtn = new Button(editBtn.Frame.Right + 3, gameRect.Bottom + 3, "Start");
        var changeSpeedBtn = new Button(startBtn.Frame.Right + 3, gameRect.Bottom + 3, "Change Speed");

        win.Add(
            editBtn,
            startBtn,
            changeSpeedBtn
        );

        var frame = new FrameView(gameRect, "Life");
        var BoardView = new BoardView(0, 0, gameRect.Width - 2, gameRect.Height - 2);
     
        var statsView = new StatView(new Rect(
            top.Frame.Width - 30,
            top.Frame.Top + 2,
            28,
            top.Frame.Height - 12),
            BoardView
        );

        BoardView.Updated += statsView.UpdateStats;

        frame.Add(BoardView);
        win.Add(statsView);
        win.Add(frame);        

	// Creates a menubar
        var menu = new MenuBar (new MenuBarItem [] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "", NewGame),
                new MenuItem ("_Open", "", OpenGame),
                new MenuItem ("_Save", "", SaveGame),
                new MenuItem ("_Export", "", Export),
                new MenuItem ("_Close", "", CloseGame),
                new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
            }),
            new MenuBarItem ("_Insert", new MenuItem [] {
                new MenuItem("Still Life", "", Insert(BoardView, Entities.StillLife)),
                new MenuItem("Oscillator", "", Insert(BoardView, Entities.Oscillator)),
                new MenuItem("Glider", "", Insert(BoardView, Entities.Glider)),
                new MenuItem("Glider Gun", "", Insert(BoardView, Entities.GliderGun)),
                new MenuItem("Puffer Train", "", Insert(BoardView, Entities.PufferTrain)),
                new MenuItem("R-Pentomino", "", Insert(BoardView, Entities.RPentomino))
            }),
            new MenuBarItem("_Help", new MenuItem [] {
                new MenuItem("_About", "", HelpAbout)
            })
        });
        top.Add (menu);


        editBtn.Clicked += () => BoardView.TakeFocus(win);

        var started = false;
        startBtn.Clicked += () => 
        {
            started = !started;
            if (started)
            {
                startBtn.Text = "Stop";
                DisableTheMouse();
            }
            else
            {
                startBtn.Text = "Start";
                EnableTheMouse();
            }
        };

        var stateTimer = new Timer();
        stateTimer.AutoReset = false;
        stateTimer.Elapsed += (sender, e) => 
        {
            if (started)
            {
                BoardView.Update();
                Application.Refresh();
            }
            stateTimer.Start();
        };
        stateTimer.Interval = 50;
        stateTimer.Start();
        Application.Run();
    }

    [DllImport ("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle (int nStdHandle);

    [DllImport ("kernel32.dll")]
    static extern bool GetConsoleMode (IntPtr hConsoleHandle, out uint lpMode);


    [DllImport ("kernel32.dll")]
    static extern bool SetConsoleMode (IntPtr hConsoleHandle, uint dwMode);

    private static void DisableTheMouse()
    {
        if (_platform == PlatformID.Win32NT || _platform == PlatformID.Win32S || _platform == PlatformID.Win32Windows)
        {
            var InputHandle = GetStdHandle(-10);
            uint mode;
            GetConsoleMode(InputHandle, out mode);
            mode &= ~(uint)16;
            SetConsoleMode(InputHandle, mode);
        }
    }

    private static void EnableTheMouse()
    {
        if (_platform == PlatformID.Win32NT || _platform == PlatformID.Win32S || _platform == PlatformID.Win32Windows)
        {
            var InputHandle = GetStdHandle(-10);
            uint mode;
            GetConsoleMode(InputHandle, out mode);
            mode |= (uint)16;
            SetConsoleMode(InputHandle, mode);
        }
    }

  private static void HelpAbout()
  {
    throw new NotImplementedException();
  }

  private static void Export()
  {
    throw new NotImplementedException();
  }

  private static Action Insert(BoardView game, Entity stillLife)
  {
    return () => 
    {
      foreach (var point in stillLife.Cells)
      {
        game.Add(point.X, point.Y);
      }  
    };
  }

  private static void CloseGame()
  {
    throw new NotImplementedException();
  }

  private static void SaveGame()
  {
    throw new NotImplementedException();
  }

  private static void OpenGame()
  {
    throw new NotImplementedException();
  }

  private static void NewGame()
  {
    throw new NotImplementedException();
  }
}