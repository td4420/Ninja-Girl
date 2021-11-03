using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Slider healthBar;
    private GameObject player;
    float distance, maxX, minX, speed;
    public bool isMoveLeft, isAttacking;
    public float Hp, damage;
    private float delay;
    private AudioSource effect;
    public AudioClip hit;
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
        healthBar.maxValue = Hp;
        healthBar.value = Hp;
        effect = gameObject.AddComponent<AudioSource>();
        effect.loop = false;
        effect.volume = MenuController.SFX;
        effect.playOnAwake = false;

    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
        checkPlayer();
    }
    void moveEnemy()
    {
        if ((transform.position.x <= minX + 0.05f && isMoveLeft)|| (transform.position.x >= maxX - 0.05f && !isMoveLeft))
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
        if (collision.tag == "Target" && player.GetComponent<PlayerController>().isAttacking)
        {
            StartCoroutine(beAttacked(player.GetComponent<PlayerController>().damage));
            player.GetComponent<PlayerController>().isAttacking = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.tag == "Player")
        {
            
        }
        if (collision.collider.tag == "Player" && animator.GetBool("seePlayer"))
        {
            
        }
    }
    IEnumerator setPlayerCanAttack()
    {
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerController>().hurtPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerController>().canAttack = true;
        player.GetComponent<PlayerController>().speed = 3;
        isAttacking = false;
        animator.SetBool("inRange", false);
        speed = 1;
    }
    public IEnumerator beAttacked(float damage)
    {
        animator.SetBool("beAttacked", true);
        speed = 0;
        Hp -= damage;
        healthBar.value = Hp;
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("beAttacked", false);
        Died();
    }
    public void Died()
    {
        if (Hp <= 0)
        {
            speed = 0;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            animator.SetBool("isDead", true);
            Destroy(gameObject, 2.1f);
        }
        else speed = 1;
    }
    public void checkPlayer()
    {
        if(Hp > 0)
        {
            
            float distanceY = player.transform.position.y - transform.position.y;
            float distanceX = transform.position.x - player.transform.position.x;
            if ((!isAttacking) && (player.transform.position.x > minX && player.transform.position.x < maxX) && distanceY < 1.3f && distanceY > -0.3f)
            {
                if ((isMoveLeft && distanceX > 0 && distanceX<= 2*distance) || ((!isMoveLeft) && distanceX >= -2*distance && distanceX < 0))
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
    public void AttackPlayer()
    {
        if(animator.GetBool("seePlayer"))
        {
            if (Time.time - delay >= 2.0f)
            {
                delay = Time.time;
                player.GetComponent<PlayerController>().canAttack = false;
                isAttacking = true;
                animator.SetBool("inRange", true);
                speed = 0;
                effect.clip = hit;
                effect.Play();
                player.GetComponent<PlayerController>().beAttacked(damage);
                StartCoroutine(setPlayerCanAttack());
            }
        }
    }
}