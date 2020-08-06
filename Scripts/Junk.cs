using UnityEngine;

public class Junk : MonoBehaviour, ICatcheable
{
   public Vector3 Position
   {
      get => transform.position;
      set => transform.position = value;
   }
   public Vector3 Velocity { set => GetComponent<Rigidbody>().velocity = value; }

   public void AddForce(Vector3 force)
   {
      GetComponent<Rigidbody>().AddForce(force);
   }
   public void Catched()
   {
      GetComponent<Rigidbody>().useGravity = false;
   }

   public void Freed()
   {
      GetComponent<Rigidbody>().useGravity = true;
      // The current level is my parent, I need a new parent
      Play.JoinNextLevel(transform, transform.parent.gameObject);
   }

   public void Translate(Vector3 amount)
   {
      transform.Translate(amount);
   }
}
