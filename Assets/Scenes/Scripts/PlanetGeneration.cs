using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGeneration : MonoBehaviour
{

    public List<GameObject> spawnablePlanets = new List<GameObject>();
    public int gridLength = 100;
    public int gridWidth = 50;
    public int planetSpacing = 10;
    public GameObject player;
    public GameObject popUpCanavas;
    public List<GameObject> planetList = new List<GameObject>();

    GameObject currentPlanet;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < gridLength; i = i + planetSpacing)
        {
            int planetChoice = Random.Range(0, spawnablePlanets.Count);
            int planetZAxis = Random.Range(0, gridWidth);
            Vector3 newPlanetPos = new Vector3(i, 0, planetZAxis);

            if(i != 0)
            {
                while((planetList[planetList.Count - 1].transform.position - newPlanetPos).magnitude > 2000)
                {
                    planetZAxis = Random.Range(0, gridWidth);
                    newPlanetPos = new Vector3(i, 0, planetZAxis);
                }
            }

            currentPlanet = Instantiate(spawnablePlanets[planetChoice], newPlanetPos, Quaternion.identity);
            currentPlanet.GetComponent<PlanetScript>().player = player;
            currentPlanet.GetComponent<PlanetScript>().UICanavas = popUpCanavas;
            planetList.Add(currentPlanet);

            if (player.GetComponent<TestPlayer>().currentLocation == null)
            {
                player.transform.position = new Vector3(currentPlanet.transform.position.x, 400, currentPlanet.transform.position.z);
                player.GetComponent<TestPlayer>().currentLocation = currentPlanet;
            }

            if (0 == (Random.Range(0,3)))
            {
                //genarate extra planets on same X_Coordinate
                if(0 == (Random.Range(0,2)))
                {
                    // + on ZAxis
                    planetChoice = Random.Range(0, spawnablePlanets.Count);
                    currentPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(i, 0, planetZAxis + Random.Range(1000, 1500)), Quaternion.identity);
                    currentPlanet.GetComponent<PlanetScript>().player = player;
                    currentPlanet.GetComponent<PlanetScript>().UICanavas = popUpCanavas;
                    planetList.Add(currentPlanet);
                }
                else
                {
                    // - on ZAxis
                    planetChoice = Random.Range(0, spawnablePlanets.Count);
                    currentPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(i, 0, planetZAxis - Random.Range(1000, 1500)), Quaternion.identity);
                    currentPlanet.GetComponent<PlanetScript>().player = player;
                    currentPlanet.GetComponent<PlanetScript>().UICanavas = popUpCanavas;
                    planetList.Add(currentPlanet);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
