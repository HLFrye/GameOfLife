using System;
using System.Collections.Generic;
using System.IO;
using Terminal.Gui;

public class Entity
{
  private static IList<Point> ReadLines(IList<string> lines)
  {
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
            break;
          default:
            break;
        }
        x++;
      }
      y++;
    }
    return cells;
  }

  public Entity() {}

  public Entity(string name, string[] lines)
  {
    Name = name;
    Cells = ReadLines(lines);
  }
  public Entity(Stream stream)
  {
    using (var reader = new StreamReader(stream))
    {
      Name = reader.ReadLine().Substring(7);
      var map = new List<string>();
      while (!reader.EndOfStream)
      {
        var line = reader.ReadLine();
        if (line.StartsWith("!"))
        {
          if (!string.IsNullOrEmpty(Description))
            Description += "\n";
          Description += line.Substring(0);
        }
        else
        {
          map.Add(line);
        }
      }
      Cells = ReadLines(map);
    }
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public IList<Point> Cells { get; private set; }
}