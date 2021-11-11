using UnityEditor;
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

    /// <summary>
    /// Projectile Initialization. 
    /// </summary>
    /// <param name="endPoint"></param>
    /// <param name="flightTime"></param>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    /// <param name="curve"></param>
    /// <param name="doParabolicTrajectory">true - parabolic trajectory, animation curve have to consist of three keys; false - linear trajectory, animation curve have to consist of two keys</param>
    public void Init(Vector3 endPoint, float flightTime, IDamageAble target, int damage, AnimationCurve curve, bool doParabolicTrajectory = true)
    {
        _target = target;
        _damage = damage;
        _start_point = transform.position;
        _end_point = endPoint;
        _flight_time = flightTime;
        
        SetCurve(curve, doParabolicTrajectory);
    }

    private void SetCurve(AnimationCurve defaultCurve, bool doParabolicTrajectory)
    {
        _flight_curve = new AnimationCurve(defaultCurve.keys);

        float yDelta = _end_point.y - _start_point.y;
        if (yDelta == 0)
            return;

        int endKeyIndex = _flight_curve.keys.Length - 1;
        Keyframe endKey = _flight_curve.keys[endKeyIndex];
        endKey.value += yDelta;
        _flight_curve.MoveKey(endKeyIndex, endKey);

        if (doParabolicTrajectory == false)
        {
            for (int i = 0; i < _flight_curve.keys.Length; i++)
            {
                AnimationUtility.SetKeyLeftTangentMode(_flight_curve, i, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyRightTangentMode(_flight_curve, i, AnimationUtility.TangentMode.Linear);
            }
            
            return;
        }

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
