using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, ICatcheable
{
   const float GRAVITY_AMOUNT = 20f;

   [SerializeField] float groundDistance = 0.7f;
   [SerializeField] LayerMask groundLayer;
   private PlayerState state;
   private Rigidbody rig;

   public static bool ValidGravityDirection(Vector2 dir)
   {
      return (dir.x * dir.y == 0) && (dir.x + dir.y != 0);
   }

   public static void ChangeGravityDirection(Vector2 newGravityDirection)
   {
      Physics.gravity = newGravityDirection.ToVec3() * GRAVITY_AMOUNT;
   }

   public Vector3 Position
   {
      get => transform.position;
      set => transform.position = value;
   }
   public Vector3 Velocity
   {
      get => rig.velocity;
      set => rig.velocity = value;
   }

   public bool Grounded
   {
      get {
         var grav = Physics.gravity.normalized;
         // Perpendicular vector
         var perp = Vector2.Perpendicular(grav.ToVec2()).ToVec3();

         var firstPoint = transform.position + perp / 2;
         var secPoint = transform.position - perp / 2;

         // Raycast at the center and the corners
         return grav == Vector3.zero 
           || Physics.Raycast(transform.position, grav, groundDistance, groundLayer)
           || Physics.Raycast(firstPoint, grav, groundDistance, groundLayer)
           || Physics.Raycast(secPoint, grav, groundDistance, groundLayer);
      }
   }

   void ICatcheable.Catched()
   {
      ChangeState(new PlayerCatched(this));
      
      // Go to next level
      Play.NextLevel();
   }

   void ICatcheable.AddForce(Vector3 force)
   {
      rig.AddForce(force);
   }

   void ICatcheable.Translate(Vector3 translation)
   {
      transform.Translate(translation);
   }

   void ICatcheable.Freed()
   {
      ChangeState(new PlayerGravity(this));
   }


   private void ChangeState(PlayerState state)
   {
      this.state.Exit();
      state.Enter();
      this.state = state;
   }

   /// <summary>
   /// Only changes state if the new state is not null and is different than the current
   /// </summary>
   /// <param name="state"></param>
   private void ChangeIfNew(PlayerState state)
   {
      if(state != null && state != this.state)
      {
         ChangeState(state);
      }
   }

   private void OnCollisionEnter(Collision collision)
   {
      ChangeIfNew(state.CollisionEnter(collision));
   }

   private void OnCollisionExit(Collision collision)
   {
      ChangeIfNew(state.CollisionExit(collision));
   }

   private void Update()
   {
      ChangeIfNew(state.Update());
   }

   private void Start()
   {
      rig = GetComponent<Rigidbody>();
      
      state = new PlayerGravity(this);
      state.Enter();
   }
}