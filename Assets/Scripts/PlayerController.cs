using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedRotation = 500f;
    [SerializeField] private Joystick _joystick;

    private Animator _animator;
    private string _animationSpeed = "Speed";
    private string _animationAttack = "Attack";

    private Vector3 _direction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 direction = new(_joystick.Horizontal, 0f, _joystick.Vertical);

        _direction = direction;
        if (direction != Vector3.zero)
            transform.forward = Vector3.Lerp(transform.forward, _direction, _speedRotation * Time.deltaTime);

        transform.position += _speed * Time.deltaTime * direction;

        float speed = 0;
        if (direction != Vector3.zero)
            speed = direction.magnitude / _speed / Time.deltaTime;

        _animator.SetFloat(_animationSpeed, Mathf.Abs(speed));

        if (transform.rotation.z != 0 || transform.rotation.x != 0)
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }

    public void Attack()
    {
        _animator.SetTrigger(_animationAttack);
    }
}
