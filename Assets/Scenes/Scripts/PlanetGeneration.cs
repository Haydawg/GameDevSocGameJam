using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlanetGeneration : MonoBehaviour, ISaveable
{
    protected static PlanetGeneration _Instance = null;
    public static PlanetGeneration Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<PlanetGeneration>();

            return _Instance;
        }
    }
    public List<GameObject> spawnablePlanets = new List<GameObject>();
    public int gridLength = 100;
    public int gridWidth = 50;
    public int planetSpacing = 10;
    public GameObject player;
    public GameObject popUpCanavas;
    public List<GameObject> planetList = new List<GameObject>();
    public EnemyFleet enemyFleet;
    public SaveLoadSystem saveLoadSystem;

    GameObject currentPlanet;
    public List<GameObject> visitedPlanets = new List<GameObject>();
    [Serializable]
    private struct SaveData
    {
        public List<(float, float, float)> planetVectors;
        public int playerIndex;
        public float enemyX;
        public float enemyY;
        public float enemyZ;
        public List<int> visitedPlanetIndices;
    }

    public void LoadState(object state)
    {
        planetList.Clear();
        var saveData = (SaveData)state;
        foreach ((float x, float y, float z) in saveData.planetVectors)
        {
            int planetChoice = UnityEngine.Random.Range(0, spawnablePlanets.Count);
            currentPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(x,y,z), Quaternion.identity);
            currentPlanet.GetComponent<PlanetScript>().player = player;
            currentPlanet.GetComponent<PlanetScript>().UICanavas = popUpCanavas;
            planetList.Add(currentPlanet);
        }
        player.GetComponent<TestPlayer>().currentLocation = planetList[saveData.playerIndex];
        player.transform.position = new Vector3(planetList[saveData.playerIndex].transform.position.x, 400, planetList[saveData.playerIndex].transform.position.z);
        planetList[saveData.playerIndex].GetComponent<PlanetScript>().Travelable = false;
        enemyFleet.MoveTo(new Vector3(saveData.enemyX, saveData.enemyY, saveData.enemyZ));
        foreach(int index in saveData.visitedPlanetIndices)
        {
            planetList[index].GetComponent<PlanetScript>().Travelable = false;
            visitedPlanets.Add(planetList[index]);
        }
        planetList[planetList.Count - 1].GetComponent<PlanetScript>().finalPLanet = true;
    }

    public object SaveState()
    {
        SaveData saveData = new SaveData();
        saveData.planetVectors = new List<(float,float,float)>();
        foreach (GameObject planet in planetList)
        {
            saveData.planetVectors.Add((planet.transform.position.x,planet.transform.position.y, planet.transform.position.z));
        }
        saveData.playerIndex = planetList.IndexOf(player.GetComponent<TestPlayer>().currentLocation);
        saveData.visitedPlanetIndices = new List<int>();
        foreach(GameObject planet in visitedPlanets)
        {
            int index = planetList.IndexOf(planet);
            saveData.visitedPlanetIndices.Add(index);
        }
        saveData.enemyX = enemyFleet.transform.position.x;
        saveData.enemyY = enemyFleet.transform.position.y;
        saveData.enemyZ = enemyFleet.transform.position.z;
        return saveData;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyFleet = FindObjectOfType<EnemyFleet>();
        saveLoadSystem = FindObjectOfType<SaveLoadSystem>();
        if(GameManager.Instance.newGame == true)
        {

            for (int i = 0; i < gridLength; i = i + planetSpacing)
            {
                int planetChoice = UnityEngine.Random.Range(0, spawnablePlanets.Count);
                int planetZAxis = UnityEngine.Random.Range(0, gridWidth);
                Vector3 newPlanetPos = new Vector3(i, 0, planetZAxis);

                if (i != 0)
                {
                    while ((planetList[planetList.Count - 1].transform.position - newPlanetPos).magnitude > 2000)
                    {
                        planetZAxis = UnityEngine.Random.Range(0, gridWidth);
                        newPlanetPos = new Vector3(i, 0, planetZAxis);
                    }
                }

                currentPlanet = Instantiate(spawnablePlanets[planetChoice], newPlanetPos, Quaternion.identity);
                currentPlanet.GetComponent<PlanetScript>().player = player;
                currentPlanet.GetComponent<PlanetScript>().UICanavas = popUpCanavas;
                planetList.Add(currentPlanet);


                if (0 == (UnityEngine.Random.Range(0, 3)))
                {
                    //genarate extra planets on same X_Coordinate
                    if (0 == (UnityEngine.Random.Range(0, 2)))
                    {
                        // + on ZAxis
                        planetChoice = UnityEngine.Random.Range(0, spawnablePlanets.Count);
                        currentPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(i, 0, planetZAxis + UnityEngine.Random.Range(1000, 1500)), Quaternion.identity);
                        currentPlanet.GetComponent<PlanetScript>().player = player;
                        currentPlanet.GetComponent<PlanetScript>().UICanavas = popUpCanavas;
                        planetList.Add(currentPlanet);
                    }
                    else
                    {
                        // - on ZAxis
                        planetChoice = UnityEngine.Random.Range(0, spawnablePlanets.Count);
                        currentPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(i, 0, planetZAxis - UnityEngine.Random.Range(1000, 1500)), Quaternion.identity);
                        currentPlanet.GetComponent<PlanetScript>().player = player;
                        currentPlanet.GetComponent<PlanetScript>().UICanavas = popUpCanavas;
                        planetList.Add(currentPlanet);
                    }
                }
            }
            planetList[planetList.Count - 1].GetComponent<PlanetScript>().finalPLanet = true;

            GameManager.Instance.newGame = false;

            if (player.GetComponent<TestPlayer>().currentLocation == null)
            {
                player.transform.position = new Vector3(planetList[0].transform.position.x, 400, planetList[0].transform.position.z);
                player.GetComponent<TestPlayer>().currentLocation = planetList[0];
                visitedPlanets.Add(planetList[0]);
            }
        }
        else
        {
            
            saveLoadSystem.Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene()
    {
        saveLoadSystem.Save();
        Debug.Log((int)player.GetComponent<TestPlayer>().currentLocation.GetComponent<PlanetScript>().minigame + 1);
        if (player.GetComponent<TestPlayer>().currentLocation.GetComponent<PlanetScript>().Travelable == true)
        {
            GameManager.Instance.LoadScene((int)player.GetComponent<TestPlayer>().currentLocation.GetComponent<PlanetScript>().minigame + 1);
        }
    }
}
