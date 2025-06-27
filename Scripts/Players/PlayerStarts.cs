using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    // Level
    public int _level;

    // Exp
    public float _currentExp; // exp hiện tại
    public float _requiredExp; // exp cần để lên cấp

    // HP
    public float _maxHealth; // máu tối đa
    public float _currentHealth; // máu hiện tại

    // Move speed
    public float _walkSpeed; // tốc độ đi bộ
    public float _runSpeed; // tốc độ chạy

    // Nhảy
    public float _jumpForce; // lực nhảy

    // Damage
    public float _physicalDamage; // ST vật lý
    public float _magicDamage; // ST phép

    // Giáp
    public float _armor; // giáp
    public float _magicResist; // kháng phép

    // Dash
    public float _dashSpeed; // tốc độ trượt

    // Attack speed
    public float _attackSpeed; // tốc độ đánh

    // Delay
    public float _delay; // ảnh hưởng tốc độ đánh, di chuyển, khoảng cách dash

    // Tỉ lệ chí mạng
    public float _critChancePhysical; // ST vật lý
    public float _critChanceMagic; // ST phép

    // Giảm hồi chiêu
    public float _cooldownReduction;

    // Kỹ năng đã mở
    public bool _doubleJump;
    public bool _skillQ;
    public bool _skillW;
    public bool _skillE;

    // Tính tốc độ di chuyển dựa theo trạng thái chạy hoặc đi bộ và ảnh hưởng của delay.
    public float GetMoveSpeed(bool isRunning)
    {
        return isRunning ? _runSpeed * _delay : _walkSpeed * _delay;
    }

    // tăng exp
    public bool GainExp(float amount)
    {
        _currentExp += amount;

        bool leveledUp = false;

        while (_currentExp >= _requiredExp)
        {
            LevelUp();
            leveledUp = true;
        }

        return leveledUp;
    }

    // xử lý lên cấp
    private void LevelUp()
    {
        _level++;
        _currentExp -= _requiredExp;
        _requiredExp = Mathf.RoundToInt(_requiredExp * 1.25f);

        _maxHealth += 20;
        _physicalDamage += 5;
        _magicDamage += 5;
        _armor += 2;
        _magicResist += 2;
        _attackSpeed += 0.05f;

        _currentHealth = _maxHealth;
    }
}
