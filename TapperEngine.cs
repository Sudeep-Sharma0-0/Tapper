using Tmds.DBus;
using System.Diagnostics;

namespace Tapper
{
  [DBusInterface("org.freedesktop.IBus.Engine")]
  interface IEngine : IDBusObject
  {
    Task<bool> ProcessKeyEventAsync(uint keyval, uint keycode, uint state);
  }

  class TapperEngine : IDBusObject, IEngine
  {
    public ObjectPath ObjectPath => new ObjectPath("/org/freedesktop/IBus/Engine/KeySound");

    public readonly string defaultSound;

    public TapperEngine(string defaultSound)
    {
      this.defaultSound = defaultSound;
    }

    public Task<bool> ProcessKeyEventAsync(uint keyval, uint keycode, uint state)
    {
      try
      {
        var psi = new ProcessStartInfo("paplay", defaultSound)
        {
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = true,
        };
        Process.Start(psi);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed to play sound: " + ex.Message);
      }
      return Task.FromResult(false);
    }
  }
}
