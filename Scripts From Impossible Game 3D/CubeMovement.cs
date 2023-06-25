using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameManager gm;
    [SerializeField] CameraIncline cameraIncline;
    [SerializeField] PlayerControlls controlls;

    [SerializeField] AudioSource music;
    [SerializeField] Animator fader;
    [SerializeField] Animator fader2;

    [SerializeField] float currentXPosition;
    [SerializeField] float currentYPosition;
    [SerializeField] float currentZPosition;

    //InputSystem
    [SerializeField] Vector2 move;
    [SerializeField] float speed;

    //Camera Switch
    [SerializeField] CinemachineVirtualCamera normalCam;
    [SerializeField] CinemachineVirtualCamera zoomCam;

    [SerializeField] float runSpeed = 200f;
    [SerializeField] float maxJumpForce = 5f;

    [SerializeField] float jumpForce = 15f;
    [SerializeField] float jumpImpulse = 10f;
    [SerializeField] bool jumpReady = true;
    [SerializeField] float jumpCD = 0.0001f;
    [SerializeField] float jumpCDcurrent = 0.0f;


    [SerializeField] float strafeSpeed = 75f;
    [SerializeField] float maxStrafeSpeed = 6f;
    [SerializeField] float strafeDrag = 6f;

    [SerializeField] bool leftInput = false;
    [SerializeField] bool rightInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool isDead;
    [SerializeField] UnityEvent death;

    [SerializeField] Rotation Visual;
    [SerializeField] GameObject deathEffect;

    [SerializeField] int attempts;
    [SerializeField] Text attemptsText;

    [SerializeField] bool isGrounded;
    [SerializeField] string groundTag = "Ground";
    [SerializeField] string speedTag = "SpeedBoost";
    [SerializeField] string jumpTag = "GroundJump";

    //Binds
    [SerializeField] KeyCode left = KeyCode.A;
    [SerializeField] KeyCode right = KeyCode.D;
    [SerializeField] KeyCode jump = KeyCode.Space;
    [SerializeField] KeyCode jumpAlt = KeyCode.Mouse0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        Cursor.visible = false;
        fader.enabled = true;
        fader2.enabled = false;
        fader.GetComponent<Animator>().Play("FadeIntoGame");
    }
    public void OnEnable()
    {
        deathEffect.SetActive(false);
        CameraSwitcher.Register(normalCam);
        CameraSwitcher.Register(zoomCam);
        CameraSwitcher.SwitchCamera(normalCam);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(normalCam);
        CameraSwitcher.Unregister(zoomCam);
    }
    private void Deathu()
    {
        fader2.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrier"))
        {
            normalCam.enabled = false;
            zoomCam.enabled = false;
            Visual.gameObject.SetActive(false);
            deathEffect.SetActive(true);
            gm.EndGame();
            music.Pause();
            isDead = true;
            death?.Invoke();
            Attempts.attempts += 1;
            Invoke("Deathu", 1f);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Visual.jump = false;
        if (collision.gameObject.tag == groundTag)
        {
            isGrounded = true;
        }
        else
        {
            if (CameraSwitcher.IsActiveCamera(normalCam))
                CameraSwitcher.SwitchCamera(zoomCam);
            else if (CameraSwitcher.IsActiveCamera(zoomCam))
                CameraSwitcher.SwitchCamera(normalCam);
            runSpeed = 100f;
            JumpOff();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == groundTag)
        {
            isGrounded = false;
            runSpeed = 35f;
        }
        if (collision.gameObject.tag == jumpTag && !jumpInput)
        {
            JumpStartFrom();
        }
    }

    void JumpStart()
    {
        if (jumpReady)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            Visual.jump = true;
            jumpCDcurrent = 0f;
        }
    }

    void JumpStartFrom()
    {
        rb.AddForce(Vector3.up * 175000, ForceMode.Force);
        Visual.jump = true;
        runSpeed = 35f;
    }
    void JumpOff()
    {
        Visual.jump = false;
        jumpInput = false;
    }

    void Update()
    {
        leftInput = Input.GetKey(left);
        rightInput = Input.GetKey(right);
        jumpInput = Input.GetKey(jump);

        if (transform.position.y < -5f)
        {
            normalCam.enabled = false;
            zoomCam.enabled = false;
            gm.EndGame();
            music.Pause();
            Visual.gameObject.SetActive(false);
            deathEffect.SetActive(true);
            Attempts.attempts += 1;
            fader2.GetComponent<Animator>().Play("FadeFromGame3");
        }
    }

    void FixedUpdate()
    {
        HorizontalMovement();

        rb.MovePosition(rb.position + Vector3.forward * runSpeed * Time.fixedDeltaTime);

        if (jumpCDcurrent >= jumpCD)
            jumpReady = true;
        else
        {
            jumpCDcurrent = jumpCDcurrent + Time.fixedDeltaTime;
            jumpReady = false;
        }

        if (isGrounded && Input.GetMouseButton(0) || Input.GetKey(jump))
        {
            JumpStart();
        }
    }

    private void HorizontalMovement()
    {
        if (leftInput)
        {
            rb.AddForce(-strafeSpeed, 0, 0, ForceMode.Acceleration);
        }

        else if (rightInput)
        {
            rb.AddForce(strafeSpeed, 0, 0, ForceMode.Acceleration);
        }

        var clampedVelocity = Mathf.Clamp(rb.velocity.x, -maxStrafeSpeed, maxStrafeSpeed);
        clampedVelocity = Mathf.MoveTowards(clampedVelocity, 0, strafeDrag * Time.fixedDeltaTime);
        rb.velocity = new Vector3(clampedVelocity, rb.velocity.y, rb.velocity.z);

        cameraIncline.Incline(clampedVelocity);
    }
}


