using UnityEditor.Callbacks;
using UnityEngine;
using Unity.Cinemachine;
using TMPro.EditorUtilities;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 15f;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;

    [SerializeField] private CinemachineCamera _Camera;


    private Animator _animator;

    private bool _isGrounded;

    private Rigidbody2D _rb;
    [SerializeField] private float _cameraPos = 0.15f;
    public float _cameraSpeed = -0.15f; // Tốc độ camera ban đầu


    private float _timer = 0f;
    private float _interval = 1f / 40f; // 10 lần mỗi giây

    private bool _isAttack = false;

    //------------ Attack
    [SerializeField] private Transform _attackPos;
    [SerializeField] private GameObject _attackPrefabs;
    public float DelayAttack = 0.3f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isAttack)
        {
            // Dừng hẳn khi đang tấn công
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
        else
        {
            handleMovement();
            handleJump();
            attack();
        }
        updateAnimation();
    }


    void handleMovement()
    {
        float _moveInput = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(_moveInput * _moveSpeed, _rb.linearVelocity.y);

        if (_moveInput > 0.1)
        {
            transform.localScale = new Vector3(1, 1, 1);
            updateCameraSpeed(_moveInput);
            _Camera.GetComponent<CinemachinePositionComposer>().Composition.ScreenPosition = new Vector2(_cameraSpeed, 0f);
        }
        else if (_moveInput < -0.1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            updateCameraSpeed(_moveInput);
            _Camera.GetComponent<CinemachinePositionComposer>().Composition.ScreenPosition = new Vector2(_cameraSpeed, 0f);
        }
    }

    void handleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        }

        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    void updateAnimation()
    {
        _animator.SetFloat("IsRunning", Mathf.Abs(_rb.linearVelocity.x));
        _animator.SetBool("IsJumping", !_isGrounded);
    }

    void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("IsAttack1");
            Invoke(nameof(SpawnAttack), DelayAttack);
            _isAttack = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _animator.SetTrigger("IsAttack2");
            Invoke(nameof(SpawnAttack), DelayAttack);
            _isAttack = true;
        }
    }

    public void EndAttack()
    {
        _isAttack = false;
    }

    void SpawnAttack()
    {
        Instantiate(_attackPrefabs, _attackPos.position, _attackPos.rotation, transform);
    }

    void updateCameraSpeed(float _move)
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            _timer = 0f;
            if (_cameraSpeed >= -_cameraPos && _move > 0.1)
            {
                _cameraSpeed -= 0.01f;
            }
            else if (_cameraSpeed <= _cameraPos && _move < 0.1)
            {
                _cameraSpeed += 0.01f;
            }
        }
    }
}
