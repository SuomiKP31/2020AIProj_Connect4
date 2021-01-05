using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public enum HeurAttribute
    {
        BlockFactor = 0,
        GoodFactor = 1,
        WinFactor = 2,
        DangerFactor = 3,
        LoseFactor = 4
    }
    
    private static MenuController m_instance;
    [SerializeField] public GameObject m_playerHolder;
    [SerializeField] public GameObject m_heuristicHolder;
    public List<GameObject> m_heuristicList;
    public List<GameObject> m_p1List;
    public List<GameObject> m_p2List;

    public GameObject player1;
    public GameObject player2;
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

    private void Start()
    {
        SceneManager.LoadScene("menu");
        StartCoroutine(WaitTillLoadEnd());
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

    public void ChangePlayerSearchDepth(int index, int value)
    {
        switch (index)
        {
            case 1:
                player1.GetComponent<BaseAI>().ChangeSearchDepth(value);
                break;
            case 2:
                player2.GetComponent<BaseAI>().ChangeSearchDepth(value);
                break;
        }
    }

    public void ChangeHeuristicAttribute(Dropdown.OptionData data, HeurAttribute attribute, string value)
    {
        var heur = m_heuristicHolder.transform.Find(data.text).gameObject.GetComponent<BaseHeuristic>();
        int param = Convert.ToInt32(value);
        switch (attribute)
        {
            case HeurAttribute.BlockFactor:
                heur.ChangeBlockFac(param);
                break;
            case HeurAttribute.GoodFactor:
                heur.ChangeGoodFac(param);
                break;
            case HeurAttribute.WinFactor:
                heur.ChangeWinFac(param);
                break;
            case HeurAttribute.DangerFactor:
                heur.ChangeDangerFac(param);
                break;
            case HeurAttribute.LoseFactor:
                heur.ChangeLoseFac(param);
                break;
            default:
                Debug.LogError("Attribute Config Error");
                break;
        }
    }

    public List<int> GetHeurAttributeList(Dropdown.OptionData data)
    {
        var heur = m_heuristicHolder.transform.Find(data.text).gameObject.GetComponent<BaseHeuristic>();
        return heur.GetFactorList();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("game");
        // StartCoroutine(WaitTillLoadEnd());
    }

    private IEnumerator WaitTillLoadEnd()
    {
        while (SceneManager.GetActiveScene().buildIndex != 1)
        {
            yield return null;
        }
        
    }
}
