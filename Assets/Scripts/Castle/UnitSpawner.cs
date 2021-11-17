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

    [Header("Castle Weapon Spawner")]
    [SerializeField] private CastleWeapon _ballistaPrefab;
    [SerializeField] private Transform _castleWeaponRaycastPoint;
    [SerializeField] private Transform _ballistaSpawnPoint;

    private Castle _enemy_castle;
    private Castle _myCastle;
    private Vector2 _normalizeDirectionToEnemySpawn;
    private Queue<int> _queueOfUnitIndexes = new Queue<int>();
    private float _unitSpawnTimer;

    public UnitPreset[] UnitsPresets => _unitsPresets;
    public event UnityAction<int, float> SpawnProgressUpdated;

    private void Start()
    {
        _myCastle = GetComponent<Castle>();

        FindEnemyCastle();
        _normalizeDirectionToEnemySpawn = (_enemy_castle.TargetPoint.transform.position - _spawn_point.position).normalized;

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
            SpawnProgressUpdated?.Invoke(_queueOfUnitIndexes.Peek(), nomalizeTime);
        }

        if (_unitSpawnTimer >= _unitSpawnDelay)
        {
            SpawnUnit(_queueOfUnitIndexes.Dequeue());
            _unitSpawnTimer -= _unitSpawnDelay;
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
        go.Init(_team_index, _enemy_castle, _normalizeDirectionToEnemySpawn);
    }

    private void FindEnemyCastle()
    {
        foreach (var p in FindObjectsOfType<Castle>())
        {
            if (p != GetComponent<Castle>())
                _enemy_castle = p;
        }
    }
}
