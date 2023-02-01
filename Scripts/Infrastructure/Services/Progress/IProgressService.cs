using System;
using StarGravity.Data;

namespace StarGravity.Infrastructure.Services.Progress
{
  public interface IProgressService
  {
    UserData UserData { get; set; }
    event Action OnDataChanged;
    void Load();
    void Save();
    void ResetTutorials();
    bool IsValidBestScore(LBEntry userLBEntry);
  }
}