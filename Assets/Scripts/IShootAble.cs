using UnityEngine;

public interface IShootAble
{
    public AnimationCurve DefaultBulletFlightCurve { get; }
    public Transform ShootPoint { get; }
    public float BulletFlightTime { get; }
    public Projectile ProjectilePrefab { get; }

    public abstract void CheckRangedTarget();
}
