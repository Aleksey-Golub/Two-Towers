using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Castle))]
public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private UnitPreset[] _unitsPresets;  // [knight, archer, mage, catapult]
    [SerializeField] private int _team_index;
    [SerializeField] private Transform _spawn_point;
    [SerializeField] private float _unitSpawnDelay = 1.5f;
    [SerializeField] private float _checkBlockRange = 1.1f;
    
    [Header("Castle Weapon Spawner")]
    [SerializeField] private CastleWeapon _ballistaPrefab;
    [SerializeField] private Transform _castleWeaponRaycastPoint;
    [SerializeField] private Transform _ballistaSpawnPoint;

    private Castle _myCastle;
    private Vector2 _normalizeDirectionToEnemySpawn;
    private int _enemyLayerMask;
    private int _myLayerMask;
    private int _myLayer;
    private Queue<int> _queueOfUnitIndexes = new Queue<int>();
    private float _unitSpawnTimer;

    public UnitPreset[] UnitsPresets => _unitsPresets;
    public event UnityAction<int, float> SpawnProgressUpdated;
    public int TeamIndex => _team_index;
    public Castle Enemy_castle { get; private set; }

    private void Awake()
    {
        FindEnemyCastle();
        _normalizeDirectionToEnemySpawn = (Enemy_castle.TargetPoint.transform.position - _spawn_point.position).normalized;
        _myLayer = _team_index == 0 ? 30 : 31;
        _myLayerMask = 1 << (_team_index == 0 ? 30 : 31);
        _enemyLayerMask = 1 << (_team_index == 0 ? 31 : 30);
    }

    private void Start()
    {
        _myCastle = GetComponent<Castle>();

        // test zone
        // _unitsPrefabs[knight, archer, mage, catapult]

        SpawnWeapon();
        //SpawnUnit(_unitsPrefabs[0]);
        //StartCoroutine(Spawn5Units(0));
    }

    private void Update()
    {
        if (_queueOfUnitIndexes.Count > 0)
        {
            _unitSpawnTimer += Time.deltaTime;

            float nomalizeTime = _unitSpawnTimer / _unitSpawnDelay;
            nomalizeTime = Mathf.Clamp(nomalizeTime, 0, 0.99f);
            SpawnProgressUpdated?.Invoke(_queueOfUnitIndexes.Peek(), nomalizeTime);
        }

        if (IsPossibleToSpawnUnit())
        {
            SpawnProgressUpdated?.Invoke(_queueOfUnitIndexes.Peek(), 1);
            SpawnUnit(_queueOfUnitIndexes.Dequeue());
            _unitSpawnTimer = 0;
        }
    }

    private void OnValidate()
    {
        if (TryGetComponent(out SimpleAI aI))
        {
            aI.ValidateMaxUnitIndex();
        }
    }

    public bool TryBuyUnit(int unitIndex)
    {
        int unitCost = _unitsPresets[unitIndex].Prefab.Cost;

        if (_myCastle.Wallet.Money >= unitCost)
        {
            _myCastle.Wallet.SpendMoney(unitCost);
            _queueOfUnitIndexes.Enqueue(unitIndex);
            return true;
        }
        return false;
    }

    private bool IsPossibleToSpawnUnit()
    {
        return _unitSpawnTimer >= _unitSpawnDelay && !IsBlock();
    }

    private bool IsBlock()
    {
        var hit = Physics2D.Raycast(_spawn_point.position, _normalizeDirectionToEnemySpawn, _checkBlockRange, _myLayerMask);
        
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private IEnumerator Spawn5Units(int unitIndex)
    {
        var waitForTwoSecond = new WaitForSeconds(2f);
        for (int i = 0; i < 5; i++)
        {
            SpawnUnit(unitIndex);
            yield return waitForTwoSecond;
        }
    }

    private void SpawnWeapon()
    {
        var go = Instantiate(_ballistaPrefab, _ballistaSpawnPoint.position, _ballistaSpawnPoint.rotation, _ballistaSpawnPoint) as CastleWeapon;
        go.Init(_team_index, _normalizeDirectionToEnemySpawn, _castleWeaponRaycastPoint);
    }

    private void SpawnUnit(int unitIndex)
    {
        var unit = _unitsPresets[unitIndex].Prefab;

        var go = Instantiate(unit, _spawn_point.position, _spawn_point.rotation) as Unit;
        go.Init(_team_index, Enemy_castle, _normalizeDirectionToEnemySpawn, _myLayer, _enemyLayerMask);
    }

    private void FindEnemyCastle()
    {
        foreach (var p in FindObjectsOfType<Castle>())
        {
            if (p != GetComponent<Castle>())
                Enemy_castle = p;
        }
    }
}
