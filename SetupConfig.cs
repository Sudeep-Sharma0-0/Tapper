using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Tapper
{
  public static class ConfigManager
  {
    private static readonly string home = Environment.GetEnvironmentVariable("HOME") ?? "";
    private static readonly string configDir = Path.Combine(home, ".config", "Tapper");
    private static readonly string configFile = Path.Combine(configDir, "tapper.conf");
    private static readonly string defaultConfig = Path.Combine(Directory.GetCurrentDirectory(), "config/tapper.conf");

    private static readonly string soundsDir = Path.Combine(configDir, "sounds");
    private static readonly string soundFile = Path.Combine(soundsDir, "default.wav");
    private static readonly string defaultSound = Path.Combine(Directory.GetCurrentDirectory(), "config/sounds/default.wav");

    public static Config CurrentConfig { get; private set; } = new Config();

    public static void SetupConfig()
    {
      if (!Directory.Exists(configDir))
      {
        Directory.CreateDirectory(configDir);
        Console.WriteLine("Created config directory: " + configDir);
      }
      if (!File.Exists(configFile) || new FileInfo(configFile).Length < 1)
      {
        if (File.Exists(defaultConfig))
        {
          File.Copy(defaultConfig, configFile, overwrite: true);
          Console.WriteLine("Copied default config to: " + configFile);
        }
        else
        {
          Console.WriteLine("⚠ Default config not found in project root: " + defaultConfig);
        }
      }

      if (!Directory.Exists(soundsDir))
      {
        Directory.CreateDirectory(soundsDir);
        Console.WriteLine("Created sounds directory: " + soundsDir);
      }
      if (!File.Exists(soundFile) || new FileInfo(soundFile).Length < 1)
      {
        if (File.Exists(defaultSound))
        {
          File.Copy(defaultSound, soundFile, overwrite: true);
          Console.WriteLine("Copied default sound to: " + soundFile);
        }
        else
        {
          Console.WriteLine("⚠ Default sound not found in project root: " + defaultSound);
        }
      }
    }

    public static void Load(string path = "")
    {
      if (string.IsNullOrEmpty(path))
      {
        SetupConfig();
        path = configFile;
      }

      try
      {
        var yaml = File.ReadAllText(path);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var config = deserializer.Deserialize<Config>(yaml) ?? new Config();

        if (!string.IsNullOrEmpty(config.Default?.Sound?.Path))
        {
          config.Default.Sound.Path = config.Default.Sound.Path.Replace("$HOME", home);
        }

        CurrentConfig = config;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed to load config: " + ex.Message);
        CurrentConfig = new Config();
      }
    }
  }
}
