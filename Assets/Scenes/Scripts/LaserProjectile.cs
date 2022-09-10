using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    public Vector3 targetPos;
    Vector3 moveDir;
    // Start is called before the first frame update
    void Start()
    {

        moveDir = targetPos - transform.position;
        moveDir = moveDir.normalized;
        transform.position += moveDir * speed * Time.deltaTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = targetPos - transform.position;
        moveDir = moveDir.normalized;
        transform.position += moveDir * speed * Time.deltaTime;
        transform.LookAt(targetPos);
    }
}
