using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{
    GameManager gameManager;

    public GameObject currentLocation;
    public float fuel;
    public float moveTime = 0;
    public GameObject playerCamera;
    public GameObject enemy;
    public bool alive = true;
    public Text fuelLeftText;
    public List<GameObject> engineFX = new List<GameObject>();
    public Text launchGameText;
    public Text victoryText;
    public GameObject exitButton;

    public AudioClip engineSound;

    Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        fuelLeftText.text = "Fuel left: " + fuel;
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
                victoryText.enabled = true;
                exitButton.SetActive(true);
            }
        }

        launchGameText.text = currentLocation.GetComponent<PlanetScript>().minigame.ToString();
    }

    public void MoveToNextPlanet(GameObject nextPlanet, float travelCost)
    {
        if (travelCost <= fuel)
        {
            nextPos = new Vector3(nextPlanet.transform.position.x, 400, nextPlanet.transform.position.z);
            gameObject.transform.LookAt(nextPos, Vector3.up);
            StartCoroutine(LerpPosition(nextPos, 7f));
            currentLocation.GetComponent<PlanetScript>().Travelable = false;
            currentLocation = nextPlanet;
            fuel = fuel - travelCost;
            fuelLeftText.text = "Fuel left: " + fuel;
            foreach (GameObject engine in engineFX)
            {
                engine.GetComponent<ParticleSystem>().Play(true);
            }
            gameObject.GetComponent<AudioSource>().clip = engineSound;
            gameObject.GetComponent<AudioSource>().Play();

            enemy.GetComponent<EnemyFleet>().MoveForward();
        }
    }

    public void LosdNextLevel()
    {
        gameManager.LoadScene((int)currentLocation.GetComponent<PlanetScript>().minigame);
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