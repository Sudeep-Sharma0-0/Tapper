namespace Tapper
{
  class Program
  {
    static async Task Main(string[] args)
    {
      ConfigManager.Load();

      var soundPath = ConfigManager.CurrentConfig.Default.Sound.Path;

      if (string.IsNullOrEmpty(soundPath))
      {
        Console.WriteLine("No sound path configured. Please set the sound path in your config.");
        return;
      }

      Console.WriteLine("Using sound: " + soundPath);

      var engine = new TapperEngine(soundPath);
      await engine.StartAsync();

      Console.WriteLine("TapperEngine is running. Press Ctrl+C to exit.");
      await Task.Delay(-1);
    }
  }
}
