using System;

namespace StarGravity.GamePlay.Common.Components
{
  [Serializable]
  public struct CheckOutOfScreenComponent
  {
    public float LeftOffset;
    public float TopOffset;
    public float RightOffset;
    public float BottomOffset;
    public OutOfScreenAction IfOutAction;
  }
}