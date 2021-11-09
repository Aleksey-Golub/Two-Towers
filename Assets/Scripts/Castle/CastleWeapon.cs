using UnityEngine;

public class CastleWeapon : MonoBehaviour
{
    [SerializeField] private AnimationCurve _defaultProjectileFlightCurve;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _bulletFlightTime = 0.25f;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _attackRechargeTime = 3;
    [SerializeField] private float _attackRange = 3;
    [SerializeField] private int _damage = 20;

    private Transform _raycastOrigin;
    private Transform _target;
    //private RaycastHit2D _hit;
    private float _attackTimer;
    private Vector2 _shootDirection;
    private int _enemyLayerMask = -1;

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackRechargeTime && _target)
            DamageTarget();
    }

    private void FixedUpdate()
    {
        if (_attackTimer >= _attackRechargeTime)
            CheckRangedTarget();
    }

    private void CheckRangedTarget()
    {
        var _hit = Physics2D.Raycast(_raycastOrigin.position, _shootDirection, _attackRange, _enemyLayerMask);

        if (_hit.collider != null)
        {
            _target = _hit.collider.transform;
        }
        else
        {
            _target = null;
        }
    }

    private void DamageTarget()
    {
        // запуск снаряда
        var projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.Init(_target.GetComponentInChildren<ProjectileTargetPoint>().transform.position, _bulletFlightTime, _target.GetComponent<IDamageAble>(), _damage, _defaultProjectileFlightCurve, false);

        _attackTimer = 0;
    }

    public void Init(int teamIndex, Vector2 moveDirection, Transform targetCheckedPosition)
    {
        _raycastOrigin = targetCheckedPosition;
        _shootDirection = moveDirection;

        gameObject.layer = teamIndex == 0 ? 30 : 31;
        _enemyLayerMask = 1 << (teamIndex == 0 ? 31 : 30);
    }
}
