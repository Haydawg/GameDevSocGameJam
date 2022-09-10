using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelSelection
{
    Overworld,
    Astroid,
    Drilling,
    Scanning,
};
public class GameManager : MonoBehaviour
{
    protected static GameManager _Instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<GameManager>();

            return _Instance;
        }
    }
    public LevelSelection currentLevel;
    public float resourceGathered;
    [Header("Ship Stats")]
    public float shipHealth;

    public float thrusterHealth;
    public float thrusterSpeed;

    public float weaponHealth;
    public float weaponDamage;
    public float weaponFireRate;

    public float scannerHealth;
    public int scannerAmount;
    public float scannerRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
