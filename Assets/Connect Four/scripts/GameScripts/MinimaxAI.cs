using System;
using System.Collections;
using System.Collections.Generic;
using ConnectFour;
using UnityEngine;


public class MinimaxAI : BaseAI
{
    // [SerializeField] private BaseHeuristic m_heuristic;
    // Start is called before the first frame update

    public override Vector3 GetAction()
    {
        if (m_gameController == null)
        {
            m_gameController = GameController.GetController();
        }
        return new Vector3(IDdfs(7, 1, 0, 0), 0, 0);
    }
    /// <summary>
    /// Basic Function of any Heuristic functions. Takes the board and player tag as input, output a evaluation as a int
    /// </summary>
    /// <param name="maxdepth"> The maximum depth for our iddfs.</param>
    /// <param name="cur_step_color"> What number is representing the target player, typically 1(Blue) or 2(Red). You only need to specify initial conditions, and this function will automatically handle the switch process.</param>
    /// <param name="alpha"> Used for alpha-beta proning. Now unused.</param>
    /// <param name="beta"> Used for alpha-beta proning. Now unused.</param>
    /// <returns></returns>
    private int IDdfs(int maxdepth,int cur_step_color,int alpha,int beta)
    {
        // guestsh1: The reason we need two parameters (depth, maxdepth) 
        // instead of 1 (decreasing depth) is that, for minimax agent, we
        // need to know which layer is min layer, and, if we only have
        // one parameter, it is hard to do this.
        List<int> moves = m_gameController.GetPossibleMoves();
        int step_cnt = 0;

        if (moves.Count == 0 || maxdepth==0)
        {
            // Full chess board, draw game || iddfs constraint reached
            return m_heuristic.GetScoreOfBoard(m_gameController.GetField(), cur_step_color);
        }

        return 0;
    }
}
