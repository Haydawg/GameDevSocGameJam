using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrillLevel : MonoBehaviour
{
    protected static DrillLevel _Instance = null;
    public static DrillLevel Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<DrillLevel>();

            return _Instance;
        }
    }
    Camera camera;
    public float weaponDamage;
    public float weaponFireRate;
    [SerializeField]GameObject weapon;
    [SerializeField]LaserProjectile weaponProjectile;
    float timeSinceLastFired;
    float resourceGathered;
    [SerializeField] Button endButton;
    [SerializeField] Text endText;
    public float remainingTime;
    public Image timeBar;
    public float shipHealth;
    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        endButton.gameObject.SetActive(false);
        endText.enabled = false;
        shipHealth = 100;
        remainingTime = 60;
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
        if(shipHealth <= 0)
        {
            LeaveLevel();
        }
        remainingTime -= Time.deltaTime;
        timeBar.fillAmount = remainingTime / 60;
        healthBar.fillAmount = shipHealth / 100;
        if(remainingTime <= 0)
        {
            AllDrilled();
        }
        resourceGathered += Time.deltaTime / 2;
    }
    void Fire()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            timeSinceLastFired = 0;
            LaserProjectile laser = Instantiate(weaponProjectile, new Vector3(weapon.transform.position.x, weapon.transform.position.y - 10, weapon.transform.position.z), Quaternion.identity);
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
    void LeaveLevel()
    {
        endText.enabled = true;
        endText.text = "Youv'e sustained to much damage you leave with " + resourceGathered.ToString();
        endButton.gameObject.SetActive(true);
    }
    void AllDrilled()
    {
        endButton.gameObject.SetActive(true);
        endText.enabled = true;
        endText.text = "You've drained this planet you leave with " + resourceGathered.ToString();
    }
    
    public void LoadScene()
    {
        GameManager.Instance.LoadScene(1);
    }
}
