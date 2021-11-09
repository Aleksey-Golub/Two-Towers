using UnityEngine;

public class CastleTargetPoint : MonoBehaviour, IDamageAble
{
    private Castle _my_castle;

    private void Start()
    {
        _my_castle = GetComponentInParent<Castle>();
    }

    public void TakeDamage(int damage)
    {
        _my_castle.TakeDamage(damage);
    }
}
