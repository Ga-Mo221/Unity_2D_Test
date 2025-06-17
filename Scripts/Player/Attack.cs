using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float _timeDestroy = 0.2f;
    void Start()
    {
        Destroy(gameObject, _timeDestroy);
    }
}
