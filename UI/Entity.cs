using System;
using System.Collections.Generic;
using System.IO;
using Terminal.Gui;

public class Entity
{
  private static IList<Point> ReadLines(IList<string> lines, out Rect bounds)
  {
    int mw = 0;
    int mh = 0;
    var cells = new List<Point>();
    int y = 0;
    foreach (var line in lines)
    {
      int x = 0;
      foreach (var c in line)
      {
        switch (c)
        {
          case 'O':
            cells.Add(new Point(x, y));
            if (x > mw) mw = x;
            if (y > mh) mh = y;
            break;
          default:
            break;
        }
        x++;
      }
      y++;
    }
    bounds = new Rect(0, 0, mw+1, mh+1);
    return cells;
  }

  public Entity() {}

  public Entity(string name, string[] lines)
  {
    Name = name;
    Cells = ReadLines(lines, out Rect size);
    Size = size;
  }
  public Entity(string name, Stream stream)
  {
    Name = name;
    using (var reader = new StreamReader(stream))
    {
      FullName = reader.ReadLine().Substring(7);
      var map = new List<string>();
      while (!reader.EndOfStream)
      {
        var line = reader.ReadLine();
        if (line.StartsWith("!"))
        {
          if (!string.IsNullOrEmpty(Description))
            Description += "\n";
          Description += line.Substring(1);
        }
        else
        {
          map.Add(line);
        }
      }
      Cells = ReadLines(map, out Rect size);
      Size = size;
    }
  }

  public string Name { get; private set; }
  public string FullName { get; private set; }
  public string Description { get; private set; }
  public IList<Point> Cells { get; private set; }
  public Rect Size { get; private set; }
}