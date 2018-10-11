using System;
using System.Threading;
using Terminal.Gui;

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

        var gameRect = CalcGameRect(top.Frame);

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
        var gameView = new GameView(0, 0, gameRect.Width - 2, gameRect.Height - 2);
        frame.Add(gameView);
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
                new MenuItem("Still Life", "", Insert(gameView, Entities.StillLife)),
                new MenuItem("Oscillator", "", Insert(gameView, Entities.Oscillator)),
                new MenuItem("Glider", "", Insert(gameView, Entities.Glider)),
                new MenuItem("Glider Gun", "", Insert(gameView, Entities.GliderGun)),
                new MenuItem("Puffer Train", "", Insert(gameView, Entities.PufferTrain)),
                new MenuItem("R-Pentomino", "", Insert(gameView, Entities.RPentomino))
            }),
            new MenuBarItem("_Help", new MenuItem [] {
                new MenuItem("_About", "", HelpAbout)
            })
        });
        top.Add (menu);


        editBtn.Clicked += () => gameView.TakeFocus(win);

        var autoEvent = new AutoResetEvent(false);
        var started = false;
        startBtn.Clicked += () => 
        {
            started = !started;
            if (started)
            {
                startBtn.Text = "Stop";
            }
            else
            {
                startBtn.Text = "Start";
            }
        };

        var stateTimer = new Timer(x => 
        {
            if (started)
            {
                gameView.Update();
                Application.Refresh();
            }
        }, 
        autoEvent, 1000, 500);

        Application.Run ();

    }

  private static void HelpAbout()
  {
    throw new NotImplementedException();
  }

  private static void Export()
  {
    throw new NotImplementedException();
  }

  private static Action Insert(GameView game, Entity stillLife)
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