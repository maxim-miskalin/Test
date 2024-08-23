using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackSpeed = 1.2f;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    private NavMeshAgent _agent;
    private float _distanceToPlayer;
    private float _distanceToBase;
    private float _normalSpeed;

    private Animator _animator;
    private string _animationSpeed = "Speed";
    private string _animationAttack = "Attack";
    private bool _isIttacking = false;

    [SerializeField] private Spawner _spawner;
    private Health _health;
    private Health _playerHealth;

    public Spawner Spawner => _spawner;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _playerHealth = _player.GetComponent<Health>();
    }

    private void Start()
    {
        _wait = new(_attackSpeed);
        _distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        _distanceToBase = Vector3.Distance(transform.position, _spawner.transform.position);
        _normalSpeed = _agent.speed;
    }

    private void Update()
    {
        if (_health.IsLife)
        {
            if (_playerHealth.IsLife)
            {
                _agent.SetDestination(_player.position);

                if (_distanceToPlayer <= _attackDistance)
                {
                    _coroutine = StartCoroutine(Attack());
                }
                else
                {
                    if (_coroutine != null && !_isIttacking)
                        StopCoroutine(Attack());
                }
            }
            else
            {

                if (_distanceToBase <= _attackDistance)
                {
                    _spawner.ReturnToPool(this);
                }
                else
                {
                    _agent.SetDestination(_spawner.transform.position);
                }
            }

            if (_agent.speed == 0 && !_isIttacking)
                _agent.speed = _normalSpeed;

            _animator.SetFloat(_animationSpeed, Mathf.Abs(_agent.speed));

            if (transform.rotation.z != 0 || transform.rotation.x != 0)
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

            if (_playerHealth.IsLife)
            {
                transform.LookAt(_player);
                _distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            }
            else
            {
                transform.LookAt(_spawner.transform);
                _distanceToBase = Vector3.Distance(transform.position, _spawner.transform.position);
            }
        }
    }

    public void SetBase(Spawner baseSpawner)
    {
        _spawner = baseSpawner;
    }

    private IEnumerator Attack()
    {
        _agent.speed = 0;
        _isIttacking = true;
        _animator.SetTrigger(_animationAttack);
        yield return _wait;
        _isIttacking = false;
    }
}
