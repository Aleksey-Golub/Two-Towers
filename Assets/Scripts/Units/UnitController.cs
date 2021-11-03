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
    public float _attack_recharge = 3;
    public float _size_multiplier = 1;
    public Color _color = Color.white;
    public float _attack_range = 1;
    public int _team;
    public UnitType _type;
    public int _grade = 1;
    public AnimationCurve _curve;

    float _attack_timer;
    Castle _enemy_castle;
    UnitState _currentState = UnitState.stay;
    UnitController _target_enemy;
    Rigidbody2D _rb;
    Vector2 _move_direction;
    bool _is_blocked = false;
    bool _is_atacking = false;
    RaycastHit2D _hit;// = new RaycastHit2D[10];
    int _enemy_layer_mask = -1;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        _attack_timer += Time.deltaTime;

        //if (_is_atacking == false && ((_is_blocked == false && Vector2.Distance(transform.position, _enemy_spawn.position) > _attack_range) && (_target_enemy == null || _target_enemy && _attack_timer < _attack_recharge)))
        ////if ((_is_blocked == false && Vector2.Distance(transform.position, _enemy_spawn.position) > _attack_range && _target_enemy == null) 
        //// || (_is_blocked == false && Vector2.Distance(transform.position, _enemy_spawn.position) > _attack_range && _target_enemy && _attack_timer < _attack_recharge))
        //{
        //    if (_currentState != UnitState.walk)
        //    {
        //        _currentState = UnitState.walk;
        //        _viewer.AnimatorChanger(1);
        //    }
        //}
        //else if (_attack_timer >= _attack_recharge && (_target_enemy || Vector2.Distance(transform.position, _enemy_spawn.position) <= _attack_range))
        //{
        //    if (_currentState != UnitState.attack)
        //    {
        //        _currentState = UnitState.attack;
        //        _viewer.AnimatorChanger(0);
        //    }
        //    Attack(_target_enemy);
        //}
        //else
        //{
        //    if (_currentState != UnitState.stay)
        //    {
        //        _currentState = UnitState.stay;
        //        _viewer.AnimatorChanger(2);
        //    }
        //}

        switch (_currentState)
        {
            case UnitState.stay:
                _viewer.AnimatorChanger(2);

                if (_is_blocked == false && _is_atacking == false && Vector2.Distance(transform.position, _enemy_castle.TargetPoint.position) > _attack_range)
                    _currentState = UnitState.walk;
                else if (_target_enemy && _attack_timer >= _attack_recharge && _is_atacking == false && Vector2.Distance(transform.position, _target_enemy.transform.position) <= _attack_range)
                    _currentState = UnitState.attack;
                break;

            case UnitState.walk:
                _viewer.AnimatorChanger(1);

                if (_is_blocked || Vector2.Distance(transform.position, _enemy_castle.TargetPoint.position) <= _attack_range)
                    _currentState = UnitState.stay;
                else if (_target_enemy && _attack_timer >= _attack_recharge && Vector2.Distance(transform.position, _target_enemy.transform.position) <= _attack_range)
                    _currentState = UnitState.attack;
                break;

            case UnitState.attack:
                _viewer.AnimatorChanger(0);
                Attack(_target_enemy);

                _currentState = UnitState.stay;
                break;
        }
    }
    private void FixedUpdate()
    {
        if (_attack_timer >= _attack_recharge)
        {
            //int hit_count = Physics2D.RaycastNonAlloc(transform.position, _move_direction, _hits, _attack_range);
            _hit = Physics2D.Raycast(transform.position, _move_direction, _attack_range, _enemy_layer_mask);

            if (_hit.collider != null)//(hit_count > 1)
            {
                _target_enemy = _hit.collider.gameObject.GetComponent<UnitController>();
            }
            else
                _target_enemy = null;
        }

        if (_currentState == UnitState.walk)
            Move();

        CorrectSize();
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
            _target_enemy = unit;
            _is_blocked = true;
        }
        else
        {
            if (IsUnitInFrontOfUs(unit))
                _is_blocked = true;

            if (unit._type == _type && unit._grade == _grade && _grade < 4)
            {
                // to do // убирать в пул объектов
                unit.gameObject.SetActive(false);

                UpGrade();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out UnitController unit))
            return;
            
        if (unit._team != _team)
        {
            _target_enemy = null;
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
        //transform.position = Vector2.MoveTowards(transform.position, _enemy_spawn.position, _movement_speed * Time.deltaTime);
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
        attack = 0,
        walk = 1,
        stay = 2,
        death = 3
    }

    public enum UnitType
    {
        knight,
        archer,
        wizard
    }
}
