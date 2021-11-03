using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiController : MonoBehaviour
{
    private float speed, damage, range;
    private float maxX, minX;
    private GameObject character;
    private bool left;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        range = 3;
        character = GameObject.FindGameObjectWithTag("Player");
        damage = character.GetComponent<PlayerController>().kunaiDamage;
        maxX = transform.position.x + range;
        minX = transform.position.x - range;
        left = character.GetComponent<PlayerController>().isLeft;
    }

    // Update is called once per frame
    void Update()
    {
        moveKunai();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<EnemyController>().Hp > 0)
            {
                collision.gameObject.GetComponent<EnemyController>().StartCoroutine(collision.gameObject.GetComponent<EnemyController>().beAttacked(damage));
                RemoveKunai();
            }    
            
        }
    }
    void moveKunai()
    {
        if (transform.position.x >= maxX - 0.1f || transform.position.x <= minX + 0.1f)
        {
            RemoveKunai();
        }
        if (left)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x - Time.deltaTime * speed, minX, maxX),
                                             transform.position.y, transform.position.z);
            
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + Time.deltaTime * speed, minX, maxX),
                                             transform.position.y, transform.position.z);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
    }
    void RemoveKunai()
    {
        Destroy(gameObject);
    }
}
