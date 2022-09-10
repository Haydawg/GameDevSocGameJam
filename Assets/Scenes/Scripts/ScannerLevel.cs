using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(scannerResourceGenerator, new Vector3(300, 0, 150), Quaternion.identity);
        camera = Camera.main;
        scannerRadius = GameManager.Instance.scannerHealth;
        remainingScannerBuoys = 3;
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
                }
            }
        }
    }
    void EndLevel()
    {
        GameManager.Instance.resourceGathered = resourceGathered;
    }


}
