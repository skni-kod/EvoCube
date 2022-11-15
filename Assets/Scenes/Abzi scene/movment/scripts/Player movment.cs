using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;
public class Playermovment : MonoBehaviour
{
    [Inject]
    public IMovment ruch;
    
    public int speed;
    public float mouseSensitvity = 150f;
    [SerializeField]
    private Camera camerka;
    private Camera camerka3Person;

    private float xRotation=0f;

    private float yRotation = 0f;
    private Rigidbody rb;
    [Header("Ground check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool watchingIn3Person;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CinemachineFreeLook>().Follow = transform;
        FindObjectOfType<CinemachineFreeLook>().LookAt = transform;
        camerka3Person = FindObjectOfType<CinemachineBrain>().GetComponent<Camera>();
              FindObjectOfType<Camera>().gameObject.transform.parent.parent = transform;
        FindObjectOfType<Camera>().gameObject.transform.parent.localPosition = Vector3.zero;
        camerka = FindObjectOfType<Camera>();
        speed = ruch.speed;

        //dodanie rigibody i box colider
     //   BoxCollider bc = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
       CapsuleCollider Cp= gameObject.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
        
        rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        transform.position = new Vector3(0, 200f, 0);
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;
        rb.mass = 2;
        rb.drag = 5f;
        groundDrag = rb.drag;
        readyToJump = true;
        playerHeight = 1;

        jumpForce = 24;
        jumpCooldown = 0.25f;
        airMultiplier = 0.01f;
        //ustawic mapie wlasna maske!!!!
        whatIsGround = LayerMask.GetMask("Default");
       // Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ruch.speed = speed;
        ruch.speedObrotu = speed;
        //zmiana widoku
        if(watchingIn3Person)
        { camerka3Person.gameObject.SetActive(true); camerka.gameObject.SetActive(false); }
        else
        {
            camerka3Person.gameObject.SetActive(false);
            camerka.gameObject.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.F1))
        {
            watchingIn3Person = !watchingIn3Person;
        }
        // obrot kamera(pierwszo osobowa)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitvity * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitvity * Time.deltaTime; 
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -70f, 90f);
        camerka.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        //obrot postaci w zaleznosci od kierunku w ktorym idzie
 
        //wziecie camery z cameradirectordirector
        camerka.gameObject.transform.localPosition = Vector3.zero;


        //ruch
        
        ruch.MyInput();
       if(Input.GetKey(KeyCode.Space)&& readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        //sprawdzenie czy jest na ziemi
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void Ruch()
    {
        if (!watchingIn3Person) ruch.moveDirection = camerka.transform.forward * ruch.verticalInput + camerka.transform.right * ruch.horizontalInput;
        else { ruch.moveDirection = camerka3Person.transform.forward * ruch.verticalInput + camerka3Person.transform.right * ruch.horizontalInput; }
        if (grounded)//on ground
        {
            rb.AddForce(ruch.moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else // in air
        {
            rb.AddForce(ruch.moveDirection.normalized * speed * 10f*airMultiplier, ForceMode.Force);
        }


    }
    private void FixedUpdate()
    {
        speedControl();
        Ruch();
    }
    private void speedControl()//mozna usunac jezeli chcemy by sie rozpedzal
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude>speed)
        {
            Vector3 limited = flatVel.normalized * speed;
            rb.velocity = new Vector3(limited.x, limited.y, limited.z);
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
