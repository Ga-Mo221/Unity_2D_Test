using Unity.Mathematics;
using UnityEngine;

public class Boss1 : Enemy
{
    protected override void Update()
    {
        if (_isPlayerDetected)
        {
            moveToPlayer();    
        }else 
            base.Update(); // Gọi lại Update() từ Enemy để giữ logic tuần tra
            updateAnimation(); // Thêm phần cập nhật animation riêng cho boss
    }

    private void updateAnimation()
    {
        // Điều kiện chuyển animation: nếu đang di chuyển thì bật walking
        bool isWalking = Mathf.Abs(_vecto) > 0;
        _animator.SetBool("iswalking", isWalking);
    }

    private void moveToPlayer()
    {
        float direction = Mathf.Sign(_player.position.x - transform.position.x); // trái (-1) hoặc phải (1)
        _vecto = (int)direction; // để Flip & animation

        Flip(_vecto);

        _rb.linearVelocity = new Vector2(_vecto * _speedRun, _rb.linearVelocity.y);

        _animator.SetBool("isrunning", true);

        Debug.Log("duoi theo");

        if (Mathf.Abs(_player.position.y - transform.position.y) > 5)
        {
            _isPlayerDetected = false;
            _animator.SetBool("isrunning", false);
        }
    }
}
