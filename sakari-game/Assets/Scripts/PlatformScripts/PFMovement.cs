using UnityEngine;

public class PFMovement : MonoBehaviour
{
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [SerializeField] private float speed;
    [SerializeField] private float jumoPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    [SerializeField] private AudioSource jumpSoundEffect;

    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        respawnPoint = transform.position;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        


        //Flip player when moving
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
            

        if(Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(isGrounded())
            {
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }
        }

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);

        // if(Input.GetKey(KeyCode.Space))
        //     Jump();

    
    //     if(wallJumpCooldown > 0.2f)
    //     {


    //         body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

    //         if (onWall() && !isGrounded())
    //         {
    //             body.gravityScale = 0;
    //             body.velocity = Vector2.zero;
    //         }
    //         else
    //             body.gravityScale = 7;

    //         if (Input.GetKey(KeyCode.Space))
    //             Jump();
    //     }
    //     else
    //         wallJumpCooldown += Time.deltaTime;
  }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
    }

    private void Jump()
    {

        jumpSoundEffect.Play();

        if(coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;

        if(onWall())
            WallJump();
        else
        {
            if(isGrounded())

                
                body.velocity = new Vector2(body.velocity.x, jumoPower);
                

            else
            {
                if (coyoteCounter > 0)
                  body.velocity = new Vector2(body.velocity.x, jumoPower); 
                else 
                {
                    if(jumpCounter > 0)
                    {
                        
                        body.velocity = new Vector2(body.velocity.x, jumoPower);
                        jumpCounter--;
                    }
                }
            }

            coyoteCounter = 0;
        }

        // if (isGrounded())
        // {
        //     body.velocity = new Vector2(body.velocity.x, jumoPower);
        // }
        // else if (onWall() && !isGrounded())
        // {
        //     if (horizontalInput == 0)
        //     {
        //         body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
        //         transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //     }
        //     else
        //         body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

        //     wallJumpCooldown = 0;
            
        // }
        
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }


}
