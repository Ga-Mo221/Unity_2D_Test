using System.Collections;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // phát hiện player
    [SerializeField] private float _detectionRange = 5f;
    protected Transform _player;
    protected bool _isPlayerDetected = false;


    [SerializeField] private float _speedWalk = 2f;
    [SerializeField] protected float _speedRun = 4f;
    [SerializeField] private float _distance = 5f;
    protected Rigidbody2D _rb;
    protected Animator _animator;

    protected bool _thuGian = true;
    protected bool _cuonNo = false;

    private Vector3 _starPos;
    private bool _movingRight = true;

    public int _vecto = 1;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        _starPos = transform.position;
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            _player = playerGO.transform;
    }

    protected virtual void Update()
    {
        Patrol();
        if (!_isPlayerDetected)
        {
            _isPlayerDetected = playerDetected();
            Debug.Log(_isPlayerDetected);
        }
    }

    protected void Patrol()
    {
        if (_thuGian && !_cuonNo)
        {
            float _leftBound = _starPos.x - _distance;
            float _rightBound = _starPos.x + _distance;

            if (_movingRight)
            {
                _vecto = 1;
                if (transform.position.x >= _rightBound)
                {
                    _movingRight = false;
                    StartCoroutine(DoWaiting()); // Đợi khi chạm phải
                    return; // Không cập nhật di chuyển khi đang chờ
                }
            }
            else
            {
                _vecto = -1;
                if (transform.position.x <= _leftBound)
                {
                    _movingRight = true;
                    StartCoroutine(DoWaiting()); // Đợi khi chạm phải
                    return; // Không cập nhật di chuyển khi đang chờ
                }
            }
            Flip(_vecto);
            _rb.linearVelocity = new Vector2(_vecto * _speedWalk, _rb.linearVelocity.y);
        }
    }

    protected bool playerDetected()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        if (distanceToPlayer <= _detectionRange)
        {
            return isPlayerInFront();
        }
        return false;
    }

    private bool isPlayerInFront()
    {
        Vector2 directionToPlayer = (_player.position - transform.position).normalized;

        // Lấy hướng nhìn hiện tại của boss (1 là phải, -1 là trái)
        float facingDirection = Mathf.Sign(transform.localScale.x);

        // So sánh hướng nhìn và hướng tới player theo trục X
        return Mathf.Sign(directionToPlayer.x) == facingDirection;
    }

    protected void Flip(float _vecto)
    {
        if (_vecto == 1) transform.localScale = new Vector3(1, 1, 1);
        if (_vecto == -1) transform.localScale = new Vector3(-1, 1, 1);
    }

    private IEnumerator DoWaiting()
    {
        _thuGian = false;                  // Dừng chuyển động
        _rb.linearVelocity = Vector2.zero;      // Không di chuyển
        _vecto = 0;
        yield return new WaitForSeconds(3f); // CHỜ 3 GIÂY
        _thuGian = true;                   // Tiếp tục đi
    }
}
