using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private static MenuController m_instance;
    [SerializeField] private GameObject m_playerHolder;
    [SerializeField] private GameObject m_heuristicHolder;
    public List<GameObject> m_heuristicList;
    public List<GameObject> m_p1List;
    public List<GameObject> m_p2List;

    private GameObject player1;
    private GameObject player2;
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (m_instance != null && m_instance != this) 
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(m_playerHolder);
        DontDestroyOnLoad(m_heuristicHolder);
        m_instance = this;
    }

    public static MenuController GetMenuController()
    {
        return m_instance;
    }

    public void ChangePlayer(int index, Dropdown.OptionData data)
    {
        Debug.Log(data.text);
        switch (index)
        {
            case 1:
                player1 = m_playerHolder.transform.Find(data.text).gameObject;
                break;
            case 2:
                player2 = m_playerHolder.transform.Find(data.text).gameObject;
                break;
        }
    }
    public void ChangePlayerHeuristic(int index, Dropdown.OptionData data)
    {
        switch (index)
        {
            case 1:
                player1.GetComponent<BaseAI>().ChangeHeuristic(m_heuristicHolder.transform.Find(data.text).gameObject.GetComponent<BaseHeuristic>());
                break;
            case 2:
                player2.GetComponent<BaseAI>().ChangeHeuristic(m_heuristicHolder.transform.Find(data.text).gameObject.GetComponent<BaseHeuristic>());
                break;
        }
    }
}
