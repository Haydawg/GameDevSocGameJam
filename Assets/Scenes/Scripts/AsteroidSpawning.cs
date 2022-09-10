using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawning : MonoBehaviour
{

    public List<GameObject> spawnableObj = new List<GameObject>();
    public List<GameObject> spawnAreas = new List<GameObject>();
    public GameObject spawnLoc;
    public float SpawnDelay = 2f;
    float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed = timePassed + Time.deltaTime;
        if(timePassed >= SpawnDelay)
        {
            SpawnStuff();
            timePassed = 0;
        }
    }

    public void SpawnStuff()
    {
        int tempSpawnLoc = Random.Range(0, 3);

        switch(tempSpawnLoc)
        {
            case 0: spawnLoc = spawnAreas[0];
                break;
            case 1:
                spawnLoc = spawnAreas[1];
                break;
            case 2:
                spawnLoc = spawnAreas[2];
                break;
            case 3:
                spawnLoc = spawnAreas[3];
                break;
            case 4:
                spawnLoc = spawnAreas[4];
                break;
            case 5:
                spawnLoc = spawnAreas[5];
                break;
            case 6:
                spawnLoc = spawnAreas[6];
                break;
        }

        GameObject obj = Instantiate(spawnableObj[Random.Range(0, spawnableObj.Count)], spawnLoc.transform.position, gameObject.transform.rotation);
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * -13, ForceMode.Impulse);
    }
}
