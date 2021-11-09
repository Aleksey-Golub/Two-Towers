using UnityEngine;

public class TestA : MonoBehaviour
{
    [SerializeField] private int _health;

    private void Start()
    {
        Debug.Log("Start TestA");
    }

    private void Update()
    {
        Debug.Log("Update TestA");
    }
}
