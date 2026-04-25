using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ProjectileShooter))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(HealthSystemPlayer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float gravityFall = 12f;
    [SerializeField] private float gravityFloat = 5f;
    [SerializeField] private float _coyoteTime = .05f;
    [SerializeField] private float _earlyJumpTime = .05f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.0625f;

    [SerializeField] private AudioClip _soundJump;
    [SerializeField] private AudioClip _soundFail;

    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private Animator _animator;
    private ProjectileShooter _shooter;
    private HealthSystemPlayer _healthSystem;

    private PlayerInput _input;
    private float _horizontalInput = 0f;
    private bool _isJumpDown = false;
    private bool _isJumpHeldDown = false;

    public bool _isFacingRight = true;
    private bool _isInKickBack = false;
    private bool _isFainting = false;
    private bool _hasLeftGround = false;
    private float _coyoteTimeLeft = 0f;
    private float _earlyJumpTimeLeft = 0f;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _shooter = GetComponent<ProjectileShooter>();
        _healthSystem = GetComponent<HealthSystemPlayer>();

        _rb.gravityScale = gravityFall;
    }

    private void Update()
    {
        if (_input.actions["PauseToggle"].WasPressedThisFrame()) GameManager.Instance.TogglePause();
        if (GameManager.Instance.State != GameState.Playing || _isFainting) return;

        _coyoteTimeLeft -= Time.deltaTime;
        _earlyJumpTimeLeft -= Time.deltaTime;

        UpdateInputs();
        UpdateVisuals();
    }

    private void UpdateInputs()
    {
        if (_input.actions["Fire"].WasPressedThisFrame())
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mouseWorld - transform.position);

            _shooter.Fire(direction.normalized);
            _healthSystem.SelfDamage(1);
        }

        if (_input.actions["CycleShot"].WasPressedThisFrame())
        {
            _shooter.ReadyNext();
        }

        if (_input.actions["Jump"].WasPressedThisFrame())
        {
            _isJumpDown = true;
            _earlyJumpTimeLeft = _earlyJumpTime;
        }

        _isJumpHeldDown = _input.actions["Jump"].IsPressed();
        _horizontalInput = _input.actions["Move"].ReadValue<Vector2>().x;
    }

    private void UpdateVisuals()
    {
        if (_horizontalInput > 0 && !_isFacingRight) FlipSprite();
        if (_horizontalInput < 0 && _isFacingRight) FlipSprite();

        if (_horizontalInput.Equals(0))
        {
            _animator.Play("player_idle");
        }
        else
        {
            _animator.Play("player_walk");
        }
    }

    private void FixedUpdate()
    {
        if (_isFainting || _isInKickBack) return;

        _rb.velocity = new Vector2(_horizontalInput * speed, _rb.velocity.y);

        if (!_hasLeftGround && !IsOnGround())
        {
            _coyoteTimeLeft = _coyoteTime;
            _hasLeftGround = true;
        }

        if ((_coyoteTimeLeft > 0f || _hasLeftGround) && IsOnGround())
        {
            _coyoteTimeLeft = 0f;
            _hasLeftGround = false;
        }

        if (_isJumpDown || _earlyJumpTimeLeft > 0f)
        {
            _isJumpDown = false;

            if (IsOnGround() || _coyoteTimeLeft > 0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _coyoteTimeLeft = 0f;
                _earlyJumpTimeLeft = 0f;
                _hasLeftGround = true;
                AudioSystem.Instance.PlaySound(_soundJump, transform.position);
            }
        }

        _rb.gravityScale = (_isJumpHeldDown && _rb.velocity.y > 0) ? gravityFloat : gravityFall;
    }

    private bool IsOnGround()
    {
        var bounds = _collider.bounds;
        var bottomCornerRight = new Vector2(bounds.max.x, bounds.min.y);
        var bottomCornerLeft = new Vector2(bounds.min.x, bounds.min.y);

        RaycastHit2D hitRight = Physics2D.Raycast(bottomCornerRight, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(bottomCornerLeft, Vector2.down, groundCheckDistance, groundLayer);

        return hitRight.collider || hitLeft.collider;
    }

    private void FlipSprite()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
    }

    public void KickBack(Vector2 directionVector)
    {
        _rb.AddForce(directionVector, ForceMode2D.Impulse);

        _isInKickBack = true;

        Invoke(nameof(ReturnControl), .2f);
    }

    public void ReturnControl()
    {
        _isInKickBack = false;
    }

}