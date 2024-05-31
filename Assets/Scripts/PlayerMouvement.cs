using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMouvement : MonoBehaviour
{
    public Animator animator;

    public float horizontal = 0;
    public float speed = 8f;
    public float jumpingPower = 27f;
    public float backwardSpeed = 7f;

    //Personnage adverse
    public GameObject enemy;

    public GameObject self;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public Transform behindWall;

    public bool isAttacking = false;

    void Update()
    {
        Flip();
        if (!self.GetComponent<PlayerCombat>().isUltOn)
        {
            fctMouvementForFrame();
        }
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        if (!IsGrounded())
        {
            animator.SetBool("Jump", true);
            if(rb.velocity.y < 0 || !enemy.GetComponent<PlayerMouvement>().IsGrounded() || this.GetComponentInChildren<JumpProblems>().IsThereAWall() || Physics2D.OverlapCircle(behindWall.position, 0.5f, groundLayer))
            {
                GetComponent<Collider2D>().enabled = true;
            }
        }
        else
        {
            animator.SetBool("Jump", false);
        }

    }

    public bool IsGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip(){
        Vector3 localScale = transform.localScale;
        if (transform.position.x - enemy.transform.position.x > 0 && localScale.x > 0){
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }
        else if (transform.position.x - enemy.transform.position.x < 0 && localScale.x < 0){
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }
    }

    public bool IsFacingRight(){
        return (transform.position.x - enemy.transform.position.x <= 0) ? true : false;
    }

    void fctMouvement(InputAction.CallbackContext ctx) {
        //uniformisation manette/clavier
        horizontal = 0;
        if (ctx.ReadValue<Vector2>().x > 0.7 || ctx.ReadValue<Vector2>().x < -0.7)
        {
            horizontal = ctx.ReadValue<Vector2>().x;
        }
    }

    void fctMouvementForFrame()
    {
        if (IsGrounded() && !isAttacking)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            //marche arriere
            if (((horizontal < 0 && IsFacingRight()) || (horizontal > 0 && !IsFacingRight())))
            {
                rb.velocity = new Vector2(horizontal * backwardSpeed, rb.velocity.y);
            }
            //marche avant
            else if (((horizontal > 0 && IsFacingRight()) || (horizontal < 0 && !IsFacingRight())))
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        }
        else if (IsGrounded() && isAttacking)
        {
            rb.velocity = new Vector2(horizontal * speed * 0.5f, rb.velocity.y);
        }
        else if(!IsGrounded() && isAttacking)
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(speed * 0.5f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed * 0.5f, rb.velocity.y);
            }
        }
    }

    void fctJump(InputAction.CallbackContext ctx){
        if (ctx.action.triggered && IsGrounded() && !isAttacking){
            if (!Physics2D.OverlapCircle(behindWall.position, 0.5f, groundLayer)) {
                GetComponent<Collider2D>().enabled = false;
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
    }


}
