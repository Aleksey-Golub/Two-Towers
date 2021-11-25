using System.Collections;
using UnityEngine;

public class TestA : MonoBehaviour
{
    [SerializeField] private int _health;

    private void Start()
    {
        Debug.Log("Start TestA");
    }

    protected virtual void Update()
    {
        Debug.Log("Update TestA");
    }

    public static void Foo()
    {
        Debug.Log("Foo");
    }
}
