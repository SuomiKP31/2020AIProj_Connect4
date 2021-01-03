using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckButtonScript : MonoBehaviour
{
    [SerializeField] int color;
    TestAction TestManager;
    private void Start()
    {
        TestManager = TestAction.GetTestManager();
    }

    public void OnClickColor()
    {
        TestManager.color = color;
    }
}
