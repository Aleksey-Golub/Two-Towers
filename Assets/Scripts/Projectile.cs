using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private AnimationCurve _flight_curve;

    private IDamageAble _target;
    private int _damage;
    private Vector3 _start_point;
    private Vector3 _end_point;
    private float _flight_time;
    private float _timer;

    public void Init(Vector3 endPoint, float flightTime, IDamageAble target, int damage, AnimationCurve curve, bool needCorrectDefaultCurve = true)
    {
        _target = target;
        _damage = damage;
        _start_point = transform.position;
        _end_point = endPoint;
        _flight_time = flightTime;
        
        SetCurve(curve, needCorrectDefaultCurve);
    }

    private void SetCurve(AnimationCurve defaultCurve, bool needCorrectDefaultCurve)
    {
        _flight_curve = new AnimationCurve(defaultCurve.keys);

        if (needCorrectDefaultCurve == false)
            return;

        float yDelta = _end_point.y - _start_point.y;
        if (yDelta == 0)
            return;

        Keyframe endKey = _flight_curve.keys[2];
        endKey.value += yDelta;
        _flight_curve.MoveKey(2, endKey);

        Keyframe middleKey = _flight_curve.keys[1];
        middleKey.value = yDelta > middleKey.value ? yDelta : middleKey.value;
        middleKey.time = Mathf.Clamp(middleKey.time + yDelta, 0.2f, 0.95f);
        _flight_curve.MoveKey(1, middleKey);
    }

    private void Update()
    {
        Vector3 newPos = Vector3.zero;
        
        _timer += Time.deltaTime;
        if (_timer >= _flight_time)
        {
            // проверка target на возможность быть пораженной: в пуле или null
            _target.TakeDamage(_damage);

            // to do //obj pool
            Destroy(gameObject);
        }

        float normalizeFlightTime = _timer / _flight_time;

        newPos.x = Mathf.Lerp(_start_point.x, _end_point.x, normalizeFlightTime);
        newPos.y = _start_point.y + _flight_curve.Evaluate(normalizeFlightTime);

        transform.LookAt(newPos);
        transform.position = newPos;
    }
}
