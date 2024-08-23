using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Attacker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
            health.TakeDamage();
    }
}
