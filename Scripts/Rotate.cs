using UnityEngine;

public class Rotate : MonoBehaviour
{
   [SerializeField] Vector3 rotation;
   private void FixedUpdate()
   {
      transform.Rotate(rotation*Time.fixedDeltaTime);
   }
}
