using UnityEngine;

public class FiringUnitController : UnitController
{
    [Header("Для дальней атаки")]
    [SerializeField] private AnimationCurve _default_bullet_flight_curve;
    [SerializeField] private Transform _shoot_point;
    [SerializeField] private float _bullet_flight_time = 1f;
    [SerializeField] private Projectile _projectile_prefab;

    protected override void CheckRangedTarget()
    {
        _hit = Physics2D.Raycast(transform.position, _move_direction, _attack_range, _enemy_layer_mask);

        if (_hit.collider != null)
        {
            _target = _hit.collider.transform;
        }
        else
            _target = _enemy_castle.TargetPoint.transform;
    }

    public override void DamageTarget()
    {
        // запуск снаряда
        var projectile = Instantiate(_projectile_prefab, _shoot_point.position, _shoot_point.rotation);
        projectile.Init(_target.GetComponentInChildren<ProjectileTargetPoint>().transform.position, _bullet_flight_time, _target.GetComponent<IDamageAble>(), _damage, _default_bullet_flight_curve);
    }
}
