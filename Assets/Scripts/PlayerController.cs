using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed, force, delayAttack;
    private bool isGrounded, isDead;
    private int numberOfKunais;
    public bool isAttacking, isLeft, canAttack, haveKey, reachDestination;
    public float knifeDamage = 100, kunaiDamage = 350;
    public float damage;
    private Animator animator;
    public Text number;
    public GameObject kunai, target;
    public float Hp = 100;
    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        force = 250;
        numberOfKunais = 10;
        number.text = "X" + numberOfKunais.ToString();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("isGrounded", true);
        animator.SetBool("isDead", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isThrow", false);
        animator.SetBool("isMoving", false);
        isLeft = false;
        healthBar.maxValue = Hp;
        healthBar.value = Hp;
        canAttack = true;
        haveKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveCharacter();
        StartCoroutine(Attack());
    }
    void moveCharacter()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            target.transform.position = new Vector3(transform.position.x + 0.53f, transform.position.y, transform.position.z);
            isLeft = false;
            if (!animator.GetBool("isMoving"))
            {
                animator.SetBool("isMoving", true);
            }
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(Time.deltaTime * speed, 0, 0);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            target.transform.position = new Vector3(transform.position.x - 0.53f, transform.position.y, transform.position.z);
            isLeft = true;
            if (!animator.GetBool("isMoving"))
            {
                animator.SetBool("isMoving", true);
            }
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(-Time.deltaTime * speed, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
        }

        if (!Input.anyKey)
        {
            animator.SetBool("isMoving", false);
        }
    }
    IEnumerator Attack()
    {
        if(canAttack)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time - delayAttack >= 1)
            {
                damage = knifeDamage;
                isAttacking = true;
                delayAttack = Time.time;
                animator.SetBool("isAttacking", true);
                yield return new WaitForSeconds(0.3f);
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
            else if (Input.GetKeyDown(KeyCode.Q) && Time.time - delayAttack >= 2 && numberOfKunais > 0)
            {
                Instantiate(kunai, target.transform.position, target.transform.rotation, GameObject.FindGameObjectWithTag("Canvas").transform);
                numberOfKunais--;
                number.text = "X" + numberOfKunais.ToString();
                damage = kunaiDamage;
                delayAttack = Time.time;
                animator.SetBool("isThrow", true);
                yield return new WaitForSeconds(0.55f);
                animator.SetBool("isThrow", false);
            }
        }    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && transform.position.y - collision.gameObject.transform.position.y > 0.3f)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Key")
        {
            haveKey = true;
            Destroy(collision.gameObject, 0.2f);
        }
        if(collision.tag == "House")
        {
            reachDestination = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "House")
        {
            reachDestination = false;
        }
    }
    public void beAttacked(float damage)
    {
        speed = 0;
        Hp -= damage;
        healthBar.value = Hp;
        if (Hp <= 0)
        {
            Dead();
        }
    }
    void Dead()
    {
        speed = 0;
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        animator.SetBool("isDead", true);
    }
}
