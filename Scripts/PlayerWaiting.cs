using UnityEngine;

internal class PlayerWaiting : PlayerState
{
   const float WAIT_TIME = 0.5f;

   private float endTime;

   public PlayerWaiting(Player player) : base(player)
   {
   }

   public override void Enter()
   {
      endTime = Time.time + WAIT_TIME;
   }

   public override void Exit()
   {
   }

   public override PlayerState Update()
   {
      if(Time.time > endTime)
      {
         return new PlayerGravity(player);
      }

      return null;
   }
}