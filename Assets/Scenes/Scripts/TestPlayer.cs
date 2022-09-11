using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{

    public GameObject currentLocation;
    public float moveTime = 0;
    public GameObject playerCamera;
    public GameObject enemy;
    public bool alive = true;
    public Text fuelLeftText;
    public List<GameObject> engineFX = new List<GameObject>();
    public Text launchGameText;
    public Text endText;
    public GameObject exitButton;
    public GameObject damageButton;
    public Text damageText;

    public AudioClip engineSound;

    Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        fuelLeftText.text = "Fuel left: " + GameManager.Instance.fuelAmount;
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.position = new Vector3(gameObject.transform.position.x, 2600, gameObject.transform.position.z);
        if (nextPos == gameObject.transform.position)
        {
            foreach (GameObject engine in engineFX)
            {
                engine.GetComponent<ParticleSystem>().Stop(true);
            }
        }

        if (currentLocation != null)
        {
            if (currentLocation.GetComponent<PlanetScript>().finalPLanet == true)
            {
                endText.enabled = true;
                exitButton.SetActive(true);
            }
        }

        if(alive == false)
        {
            endText.text = "The Enenmy caught you";
            endText.enabled = true;
            exitButton.SetActive(true);
        }

        launchGameText.text = currentLocation.GetComponent<PlanetScript>().minigame.ToString();
    }

    public void MoveToNextPlanet(GameObject nextPlanet, float travelCost)
    {
        if (travelCost <= GameManager.Instance.fuelAmount)
        {
            nextPos = new Vector3(nextPlanet.transform.position.x, 400, nextPlanet.transform.position.z);
            gameObject.transform.LookAt(nextPos, Vector3.up);
            StartCoroutine(LerpPosition(nextPos, 7f));
            currentLocation.GetComponent<PlanetScript>().Travelable = false;
            currentLocation = nextPlanet;
            PlanetGeneration.Instance.visitedPlanets.Add(currentLocation);
            GameManager.Instance.fuelAmount = GameManager.Instance.fuelAmount - travelCost;
            fuelLeftText.text = "Fuel left: " + GameManager.Instance.fuelAmount;
            foreach (GameObject engine in engineFX)
            {
                engine.GetComponent<ParticleSystem>().Play(true);
            }
            gameObject.GetComponent<AudioSource>().clip = engineSound;
            gameObject.GetComponent<AudioSource>().Play();

            enemy.GetComponent<EnemyFleet>().MoveForward();

            float randomDamage = 0;
            int randomSystem = Random.Range(0, 5);
            switch(randomSystem)
            {
                case 0: randomDamage = Random.Range(0.1f, 0.15f);
                    GameManager.Instance.thrusterHealth = GameManager.Instance.thrusterHealth - randomDamage;
                    damageText.text = "Your thrusters took " + randomDamage*10 + " damage \n thruster health: " + GameManager.Instance.thrusterHealth + "/100";
                    break;
                case 1: randomDamage = Random.Range(10, 15);
                    GameManager.Instance.scannerHealth = GameManager.Instance.thrusterHealth - randomDamage;
                    damageText.text = "Your scanners took " +randomDamage+ " damage \n scanner health: " + GameManager.Instance.scannerHealth + "/100";
                    break;
                case 2: randomDamage = Random.Range(10, 15);
                    GameManager.Instance.weaponHealth = GameManager.Instance.weaponHealth - randomDamage;
                    damageText.text = "Your weapons took " +randomDamage+ " damage \n weapon health: " + GameManager.Instance.weaponHealth + "/100";
                    break;
                case 3: damageText.text = "You took no damage this trip";
                    break;

            }
            damageButton.SetActive(true);
        }
    }

    public void TurnOffDamageButton()
    {
        damageButton.SetActive(false);
    }

    public void LosdNextLevel()
    {
        GameManager.Instance.LoadScene((int)currentLocation.GetComponent<PlanetScript>().minigame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
