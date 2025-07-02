using UnityEngine;

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
        
        LoadGameOrCreateNew();
    }

    public void SaveGame()
    {
        SaveSystem.SavePlayer(Stats);
    }

    public void LoadGameOrCreateNew()
    {
        PlayerStats loaded = SaveSystem.LoadPlayer();
        if (loaded != null)
        {
            Stats = loaded;
            Debug.Log("Đã load dữ liệu cũ");
        }
        else
        {
            Stats = CreateNewStats(); // Khi không có save file
            Debug.Log("Tạo mới nhân vật");
        }
    }

    private PlayerStats CreateNewStats()
    {
        return new PlayerStats
        {
            // Level
            _level = 1,

            // Exp
            _currentExp = 0f,
            _requiredExp = 100f,

            // HP
            _maxHealth = 100f,
            _currentHealth = 0f,

            // Move speed
            _walkSpeed = 2f,
            _runSpeed = 5f,

            // Nhảy
            _jumpForce = 15f,

            // Damage
            _physicalDamage = 10f,
            _magicDamage = 5f,

            // Giáp
            _armor = 10f,
            _magicResist = 5f,

            // Dash
            _dashSpeed = 20f,

            // Attack speed
            _attackSpeed = 1f,

            // Delay
            _delay = 1f,

            // Tỉ lệ chí mạng
            _critChancePhysical = 0f,
            _critChanceMagic = 0f,

            // Giảm hồi chiêu
            _cooldownReduction = 1,

            // Tiền tệ
            _xeng = 0,

            // Kỹ năng đã mở
            _doubleJump = false,
            _skillQ = false,
            _skillW = false,
            _skillE = false
        };
    }

    public void ResetGame() // gọi khi chọn "Chơi mới"
    {
        SaveSystem.DeleteSave();
        Stats = CreateNewStats();
    }
}

