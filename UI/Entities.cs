using System;
using System.IO;
using System.Reflection;

public static class Entities
{
  private static Stream GetEmbeddedStream(string name) => Assembly.GetAssembly(typeof(Entities)).GetManifestResourceStream($"gui-cs-demo.Entities.{name}");

  public static Entity StillLife => new Entity("Still Life", new []{"OOO", "OOO", "OOO"});
  public static Entity Oscillator => new Entity("Oscillator", new []{"OOO"});
  public static Entity Glider => new Entity("Glider", GetEmbeddedStream("Glider.cells"));
  public static Entity GliderGun => new Entity("Glider Gun", GetEmbeddedStream("GliderGun.cells"));
  public static Entity PufferTrain => new Entity("Puffer Train", GetEmbeddedStream("HiveNudger2.cells"));
  public static Entity RPentomino => new Entity("R-Pentomino", GetEmbeddedStream("RPentomino.cells"));
}