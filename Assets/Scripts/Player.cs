using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config variables
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 28f;

    // State
    bool isAlive = true;

    // Cached references
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    Collider2D myCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to  +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);

        myRigidBody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("Running", playerHasHorizontalSpeed);

        FlipSprite(playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && CrossPlatformInputManager.GetButtonDown("Jump"))
        {

            Vector2 jumpVelocitToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody2D.velocity += jumpVelocitToAdd;
        }
    }

    private void FlipSprite(bool playerHasHorizontalSpeed)
    {
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
        }
    }
}
