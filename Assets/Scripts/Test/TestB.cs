using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestB : TestA
{
    private void Start()
    {
        Debug.Log("Start TestB");
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log("Update TestB");
    }
}
