using UnityEngine;

 public abstract class BaseUnit : MonoBehaviour
{
    [SerializeField] protected float _attackRechargeTime = 3;
    [SerializeField] protected int _damage = 30;
    [SerializeField] protected float _attackRange = 3.5f; // требует возможной коррекции при увеличении коллайдеров юнитов
    /*          2.4	2.2	2.0     матрица рассчета необходимой _attackRange
        	1.2	3.6	3.4	3.2
			1.1	3.5	3.3	3.1
			1.0	3.4	3.2	3.0
    */

    protected Transform _target;
    protected float _attackTimer;
    protected Vector2 _directionToEnemy;
    protected int _enemyLayerMask = -1;

    public abstract void DamageTarget();
}
