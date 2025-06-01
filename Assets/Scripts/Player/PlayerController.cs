using System.Collections;
using UnityEngine;


public class PlayerController : Singelton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 1f;
    // [SerializeField] private float jumpForce = 2f;
    [SerializeField] private TrailRenderer playerTrailRenderer;
    [SerializeField] private Transform weaponcollider;
    [SerializeField] private Transform slashAnimationSpawnPoint;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private KnockBack knockBack;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method

        knockBack = GetComponent<KnockBack>();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
        startingMoveSpeed = moveSpeed;
        playerControls.DashJump.Dash.performed += _ => Dash();
        CameraController.Instance.SetPlayerCameraFollow();
        ActiveInventory.Instance.ChangeActiveInventory(1); // 1 is default Sword
        // playerControls.DashJump.Jump.performed += _ => Jump();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();

    }
    private void FixedUpdate()
    {
        Move();
        AdjustPlayerFacingDirection();
    }
    public Transform GetWeaponcollider(){
        return weaponcollider; 
    }
    public Transform GetSlashAnimationSpawnPoint(){
        return slashAnimationSpawnPoint; 
    }
    

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if ( knockBack.GettingKnockedBack || PlayerHealth.Instance.IsDead) 
        {
            return; // Prevent player movement while knocked back
        }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false;
        }

    }

    private void Dash(){
        if (!isDashing && Stamina.Instance.currentStamina > 0)
        {
            StartCoroutine(DashRoutine());
            Stamina.Instance.UseStamina();
        }
    }

    private IEnumerator DashRoutine() {
        playerTrailRenderer.emitting = true;
        isDashing = true;
        moveSpeed *= dashSpeed;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        playerTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
    // private void Jump()
    // {

    // }
}
