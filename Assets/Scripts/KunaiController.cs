using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiController : MonoBehaviour
{
    public float speed, damage, range;
    private float maxX, minX;
    private GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        damage = character.GetComponent<PlayerController>().kunaiDamage;
        maxX = transform.position.x + range;
        minX = transform.position.x - range;
    }

    // Update is called once per frame
    void Update()
    {
        moveKunai();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {

            collision.gameObject.GetComponent<EnemyController>().StartCoroutine(collision.gameObject.GetComponent<EnemyController>().beAttacked(damage));
            RemoveKunai();
        }
    }
    void moveKunai()
    {
        if (character.GetComponent<PlayerController>().isLeft)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x - Time.deltaTime * speed, minX, maxX),
                                             transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + Time.deltaTime * speed, minX, maxX),
                                             transform.position.y, transform.position.z);
        }
        if(transform.position.x >= maxX || transform.position.x <= minX)
        {
            RemoveKunai();
        }
    }
    void RemoveKunai()
    {
        Destroy(gameObject);
    }
}
