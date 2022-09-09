using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerBuoy : MonoBehaviour
{
    float scannerRadius;
    Resource[] resourcesInLevel;
    List<Resource> resourcesInRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public float ResourcesFound()
    {
        float returnValue = 0;
        foreach(Resource resource in resourcesInRange)
        {
            returnValue += resource.amount;
        }
        return returnValue;
    }

    public void Scan()
    {
        resourcesInLevel = FindObjectsOfType<Resource>();
        foreach(Resource resource in resourcesInLevel)
        {
            if(Vector3.Distance(this.transform.position, resource.transform.position) < scannerRadius)
            {
                resourcesInRange.Add(resource);
            }
        }
    }

    public void Drop(Vector3 dropPoint)
    {

    }
}
