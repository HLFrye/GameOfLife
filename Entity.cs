using System;
using System.Collections.Generic;
using System.IO;
using Terminal.Gui;

public class Entity
{
  public Entity() {}

  public Entity(Stream stream)
  {

  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public string URL { get; private set; }
  public IList<Point> Cells { get; private set; }
}