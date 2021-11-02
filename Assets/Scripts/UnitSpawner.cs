using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private UnitController _knightPrefab;
    [SerializeField] private UnitController _archerPrefab;
    [SerializeField] private int _team_index;
    [SerializeField] private Transform _spawn_point;

    private Transform _enemy_spawn_point;
    private Vector2 _normalizeDirectionToEnemySpawn;

    private void Start()
    {
        FindEnemySpawn();
        _normalizeDirectionToEnemySpawn = (_enemy_spawn_point.position - _spawn_point.position).normalized;

        //SpawnUnit(_archerPrefab);
        StartCoroutine(Spawn5Units(_archerPrefab));
    }


    private IEnumerator Spawn5Units(UnitController unit)
    {
        var waitForTwoSecond = new WaitForSeconds(2f);
        for (int i = 0; i < 5; i++)
        {
            SpawnUnit(unit);
            yield return waitForTwoSecond;
        }
    }

    private void SpawnUnit(UnitController unit)
    {
        var go = Instantiate(unit, _spawn_point.position, _spawn_point.rotation) as UnitController;
        go.Init(_team_index, _enemy_spawn_point, _normalizeDirectionToEnemySpawn);
    }

    private void FindEnemySpawn()
    {
        foreach (var p in FindObjectsOfType<SpawnPoint>())
        {
            if (p.transform != _spawn_point)
                _enemy_spawn_point = p.transform;
        }
    }
}
