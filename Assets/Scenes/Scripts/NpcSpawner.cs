using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Npc npcToSpawn;
    int currentNpcsSpawned;
    int totalspawned;
    [SerializeField] int maxSpawn;
    [SerializeField] float spanwRate;
    float timeSinceLastSpawn;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    Terrain terrain;
    // Start is called before the first frame update
    void Start()
    {
        terrain = FindObjectOfType<Terrain>();
        totalspawned = 0;
        timeSinceLastSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (totalspawned >= maxSpawn)
            return;
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > spanwRate)
        {
            Spawn();
            timeSinceLastSpawn = 0;
        }
    }

    void Spawn()
    {
        float xLoc = Random.Range(minX, maxX);
        float height = terrain.SampleHeight(new Vector3(xLoc, 0, transform.position.z));
        Npc npc = Instantiate(npcToSpawn, (new Vector3(xLoc, height, transform.position.z)),Quaternion.identity);
        float targetheight = terrain.SampleHeight(new Vector3(target.transform.position.x, 0, target.transform.position.z));
        Vector3 targetPos = new Vector3(target.transform.position.x, targetheight, target.transform.position.z);


        npc.target = target;
        npc.agent.SetDestination(targetPos);
        totalspawned++;
    }
}
