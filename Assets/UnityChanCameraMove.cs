using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class UnityChanCameraMove : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runMultiplier = 2f;
    public float jumpForce = 5f;
    public CinemachineFreeLook freeLookCam;

    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // --- Orbit Camera with Two-Finger Horizontal/Vertical Swipe ---
        if (Input.touchSupported && Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
            if (touch.phase == TouchPhase.Moved)
            {
                freeLookCam.m_XAxis.Value += touch.deltaPosition.x * 0.1f; // horizontal orbit
                freeLookCam.m_YAxis.Value -= touch.deltaPosition.y * 0.001f; // vertical orbit (invert if needed)
            }
            }
        }
        else
        {
            // Fallback for mouse/trackpad horizontal/vertical drag
            if (Input.GetMouseButton(1)) // right click drag
            {
            freeLookCam.m_XAxis.Value += Input.GetAxis("Mouse X") * 5f;
            freeLookCam.m_YAxis.Value -= Input.GetAxis("Mouse Y") * 0.02f; // vertical orbit (invert if needed)
            }
        }

        // --- Movement Input ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Calculate camera's yaw (horizontal rotation) from FreeLook XAxis value
        float camYaw = freeLookCam.m_XAxis.Value;
        Quaternion camRotation = Quaternion.Euler(0, camYaw, 0);

        // WASD movement relative to camera's X axis (yaw)
        Vector3 move = camRotation * new Vector3(h, 0, v);
        move.Normalize();

        if (move.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(move); // face move direction
            float currentSpeed = moveSpeed * (isRunning ? runMultiplier : 1f);
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        // --- Animator ---
        animator.SetFloat("Speed", move.magnitude * (isRunning ? runMultiplier : 1f));
        animator.SetFloat("Direction", move.x);

        // --- Jump ---
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

        // --- Rest ---
        animator.SetBool("Rest", Input.GetKey(KeyCode.R));

        // --- Jump Height / Gravity Control ---
        animator.SetFloat("JumpHeight", isGrounded ? 0f : rb.velocity.y);
        animator.SetFloat("GravityControl", isGrounded ? 0f : 1f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
