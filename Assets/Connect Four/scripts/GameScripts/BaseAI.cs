using System.Collections;
using System.Collections.Generic;
using ConnectFour;
using UnityEngine;

// A random AI, used as base class for all AIs, including human-controlled ones.
public class BaseAI : MonoBehaviour
{
    // The controller Class. You can get the game field and possible moves from it.
    // private GameController m_controller; // Now I made it a singleton so we can access it anytime we want
    
    // Human or not? If it's human, the controller wait until the player makes an action, and GetAction() will not be called at any time.
    public bool isHuman = false;
    protected GameController m_gameController;
    
    [SerializeField] protected BaseHeuristic m_heuristic;
    private void Start()
    {
        m_gameController = GameController.GetController();
    }
    public virtual Vector3 GetAction()
    {
        Vector3 spawnPos = new Vector3();
        
        List<int> moves = m_gameController.GetPossibleMoves();
        if (moves.Count != 0)
        {
            int column = moves[Random.Range (0, moves.Count)];
            Debug.Log("Random AI chooses column" + column);
            spawnPos = new Vector3(column, 0, 0);
        }
        // Here column should be an int which represents next move made by the AI. It's value should lie in [0,column_number)
        return spawnPos;
    }

    public void ChangeHeuristic(BaseHeuristic heur)
    {
        m_heuristic = heur;
    }
}
