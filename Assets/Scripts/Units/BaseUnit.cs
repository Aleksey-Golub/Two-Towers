using UnityEngine;

 public abstract class BaseUnit : MonoBehaviour
{
    [SerializeField] protected float _attackRechargeTime = 3;
    [SerializeField] protected float _attackRange = 2.5f; // 2.5 ��� ���� -  1+1+0,2+0,2 � 0,1 �����
    [SerializeField] protected int _damage = 30;

    protected Transform _target;
    protected float _attackTimer;
    protected Vector2 _directionToEnemy;
    protected int _enemyLayerMask = -1;

    public abstract void DamageTarget();
}
