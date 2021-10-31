using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform CheckGrounded;
    public LayerMask ground;
    public float speed, force, delayAttack;
    private bool isGrounded, isDead;
    public int numberOfKunais;
    public bool isAttacking, isLeft, canAttack, haveKey, reachDestination;
    public float knifeDamage = 150, kunaiDamage = 350;
    public float damage;
    private Animator animator;
    public Text number;
    public GameObject kunai, target, hurtPanel;
    public float Hp = 100;
    public Slider healthBar;
    private AudioSource effect;
    public AudioClip jump, attack, throwKunai, death;
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
        effect = gameObject.AddComponent<AudioSource>();
        effect.loop = false;
        effect.playOnAwake = false;
        effect.volume = MenuController.SFX;
        hurtPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(CheckGrounded.position, 0.1f, ground);
        animator.SetBool("isGrounded", isGrounded);
        healthBar.value = Hp;
        number.text = "X" + numberOfKunais.ToString();
        moveCharacter();
        StartCoroutine(Attack());
    }
    void moveCharacter()
    {
        //Move Right
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

        //Move Left
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

        //Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
            effect.clip = jump;
            effect.Play();
        }

        //Idle
        if (!Input.anyKey)
        {
            animator.SetBool("isMoving", false);
        }
    }
    IEnumerator Attack()
    {
        if(canAttack)
        {

            //Use knife, delay 1s
            if (Input.GetKeyDown(KeyCode.Space) && Time.time - delayAttack >= 1)
            {
                effect.clip = attack;
                effect.Play();
                damage = knifeDamage;
                isAttacking = true;
                delayAttack = Time.time;
                animator.SetBool("isAttacking", true);
                yield return new WaitForSeconds(0.3f);
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }

            //Throw Kunai, delay 2s
            else if (Input.GetKeyDown(KeyCode.Q) && Time.time - delayAttack >= 2 && numberOfKunais > 0)
            {
                effect.clip = throwKunai;
                effect.Play();
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
        hurtPanel.SetActive(true);
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
        effect.clip = death;
        effect.Play();
    }
}
