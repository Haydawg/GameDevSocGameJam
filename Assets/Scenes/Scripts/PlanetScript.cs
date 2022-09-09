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

    // Start is called before the first frame update
    void Start()
    {
        Invoke("MakeConections", 1.0f);
    }

    public void MakeConections()
    {
        float distance;
        planetArray = GameObject.FindGameObjectsWithTag("Planet");
        for(int i = 0; i < (planetArray.Length-1); i++)
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
        
    }

    private void OnMouseOver()
    {
        GameObject currentPlanet = player.GetComponent<TestPlayer>().currentLocation;

        if (planetsConnections.Contains(currentPlanet))
        {
            int costIndex;

            costIndex = currentPlanet.GetComponent<PlanetScript>().planetsConnections.IndexOf(gameObject);

            UICanavas.SetActive(true);


            UICanavas.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 500, gameObject.transform.position.z);
            GameObject travelCostText = UICanavas.transform.FindChild("TravelCost").gameObject;
            travelCostText.GetComponent<Text>().text = ("Travel Cost: " + currentPlanet.GetComponent<PlanetScript>().conectionCosts[costIndex]);

            Debug.Log(currentPlanet.GetComponent<PlanetScript>().conectionCosts[costIndex]);
        }
    }

    private void OnMouseExit()
    {
        UICanavas.SetActive(false);
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
