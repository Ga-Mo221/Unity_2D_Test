using UnityEngine;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    // di chuyển nhân vật 
    private Rigidbody2D _rb;

    // chuyển đổi animation 
    private Animator _animator;

    // check mặt đất
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    private bool _isGrounded;

    // di chuyển tiền cảnh
    [SerializeField] private CinemachineCamera _Camera;
    [SerializeField] private Transform _br1;
    [SerializeField] private Transform _br2;
    [SerializeField] private Transform _br3;
    private Vector3 _playerPos;

    // lia camera theo nhân vật
    [SerializeField] private float _cameraPos = 0.15f;
    private float _cameraSpeed = -0.15f; // Tốc độ camera ban đầu
    private float _timer = 0f;
    private float _interval = 1f / 40f; // 40 lần mỗi giây


    // kiểm tra đang attack
    public bool _isAttack = false;
    [SerializeField] private Transform _attackPos;
    [SerializeField] private GameObject _attackPrefabs;
    public float _delayAttack = 0f;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerPos = transform.position;  
    }

    void Start()
    {
        _delayAttack = attackDelay(); // ✅ Sửa: Lưu giá trị trả về
    }

    void Update()
    {
        checkGrounded();

        if (_isAttack && _isGrounded) _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        else handleMovement();
        if (_isGrounded) handleJump();
        if (!_isAttack) attack();
        
        updateAnimation();
        updateBR();
    }

    private void checkGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    // lấy thời gian delay của attack
    private float attackDelay()
    {
        // Lấy tất cả animation clip từ Animator Controller
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
            if (clip.name == "Player_Attack1")
                return clip.length / 2;
        Debug.Log("delay :" + clips.Length / 2);
        return 1f;
    }

    // di chuyển qua lại "Horizontal"
    private void handleMovement()
    {
        float _moveInput = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(_moveInput * PlayerManager.Instance.Stats.moveSpeed(), _rb.linearVelocity.y);

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

    // nhảy 
    private void handleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, PlayerManager.Instance.Stats._jumpForce);
    }

    private void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("IsAttack1");
            Invoke(nameof(SpawnAttack), _delayAttack);
            _isAttack = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _animator.SetTrigger("IsAttack2");
            Invoke(nameof(SpawnAttack), _delayAttack);
            _isAttack = true;
        }
    }

    private void SpawnAttack()
    {
        Instantiate(_attackPrefabs, _attackPos.position, _attackPos.rotation, transform);
    }

    public void EndAttack()
    {
        _isAttack = false;
    }


    private void updateAnimation()
    {
        _animator.SetFloat("IsRunning", Mathf.Abs(_rb.linearVelocity.x));
        _animator.SetBool("IsJumping", !_isGrounded);
    }

    // cập nhật tốc độ chuyển động của camera follow nhân vật
    private void updateCameraSpeed(float _move)
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            _timer = 0f;
            if (_cameraSpeed >= -_cameraPos && _move > 0.1)
                _cameraSpeed -= 0.01f;
            else if (_cameraSpeed <= _cameraPos && _move < -0.1)
                _cameraSpeed += 0.01f;
        }
    }
    
    // cập nhật vị trí background theo chuển động nhân vật tạo hiệu ứng chiều sâu
    private void updateBR()
    {
        float _moveBr1 = PlayerManager.Instance.Stats.moveSpeed() / 300;
        float _moveBr2 = PlayerManager.Instance.Stats.moveSpeed() / 450;
        float _moveBr3 = PlayerManager.Instance.Stats.moveSpeed() / 550;
        if (transform.position.x > _playerPos.x)
        {
            _playerPos = transform.position;
            _br1.transform.position = new Vector3(_br1.transform.position.x + _moveBr1, _br1.transform.position.y, _br1.transform.position.z);
            _br2.transform.position = new Vector3(_br2.transform.position.x + _moveBr2, _br2.transform.position.y, _br2.transform.position.z);
            _br3.transform.position = new Vector3(_br3.transform.position.x + _moveBr3, _br3.transform.position.y, _br3.transform.position.z);
        }
        else if (transform.position.x < _playerPos.x)
        {
            _playerPos = transform.position;
            _br1.transform.position = new Vector3(_br1.transform.position.x - _moveBr1, _br1.transform.position.y, _br1.transform.position.z);
            _br2.transform.position = new Vector3(_br2.transform.position.x - _moveBr2, _br2.transform.position.y, _br2.transform.position.z);
            _br3.transform.position = new Vector3(_br3.transform.position.x - _moveBr3, _br3.transform.position.y, _br3.transform.position.z);
        }
    }
}
