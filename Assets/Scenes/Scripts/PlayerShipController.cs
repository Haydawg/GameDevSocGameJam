using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] LevelSelection currentLevel;
    Camera camera;
    [Header("ship")]
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 moveToLoc;
    [SerializeField] float speed;
    public float shipHealth;

    // ships thurster stats
    [Header("Thruster")]
    public float thrusterHealth;
    public float thrusterSpeed;

    // ship weaopons stats
    [Header("Weapons")]
    [SerializeField] GameObject weapon;
    [SerializeField] float weaponHealth;
    [SerializeField] float weaponDamage;
    [SerializeField] float weaponFireRate;
    float timeSinceLastFired;
    [SerializeField] float laserDisplayTime;
    LineRenderer lineRenderer;

    // ship scanner stats
    [Header("Scanner")]
    [SerializeField] GameObject scannerBuoyPrefab;
    [SerializeField] float scannerHealth;
    [SerializeField] int scannerAmount;
    [SerializeField] float scannerRadius;
    ScannerBuoy[] scannerBuoys;
    int BouyIndex;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, moveToLoc) > 0.1)
        {
            Debug.Log("Moving");
            moveDir = moveToLoc - transform.position;
            moveDir = moveDir.normalized;
            transform.position += moveDir * speed * Time.deltaTime;
        }
        switch(currentLevel)
        {
            case LevelSelection.Overworld:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    MoveToPlanet();
                }
                break;

            case LevelSelection.Astroid:
                break;
            case LevelSelection.Scanning:

                break;
            case LevelSelection.Drilling:
                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    timeSinceLastFired += Time.deltaTime;
                    if(timeSinceLastFired > weaponFireRate)
                        Fire();
                }
                break;
        }
    }

    void Fire()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Physics.Raycast(weapon.transform.position, hit.transform.position - weapon.transform.position, out hit))
            {
                timeSinceLastFired = 0;
                lineRenderer.SetPosition(1, hit.point);
                StartCoroutine(Laser(laserDisplayTime));
                if (hit.collider.tag == "Enemy")
                {
                    // add doing damage to enemy here

                }
            }
        }

    }
    IEnumerator Laser(float laserActivationTime)
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserActivationTime);
        lineRenderer.enabled = false;
    }
    void MoveToPlanet()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.tag == "Planet")
            {
                moveToLoc = hit.point;
            }
        }
    }

    void DropScannerBuoy()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Ground")
            {
                scannerBuoys[BouyIndex].Drop(hit.point);
            }
        }
    }
}
