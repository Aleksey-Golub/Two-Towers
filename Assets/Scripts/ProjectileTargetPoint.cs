using UnityEngine;

public class ProjectileTargetPoint : MonoBehaviour, IDamageAble
{
    [SerializeField] private CastleTargetPoint _targetPoint;

    public void TakeDamage(int damage)
    {
        _targetPoint.TakeDamage(damage);
    }
}
