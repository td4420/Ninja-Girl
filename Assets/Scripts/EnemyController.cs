using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    float distance, maxX, minX, speed;
    bool isMoveLeft;
    void Start()
    {
        isMoveLeft = true;
        speed = 1;
        distance = 1;
        maxX = transform.position.x + distance;
        minX = transform.position.x - distance;
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }
    void moveEnemy()
    {
        if(transform.position.x == minX || transform.position.x == maxX)
        {
            isMoveLeft = !isMoveLeft;
        }
        if(isMoveLeft && transform.position.x > minX)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x - speed * Time.deltaTime, minX, maxX),
                                             transform.position.y, transform.position.z);
        }
        if (!isMoveLeft && transform.position.x < maxX)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + speed * Time.deltaTime, minX, maxX),
                                             transform.position.y, transform.position.z);
        }
    }
}
