using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextChangeWithSlider : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private Slider m_slider;
    [SerializeField] private int playerNum;
    private MenuController m_menuController;

    private void Start()
    {
        StartCoroutine(WaitToGetController());
    }

    public void ChangeTextAndValue()
    {
        int value = (int)m_slider.value;
        m_text.text = value.ToString();
        m_menuController.ChangePlayerSearchDepth(playerNum,value);
    }

    private int GetPlayerSearchDepth(int index)
    {
        int val = 0;
        switch (index)
        {
            case 1:
                val = m_menuController.player1.GetComponent<BaseAI>().GetSearchDepth();
                break;
            case 2:
                val = m_menuController.player2.GetComponent<BaseAI>().GetSearchDepth();
                break;
        }

        return val;
    }

    IEnumerator WaitToGetController()
    {
        while (m_menuController == null)
        {
            m_menuController = MenuController.GetMenuController();
            yield return null;
        }

        while (m_menuController.player2 == null)
        {
            yield return null;
        }

        while (m_text == null || m_slider == null)
        {
            yield return null;
        }
        
        RefreshTextAndSlider(playerNum);
        m_slider.onValueChanged.AddListener(delegate(float _) { ChangeTextAndValue(); });
    }

    public void RefreshTextAndSlider(int player)
    {
        int value = GetPlayerSearchDepth(player);
        m_text.text = value.ToString();
        m_slider.value = value;
    }
}
