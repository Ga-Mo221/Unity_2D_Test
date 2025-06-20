using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float _timeDestroy = 0.05f;
    void Start()
    {
        Destroy(gameObject, _timeDestroy);
    }
}
