using System;
using UnityEngine;

namespace StarGravity.GamePlay.Player.Perks
{
  public abstract class ShipPerkWithCooldown : ShipPerk
  {
    public float Cooldown = 5;
    
    private float _cooldownLeft;

    public event Action OnStartCooldown;
    
    private void Update()
    {
      if (IsCooldown())
        _cooldownLeft -= Time.deltaTime;
      
      OnUpdate();
    }

    protected virtual void OnUpdate()
    {
    }

    protected bool IsCooldown() => 
      _cooldownLeft > 0;

    protected void SetCooldown()
    {
      _cooldownLeft = Cooldown;
      OnStartCooldown?.Invoke();
    }
  }
}