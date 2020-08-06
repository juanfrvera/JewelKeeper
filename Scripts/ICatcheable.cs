using UnityEngine;

internal interface ICatcheable
{
   Vector3 Position { get; set;  }
   Vector3 Velocity { set; }

   void Catched();
   void AddForce(Vector3 force);
   void Translate(Vector3 amount);
   void Freed();
}