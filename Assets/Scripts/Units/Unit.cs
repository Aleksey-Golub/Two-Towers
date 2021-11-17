using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Unit : BaseUnit, IDamageAble
{
    [SerializeField] private int _cost;
    [SerializeField] private Unit_Viewer _viewer;
    [SerializeField] private ParticleSystem _hit_effect;
    [SerializeField] protected int _currentHealth;

    public int Cost => _cost;
    public int _maxHealth = 100;
    public int _armor = 0;
    public float _movement_speed = 1.5f;
    public float _stop_distance = 2.4f; // 2,4 для всех типов юнитов
    public float _size_multiplier = 1;
    public Color _color = Color.white;
    public int _team;
    public UnitType _type;
    public int _grade = 1;

    public event UnityAction<int, int> HealthChanged;
    public event UnityAction<int> GradeChanged;

    protected Castle _enemy_castle;


    UnitState _currentState = UnitState.stay;
    Rigidbody2D _rb;
    bool _is_blocked = false;
    /// <summary>
    /// Нахождение в состоянии атаки. Устанавливается в false из AnimationEvent в анимации Attack юнита, в true во время атаки
    /// </summary>
    bool _is_atacking = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
        GradeChanged?.Invoke(_grade);
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        switch (_currentState)
        {
            case UnitState.stay:
                _viewer.AnimatorChanger(2);

                if (_is_blocked == false && _is_atacking == false && Vector2.Distance(transform.position, _enemy_castle.TargetPoint.transform.position) > _stop_distance)
                    _currentState = UnitState.walk;
                else if (_attackTimer >= _attackRechargeTime && _is_atacking == false && Vector2.Distance(transform.position, _target.position) <= _attackRange)
                    _currentState = UnitState.attack;
                break;

            case UnitState.walk:
                _viewer.AnimatorChanger(1);

                if (_is_blocked || Vector2.Distance(transform.position, _enemy_castle.TargetPoint.transform.position) <= _stop_distance)
                    _currentState = UnitState.stay;
                else if (_attackTimer >= _attackRechargeTime && Vector2.Distance(transform.position, _target.position) <= _attackRange)
                    _currentState = UnitState.attack;
                break;

            case UnitState.attack:
                _viewer.AnimatorChanger(0);

                _is_atacking = true;
               

                _currentState = UnitState.stay;
                break;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (_currentState == UnitState.walk)
            Move();

        CorrectSize();
    }

    public override void DamageTarget()
    {
        _attackTimer = 0;
        _target.GetComponent<IDamageAble>().TakeDamage(_damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Unit unit))
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

    private bool IsPossibleToMergeWith(Unit otherUnit)
    {
        return otherUnit._type == _type && otherUnit._grade == _grade && _grade < 4;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Unit unit))
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

    private bool IsUnitInFrontOfUs(Unit unit)
    {
        return ((_directionToEnemy.x > 0 && transform.position.x < unit.transform.position.x) || (_directionToEnemy.x < 0 && transform.position.x > unit.transform.position.x));
    }

    private void Move()
    {
        Debug.Log("Move");
        _rb.MovePosition(_rb.position + _directionToEnemy * _movement_speed * Time.deltaTime);
    }

    public void OffAttacking()
    {
        _is_atacking = false;
    }

    private void UpGrade()
    {
        _grade++;
        GradeChanged?.Invoke(_grade);

        // логика улучшения
        _size_multiplier += 0.1f;
        _currentHealth *= 2;
        _maxHealth *= 2;
    }

    public void Init(int teamIndex, Castle enemyCastle, Vector2 moveDirection)
    {
        _team = teamIndex;
        _enemy_castle = enemyCastle;
        _target = _enemy_castle.TargetPoint.transform;
        _directionToEnemy = moveDirection;

        gameObject.layer = _team == 0 ? 30 : 31;
        _enemyLayerMask = 1 << (_team == 0 ? 31 : 30);
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
        _currentHealth -= reseavedDamage;
        _hit_effect.Play();
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
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
        wizard,
        catapult,
        ninja,
        samuraiHeavy,
        samuraiLight,
        vikingLight,
        vikingShield,
        vikingTwoHandedAxe
    }
}
