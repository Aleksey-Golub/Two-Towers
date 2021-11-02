using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class UnitController : MonoBehaviour
{
    [SerializeField] private Unit_Viewer _viewer;

    public int _health = 100;
    public int _armor = 0;
    public float _movement_speed = 1.5f;
    public float _attack_recharge = 3;
    public float _size_multiplier = 1;
    public Color _color = Color.white;
    public float _attack_range = 1;
    public int _team;
    public UnitType _type;
    public int _grade = 1;

    float _attack_timer;
    Transform _enemy_spawn;
    UnitState _currentState = UnitState.stay;
    UnitController _target_enemy;
    Rigidbody2D _rb;
    Vector2 _move_direction;
    bool _is_blocked = false;
    bool _is_atacking = false;
    RaycastHit2D[] _hits;// = new RaycastHit2D[10];

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _attack_timer += Time.deltaTime;

        if (_attack_timer >= _attack_recharge)
        {
            //int hit_count = Physics2D.RaycastNonAlloc(transform.position, _move_direction, _hits, _attack_range);
            _hits = Physics2D.RaycastAll(transform.position, _move_direction, _attack_range);

            if (_hits.Length > 1)//(hit_count > 1)
            {
                var hit = _hits.FirstOrDefault(h => h.collider.gameObject.GetComponent<UnitController>()._team != _team);

                _target_enemy = hit.collider.gameObject.GetComponent<UnitController>();
            }
        }
        


        if (_is_atacking == false && ((_is_blocked == false && Vector2.Distance(transform.position, _enemy_spawn.position) > _attack_range) && (_target_enemy == null || _target_enemy && _attack_timer < _attack_recharge)))
        //if ((_is_blocked == false && Vector2.Distance(transform.position, _enemy_spawn.position) > _attack_range && _target_enemy == null) 
        // || (_is_blocked == false && Vector2.Distance(transform.position, _enemy_spawn.position) > _attack_range && _target_enemy && _attack_timer < _attack_recharge))
        {
            if (_currentState != UnitState.walk)
            {
                _currentState = UnitState.walk;
                _viewer.AnimatorChanger(1);
            }
        }
        else if (_attack_timer >= _attack_recharge && (_target_enemy || Vector2.Distance(transform.position, _enemy_spawn.position) <= _attack_range))
        {
            if (_currentState != UnitState.attack)
            {
                _currentState = UnitState.attack;
                _viewer.AnimatorChanger(0);
            }
            Attack(_target_enemy);
        }
        else
        {
            if (_currentState != UnitState.stay)
            {
                _currentState = UnitState.stay;
                _viewer.AnimatorChanger(2);
            }
        }

        //switch (_currentState)
        //{
        //    case UnitState.stay:
        //        if (_is_blocked == false)
        //        {

        //        }
        //        break;
        //    case UnitState.walk:
        //        break;
        //    case UnitState.attack:
        //        break;
        //    case UnitState.death:
        //        break;
        //}
    }
    private void FixedUpdate()
    {
        if (_currentState == UnitState.walk)
            Move();
        CorrectSize();
    }

    
    private void Attack(UnitController unit)
    {
        _is_atacking = true;
        // attack logic

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

    public void Init(int team_index, Transform enemy_spawn, Vector2 move_direction)
    {
        _team = team_index;
        _enemy_spawn = enemy_spawn;
        _move_direction = move_direction;
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
