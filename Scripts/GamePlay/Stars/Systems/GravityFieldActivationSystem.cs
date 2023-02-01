using Leopotam.Ecs;
using StarGravity.GamePlay.Common.Components;
using StarGravity.GamePlay.Stars.Components;

namespace StarGravity.GamePlay.Stars.Systems
{
  public class GravityFieldActivationSystem : IEcsRunSystem
  {
    private EcsFilter<Collider2DComponent> _gravityFields;

    public void Run()
    {
      foreach (int i in _gravityFields)
      {
        ref var entity = ref _gravityFields.GetEntity(i);
        ref var collider = ref _gravityFields.Get1(i);

        collider.Collider2D.enabled = !entity.Has<SwitchOff>();
      }
    }
  }
}