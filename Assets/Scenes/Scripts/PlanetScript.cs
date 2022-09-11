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
    public bool finalPLanet = false;
    public GameObject finalTarget;
    public Material lineMat;
    public ParticleSystem planetExp;

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
                conectionCosts.Add(30f);
            }
            else if(distanceCost < 1300)
            {
                conectionCosts.Add(50f);
            }
            else
            {
                conectionCosts.Add(70f);
            }

            GameObject newChild = new GameObject();
            newChild.transform.parent = gameObject.transform;
            newChild.AddComponent<LineRenderer>();
            planetLines.Add(newChild);
            newChild.GetComponent<LineRenderer>().startWidth = 10;
            newChild.GetComponent<LineRenderer>().endWidth = 10;
            newChild.GetComponent<LineRenderer>().material = lineMat;
            newChild.GetComponent<LineRenderer>().startColor = Color.blue;
            newChild.GetComponent<LineRenderer>().endColor = Color.blue;
            newChild.GetComponent<LineRenderer>().positionCount = 2;

            newChild.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position);
            newChild.GetComponent<LineRenderer>().SetPosition(1, planet.transform.position);
        }
        if(finalPLanet == true)
        {
            Vector3 targetPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 400, gameObject.transform.position.z);
            Instantiate(finalTarget, targetPoint, Quaternion.Euler(90,0,0));
            gameObject.GetComponent<MeshFilter>().mesh = planetMeshs[2];
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
                planetExp.Play();
                gameObject.GetComponent<MeshCollider>().enabled = false;
                gameObject.transform.localScale = gameObject.transform.localScale + new Vector3(4, 4, 4);
                planetDestroyed = true;
            }
        }

        if(player.GetComponent<TestPlayer>().alive == false)
        {
            Travelable = false;
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
                GameObject travelCostText = UICanavas.transform.Find("TravelCost").gameObject;
                travelCostText.GetComponent<Text>().text = ("Travel Cost: " + travelCost + " fuel");
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
