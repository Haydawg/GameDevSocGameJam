using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidPlayer : MonoBehaviour
{
    public GameManager gameManager;

    public float speed = 1;
    public GameObject healthImage;
    public int playerHealth = 3;
    public float gameTimer = 60;
    public Text timerText;
    public Text endText;
    public Button endButton;
    bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
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
                if (gameObject.transform.position.x > -50)
                {
                    gameObject.transform.position = gameObject.transform.position + (new Vector3(-speed, 0, 0) * Time.deltaTime);
                    gameObject.transform.Find("ThrustRF").gameObject.GetComponent<ParticleSystem>().Play(true);
                    gameObject.transform.Find("ThrustRB").gameObject.GetComponent<ParticleSystem>().Play(true);
                }
            }
            else if (Input.GetKey(KeyCode.D)) //right +
            {
                if (gameObject.transform.position.x < 50)
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


        if (playerHealth == 0)
        {
            gameObject.transform.Find("AsteroidShipExp").gameObject.GetComponent<ParticleSystem>().Play(true);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            playerHealth = playerHealth - 1;

            gameEnded = true;
            endText.text = "You got no fuel and suffered sever damage";
            endText.enabled = true;
            endButton.gameObject.SetActive(true);
        }

        if(gameTimer <= 0)
        {
            gameEnded = true;
            switch(playerHealth)
            {
                case 3: endText.text = "Well done you got the fuel with out taking any damage!";
                    break;
                case 2:
                    endText.text = "You got the fuel and only took minor damage";
                    break;
                case 1:
                    endText.text = "You got the fuel but suffered major damage";
                    break;
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
    }

    public void LoadNextLevel()
    {
        gameManager.LoadScene(1);
    }
}
