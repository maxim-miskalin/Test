using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(Animator))]
public class PlayerHealth : Health
{
    [SerializeField] private TextMeshProUGUI _healthPoint;

    private PlayerController _playerController;
    private Animator _animator;
    private string _animationDie = "Die";
    private string _animationDamage = "Damage";
    private string _animationDeath = "IsLife";

    public event Action DieHero;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _healthPoint.text = $"HP: {_amountHealth}";
    }

    public void Resurrect()
    {
        _amountHealth = _maxHealth;
        _healthPoint.text = $"HP: {_amountHealth}";
        _isLife = true;
        _animator.SetBool(_animationDeath, _isLife);
        _playerController.enabled = true;
        transform.position = Vector3.zero;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        _animator.SetTrigger(_animationDamage);
        _healthPoint.text = $"HP: {_amountHealth}";

        if (_amountHealth <= 0 && _isLife)
        {
            _amountHealth = 0;
            _healthPoint.text = $"HP: {_amountHealth}";
            Die();
        }
    }

    private void Die()
    {
        _playerController.enabled = false;
        DieHero?.Invoke();
        _isLife = false;
        _animator.SetTrigger(_animationDie);
        _animator.SetBool(_animationDeath, _isLife);
    }
}
