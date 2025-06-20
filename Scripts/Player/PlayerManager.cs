using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    // Cấp độ hiện tại của nhân vật
    public int _level = 1;

    // Kinh nghiệm hiện tại đang có
    public float _currentExp = 0;

    // Kinh nghiệm cần thiết để lên cấp
    public float _requiredExp = 100;

    // Máu tối đa của nhân vật
    public float _maxHealth = 100;

    // Máu hiện tại
    public float _currentHealth = 100;

    // Sát thương vật lý gây ra bởi đòn đánh thường hoặc kỹ năng vật lý
    public float _physicalDamage = 10;

    // Sát thương phép gây ra bởi kỹ năng phép thuật
    public float _magicDamage = 10;

    // Tốc độ tấn công (số đòn/phút), ảnh hưởng tốc độ đánh
    public float _attackSpeed = 1f;

    // Tỉ lệ chí mạng vật lý (0.1 = 10%), áp dụng cho đòn vật lý
    public float _critChancePhysical = 0.1f;

    // Tỉ lệ chí mạng phép (áp dụng cho kỹ năng phép nếu có hệ thống hỗ trợ)
    public float _critChanceMagic = 0.1f;

    // Giáp – giảm sát thương vật lý nhận vào
    public float _armor = 10;

    // Kháng phép – giảm sát thương phép nhận vào
    public float _magicResist = 10;

   // Hệ số làm chậm tốc độ di chuyển (1 = không chậm, <1 = bị làm chậm)
    public float _delayMoveSpeed = 1f;

    // Tốc độ di chuyển cơ bản của nhân vật
    public float _moveSpeed = 5f;

    // Lực nhảy cơ bản của nhân vật
    public float _jumpForce = 20f;

    // Giảm thời gian hồi chiêu (0.2 = 20%)
    public float _cooldownReduction = 0;

    // Tỉ lệ hút máu từ sát thương vật lý (0.1 = 10%)
    public float _lifeSteal = 0;

    // Tỉ lệ hút máu từ sát thương phép (spell vamp)
    public float _spellVamp = 0;

    /// <summary>
    /// Nhận kinh nghiệm. Nếu đủ để lên cấp thì thực hiện tăng cấp.
    /// </summary>
    public bool GainExp(float amount)
    {
        _currentExp += amount;

        if (_currentExp >= _requiredExp)
        {
            LevelUp();
            return true; // Trả về true nếu đã lên cấp
        }

        return false; // Chưa đủ exp để lên cấp
    }

    public float moveSpeed()
    {
        return _moveSpeed * _delayMoveSpeed;
    }

    /// <summary>
    /// Thực hiện lên cấp: tăng level, nâng chỉ số, đặt lại máu, tăng yêu cầu EXP.
    /// </summary>
    public void LevelUp()
    {
        _level++; // Tăng cấp
        _currentExp -= _requiredExp; // Trừ đi số exp vừa dùng để lên cấp
        _requiredExp = Mathf.Round(_requiredExp * 1.25f); // EXP cần cho cấp sau tăng 25%

        // Tăng các chỉ số khi lên cấp (có thể chỉnh lại theo từng class hoặc hệ phái)
        _maxHealth += 20;
        _physicalDamage += 5;
        _magicDamage += 5;
        _armor += 2;
        _magicResist += 2;
        _attackSpeed += 0.05f;

        // Hồi đầy máu khi lên cấp
        _currentHealth = _maxHealth;
    }
}


public class PlayerManager : MonoBehaviour
{
    // Singleton để truy cập PlayerManager ở bất kỳ đâu
    public static PlayerManager Instance { get; private set; }

    // Dữ liệu chỉ số nhân vật chính
    public PlayerStats Stats = new PlayerStats();

    private void Awake()
    {
        // Đảm bảo chỉ có một PlayerManager tồn tại trong game
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Huỷ đối tượng trùng lặp
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại object này khi chuyển scene
    }

    /// <summary>
    /// Gây sát thương cho nhân vật.
    /// Nếu isMagic = true thì dùng kháng phép, ngược lại dùng giáp.
    /// </summary>
    public void TakeDamage(float amount, bool isMagic = false)
    {
        float effectiveDamage = amount;

        if (isMagic)
        {
            // Tính sát thương sau khi trừ kháng phép
            effectiveDamage = amount * 100 / (100 + Stats._magicResist);
        }
        else
        {
            // Tính sát thương sau khi trừ giáp
            effectiveDamage = amount * 100 / (100 + Stats._armor);
        }

        Stats._currentHealth -= effectiveDamage;

        // Nếu máu <= 0 thì nhân vật coi như đã chết (sau này có thể trigger chết)
        if (Stats._currentHealth <= 0)
        {
            Stats._currentHealth = 0;
        }
    }

    /// <summary>
    /// Hồi máu cho nhân vật (không vượt quá máu tối đa).
    /// </summary>
    public void Heal(float amount)
    {
        Stats._currentHealth = Mathf.Min(Stats._currentHealth + amount, Stats._maxHealth);
    }
}

