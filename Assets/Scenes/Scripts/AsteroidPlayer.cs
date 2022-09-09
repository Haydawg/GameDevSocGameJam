using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlayer : MonoBehaviour
{

    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)) //left -
        {
            if (gameObject.transform.position.x > -50)
            {
                gameObject.transform.position = gameObject.transform.position + (new Vector3(-speed, 0, 0) * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.D)) //right +
        {
            if (gameObject.transform.position.x < 50)
            {
                gameObject.transform.position = gameObject.transform.position + (new Vector3(speed, 0, 0) * Time.deltaTime);
            }
        }
    }
}
