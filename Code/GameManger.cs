using TMPro;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public EnemyHelicop helicop { get; set; }
    public PlayerController PlayerController { get; set; }

    public PlayerController playerController;
    [SerializeField] public GameObject[] stages;
    [SerializeField] public Rigidbody2D[] stage_rb;
    [SerializeField] public float speedStage = -2f;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public Transform[] checkPoints;
    [SerializeField] public GameObject fuelBar;
    [SerializeField] public Transform minFuel;
    [SerializeField] public Transform maxFuel;
    [SerializeField] public float fuelDuration = 30f;
    [SerializeField] private TextMeshProUGUI scorePoints_txt;
    

    private float currentFuel; // 0 =  vazio, 1 = cheio
    private int score;

    void Start()
    {
        stages[0].SetActive(true);
        stage_rb[0] = stages[0].GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        checkPoints[0].transform.position = checkPoints[0].transform.position;

        currentFuel = 1f;

        score = 0;
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stages.Length; i++)
        {

            if (stage_rb[i] != null)
            {
                stage_rb[i].linearVelocity = new Vector2(0, speedStage);

            }
        }

        updateFuel();
    }

    private void LateUpdate()
    {
        // controle camera

        mainCamera.transform.position = new Vector3(0, 0.7f, -10f);

        
    }

    public void resetElements()
    {
        helicop.gameObject.SetActive(true);
        transform.localPosition = helicop.startPosition; // volta para o lugar de origem

        helicop.isDead = false; // revive de verdade
        helicop.enemyHelicopAnima.SetBool("isDead", false);

    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp01(currentFuel);
    }

    public void updateFuel ()
    {

        // gasta combustível com o tempo
        currentFuel -= Time.deltaTime / fuelDuration;
        currentFuel = Mathf.Clamp01(currentFuel); // nunca passa de 0 a 1

        // move a barra entre maxFuel e minFuel
        fuelBar.transform.localPosition = Vector3.Lerp(maxFuel.localPosition, minFuel.localPosition, 1f - currentFuel);

        // acabou o combustível -> player morre
        if (currentFuel <= 0f)
        {
            playerController.transform.position = new Vector2(0, 0f);
           
            playerController.StartCoroutine(playerController.IEdeadTime());
            currentFuel = 1f;

            //Destroy(playerController.gameObject);
        }

    }

    public void moreFuel(float rate)
    {

        // adiciona combustível suavemente enquanto o player está no trigger
        currentFuel += (Time.deltaTime / fuelDuration) * rate;
        currentFuel = Mathf.Clamp01(currentFuel);

    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scorePoints_txt.text = score.ToString();
    }
    public bool IsFuelFull()
    {
        return currentFuel >= 1f;
    }

}
