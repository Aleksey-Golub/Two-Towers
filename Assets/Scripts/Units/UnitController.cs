using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class UnitController : MonoBehaviour, IDamageAble
{
    [SerializeField] private Unit_Viewer _viewer;

    public int _health = 100;
    public int _armor = 0;
    public int _damage = 30;
    public float _movement_speed = 1.5f;
    public float _stop_distance = 2.4f; // 2,4 для всех типов юнитов
    public float _attack_recharge = 3;
    public float _attack_range = 2.5f; // 2.5 для мили 1+1+0,2+0,2 и 0,1 запас
    public float _size_multiplier = 1;
    public Color _color = Color.white;
    public int _team;
    public UnitType _type;
    public int _grade = 1;
    public AnimationCurve _bullet_flight_curve;
    public AttackType _attack_type;

    float _attack_timer;
    Castle _enemy_castle;
    UnitState _currentState = UnitState.stay;
    Transform _target;
    Rigidbody2D _rb;
    Vector2 _move_direction;
    bool _is_blocked = false;
    /// <summary>
    /// Устанавливается в false из AnimationEvent в анимации Attack юнита, в true во время атаки
    /// </summary>
    bool _is_atacking = false;
    RaycastHit2D _hit;
    int _enemy_layer_mask = -1;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _attack_timer += Time.deltaTime;

        switch (_currentState)
        {
            case UnitState.stay:
                _viewer.AnimatorChanger(2);

                if (_is_blocked == false && _is_atacking == false && Vector2.Distance(transform.position, _enemy_castle.TargetPoint.transform.position) > _stop_distance)
                    _currentState = UnitState.walk;
                else if (_attack_timer >= _attack_recharge && _is_atacking == false && Vector2.Distance(transform.position, _target.position) <= _attack_range)
                    _currentState = UnitState.attack;
                break;

            case UnitState.walk:
                _viewer.AnimatorChanger(1);

                if (_is_blocked || Vector2.Distance(transform.position, _enemy_castle.TargetPoint.transform.position) <= _stop_distance)
                    _currentState = UnitState.stay;
                else if (_attack_timer >= _attack_recharge && Vector2.Distance(transform.position, _target.position) <= _attack_range)
                    _currentState = UnitState.attack;
                break;

            case UnitState.attack:
                _viewer.AnimatorChanger(0);
                Attack(_target.GetComponent<IDamageAble>());

                _currentState = UnitState.stay;
                break;
        }
    }
    private void FixedUpdate()
    {
        if (_attack_type == AttackType.ranged)
            CheckRangedTarget();

        if (_currentState == UnitState.walk)
            Move();

        CorrectSize();
    }

    private void CheckRangedTarget()
    {
        if (_attack_timer >= _attack_recharge)
        {
            _hit = Physics2D.Raycast(transform.position, _move_direction, _attack_range, _enemy_layer_mask);

            if (_hit.collider != null)
            {
                _target = _hit.collider.transform;
            }
            else
                _target = _enemy_castle.TargetPoint.transform;
        }
    }

    private void Attack(IDamageAble target)
    {
        _is_atacking = true;
        // attack logic
        target.TakeDamage(_damage);

        _attack_timer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out UnitController unit))
            return;

        if (unit._team != _team)
        {
            _target = unit.transform;
            _is_blocked = true;
        }
        else
        {
            if (IsUnitInFrontOfUs(unit))
                _is_blocked = true;

            if (IsPossibleToMergeWith(unit))
            {
                // to do // убирать в пул объектов
                unit.gameObject.SetActive(false);

                UpGrade();
            }
        }
    }

    private bool IsPossibleToMergeWith(UnitController otherUnit)
    {
        return otherUnit._type == _type && otherUnit._grade == _grade && _grade < 4;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out UnitController unit))
            return;
            
        if (unit._team != _team)
        {
            _target = _enemy_castle.TargetPoint.transform.transform;
            _is_blocked = false;
        }
        else
        {
            if (IsUnitInFrontOfUs(unit))
                _is_blocked = false;
        }
    }

    private bool IsUnitInFrontOfUs(UnitController unit)
    {
        return ((_move_direction.x > 0 && transform.position.x < unit.transform.position.x) || (_move_direction.x < 0 && transform.position.x > unit.transform.position.x));
    }

    private void Move()
    {
        Debug.Log("Move");
        _rb.MovePosition(_rb.position + _move_direction * _movement_speed * Time.deltaTime);
    }

    public void OffAttacking()
    {
        _is_atacking = false;
    }

    public void UpGrade()
    {
        _grade++;

        // логика улучшения
        _size_multiplier += 0.1f;
        _health *= 2;
    }

    public void Init(int team_index, Castle enemy_castle, Vector2 move_direction)
    {
        _team = team_index;
        _enemy_castle = enemy_castle;
        _target = _enemy_castle.TargetPoint.transform;
        _move_direction = move_direction;

        gameObject.layer = _team == 0 ? 30 : 31;
        _enemy_layer_mask = 1 << (_team == 0 ? 31 : 30);
    }

    public IEnumerator ChangeScale(float targetSizeMultiplier, float duration)
    {
        float timer = 0;
        float startSize = _size_multiplier;
        float nextSize;

        while (timer < duration)
        {
            nextSize = Mathf.Lerp(startSize, targetSizeMultiplier, timer / duration);

            _size_multiplier = nextSize;
            Vector3 newScale = Vector3.one * nextSize;
            transform.localScale = newScale;

            timer += Time.deltaTime;
            yield return null;
        }
        _size_multiplier = targetSizeMultiplier;
        transform.localScale = Vector3.one * targetSizeMultiplier;
    }

    private void CorrectSize()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(1 * _size_multiplier, 1 * _size_multiplier), .2f);
    }

    public void TakeDamage(int damage)
    {
        int reseavedDamage = Mathf.Max(0, damage - _armor);
        _health -= reseavedDamage;

        if(_health <= 0)
        {
            _viewer.AnimatorChanger(3);

            Die();
        }
    }

    private void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        enabled = false;
    }

    public enum UnitState
    {
        attack,
        walk,
        stay,
        death
    }

    public enum UnitType
    {
        knight,
        archer,
        wizard
    }

    public enum AttackType
    {
        melee,
        ranged
    }
}
