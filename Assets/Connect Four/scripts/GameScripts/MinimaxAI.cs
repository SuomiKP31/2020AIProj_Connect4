﻿using System;
using System.Collections;
using System.Collections.Generic;
using ConnectFour;
using UnityEngine;


public class MinimaxAI : BaseAI
{
    // [SerializeField] private BaseHeuristic m_heuristic;
    // Start is called before the first frame update

    private int[,] field;
    [SerializeField] private int playerNum;
    private int BestAction = 0;
    [SerializeField] private bool EnableProne = false;

    public override Vector3 GetAction()
    {
        if (m_gameController == null)
        {
            m_gameController = GameController.GetController();
        }

        field = m_gameController.GetField().Clone() as int[,];
        int alpha = Int32.MinValue, beta = Int32.MaxValue;
        IDdfs(4, playerNum, true, alpha, beta);

        return new Vector3(BestAction, 0, 0);
    }

    /// <summary>
    /// Returns the value of optimal score for player curStepColor.
    /// </summary>
    /// <param name="maxdepth"> The maximum depth for our iddfs.</param>
    /// <param name="curStepColor"> What number is representing the target player, typically 1(Blue) or 2(Red). You only need to specify initial conditions, and this function will automatically handle the switch process.</param>
    /// <param name="is_max"> true if current agent is MAX agent, false otherwise. </param>
    /// <param name="alpha"> Used for alpha-beta proning. Now unused.</param>
    /// <param name="beta"> Used for alpha-beta proning. Now unused.</param>
    /// <returns></returns>
    public int IDdfs(int maxdepth, int curStepColor, bool is_max, int alpha, int beta)
    {
        BestAction = 0;
        List<int> moves = m_gameController.GetPossibleMoves(field);
        int step_cnt = 0, ans = 0, tmp, ta = alpha, tb = beta;

        if (moves.Count == 0 || maxdepth == 0)
        {
            // Full chess board, draw game || iddfs constraint reached
            return m_heuristic.GetScoreOfBoard(field, curStepColor);
        }

        if (is_max)
        {
            // Max agent
            ans = Int32.MinValue;
            for (int i = 0; i < moves.Count; i++)
            {
                // Now change the field.
                for (int j = 0; j < m_gameController.numRows; j++)
                {
                    if (field[moves[i], j] != 0)
                    {
                        field[moves[i], j - 1] = curStepColor;
                        if (MinimaxAICheckWin(j - 1, moves[i]))
                        {
                            // Terminal state.
                            Debug.Log("Returning from terminal state");
                            tmp = m_heuristic.GetScoreOfBoard(field, curStepColor);
                        }
                        else
                        {
                            // We need to go deeper.
                            //Debug.Log("Calling IDdfs");
                            tmp = IDdfs(maxdepth - 1, curStepColor == 1 ? 2 : 1, !is_max, alpha, beta);
                        }

                        // Now change back
                        field[moves[i], j - 1] = 0;

                        if (tmp > ans)
                        {
                            // Do not use Math.Max here since we need to add Alpha-Beta later.
                            BestAction = moves[i];
                            ans = tmp;
                        }

                        if (ans >= beta && EnableProne)
                        {
                            Debug.Log("Now Prone in Max agent with beta " + beta.ToString());
                            return ans;
                        }

                        alpha = Math.Max(alpha, ans);
                    }

                    // Default behavior on error is do nothing and return.
                }
            }
        }
        else
        {
            // Min agent
            ans = Int32.MaxValue;
            for (int i = 0; i < moves.Count; i++)
            {
                // Now change the field.
                for (int j = 0; j < m_gameController.numRows; j++)
                {
                    if (field[moves[i], j] != 0)
                    {
                        field[moves[i], j - 1] = curStepColor;
                        if (MinimaxAICheckWin(j - 1, moves[i]))
                        {
                            // Terminal state.
                            Debug.Log("Returning from terminal state");
                            tmp = m_heuristic.GetScoreOfBoard(field, curStepColor);
                        }
                        else
                        {
                            // We need to go deeper.
                            //Debug.Log("Calling IDdfs");
                            tmp = IDdfs(maxdepth - 1, curStepColor == 1 ? 2 : 1, !is_max, alpha, beta);
                        }

                        // Now change back
                        field[moves[i], j - 1] = 0;

                        if (tmp < ans)
                        {
                            // Do not use Math.Max here since we need to add Alpha-Beta later.
                            BestAction = moves[i];
                            ans = tmp;
                        }

                        if (ans <= alpha && EnableProne)
                        {
                            return ans;
                        }

                        beta = Math.Min(beta, ans);
                    }

                    // Default behavior on error is do nothing and return.
                }
            }
        }

        return ans;
    }

    public int gcd(int a, int b)
    {
        return b == 0 ? a : gcd(b, a % b);
    }

    public int lcm(int a, int b)
    {
        return a / gcd(a, b) * b;
    }

    public int min(int a, int b)
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
    public bool MinimaxAICheckWin(int row, int column)
    {
        int color = field[column, row];
        int count_hor = 0, count_ver = 0, count_dia_1 = 0, count_dia_2 = 0;
        int border_left = min(column, 3);
        int border_right = min(6 - column, 3);
        int border_down = min(5-row, 3);
        int border_up = min(row, 3);
        int border_left_down = min(border_left, border_down);
        int border_right_up = min(border_right, border_up);
        int border_right_down = min(border_right, border_down);
        int border_left_up = min(border_left, border_up);
        while (border_left > 0)
        {
            if (field[column - border_left, row] == color)
            {
                count_hor++;
                border_left--;
                Debug.Log("Find one border left");
            }
            else
            {
                border_left = 0;
                Debug.Log("One different border left");
            }
        }

        while (border_right > 0)
        {
            if (field[column + border_right, row] == color)
            {
                count_hor++;
                border_right--;
                Debug.Log("Find one border right");
            }
            else
            {
                border_right = 0;
                Debug.Log("One different border right");
            }
        }

        while (border_up > 0)
        {
            if (field[column, row - border_up] == color)
            {
                count_ver++;
                border_up--;
                Debug.Log("Find one border up");
            }
            else
            {
                border_up = 0;
                Debug.Log("One different border up");
            }
        }

        while (border_down > 0)
        {
            if (field[column, row + border_down] == color)
            {
                count_ver++;
                border_down--;
                Debug.Log("Find one border down");
            }
            else
            {
                border_down = 0;
                Debug.Log("One different border down");
            }
        }

        while (border_right_up > 0)
        {
            if (field[column + border_right_up, row - border_right_up] == color)
            {
                count_dia_1++;
                border_right_up--;
            }
            else
            {
                border_right_up = 0;
            }
        }

        while (border_left_down > 0)
        {
            if (field[column - border_left_down, row + border_left_down] == color)
            {
                count_dia_1++;
                border_left_down--;
            }
            else
            {
                border_left_down = 0;
            }
        }

        while (border_right_down > 0)
        {
            if (field[column + border_right_down, row + border_right_down] == color)
            {
                count_dia_2++;
                border_right_down--;
            }
            else
            {
                border_right_down = 0;
            }
        }

        while (border_left_up > 0)
        {
            if (field[column - border_left_up, row - border_left_up] == color)
            {
                count_dia_2++;
                border_left_up--;
            }
            else
            {
                border_left_up = 0;
            }
        }

        if (count_dia_1 >= 4 || count_dia_2 >= 4 || count_hor >= 4 || count_ver >= 4)
        {
            Debug.Log("Found a win");
            return true;
        }

        return false;
    }
}