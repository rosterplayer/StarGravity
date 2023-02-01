namespace StarGravity.GamePlay.Planets
{
  public class MovePoints
  {
    private readonly float[] _points;
    private int _counter;

    public MovePoints(float[] points)
    {
      _points = points;
    }

    public float? GetNextPointX()
    {
      if (_counter >= _points.Length)
        return null;

      _counter++;
      return _points[_counter - 1];
    }
  }
}