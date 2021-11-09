using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestC : TestB
{
    //private void Start()
    //{
    //    Debug.Log("Start TestC");
    //}

    protected override void Update()
    {
        base.Update();
        Debug.Log("Update TestC");
    }
}
