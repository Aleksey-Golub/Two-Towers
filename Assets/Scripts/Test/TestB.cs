using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestB : TestA
{
    private void Start()
    {
        Debug.Log("Start TestB");
    }

    protected virtual void Update()
    {
        Debug.Log("Update TestB");
    }
}
