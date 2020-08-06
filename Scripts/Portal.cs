using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
   [SerializeField] float centerTime = 0.25f;
   [SerializeField] float travelTime = 1.75f;
   [SerializeField] Transform start, end;

   private IList<ICatcheable> catchedElements;

   private void OnTriggerEnter(Collider other)
   {
      var catcheable = other.GetComponent<ICatcheable>();

      if(catcheable != null)
      {
         if (catchedElements == null)
         {
            catchedElements = new List<ICatcheable>();

            catchedElements.Add(catcheable);

            StartCoroutine(TranslateCatched(catcheable));
         }
         else
         {
            if (!catchedElements.Contains(catcheable))
            {
               catchedElements.Add(catcheable);

               StartCoroutine(TranslateCatched(catcheable));
            }
         }
      }
   }

   private IEnumerator TranslateCatched(ICatcheable catched)
   {
      // Tell him that is catched
      catched.Catched();

      catched.Velocity = Vector3.zero;

      Vector3 difference = start.position - catched.Position;
      Vector3 travelAmount = difference / travelTime;

      float counter = 0;

      // Translate to center
      while (counter < centerTime)
      {
         catched.Translate(travelAmount * Time.fixedDeltaTime);
         counter += Time.fixedDeltaTime;
         yield return new WaitForFixedUpdate();
      }

      counter = 0;
      difference = end.position - catched.Position;
      travelAmount = difference / travelTime;

      while (counter < travelTime)
      {
         catched.Translate(travelAmount * Time.fixedDeltaTime);

         counter += Time.fixedDeltaTime;

         yield return new WaitForFixedUpdate();
      }

      catched.Position = end.position;

      catched.Velocity = Vector3.zero;
      catched.Freed();

      catchedElements.Remove(catched);
   }
}
