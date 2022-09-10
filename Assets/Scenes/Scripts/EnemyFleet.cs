using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFleet : MonoBehaviour
{

    public List<GameObject> planetsList = new List<GameObject>();
    bool calcMove = true;
    public float moveAmount;
    public Text fleetMoveText; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (calcMove == true)
        {
            moveAmount = Random.Range(0, 2);
            fleetMoveText.text = ("Fleet Will Move forward " + (moveAmount + 1) + " space next time you move");
            calcMove = false;
        }
    }

    public void MoveForward()
    {
        Vector3 newPos = new Vector3(gameObject.transform.position.x + (800 * (moveAmount + 1)), 400, gameObject.transform.position.z);
        StartCoroutine(LerpPosition(newPos, 5f));
        calcMove = true;
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Planet"))
        {
            collision.gameObject.GetComponent<PlanetScript>().Travelable = false;
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<TestPlayer>().alive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            other.gameObject.GetComponent<PlanetScript>().Travelable = false;
        }

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<TestPlayer>().alive = false;
        }
    }
}
