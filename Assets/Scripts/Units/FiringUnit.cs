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

    protected override void Update()
    {
        CheckRangedTarget();
        base.Update();
    }
    
    public void CheckRangedTarget()
    {
        var hit = Physics2D.Raycast(transform.position, _directionToEnemy, _attackRange, _enemyLayerMask);

        if (hit.collider != null)
        {
            _target = hit.collider.transform;
        }
        else
            _target = _enemy_castle.TargetPoint.transform;
    }

    public override void DamageTarget()
    {
        _attackTimer = 0;

        if (Vector2.Distance(transform.position, _target.position) <= _attackRange)
        {
            // запуск снаряда
            var projectile = Instantiate(_projectile_prefab, _shoot_point.position, _shoot_point.rotation);
            projectile.Init(_target.GetComponentInChildren<ProjectileTargetPoint>().transform.position, _bullet_flight_time, _damage, _default_bullet_flight_curve, _parabolicTrajectory);
        }
    }
}
