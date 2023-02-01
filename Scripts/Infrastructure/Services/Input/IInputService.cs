namespace StarGravity.Infrastructure.Services.Input
{
  public interface IInputService
  {
    public bool PressInput { get; }
    public bool Paused { get; }
    bool UpInput { get; }
    bool DownInput { get; }
    public void GainControl();
    public void ReleaseControl();
    void Run();
  }
}