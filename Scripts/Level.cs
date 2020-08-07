using UnityEngine;

internal class Level : MonoBehaviour
{
   [SerializeField] Transform startTranform;

   public Vector3 StartPosition => startTranform.position;

   public void Show()
   {
      gameObject.SetActive(true);
   }
   public void Hide()
   {
      gameObject.SetActive(false);
   }
}