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
    int resourceGathered;
    [SerializeField] Button endButton;
    [SerializeField] Text endText;
    public float remainingTime;
    public Image timeBar;
    public float shipHealth;
    public Image healthBar;
    public bool endGame = false;
    [SerializeField] float laserDisplayTime;
    LineRenderer lineRenderer;
    [SerializeField] GameObject explosionPrefab;
    GameObject currentExplosion;
    [SerializeField] Material material;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = material;
        endButton.gameObject.SetActive(false);
        endText.enabled = false;
        shipHealth = 100;
        weaponDamage = 100;
        remainingTime = 60;
        timeSinceLastFired = weaponFireRate;
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!endGame)
        {
            timeSinceLastFired += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (timeSinceLastFired > weaponFireRate)
                    Fire();
            }
            if (shipHealth <= 0)
            {
                LeaveLevel();
            }
            remainingTime -= Time.deltaTime;
            timeBar.fillAmount = remainingTime / 60;
            healthBar.fillAmount = shipHealth / 100;
            if (remainingTime <= 0)
            {
                AllDrilled();
            }
        }
    }

    void Fire()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        lineRenderer.SetPosition(0, weapon.transform.position);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("hit");
            if (Physics.Raycast(weapon.transform.position, hit.point - weapon.transform.position, out hit))
            {
                Debug.Log("hit");

                timeSinceLastFired = 0;
                lineRenderer.SetPosition(1, hit.point);
                currentExplosion = Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                StartCoroutine(Explosion());

                StartCoroutine(Laser(laserDisplayTime));
                if (hit.collider.gameObject.GetComponent<Npc>())
                {
                    Npc npc = hit.collider.gameObject.GetComponent<Npc>();
                    npc.TakeDamage(weaponDamage);

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
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(4);

        if (currentExplosion)
            Destroy(currentExplosion);

    }
    void LeaveLevel()
    {
        resourceGathered = 60 - (int)remainingTime;
        endGame = true;
        endButton.gameObject.SetActive(true);
        endText.enabled = true;
        endText.text = "Youv'e sustained to much damage you leave with " + resourceGathered.ToString() + " units of fuel";
    }
    void AllDrilled()
    {
        resourceGathered = 60 - (int)remainingTime;
        endGame = true;
        endButton.gameObject.SetActive(true);
        endText.enabled = true;
        endText.text = "You've drained this planet you leave with " + resourceGathered.ToString() + " units of fuel";
    }
    
    public void LoadScene()
    {
        GameManager.Instance.resourceGathered = resourceGathered;
        Debug.Log("press");
        GameManager.Instance.LoadScene(1);
    }
}
