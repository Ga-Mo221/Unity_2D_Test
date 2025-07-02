using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _rButton;
    [SerializeField] private LayerMask _playerLayer;

    public bool _daDenGan = false;


    void Update()
    {
        checkdengan();
        hienthinut();
    }

    public void checkdengan()
    {
        _daDenGan = Physics2D.OverlapCircle(_rButton.transform.position, 4f, _playerLayer);
    }

    private void hienthinut()
    {
        if (_daDenGan)
        {
            _rButton.SetActive(true);
            
        }
        else
        {
            _rButton.SetActive(false);
        }
    }
}
