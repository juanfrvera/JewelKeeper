using UnityEngine;

public class Coin : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      var absorber = other.GetComponent<ICoinAbsorber>();

      if(absorber != null)
      {
         absorber.CoinAbsorbed(this);

         Destroy(gameObject);
      }
   }
}
