using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config variables
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    // State
    bool isAlive = true;

    // Cached references
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        ClimbLadder();
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
        if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && CrossPlatformInputManager.GetButtonDown("Jump"))
        {

            Vector2 jumpVelocitToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody2D.velocity += jumpVelocitToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // value is between -1 to  +1
            Vector2 climbVelocity = new Vector2(myRigidBody2D.velocity.x, controlThrow * climbSpeed);
            myRigidBody2D.velocity = climbVelocity;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody2D.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

            myRigidBody2D.gravityScale = 0;
        }
        else
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody2D.gravityScale = gravityScaleAtStart;
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
