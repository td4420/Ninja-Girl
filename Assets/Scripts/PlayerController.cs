using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed, force;
    private bool isGrounded, isDead;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2;
        speed = 1.5f;
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
        StartCoroutine(moveCharacter());   
    }
    IEnumerator moveCharacter()
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

        if (!Input.anyKey)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isGrounded", isGrounded);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            isGrounded = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(0.3f);
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
