using System.Collections;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private GameManger gameManager;
    [SerializeField] private SpriteRenderer fuel_Sr;
    [SerializeField] private Sprite[] fuel_Sprite;
    [SerializeField] private BoxCollider2D fuel_BoxCol;

    void Start()
    {
        fuel_BoxCol= GetComponent<BoxCollider2D>();
        fuel_Sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("shot"))
        {
            StartCoroutine(IEfuelTimeDead());

        }

    }

    IEnumerator IEfuelTimeDead()
    {
        fuel_BoxCol.enabled = false;
        this.fuel_Sr.sprite = fuel_Sprite[1];
        yield return new WaitForSeconds(0.2f);
        this.fuel_Sr.sprite = fuel_Sprite[2];
        yield return new WaitForSeconds(0.2f);
        this.fuel_Sr.sprite = null;
        yield return new WaitForSeconds(8f);
        resetElements();

    }

    void resetElements()
    {

        this.fuel_Sr.sprite = fuel_Sprite[0];
        fuel_BoxCol.enabled = true;
    }

}
