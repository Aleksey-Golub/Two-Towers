using UnityEngine;

public class Castle : MonoBehaviour, IDamageAble
{
    public Transform BulletTargetPoint;
    public CastleTargetPoint TargetPoint;

    private int _health = 250;

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            // to do
            Debug.Log("GAME OVER");
        }
    }
}
