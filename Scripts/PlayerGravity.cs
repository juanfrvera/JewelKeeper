using UnityEngine;

internal class PlayerGravity : PlayerState
{
   public PlayerGravity(Player player) : base(player)
   {
   }

   public override void Enter()
   {
   }

   public override void Exit()
   {
   }

   public override PlayerState Update()
   {
      var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

      var gravDir = Physics.gravity.normalized.ToVec2();

      if (input != gravDir)
      {
         if (player.Grounded)
         {
            if (Player.ValidGravityDirection(input))
            {
               Player.ChangeGravityDirection(input);

               // Wait a little before enabling another gravity change
               return new PlayerWaiting(player);
            }
         }
      }

      return null;
   }
}