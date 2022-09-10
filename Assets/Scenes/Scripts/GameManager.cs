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
    public float fuelAmount = 20;
    [Header("Ship Stats")]
    public float shipHealth;

    public float thrusterHealth = 1;

    public float weaponHealth;

    public float scannerHealth;

    public bool newGame;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        newGame = true;
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
