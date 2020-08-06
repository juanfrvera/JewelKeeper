using UnityEngine;

internal class PlayerCatched : PlayerState
{

   public PlayerCatched(Player player) : base(player)
   {
   }

   public override void Enter()
   {
      Player.ChangeGravityDirection(Vector2.zero);
   }

   public override void Exit()
   {
   }
}