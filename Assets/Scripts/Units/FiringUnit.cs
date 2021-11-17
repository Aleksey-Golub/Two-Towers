using UnityEngine;

public class FiringUnit : Unit, IShootAble
{
    [Header("Для дальней атаки")]
    [SerializeField] private AnimationCurve _default_bullet_flight_curve;
    [SerializeField] private Transform _shoot_point;
    [SerializeField] private float _bullet_flight_time = 1f;
    [SerializeField] private Projectile _projectile_prefab;
    [SerializeField] private bool _parabolicTrajectory = true;

    public AnimationCurve DefaultBulletFlightCurve => _default_bullet_flight_curve;
    public Transform ShootPoint => _shoot_point;
    public float BulletFlightTime => _bullet_flight_time;
    public Projectile ProjectilePrefab => _projectile_prefab;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckRangedTarget();
    }
    
    public void CheckRangedTarget()
    {
        var _hit = Physics2D.Raycast(transform.position, _directionToEnemy, _attackRange, _enemyLayerMask);

        if (_hit.collider != null)
        {
            _target = _hit.collider.transform;
        }
        else
            _target = _enemy_castle.TargetPoint.transform;
    }

    public override void DamageTarget()
    {
        _attackTimer = 0;

        // запуск снаряда
        var projectile = Instantiate(_projectile_prefab, _shoot_point.position, _shoot_point.rotation);
        projectile.Init(_target.GetComponentInChildren<ProjectileTargetPoint>().transform.position, _bullet_flight_time, _target.GetComponent<IDamageAble>(), _damage, _default_bullet_flight_curve, _parabolicTrajectory);
    }
}
