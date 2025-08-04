using System.Diagnostics;

namespace Tapper
{
  public class TapperEngine
  {
    private readonly string soundFile;

    public TapperEngine(string soundFile)
    {
      this.soundFile = soundFile;
    }

    public async Task StartAsync()
    {
      string byIdPath = "/dev/input/by-id/";
      if (!Directory.Exists(byIdPath))
      {
        Console.WriteLine($"{byIdPath} not found!");
        return;
      }

      var keyboardDevices = Directory.GetFiles(byIdPath)
          .Where(f => f.EndsWith("-event-kbd"))
          .ToList();

      if (!keyboardDevices.Any())
      {
        Console.WriteLine("No keyboards found!");
        return;
      }

      Console.WriteLine("Found keyboards:");
      foreach (var dev in keyboardDevices)
        Console.WriteLine(dev);

      var tasks = keyboardDevices.Select(dev => Task.Run(() => ListenForKeys(dev)));
      await Task.WhenAll(tasks);
    }

    private void ListenForKeys(string devicePath)
    {
      Console.WriteLine($"Listening on {devicePath}");

      string realPath = Path.GetFullPath(devicePath);
      using (FileStream fs = new FileStream(realPath, FileMode.Open, FileAccess.Read))
      {
        byte[] buffer = new byte[24];

        while (true)
        {
          int bytesRead = fs.Read(buffer, 0, buffer.Length);
          if (bytesRead == buffer.Length)
          {
            ushort type = BitConverter.ToUInt16(buffer, 16);
            int value = BitConverter.ToInt32(buffer, 20);

            if (type == 1 && value == 1)
            {
              Console.WriteLine("Keypress detected, playing sound!");
              PlaySound();
            }
          }
        }
      }
    }

    private void PlaySound()
    {
      Console.WriteLine("Playing sound: " + soundFile);
      try
      {
        ProcessStartInfo psi = new ProcessStartInfo
        {
          FileName = "paplay",
          Arguments = soundFile,
          UseShellExecute = false,
          CreateNoWindow = true
        };
        Process.Start(psi);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error playing sound: {ex.Message}");
      }
    }
  }
}
