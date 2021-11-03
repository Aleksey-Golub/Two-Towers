using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private UnitController _knightPrefab;
    [SerializeField] private UnitController _archerPrefab;
    [SerializeField] private int _team_index;
    [SerializeField] private Transform _spawn_point;

    private Castle _enemy_castle;
    private Vector2 _normalizeDirectionToEnemySpawn;

    private void Start()
    {
        FindEnemyCastle();
        _normalizeDirectionToEnemySpawn = (_enemy_castle.TargetPoint.transform.position - _spawn_point.position).normalized;

        // test zone
        SpawnUnit(_archerPrefab);
        SpawnUnit(_knightPrefab);
        //StartCoroutine(Spawn5Units(_archerPrefab));
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
