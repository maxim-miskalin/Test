using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyController), typeof(NavMeshAgent), typeof(Animator))]
public class EnemyHealth : Health
{
    [SerializeField] private float _timeOfDeath = 3f;

    private EnemyController _enemyController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private string _animationDie = "Die";
    private string _animationDamage = "Damage";
    private string _animationDeath = "IsLife";

    private WaitForSeconds _wait;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _wait = new(_timeOfDeath);
    }

    public void Resurrect()
    {
        _amountHealth = _maxHealth;
        _isLife = true;
        _enemyController.enabled = true;
        _navMeshAgent.enabled = true;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        _animator.SetTrigger(_animationDamage);

        if (_amountHealth <= 0 && _isLife)
            Die();
    }

    private void Die()
    {
        _isLife = false;
        _enemyController.enabled = false;
        _navMeshAgent.enabled = false;
        _animator.SetTrigger(_animationDie);
        _animator.SetBool(_animationDeath, _isLife);
        StartCoroutine(CountToShutdown());
    }

    private IEnumerator CountToShutdown()
    {
        yield return _wait;
        _enemyController.Spawner.ReturnToPool(_enemyController);
    }
}
