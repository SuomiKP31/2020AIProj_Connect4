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

    public int min (int a, int b)
    {
        return a < b ? a : b;
    }

    /// <summary>
    /// Return true if the chess at row,column causes terminal in the field, false otherwise.
    /// No need to handle the draw game as a special case.
    /// </summary>
    /// <param name="field"> The field. </param>
    /// <param name="row"> Coord </param>
    /// <param name="column"> Coord </param>
    /// <returns></returns>
    public bool MinimaxAICheckWin(int row,int column)
    {
        int color = field[row, column];
        int count_hor = 0, count_ver = 0, count_dia = 0;
        int border_left = min(column, 3);
        int border_right = min(6 - column, 3);
        int border_down = min(row, 3);
        int border_up = min(5 - row, 3);
        int border_dia_down = min(border_left, border_down);
        int border_dia_up = min(border_right, border_up);
        while(border_left > 0 || border_right > 0)
        {
            if(field[row, column - border_left] == color)
            {
                count_hor++;
                border_left--;
            }
            else
            {
                border_left = 0;
            }
            if(field[row, column + border_right] == color)
            {
                count_hor++;
                border_right--;
            }
            else
            {
                border_right = 0;
            }
        }
        while(border_up > 0 || border_down > 0)
        {
            if(field[row + border_up, column] == color)
            {
                count_ver++;
                border_up--;
            }
            else
            {
                border_up = 0;
            }
            if(field[row - border_down, column] == color)
            {
                count_ver++;
                border_down--;
            }
            else
            {
                border_down = 0;
            }
        }
        while(border_dia_up > 0 || border_dia_down > 0)
        {
            if(field[row + border_dia_up, column + border_dia_up] == color)
            {
                count_dia++;
                border_dia_up--;
            }
            else
            {
                border_dia_up = 0;
            }
            if(field[row - border_dia_down, column - border_dia_down] == color)
            {
                count_dia++;
                border_dia_down--;
            }
            else
            {
                border_dia_down = 0;
            }
        }
        if (count_dia >= 4 || count_hor >= 4 || count_ver >= 4)
        {
            return true;
        }

        return false;
    }
}