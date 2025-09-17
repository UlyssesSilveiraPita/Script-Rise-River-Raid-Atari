using UnityEngine;

public class StageController : MonoBehaviour
{ 
    private void OnBecameInvisible()
   {
       gameObject.SetActive(false);
   }

}
