using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerResourceGenerator : MonoBehaviour
{
    // The radius from the center of an object
    public float diskRadius;

    // Max and min spawn range on the terrain
    [Range(0f, 150f)]
    public float spawnRadiusMax = 100f;

    [Range(0f, 150f)]
    public float spawnRadiusMin = 20f;

    // Additional padding between disks
    [Range(2, 50)]
    public float separationMax = 20;

    [Range(2, 50)]
    public float separationMin = 5;

    // Max number of tries placing an object before moving to the next one
    public int maxTries;
    public Terrain terrain;

    // Stores objects that are to be generated
    public GameObject[] objects;

    // Stores disk/object positions
    List<GameObject> activeList = new List<GameObject>();

    GameObject currentDisk;

    // Represents the terrain map tiles
    bool[,] diskGrid;
    int gridSize;


    void Start()
    {

        gridSize = (int)spawnRadiusMax * 2;
        gridSize = gridSize % 2 == 0 ? gridSize + 1 : gridSize;
        diskGrid = new bool[gridSize, gridSize];

        terrain = FindObjectOfType<Terrain>();
        GenerateObjects();

    }


    public void GenerateObjects()
    {

        foreach (GameObject gameObject in activeList)
            Destroy(gameObject);

        activeList = new List<GameObject>();
        List<Vector3> housePositions = new List<Vector3>();
        diskGrid = new bool[gridSize, gridSize];


        activeList.Add(gameObject);

        while (activeList.Count > 0)
        {
            bool success = false;
            currentDisk = activeList[activeList.Count - 1];

            for (int i = 0; i < maxTries; i++)
            {
                // Calculate the coordinate of the next disk using polar coordinates
                float angle = Random.Range(-180, 180);
                float radius = diskRadius + Random.Range(separationMin, separationMax);
                float xOffset = radius * Mathf.Cos(angle);
                float zOffset = radius * Mathf.Sin(angle);
                float xCoord = currentDisk.transform.position.x + xOffset;
                float zCoord = currentDisk.transform.position.z + zOffset;
                Vector3 pos = new Vector3(xCoord, 0, zCoord);

                // Check if the new position is within the spawn bounds
                // If true, then place the disk/object on terrain
                if (Vector3.Distance(pos, this.gameObject.transform.position) > spawnRadiusMin && Vector3.Distance(pos, this.gameObject.transform.position) < spawnRadiusMax)
                {
                    success = PlaceDisk(xCoord, zCoord, false);
                }

                // If placement is successful, then do the next disk/object in the activeList
                if (success)
                    break;
            }

            // If all available tries were spent and placement was not successful,
            // then remove the disk/object from activeList. If there are no more disk/objects in the 
            // activeList, no stop the whole operation.
            if (!success)
            {
                activeList.RemoveAt(activeList.Count - 1);
                if (activeList.Count == 0)
                    break;
            }
        }

    }

    // This function places the object in the arbitraty grid.
    // Requires the calculated global x and z coordinates
    private bool PlaceDisk(float xCoord, float zCoord, bool isPath)
    {
        bool valid = true;

        // Calculate the offset for the grid
        int offset = (int)(gridSize - 1) / 2;
        int x = (int)(xCoord + offset) / (int)diskRadius;
        int y = (int)(zCoord + offset) / (int)diskRadius;

        // If x or y is less than 0, then x or y is 0. Otherwise, it retains its value.
        // If x or y is more than the (gridSize - 1), then x or y is (gridSize - 1).
        // Otherwise, it retains its value.

        x = (x < 0 ? 0 : x);
        x = (x > gridSize - 1 ? gridSize - 1 : x);
        y = (y < 0 ? 0 : y);
        y = (y > gridSize - 1 ? gridSize - 1 : y);

        // Check if the current cell in the diskgrid is available, if not then the whole place is invalid
        if (diskGrid[x, y])
        {
            valid = false;
        }

        // If the current diskgrid is available, then place the object
        if (valid)
        {

            diskGrid[x, y] = true;

            int objectindex = Random.Range(0, objects.Length);
            float height = terrain.SampleHeight(new Vector3(xCoord, 0, zCoord));
            if (isPath)
            {
                GameObject gameObject = Instantiate(objects[objectindex], new Vector3(xCoord, height, zCoord), Quaternion.identity, transform);
                activeList.Add(gameObject);
                return valid;
            }
            if (objects[objectindex].name == "House1" || objects[objectindex].name == "House2")
            {
                int chanceToSpawn = Random.Range(0, 10);
                if (chanceToSpawn < 4)
                {
                    GameObject gameObject = Instantiate(objects[objectindex], new Vector3(xCoord, height, zCoord), Quaternion.identity, transform);
                    activeList.Add(gameObject);
                }
            }
            else
            {
                GameObject gameObject = Instantiate(objects[objectindex], new Vector3(xCoord, height, zCoord), Quaternion.identity, transform);
                activeList.Add(gameObject);
            }


        }
        return valid;
    }
}