using UnityEngine;

public class CastleWeapon : BaseUnit, IShootAble
{
    [SerializeField] private AnimationCurve _defaultProjectileFlightCurve;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _bulletFlightTime = 0.25f;
    [SerializeField] private Projectile _projectilePrefab;

    private Transform _raycastOrigin;

    public AnimationCurve DefaultBulletFlightCurve => _defaultProjectileFlightCurve;
    public Transform ShootPoint => _shootPoint;
    public float BulletFlightTime => _bulletFlightTime;
    public Projectile ProjectilePrefab => _projectilePrefab;

    private void Update()
    {
        _attackTimer += Time.deltaTime;
              
        if (_target && _attackTimer >= _attackRechargeTime)
            DamageTarget();
    }

    private void FixedUpdate()
    {
        if (_attackTimer >= _attackRechargeTime)
            CheckRangedTarget();
    }

    public void CheckRangedTarget()
    {
        var _hit = Physics2D.Raycast(_raycastOrigin.position, _directionToEnemy, _attackRange, _enemyLayerMask);

        if (_hit.collider != null)
        {
            _target = _hit.collider.transform;
        }
        else
        {
            _target = null;
        }
    }

    public override void DamageTarget()
    {
        _attackTimer = 0;

         var projectileTargetPoint = _target.GetComponentInChildren<ProjectileTargetPoint>().transform;
        transform.LookAt(projectileTargetPoint);

        // запуск снаряда
        var projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.Init(projectileTargetPoint.position, _bulletFlightTime, _target.GetComponent<IDamageAble>(), _damage, _defaultProjectileFlightCurve, false);
        
        _target = null;
    }

    public void Init(int teamIndex, Vector2 directionToEnemy, Transform targetCheckedPosition)
    {
        _raycastOrigin = targetCheckedPosition;
        _directionToEnemy = directionToEnemy;

        gameObject.layer = teamIndex == 0 ? 30 : 31;
        _enemyLayerMask = 1 << (teamIndex == 0 ? 31 : 30);
    }
}
