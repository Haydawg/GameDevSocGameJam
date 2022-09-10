using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] float health;
    [SerializeField] Npc_Projectile projectile;
    [SerializeField] Transform handTranform;
    float height;
    public GameObject target;
    public NavMeshAgent agent;
    [SerializeField]Animator anim;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);

        if (Vector3.Distance(target.transform.position, transform.position) > attackRange)
        {
            
            anim.SetBool("isRunning", true);
            anim.SetBool("IsAttacking", false);
        }
        else if(Vector3.Distance(target.transform.position, transform.position) < attackRange)
        {
            agent.enabled = false;
            anim.SetBool("IsAttacking", true);
            anim.SetBool("isRunning", false);
            
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void Throw()
    {
        Npc_Projectile rock = Instantiate(projectile, handTranform.position, Quaternion.identity);
        rock.target = target;

    }
}
