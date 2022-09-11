using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidPlayer : MonoBehaviour
{
    public GameManager gameManager;

    public float speed = 1;
    public float engineDamage = 1;
    public GameObject healthImage;
    public int playerHealth = 3;
    public float gameTimer = 60;
    public Text timerText;
    public Text endText;
    public Button endButton;
    public bool gameEnded = false;
    bool fuelCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        engineDamage = GameManager.Instance.thrusterHealth;
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer = gameTimer - Time.deltaTime;
        timerText.text = ("Time Remaining: " + (int)gameTimer);

        if (gameEnded == false)
        {
            if (Input.GetKey(KeyCode.A)) //left -
            {
                if (gameObject.transform.position.x > -40)
                {
                    gameObject.transform.position = gameObject.transform.position + (new Vector3(-speed, 0, 0) * Time.deltaTime);
                    gameObject.transform.Find("ThrustRF").gameObject.GetComponent<ParticleSystem>().Play(true);
                    gameObject.transform.Find("ThrustRB").gameObject.GetComponent<ParticleSystem>().Play(true);
                }
            }
            else if (Input.GetKey(KeyCode.D)) //right +
            {
                if (gameObject.transform.position.x < 40)
                {
                    gameObject.transform.position = gameObject.transform.position + (new Vector3(speed, 0, 0) * Time.deltaTime);
                    gameObject.transform.Find("ThrustLF").gameObject.GetComponent<ParticleSystem>().Play(true);
                    gameObject.transform.Find("ThrustLB").gameObject.GetComponent<ParticleSystem>().Play(true);
                }
            }
        }

        gameObject.transform.Find("ThrustRF").gameObject.GetComponent<ParticleSystem>().Stop(true);
        gameObject.transform.Find("ThrustRB").gameObject.GetComponent<ParticleSystem>().Stop(true);
        gameObject.transform.Find("ThrustLF").gameObject.GetComponent<ParticleSystem>().Stop(true);
        gameObject.transform.Find("ThrustLB").gameObject.GetComponent<ParticleSystem>().Stop(true);


        if (playerHealth <= 0)
        {
            gameObject.transform.Find("AsteroidShipExp").gameObject.GetComponent<ParticleSystem>().Play(true);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            playerHealth = playerHealth - 1;

            gameEnded = true;
            endText.text = "You got no fuel and suffered sever damage";
            GameManager.Instance.fuelAmount += 0;
            endText.enabled = true;
            endButton.gameObject.SetActive(true);
        }

        if(gameTimer <= 0)
        {
            gameEnded = true;
            if (fuelCollected == false)
            {
                switch (playerHealth)
                {
                    case 3:
                        endText.text = "Well done you got 50 fuel with out taking any damage!";
                        GameManager.Instance.fuelAmount += 50;
                        fuelCollected = true;
                        break;
                    case 2:
                        endText.text = "You got 40 fuel and only took minor damage";
                        GameManager.Instance.fuelAmount += 40;
                        fuelCollected = true;
                        break;
                    case 1:
                        endText.text = "You got 30 fuel but suffered major damage";
                        GameManager.Instance.fuelAmount += 30;
                        fuelCollected = true;
                        break;
                }
            }
            endText.enabled = true;
            endButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameEnded == false)
        {
            if (other.CompareTag("Asteroid"))
            {
                playerHealth = playerHealth - 1;
                engineDamage = engineDamage - Random.Range(0.1f, 0.15f);
                if(engineDamage < 0.1f)
                {
                    engineDamage = 0.1f;
                }
                speed = speed * engineDamage;

                switch (playerHealth)
                {
                    case 3:
                        healthImage.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 10);
                        break;
                    case 2:
                        healthImage.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 10);
                        break;
                    case 1:
                        healthImage.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 10);
                        break;
                    case 0:
                        healthImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 10);
                        break;
                }
            }
        }
        GameObject boom = Instantiate((gameObject.transform.Find("AsteroidShipExp").gameObject), other.transform.position, Quaternion.identity);
        boom.GetComponent<ParticleSystem>().Play(true);
        Destroy(other.gameObject);
    }

    public void LoadNextLevel()
    {
        GameManager.Instance.thrusterHealth = engineDamage;
        gameManager.LoadScene(1);
    }
}
