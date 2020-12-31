using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Dropdown m_player1DropDown;
    [SerializeField] private Dropdown m_player2DropDown;
    [SerializeField] private Dropdown m_heur1DropDown;
    [SerializeField] private Dropdown m_heur2DropDown;
    [SerializeField] private Dropdown m_heurConfigDropDown;

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
        m_player1DropDown.value = 0;
        m_player2DropDown.value = 0;
        m_heur1DropDown.value = 0;
        m_heur2DropDown.value = 0;
        m_heurConfigDropDown.value = 0;
        m_controller.ChangePlayer(1,m_player1DropDown.options[m_player1DropDown.value]);
        m_controller.ChangePlayer(2,m_player2DropDown.options[m_player2DropDown.value]);
        m_controller.ChangePlayerHeuristic(1, m_heur1DropDown.options[m_heur1DropDown.value]);
        m_controller.ChangePlayerHeuristic(2, m_heur2DropDown.options[m_heur2DropDown.value]);
        m_configingHeur = m_heurConfigDropDown.options[m_heurConfigDropDown.value];
    }

    public void OnPlayerChanged(int value)
    {
        Dropdown mod = null;
        switch (value)
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
        m_controller.ChangePlayer(value, mod.options[mod.value]);
    }
    

    public void OnHeuristicChanged(int value)
    {
        Dropdown mod = null;
        switch (value)
        {
            case 1:
                mod = m_heur1DropDown;
                break;
            case 2:
                mod = m_heur2DropDown;
                break;
            default:
                Debug.LogError("OnPlayerChanged Error!");
                mod = m_heur1DropDown;
                break;
        }
        m_controller.ChangePlayerHeuristic(value, mod.options[mod.value]);
    }
    
}
