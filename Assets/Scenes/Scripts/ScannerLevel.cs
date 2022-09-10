using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScannerLevel : MonoBehaviour
{
    protected static ScannerLevel _Instance = null;
    public static ScannerLevel Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<ScannerLevel>();

            return _Instance;
        }
    }
    Camera camera;
    [SerializeField] ScannerResourceGenerator scannerResourceGenerator;
    [SerializeField] ScannerBuoy scannerBuoyPrefab;
    [SerializeField] GameObject playerShip;
    public float scannerRadius;
    public int remainingScannerBuoys;
    public float resourceGathered;
    public int destroyedScanEffects;
    [SerializeField] Text remaingscannersText;
    [SerializeField] Button endButton;
    [SerializeField] Text endText;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(scannerResourceGenerator, new Vector3(300, 0, 150), Quaternion.identity);
        camera = Camera.main;
        scannerRadius = GameManager.Instance.scannerHealth;
        remainingScannerBuoys = 3;
        remaingscannersText.text = remainingScannerBuoys.ToString();
        endButton.gameObject.SetActive(false);
        endText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DropScannerBuoy();
        }
        if(destroyedScanEffects == 3)
        {
            EndLevel();
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

                if (remainingScannerBuoys > 0)
                {
                    ScannerBuoy scannerBouy = Instantiate(scannerBuoyPrefab, playerShip.transform.position - new Vector3(0,-10,0), Quaternion.identity);
                    scannerBouy.scannerRadius = scannerRadius;
                    scannerBouy.Drop(hit.point);
                    remainingScannerBuoys--;
                    remaingscannersText.text = remainingScannerBuoys.ToString();
                }
            }
        }
    }
    void EndLevel()
    {
        GameManager.Instance.resourceGathered = resourceGathered;
        endText.text = "You retrieved " + resourceGathered.ToString() + " units of fuel";
        endButton.gameObject.SetActive(true);
        endText.enabled = true;
    }

    public void LoadScene()
    {
        GameManager.Instance.LoadScene(1);
    }

}
