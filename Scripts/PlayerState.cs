using UnityEngine;

internal abstract class PlayerState
{
   protected Player player;
   public PlayerState(Player player)
   {
      this.player = player;
   }

   public abstract void Enter();
   public abstract void Exit();

   public virtual PlayerState Update() => null;
   public virtual PlayerState CollisionEnter(Collision collision) => null;
   public virtual PlayerState CollisionExit(Collision collision) => null;
}