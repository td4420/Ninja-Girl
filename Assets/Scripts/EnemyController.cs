using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    private GameObject player;
    float distance, maxX, minX, speed;
    public bool isMoveLeft;
    public float Hp;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Hp = 500;
        speed = 1;
        distance = 1;
        maxX = transform.position.x + distance;
        minX = transform.position.x - distance;
        animator.SetBool("beAttacked", false);
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }
    void moveEnemy()
    {
        if (transform.position.x == minX || transform.position.x == maxX)
        {
            isMoveLeft = !isMoveLeft;
        }
        if (isMoveLeft && transform.position.x > minX)
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Target" && player.GetComponent<PlayerController>().isAttacking)
        {
            StartCoroutine(beAttacked(player.GetComponent<PlayerController>().damage));
            player.GetComponent<PlayerController>().isAttacking = false;
        }
    }
    public IEnumerator beAttacked(float damage)
    {
        animator.SetBool("beAttacked", true);
        speed = 0;
        Hp -= damage;
        Died();
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("beAttacked", false);
        if (Hp>0)speed = 1;
    }
    public void Died()
    {
        if(Hp<=0)
        {
            animator.SetBool("isDead", true);
            speed = 0;
            Destroy(gameObject, 2.1f);
        }
    }
}