using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController1 : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;
        public bool doubleJump = true;

        private bool facingRight = true;
        [HideInInspector]
        public bool deathState = false;

        public bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            // Faz o personagem andar para a ESQUERDA automaticamente
            moveInput = -1;

            Vector3 direction = transform.right * moveInput;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);

            animator.SetInteger("playerState", 1); // Animação de corrida

            if (isGrounded)
                doubleJump = true;

            if (Input.GetKeyDown(KeyCode.Space) &&
                (isGrounded || (!isGrounded && doubleJump)))
            {
                if (!isGrounded)
                {
                    doubleJump = false;
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                }

                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }

            if (!isGrounded)
                animator.SetInteger("playerState", 2); // Animação de pulo

            // Inverte o sprite se estiver virado pro lado errado
            if (facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInput < 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true;
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                Destroy(other.gameObject);
            }
        }
    }
}
