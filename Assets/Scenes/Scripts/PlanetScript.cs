using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetScript : MonoBehaviour
{
    public List<GameObject> planetsConnections = new List<GameObject>();
    GameObject[] planetArray;
    public float maxRadius = 1000.0f;
    public bool planetGenFinished = false;
    List<GameObject> planetLines = new List<GameObject>();
    public List<float> conectionCosts = new List<float>();
    public GameObject player;
    public GameObject UICanavas;
    public float travelCost;
    public bool Travelable = true;
    public List<Mesh> planetMeshs = new List<Mesh>();
    bool planetDestroyed = false;
    public LevelSelection minigame;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshFilter>().mesh = planetMeshs[0];
        minigame = (LevelSelection)Random.Range(1, 4);

        Invoke("MakeConections", 2.0f);
    }

    public void MakeConections()
    {
        float distance;
        planetArray = GameObject.FindGameObjectsWithTag("Planet");
        for(int i = 0; i < (planetArray.Length); i++)
        {
            distance = (planetArray[i].transform.position - gameObject.transform.position).magnitude;
            if(distance < maxRadius)
            {
                if (planetArray[i] != gameObject)
                {
                    planetsConnections.Add(planetArray[i]);
                }
            }
        }

        foreach(GameObject planet in planetsConnections)
        {
            float distanceCost = (planet.transform.position - gameObject.transform.position).magnitude;
            if(distanceCost < 900)
            {
                conectionCosts.Add(1f);
            }
            else if(distanceCost < 1300)
            {
                conectionCosts.Add(2f);
            }
            else
            {
                conectionCosts.Add(3f);
            }

            GameObject newChild = new GameObject();
            newChild.transform.parent = gameObject.transform;
            newChild.AddComponent<LineRenderer>();
            planetLines.Add(newChild);
            newChild.GetComponent<LineRenderer>().startWidth = 10;
            newChild.GetComponent<LineRenderer>().endWidth = 10;
            newChild.GetComponent<LineRenderer>().startColor = Color.blue;
            newChild.GetComponent<LineRenderer>().endColor = Color.blue;
            newChild.GetComponent<LineRenderer>().positionCount = 2;

            newChild.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position);
            newChild.GetComponent<LineRenderer>().SetPosition(1, planet.transform.position);
        }

        planetGenFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Travelable == false)
        {
            gameObject.GetComponent<MeshFilter>().mesh = planetMeshs[1];
            if(planetDestroyed == false)
            {
                gameObject.transform.localScale = gameObject.transform.localScale + new Vector3(4, 4, 4);
                planetDestroyed = true;
            }
        }
    }

    private void OnMouseOver()
    {
        if (Travelable == true)
        {
            GameObject currentPlanet = player.GetComponent<TestPlayer>().currentLocation;

            if (planetsConnections.Contains(currentPlanet))
            {
                int costIndex;
                costIndex = currentPlanet.GetComponent<PlanetScript>().planetsConnections.IndexOf(gameObject);
                travelCost = currentPlanet.GetComponent<PlanetScript>().conectionCosts[costIndex];

                UICanavas.SetActive(true);
                GameObject travelCostText = UICanavas.transform.FindChild("TravelCost").gameObject;
                travelCostText.GetComponent<Text>().text = ("Travel Cost: " + travelCost + " fuel");

                Debug.Log(currentPlanet.GetComponent<PlanetScript>().conectionCosts[costIndex]);
            }
        }
    }

    private void OnMouseExit()
    {
        //UICanavas.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (Travelable == true)
        {
            if (planetsConnections.Contains(player.GetComponent<TestPlayer>().currentLocation))
            {
                player.GetComponent<TestPlayer>().MoveToNextPlanet(gameObject, travelCost);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (planetGenFinished == true)
        {
            foreach (GameObject planet in planetsConnections)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(gameObject.transform.position, planet.transform.position);
            }
        }
    }
}
