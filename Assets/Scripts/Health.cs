using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int _amountHealth;
    [SerializeField] private float _timeInvulnerability = 1f;

    protected bool _isLife = true;
    protected int _maxHealth;
    private bool _isInvulnerability = false;
    private WaitForSeconds _wait;

    public bool IsLife => _isLife;

    private void Awake()
    {
        _maxHealth = _amountHealth;
        _wait = new(_timeInvulnerability);
    }

    public virtual void TakeDamage()
    {
        if (!_isInvulnerability)
        {
            _amountHealth--;
            _isInvulnerability = true;
            StartCoroutine(TakingDamage());
        }
    }

    private IEnumerator TakingDamage()
    {
        yield return _wait;
        _isInvulnerability = false;
    }
}
