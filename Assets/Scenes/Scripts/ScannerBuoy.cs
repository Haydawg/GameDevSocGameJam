using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerBuoy : MonoBehaviour
{
    public float scannerRadius;
    [SerializeField] float fallSpeed;
    [SerializeField] ScanEffect scanEffect;
    Resource[] resourcesInLevel;
    List<Resource> resourcesInRange = new List<Resource>();
    Vector3 moveToPos;
    Vector3 moveDir;
    bool hasScanned;
    // Start is called before the first frame update
    void Start()
    {
        //moveToPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, moveToPos) > 0.1f)
        {
            moveDir = moveToPos - transform.position;
            moveDir = moveDir.normalized;
            transform.position += moveDir * fallSpeed * Time.deltaTime;
            transform.LookAt(moveDir);
        }
        else
        {
            if (!hasScanned)
            {
                Scan();
                hasScanned = true;
            }
        }
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
        ScanEffect scannerSphere = Instantiate(scanEffect, this.transform.position, Quaternion.identity);
        scannerSphere.maxSize = scannerRadius;
        resourcesInLevel = FindObjectsOfType<Resource>();
        foreach(Resource resource in resourcesInLevel)
        {
            if(Vector3.Distance(this.transform.position, resource.transform.position) < scannerRadius)
            {
                resourcesInRange.Add(resource);
            }
        }
        ResourcesFound();
    }

    public void Drop(Vector3 dropPoint)
    {
        moveToPos = dropPoint;
    }
}
