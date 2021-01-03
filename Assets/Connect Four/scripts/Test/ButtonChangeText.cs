using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeText : MonoBehaviour
{
    TestAction testManager;
    [SerializeField] Text m_text;
    [SerializeField] int position_x;
    [SerializeField] int position_y;

    private void Awake()
    {
       
    
    }
    private void Start()
    {
        testManager = TestAction.GetTestManager();
        m_text.text = 0.ToString();
    }
    void ChangeText(int input)
    {
        m_text.text = input.ToString();
    }
    public void OnClickChangeValue()
    {
        //first column then row, (0,0) on left top
        int value = testManager.changeValue(position_x, position_y);
        ChangeText(value);
        if(testManager.MinimaxAICheckWin(position_y, position_x))
        {
            Debug.Log("Yes");
        }
        else
        {
            Debug.Log("Not Yet");
        }
    }

}
