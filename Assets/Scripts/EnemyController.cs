using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    private GameObject player;
    float distance, maxX, minX, speed;
    public bool isMoveLeft, isAttacking;
    public float Hp, damage;
    private float delay;
    void Start()
    {
        damage = 25;
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Hp = 500;
        speed = 1;
        distance = 2;
        maxX = transform.position.x + distance;
        minX = transform.position.x - distance;
        animator.SetBool("beAttacked", false);
        animator.SetBool("seePlayer", false);
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
        checkPlayer();
    }
    void moveEnemy()
    {
        if (transform.position.x <= minX + 0.1f  || transform.position.x >= maxX - 0.1f)
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            if(Time.time - delay >= 1.0f)
            {
                delay = Time.time;
                isAttacking = true;
                animator.SetBool("inRange", true);
                speed = 0;
                player.GetComponent<PlayerController>().beAttacked(damage);
            }
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            isAttacking = false;
            animator.SetBool("inRange", false);
            speed = 1;
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
    public void checkPlayer()
    {
        if ((!isAttacking) && player.transform.position.x > minX && player.transform.position.y - transform.position.y < 1.3f)
        {
            if ((isMoveLeft && transform.position.x - player.transform.position.x <= 3)
                || ((!isMoveLeft) && transform.position.x - player.transform.position.x >= -3 && transform.position.x - player.transform.position.x <0))
            {
                speed = 2;
                animator.SetBool("seePlayer", true);
            }
            else
            {
                speed = 1;
                animator.SetBool("seePlayer", false);
            }
        }
        else
        {
            speed = 1;
            animator.SetBool("seePlayer", false);
        }

    }
}