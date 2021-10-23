using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{

    private float speed, force, delayAttack;
    private bool isGrounded, isDead;
    public bool isAttacking;
    private float knifeDamage = 250, kunaiDamage = 75;
    public float damage;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        force = 250;
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("isGrounded", true);
        animator.SetBool("isDead", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", false);
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
            if (!animator.GetBool("isMoving"))
            {
                animator.SetBool("isMoving", true);
            }
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(Time.deltaTime * speed, 0, 0);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && transform.position.y - collision.gameObject.transform.position.y > 1.35f)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
        }
    }
}
