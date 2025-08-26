using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class UnityChanController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float jumpForce = 5f;

    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get movement input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Set animator parameters
        animator.SetFloat("Speed", v * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed));
        animator.SetFloat("Direction", h);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("Jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        // Rest
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("Rest", true);
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            animator.SetBool("Rest", false);
        }

        // Optional: control jump height & gravity
        if (!isGrounded)
        {
            animator.SetFloat("JumpHeight", rb.velocity.y);
            animator.SetFloat("GravityControl", 1f);
        }
        else
        {
            animator.SetFloat("GravityControl", 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
