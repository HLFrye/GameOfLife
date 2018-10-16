using System;
using Terminal.Gui;
using System.Diagnostics;

public class StatView: View 
{
  private readonly BoardView _board;
  private readonly Label _numCellsLabel;
  private readonly Label _numGenerationsLabel;
  private readonly Label _updateTimeLabel;
  private readonly Label _totalMemoryLabel;
  private readonly Process _currentProcess;

  public StatView(Rect area, BoardView board) : base(area)
  {
    _board = board;
    _currentProcess = Process.GetCurrentProcess();
    
    var frame = new FrameView(new Rect(0, 0, area.Width, area.Height), "Stats");

    var cellHeaderLabel = new Label(0, 0, "Cell Count:");
    _numCellsLabel = new Label(new Rect(0, 1, area.Width - 2, 1), NumCells);
    _numCellsLabel.TextAlignment = TextAlignment.Right;

    var generationsHeaderLabel = new Label(0, 3, "Generation Count:");
    _numGenerationsLabel = new Label(new Rect(0, 4, area.Width - 2, 1), NumGenerations);
    _numGenerationsLabel.TextAlignment = TextAlignment.Right;

    var updateTimeHeaderLabel = new Label(0, 6, "Update Time");
    _updateTimeLabel = new Label(new Rect(0, 7, area.Width - 2, 1), UpdateTime);
    _updateTimeLabel.TextAlignment = TextAlignment.Right;

    var totalMemoryHeaderLabel = new Label(0, 9, "Total Memory:");
    _totalMemoryLabel = new Label(new Rect(0, 10, area.Width - 2, 1), TotalMemory);
    _totalMemoryLabel.TextAlignment = TextAlignment.Right;


    frame.Add(cellHeaderLabel);
    frame.Add(_numCellsLabel);
    frame.Add(generationsHeaderLabel);
    frame.Add(_numGenerationsLabel);
    frame.Add(updateTimeHeaderLabel);
    frame.Add(_updateTimeLabel);
    frame.Add(totalMemoryHeaderLabel);
    frame.Add(_totalMemoryLabel);

    Add(frame);
  }

  public void UpdateStats()
  {
    _numCellsLabel.Text = NumCells;
    _numGenerationsLabel.Text = NumGenerations;
    _updateTimeLabel.Text = UpdateTime;
    _totalMemoryLabel.Text = TotalMemory;
    SetNeedsDisplay();
  }

  private string NumCells 
  {
    get 
    {
      return _board.GetCellCount().ToString();
    }
  }

  private string NumGenerations
  {
    get
    {
      return _board.NumGenerations.ToString();
    }
  }

  private string UpdateTime
  {
    get
    {
      return $"{_board.LastUpdate.TotalMilliseconds}ms";
    }
  }

  private string TotalMemory
  {
    get
    {
      return $"{_currentProcess.PrivateMemorySize64:n0} bytes";
    }
  }
}