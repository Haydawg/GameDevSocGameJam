using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public List<GameObject> planetsConnections = new List<GameObject>();
    GameObject[] planetArray;
    public float maxRadius = 1000.0f;
    public bool planetGenFinished = false;
    List<GameObject> planetLines = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Invoke("MakeConections", 1.0f);
    }

    public void MakeConections()
    {
        planetArray = GameObject.FindGameObjectsWithTag("Planet");
        for(int i = 0; i < (planetArray.Length-1); i++)
        {
            float distance = (planetArray[i].transform.position - gameObject.transform.position).magnitude;
            if(distance < maxRadius)
            {
                planetsConnections.Add(planetArray[i]);
            }
        }

        foreach(GameObject planet in planetsConnections)
        {
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
