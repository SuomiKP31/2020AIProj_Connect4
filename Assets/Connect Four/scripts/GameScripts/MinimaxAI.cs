using System;
using System.Collections;
using System.Collections.Generic;
using ConnectFour;
using UnityEngine;


public class MinimaxAI : BaseAI
{
    // [SerializeField] private BaseHeuristic m_heuristic;
    // Start is called before the first frame update

    private int[,] field;

    public override Vector3 GetAction()
    {
        if (m_gameController == null)
        {
            m_gameController = GameController.GetController();
        }

        field = m_gameController.GetField();
        return new Vector3(IDdfs(7, 1, false, 0, 0), 0, 0);
    }

    /// <summary>
    /// Returns the value of optimal action for player curStepColor.
    /// </summary>
    /// <param name="maxdepth"> The maximum depth for our iddfs.</param>
    /// <param name="curStepColor"> What number is representing the target player, typically 1(Blue) or 2(Red). You only need to specify initial conditions, and this function will automatically handle the switch process.</param>
    /// <param name="is_max"> true if current agent is MAX agent, false otherwise. </param>
    /// <param name="alpha"> Used for alpha-beta proning. Now unused.</param>
    /// <param name="beta"> Used for alpha-beta proning. Now unused.</param>
    /// <returns></returns>
    public int IDdfs(int maxdepth, int curStepColor, bool is_max, int alpha, int beta)
    {
        List<int> moves = m_gameController.GetPossibleMoves();
        int step_cnt = 0;

        if (moves.Count == 0 || maxdepth == 0)
        {
            // Full chess board, draw game || iddfs constraint reached
            return m_heuristic.GetScoreOfBoard(field, curStepColor);
        }

        if (is_max)
        {
            // Max agent
            int ans = Int32.MinValue;
            for (int i = 0; i < moves.Count; i++)
            {
                // Now change the field.
            }
        }
        else
        {
            // Min agent
        }

        return 0;
    }

    public int gcd(int a, int b)
    {
        return b == 0 ? a : gcd(b, a % b);
    }

    public int lcm(int a, int b)
    {
        return a / gcd(a, b) * b;
    }

    /// <summary>
    /// Return true if the chess at row,column causes win in the field, false otherwise.
    /// </summary>
    /// <param name="field"> The field. </param>
    /// <param name="row"> Coord </param>
    /// <param name="column"> Coord </param>
    /// <returns></returns>
    public bool _sh_checkwin(int[,] field,int row,int column)
    {
        return false;
    }
}