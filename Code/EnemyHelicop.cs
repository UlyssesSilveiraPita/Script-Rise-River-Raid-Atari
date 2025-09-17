using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHelicop : MonoBehaviour
{
    [Header("Script`s")]
    [SerializeField] public PlayerController playerController;
    [SerializeField] public GameManger gameManager;

    [Header("Componentes")]
    [SerializeField] public SpriteRenderer enemyDeadth_Sr;
    [SerializeField] public Rigidbody2D enemyHelicop_Rb;
    [SerializeField] public BoxCollider2D enemy_BoxC2D;
    [SerializeField] public Animator enemyHelicopAnima;
    [SerializeField] private AudioSource enemyHelicop_AudioSource;
    [SerializeField] private AudioClip enemyDead_fx;

    [Header("Variaveis")]
    [SerializeField] public float enemyHelicopSpeed;
    [SerializeField] public Vector3 startPosition;
    [SerializeField] public bool startFacingRight = true; // pode escolher no Inspector
    [SerializeField] public int enemyPoints;
    public bool isDead;
    public bool facingRight;
    public bool isVisible; // controla se está na tela




    void Start()
    {
        enemyHelicop_Rb = GetComponent<Rigidbody2D>();
        enemy_BoxC2D = GetComponent<BoxCollider2D>();
        enemyDeadth_Sr = GetComponent<SpriteRenderer>();
        enemyHelicopAnima = GetComponent<Animator>();

        // Salva a posição inicial individual deste inimigo
        startPosition = transform.localPosition;

        // define a direção inicial
        facingRight = startFacingRight;

        // garante que o sprite já comece virado corretamente
        if (!facingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void OnBecameVisible()
    {
        isVisible = true; // só liga quando aparece na tela
    }
    private void OnBecameInvisible()
    {
        isVisible = false;

        if (!isDead) // se saiu da tela sem ser morto
        {
            enemyHelicop_Rb.linearVelocity = new Vector2(0, gameManager.speedStage);
            //resetElements(); // reseta como se tivesse morrido
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isDead)
        {
            float dir = facingRight ? 1f : -1f;
            enemyHelicop_Rb.linearVelocity = new Vector2(enemyHelicopSpeed * dir, gameManager.speedStage);
        }
        else
        {
            enemyHelicop_Rb.linearVelocity = new Vector2(0, gameManager.speedStage); // parado se morto
        }


    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("shot"))
        {
            isDead = true;
            gameManager.AddScore(enemyPoints); // soma pontos só 1 vez
            StartCoroutine(IEdeadTime());

        }
        if (col.CompareTag("colStage") || col.CompareTag("colStage2") || col.CompareTag("colStage3"))
        {
            FlipDirection();
        }

    }

    IEnumerator IEdeadTime()
    {
        enemy_BoxC2D.enabled = false;
        enemyHelicop_AudioSource.PlayOneShot(enemyDead_fx);
        enemyHelicopAnima.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);

        enemyHelicopAnima.SetBool("isDead", false);
        enemyDeadth_Sr.enabled = false;

        yield return new WaitForSeconds(5f);
        enemy_BoxC2D.enabled = true;
        enemyDeadth_Sr.enabled = true;
        isDead = false;

    }


    void FlipDirection()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // inverte visualmente
        transform.localScale = scale;
    }
   
}