using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillLevel : MonoBehaviour
{
    Camera camera;
    public float weaponDamage;
    public float weaponFireRate;
    [SerializeField]GameObject weapon;
    [SerializeField]LaserProjectile weaponProjectile;
    float timeSinceLastFired;

    float shipHealth;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastFired = 0;
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFired += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (timeSinceLastFired > weaponFireRate)
                Fire();
        }
    }
    void Fire()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            timeSinceLastFired = 0;
            LaserProjectile laser = Instantiate(weaponProjectile, weapon.transform.position, Quaternion.identity);
            laser.targetPos = hit.point;

            if (hit.collider.gameObject.GetComponent<Npc>())
            {
                Npc npc = hit.collider.gameObject.GetComponent<Npc>();
                npc.TakeDamage(weaponDamage);

            }
        }

    }
    IEnumerator Laser(float laserActivationTime)
    {
        //lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserActivationTime);
        //lineRenderer.enabled = false;
    }
}
