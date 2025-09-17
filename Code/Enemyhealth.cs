using System.Collections;
using UnityEngine;

public class Enemyhealth : MonoBehaviour
{
    [SerializeField] public PlayerController playerController;
    [SerializeField] private SpriteRenderer enemyDeadth_Sr;
    [SerializeField] private Sprite[] enemysSprites;
    [SerializeField] private BoxCollider2D enemy_BoxC2D;
   
    private bool isDead;

    void Start()
    {
        enemy_BoxC2D = GetComponent<BoxCollider2D>();
        enemyDeadth_Sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("shot"))
        {
            isDead = true;
            StartCoroutine(IEdeadTime());
            
        }
    }

    IEnumerator IEdeadTime()
    {
        enemy_BoxC2D.enabled = false;
        this.enemyDeadth_Sr.sprite = playerController.playerSprite[3];
        yield return new WaitForSeconds(0.2f);
        this.enemyDeadth_Sr.sprite = playerController.playerSprite[4];
        yield return new WaitForSeconds(0.2f);
        this.enemyDeadth_Sr.sprite = null;
        yield return new WaitForSeconds(3f);
        resetElements();

    }

    void resetElements()
    {

        this.enemyDeadth_Sr.sprite = enemysSprites[0];
        enemy_BoxC2D.enabled = true;
    }
}
