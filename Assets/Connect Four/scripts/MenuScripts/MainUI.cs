using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Dropdown m_player1DropDown;
    [SerializeField] private Dropdown m_player2DropDown;
    [SerializeField] private Dropdown m_heur1DropDown;
    [SerializeField] private Dropdown m_heur2DropDown;
    [SerializeField] private Dropdown m_heurConfigDropDown;

    [SerializeField] private List<InputField> m_heurConfigList;



    private MenuController m_controller;
    private Dropdown.OptionData m_configingHeur;
    private void Start()
    {
        foreach (var player in MenuController.GetMenuController().m_p1List)
        {
            m_player1DropDown.options.Add(new Dropdown.OptionData(player.name));
        }
        foreach (var player in MenuController.GetMenuController().m_p2List)
        {
            m_player2DropDown.options.Add(new Dropdown.OptionData(player.name));
        }
        foreach (var heur in MenuController.GetMenuController().m_heuristicList)
        {
            m_heur1DropDown.options.Add(new Dropdown.OptionData(heur.name));
            m_heur2DropDown.options.Add(new Dropdown.OptionData(heur.name));
            m_heurConfigDropDown.options.Add(new Dropdown.OptionData(heur.name));
        }
        m_controller = MenuController.GetMenuController();
        
        m_controller.ChangePlayer(1,m_player1DropDown.options[m_player1DropDown.value]);
        m_controller.ChangePlayer(2,m_player2DropDown.options[m_player2DropDown.value]);
        m_controller.ChangePlayerHeuristic(1, m_heur1DropDown.options[m_heur1DropDown.value]);
        m_controller.ChangePlayerHeuristic(2, m_heur2DropDown.options[m_heur2DropDown.value]);

        foreach (var dropdown in new List<Dropdown> {m_player1DropDown, m_player2DropDown, m_heur1DropDown, m_heur2DropDown, m_heurConfigDropDown})
        {
            dropdown.captionText.text = dropdown.options[dropdown.value].text;
        }
        
        m_configingHeur = m_heurConfigDropDown.options[m_heurConfigDropDown.value];
        var facList = m_controller.GetHeurAttributeList(m_configingHeur);
        for (int i = 0; i < facList.Count; i++)
        {
            m_heurConfigList[i].text = facList[i].ToString();
            var i1 = i;
            m_heurConfigList[i].onEndEdit.AddListener(delegate{OnConfigFieldChanged(i1);});
        }

    }

    public void OnPlayerChanged(int index)
    {
        Dropdown mod = null;
        switch (index)
        {
            case 1:
                mod = m_player1DropDown;
                break;
            case 2:
                mod = m_player2DropDown;
                break;
            default:
                Debug.LogError("OnPlayerChanged Error!");
                mod = m_player1DropDown;
                break;
        }
        m_controller.ChangePlayer(index, mod.options[mod.value]);
    }
    

    public void OnHeuristicChanged(int index)
    {
        Dropdown mod = null;
        switch (index)
        {
            case 1:
                mod = m_heur1DropDown;
                break;
            case 2:
                mod = m_heur2DropDown;
                break;
            default:
                Debug.LogError("OnHeuristicChanged Error!");
                mod = m_heur1DropDown;
                break;
        }
        m_controller.ChangePlayerHeuristic(index, mod.options[mod.value]);
    }

    public void OnConfigingHeuristicChanged()
    {
        m_configingHeur = m_heurConfigDropDown.options[m_heurConfigDropDown.value];
        RefreshAllConfigField();
    }
    public void OnConfigFieldChanged(int index)
    {
        m_controller.ChangeHeuristicAttribute(m_configingHeur, (MenuController.HeurAttribute) index, m_heurConfigList[index].text);
    }

    private void RefreshAllConfigField()
    {
        var facList = m_controller.GetHeurAttributeList(m_configingHeur);
        for (int i = 0; i < facList.Count; i++)
        {
            m_heurConfigList[i].text = facList[i].ToString();
        }
    }

    public void StartGame()
    {
        m_controller.StartGame();
    }
}
