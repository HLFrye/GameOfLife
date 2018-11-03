using System;
using System.IO;
using System.Reflection;

public static class Entities
{
  private static Stream GetEmbeddedStream(string name) => Assembly.GetAssembly(typeof(Entities)).GetManifestResourceStream($"gui-cs-demo.Entities.{name}");

  public static Entity StillLife => new Entity("Still Life", new []{"OOO", "OOO", "OOO"});
  public static Entity Oscillator => new Entity("Oscillator", new []{"OOO"});
  public static Entity Glider => new Entity(GetEmbeddedStream("Glider.cells"));
  public static Entity GliderGun => new Entity(GetEmbeddedStream("GliderGun.cells"));
  public static Entity PufferTrain => new Entity(GetEmbeddedStream("HiveNudger2.cells"));
  public static Entity RPentomino => new Entity(GetEmbeddedStream("RPentomino.cells"));
}