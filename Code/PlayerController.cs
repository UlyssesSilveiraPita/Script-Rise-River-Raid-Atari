using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public EnemyHelicop enemy { get; set; }

    [SerializeField] public Rigidbody2D player_rb;
    [SerializeField] public Transform player_transform;
    [SerializeField] public Transform shotPosition;
    [SerializeField] public GameObject shotPrefab;
    [SerializeField] public SpriteRenderer playerSpriteRenderer;
    [SerializeField] public Sprite[] playerSprite;
    [SerializeField] public float player_speed;
    [SerializeField] public float velocidadeTiro;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip shot_fx;


    public bool isDead;

    public GameManger gameManager;

    void Start()
    {
        player_rb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        shotPosition = GetComponent<Transform>();
        shotPrefab = shotPrefab;

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManger>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            inputManager();
            spriteFlyPosition();
        }
    }

    void inputManager() // funcao de controle Inputs
    {
        //Variaveis de controle de movimento do Player.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        //Atribuindo o movimento ao componente player_rb 
        player_rb.linearVelocity = new Vector2(horizontal * player_speed, 0);

        if (vertical > 0)
        {
            gameManager.speedStage += gameManager.speedStage * Time.deltaTime;
        }

        else if (vertical < 0)
        {
            gameManager.speedStage -= gameManager.speedStage * Time.deltaTime;
        }

        gameManager.speedStage = Mathf.Clamp(gameManager.speedStage, -4f, -1f);

        if (Input.GetButtonUp("Vertical"))
        {
            gameManager.speedStage = -2;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            playerAudioSource.PlayOneShot(shot_fx);
            shot();
        }

    }


    private void OnCollisionEnter2D(Collision2D col)
    {

        switch (col.collider.tag)
        {
            case ("colStage"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[0].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[0].transform.position = new Vector2(0, -5f);
                StartCoroutine(IEdeadTime());
               
                break;
            case ("colStage2"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[1].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[1].transform.position = new Vector2(0, -5f);
                StartCoroutine(IEdeadTime());
               
                break;
            case ("colStage3"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[2].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[2].transform.position = new Vector2(0, -5f);
                StartCoroutine(IEdeadTime());
                
                break;
            case ("enemy"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[0].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[0].transform.position = new Vector2(0, -5f);
                StartCoroutine(IEdeadTime());
                break;
            /*case ("fuel"):
                print("gasolina");
                gameManger.AddFuel(0.6f); // recarrega 10%
                print("gasolina2");
                //Destroy(col.gameObject);
                break;*/
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case ("newStage2"):
                gameManager.stages[1].SetActive(true);
                gameManager.stage_rb[1] = gameManager.stages[1].GetComponent<Rigidbody2D>();
                gameManager.stage_rb[1].transform.position = new Vector2(0, 9.7f);
                
                break;
            case ("newStage3"):
                gameManager.stages[2].SetActive(true);
                gameManager.stage_rb[2] = gameManager.stages[2].GetComponent<Rigidbody2D>();
                gameManager.stage_rb[2].transform.position = new Vector2(0, 5);
                
                break;
            case ("newStage1"):
                gameManager.stages[0].SetActive(true);
                gameManager.stage_rb[0] = gameManager.stages[0].GetComponent<Rigidbody2D>();
                gameManager.stage_rb[0].transform.position = new Vector2(0, 7);
              
                break;
            case ("colStage"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[0].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[0].transform.position = new Vector2(0, -5f);
                gameManager.resetElements();
                StartCoroutine(IEdeadTime());

                break;
            case ("colStage2"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[1].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[1].transform.position = new Vector2(0, -5f);
                gameManager.resetElements();
                StartCoroutine(IEdeadTime());
                break;
            case ("colStage3"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[2].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[2].transform.position = new Vector2(0, -5f);
                gameManager.resetElements();
                StartCoroutine(IEdeadTime());
                break;
            case ("enemy"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[0].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[0].transform.position = new Vector2(0, -5f);
                StartCoroutine(IEdeadTime());
                break;
            case ("enemy2"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[1].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[1].transform.position = new Vector2(0, -5f);
                StartCoroutine(IEdeadTime());
                break;
            case ("enemy3"):
                player_rb.transform.position = new Vector2(gameManager.checkPoints[2].transform.position.x, gameManager.mainCamera.transform.position.y);
                gameManager.stage_rb[2].transform.position = new Vector2(0, -5f);
                StartCoroutine(IEdeadTime());
                break;
            case ("fuel"):
                //gameManager.AddFuel(0.3f); // recarrega 30%
                //Destroy(col.gameObject);
                break;
                
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("fuel"))
        {
            // enche enquanto está dentro do trigger
            gameManager.moreFuel(4f);

           
        }
    }

    //troca de sprite ao movimentar para esquerda ou direita
    void spriteFlyPosition()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0)
        {
            playerSpriteRenderer.sprite = playerSprite[1];
        }
        else if (horizontal < 0)
        {
            playerSpriteRenderer.sprite = playerSprite[2];
        }
        else
        {
            playerSpriteRenderer.sprite = playerSprite[0];
        }
    }


   public IEnumerator IEdeadTime()
    {

        if (isDead) yield break; // previne multiplas chamadas

        isDead = true;

        while (isDead == true)
        {
            playerSpriteRenderer.sprite = playerSprite[3];
            yield return new WaitForSeconds(0.2f);
            playerSpriteRenderer.sprite = playerSprite[4];
            yield return new WaitForSeconds(0.2f);
            playerSpriteRenderer.sprite = playerSprite[0];
            //yield return new WaitForSeconds(2f);
            
            isDead = false;
        }
        

    }

    void shot() // Criando os Tiros //
    {
        GameObject temp = Instantiate(shotPrefab);

        temp.transform.position = shotPosition.position;
        temp.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, velocidadeTiro);

    }

}


