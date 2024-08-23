using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private float _smoothSpeed = 5f;

    private PlayerHealth _playerHealth;
    private Vector3 _startPosition;

    private void Awake()
    {
        _playerTarget = GameObject.FindObjectOfType<PlayerController>().transform;
        _startPosition = transform.position;
    }

    private void Start()
    {
        _playerHealth = _playerTarget.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (_playerTarget != null && _playerHealth.IsLife)
        {
            Vector3 desiredPosition = new(_playerTarget.position.x, transform.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, _smoothSpeed * Time.deltaTime);
        }
    }
}
