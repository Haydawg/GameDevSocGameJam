using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGeneration : MonoBehaviour
{

    public List<GameObject> spawnablePlanets = new List<GameObject>();
    public int gridLength = 100;
    public int gridWidth = 50;
    public int planetSpacing = 10;

    GameObject previousPlanet;
    GameObject currentPlanet;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < gridLength; i = i + planetSpacing)
        {
            int planetChoice = Random.Range(0, spawnablePlanets.Count);
            int planetZAxis = Random.Range(0, gridWidth);
            
            previousPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(i, 0, planetZAxis), Quaternion.identity);  previousPlanet = currentPlanet;

            if(0 == (Random.Range(0,3)))
            {
                //genarate extra planets on same X_Coordinate
                if(0 == (Random.Range(0,2)))
                {
                    // + on ZAxis
                    planetChoice = Random.Range(0, spawnablePlanets.Count);
                    currentPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(i, 0, planetZAxis + Random.Range(1000, 1500)), Quaternion.identity);
                    //currentPlanet.GetComponent<PlanetScript>().planetsConnections.Add(previousPlanet);
                    //previousPlanet.GetComponent<PlanetScript>().planetsConnections.Add(currentPlanet);
                    //previousPlanet = currentPlanet;
                }
                else
                {
                    // - on ZAxis
                    planetChoice = Random.Range(0, spawnablePlanets.Count);
                    currentPlanet = Instantiate(spawnablePlanets[planetChoice], new Vector3(i, 0, planetZAxis - Random.Range(1000, 1500)), Quaternion.identity);
                    //currentPlanet.GetComponent<PlanetScript>().planetsConnections.Add(previousPlanet);
                    //previousPlanet.GetComponent<PlanetScript>().planetsConnections.Add(currentPlanet);
                    //previousPlanet = currentPlanet;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}