using System;
using System.Collections;
using System.Collections.Generic;
using ConnectFour;
using UnityEngine;

public class OneStepMaximizationAI : BaseAI
{
    // [SerializeField] private BaseHeuristic m_heuristic;
    [SerializeField] private int playerNum;
    private void Start()
    {
        m_gameController = GameController.GetController();
    }
    public override Vector3 GetAction()
    {
        Vector3 spawnPos = new Vector3();
        
        List<int> moves = m_gameController.GetPossibleMoves();
        if (moves.Count != 0)
        {
            int maxValue = Int32.MinValue;
            int bestMove = 0;
            foreach (int move in moves)
            {
                int value = m_heuristic.GetScoreOfBoard(
                    m_gameController.GetFieldAfterMove(m_gameController.GetField(), move, playerNum),playerNum); // Input: Field After Move, player's number
                Debug.Log("The heuristic for move " + move + " is " + value);
                if (value > maxValue)
                {
                    maxValue = value;
                    bestMove = move;
                }
                spawnPos = new Vector3(bestMove, 0, 0);
            }
            
        }
        // Here column should be an int which represents next move made by the AI. It's value should lie in [0,column_number)
        return spawnPos;
    }
}
