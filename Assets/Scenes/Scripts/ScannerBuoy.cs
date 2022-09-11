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
            transform.LookAt(moveToPos);
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
    

    public void Scan()
    {
        ScanEffect scannerSphere = Instantiate(scanEffect, this.transform.position, Quaternion.identity);
        ScannerLevel.Instance.scanners.Add(scannerSphere);
        scannerSphere.maxSize = scannerRadius;
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
        moveToPos = dropPoint;
    }
}
