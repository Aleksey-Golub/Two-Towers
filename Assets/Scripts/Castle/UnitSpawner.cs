using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _knightPrefab;
    [SerializeField] private Unit _archerPrefab;
    [SerializeField] private int _team_index;
    [SerializeField] private Transform _spawn_point;

    [Header("Castle Weapon Spawner")]
    [SerializeField] private CastleWeapon _ballistaPrefab;
    [SerializeField] private Transform _castleWeaponRaycastPoint;
    [SerializeField] private Transform _ballistaSpawnPoint;

    private Castle _enemy_castle;
    private Vector2 _normalizeDirectionToEnemySpawn;

    private void Start()
    {
        FindEnemyCastle();
        _normalizeDirectionToEnemySpawn = (_enemy_castle.TargetPoint.transform.position - _spawn_point.position).normalized;

        // test zone
        SpawnWeapon();
        //SpawnUnit(_archerPrefab);
        //SpawnUnit(_knightPrefab);
        StartCoroutine(Spawn5Units(_archerPrefab));
        //StartCoroutine(Spawn5Units(_knightPrefab));
    }

    private IEnumerator Spawn5Units(Unit unit)
    {
        var waitForTwoSecond = new WaitForSeconds(2f);
        for (int i = 0; i < 5; i++)
        {
            SpawnUnit(unit);
            yield return waitForTwoSecond;
        }
    }

    private void SpawnWeapon() 
    {
        var go = Instantiate(_ballistaPrefab, _ballistaSpawnPoint.position, _ballistaSpawnPoint.rotation, _ballistaSpawnPoint) as CastleWeapon;
        go.Init(_team_index, _normalizeDirectionToEnemySpawn, _castleWeaponRaycastPoint);
    }

    private void SpawnUnit(Unit unit)
    {
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
