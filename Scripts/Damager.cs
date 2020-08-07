using DelegateLib;
using UnityEngine;

public class Damager : MonoBehaviour
{
   /// <summary>
   /// Passes the damaged GameObject as parameter
   /// </summary>
   public event TDelegate<GameObject> OnDamageApplied;

   [SerializeField] float damage;
   private void OnCollisionEnter(Collision collision)
   {
      var damageable = collision.gameObject.GetComponent<IDamageable>();

      if(damageable != null)
      {
         damageable.GetDamage(damage);

         OnDamageApplied?.Invoke(collision.gameObject);
      }
   }
}
