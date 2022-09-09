using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float amount;
    [SerializeField] MeshRenderer resourceMesh;
    Mesh mesh;
    [SerializeField] Mesh[] meshes;
    // Start is called before the first frame update
    void Start()
    {
        resourceMesh = GetComponent<MeshRenderer>();
        mesh = GetComponent<Mesh>();
        mesh = meshes[Random.Range(0, meshes.Length - 1)];
        resourceMesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reveal()
    {
        resourceMesh.enabled = true;
    }
}
