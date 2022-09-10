using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Projectile : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float speed;
    Vector3 moveDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 0.1)
        {
            moveDir = target.transform.position - transform.position;
            moveDir = moveDir.normalized;
            transform.position += moveDir * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject == target)
        {
            //do damage;
            Debug.Log("Hurt");
            Destroy(gameObject);
        }
    }
}
