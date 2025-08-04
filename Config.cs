namespace Tapper
{
  public class Config
  {
    public DefaultConfig Default { get; set; } = new DefaultConfig();
  }

  public class DefaultConfig
  {
    public SoundConfig Sound { get; set; } = new SoundConfig();
  }

  public class SoundConfig
  {
    public string Path { get; set; } = "";
  }
}
