using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanEffect : MonoBehaviour
{
    public float maxSize;
    public Vector3 growthRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += growthRate * Time.deltaTime;
        if(transform.localScale.x >= maxSize)
        {
            ScannerLevel.Instance.destroyedScanEffects++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.gameObject.GetComponent<Resource>())
        {
            Resource resourceScanned = other.gameObject.GetComponent<Resource>();
            resourceScanned.Reveal();
        }
    }
}
