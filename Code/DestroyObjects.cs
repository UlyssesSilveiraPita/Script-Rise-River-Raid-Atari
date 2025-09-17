using UnityEngine;

public class DestroyObjects : MonoBehaviour
{ 
    private void OnBecameInvisible()
   {
       Destroy(gameObject);
   }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("fuel") || col.CompareTag("enemy") || col.CompareTag("enemy2") || col.CompareTag("enemy3") || 
            col.CompareTag("colStage") || col.CompareTag("colStage2") || col.CompareTag("colStage3"))

        {
            Destroy(gameObject);
        }

    }


}
