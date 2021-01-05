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
    [SerializeField] private int playerNum;
    private int BestAction = 0;
    [SerializeField] private bool enableProne = false;
    

    public override Vector3 GetAction()
    {
        if (m_gameController == null)
        {
            m_gameController = GameController.GetController();
        }

        field = m_gameController.GetField().Clone() as int[,];
        int alpha = Int32.MinValue, beta = Int32.MaxValue;
        // IDdfs(searchDepth, playerNum, true, alpha, beta);
        (int, int) MinimaxResult = MinimaxValue(m_gameController.GetField().Clone() as int[,], 1, playerNum, alpha,
            beta, (-1, -1));

        return new Vector3(MinimaxResult.Item2, 0, 0);
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
        List<int> moves = m_gameController.GetPossibleMoves(field);
        // Debug.Log("sizeof moves is " + moves.Count);
        int step_cnt = 0, ans = 0, tmp;
        if (maxdepth == searchDepth)
        {
            BestAction = moves[0];
        }

        if (moves.Count == 0 || maxdepth == 0)
        {
            // Full chess board, draw game || iddfs constraint reached
            return m_heuristic.GetScoreOfBoard(field, playerNum);
        }

        if (is_max)
        {
            // Max agent
            ans = Int32.MinValue;
            for (int i = 0; i < moves.Count; i++)
            {
                // Now change the field.
                for (int j = m_gameController.numRows - 1; j >= 0; j--)
                {
                    if (field[moves[i], j] == 0)
                    {
                        field[moves[i], j] = curStepColor;
                        //DumpField();
                        if (MinimaxAICheckWin(j, moves[i]))
                        {
                            // Terminal state.
                            //Debug.Log("Returning from terminal state");
                            tmp = m_heuristic.GetScoreOfBoard(field, playerNum);
                        }
                        else
                        {
                            // We need to go deeper.
                            //Debug.Log("Calling IDdfs");
                            tmp = IDdfs(maxdepth - 1, curStepColor == 1 ? 2 : 1, !is_max, alpha, beta);
                        }

                        // Now change back
                        field[moves[i], j] = 0;


                        if (tmp > ans)
                        {
                            // Do not use Math.Max here since we need to add Alpha-Beta later.
                            if (maxdepth == searchDepth)
                            {
                                Debug.Log("Modified bestAction, from " + BestAction + " to " + moves[i]);
                                BestAction = moves[i];
                            }

                            ans = tmp;
                        }

                        if (maxdepth == searchDepth)
                        {
                            Debug.Log("For action " + moves[i] + ", ans = " + ans + ", tmp = " + tmp);
                            //Debug.Log("Beta = " + beta.ToString());
                            //Debug.Log("Alpha = " + alpha);
                            Debug.Log("bestAction = " + BestAction);
                        }

                        if (ans >= beta && enableProne)
                        {
                            //Debug.Log("Now Prone in Max agent with beta " + beta.ToString());
                            return ans;
                        }

                        alpha = Math.Max(alpha, ans);
                        break;
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
                for (int j = m_gameController.numRows - 1; j >= 0; j--)
                {
                    if (field[moves[i], j] == 0)
                    {
                        field[moves[i], j] = curStepColor;
                        if (MinimaxAICheckWin(j, moves[i]))
                        {
                            // Terminal state.
                            //Debug.Log("Returning from terminal state");
                            tmp = m_heuristic.GetScoreOfBoard(field, playerNum);
                        }
                        else
                        {
                            // We need to go deeper.
                            //Debug.Log("Calling IDdfs");
                            tmp = IDdfs(maxdepth - 1, curStepColor == 1 ? 2 : 1, !is_max, alpha, beta);
                        }

                        // Now change back
                        field[moves[i], j] = 0;

                        if (tmp < ans)
                        {
                            // Do not use Math.Max here since we need to add Alpha-Beta later.
                            ans = tmp;
                        }

                        if (ans <= alpha && enableProne)
                        {
                            //Debug.Log("Now Prone in Min agent with alpha " + alpha.ToString());
                            return ans;
                        }

                        beta = Math.Min(beta, ans);
                        break;
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

    public void DumpField()
    {
        String ans = "";
        for (int r = 0; r < m_gameController.numRows; r++)
        {
            for (int c = 0; c < m_gameController.numColumns; c++)
            {
                ans = ans + field[c, r] + " ";
            }

            ans = ans + "\n";
        }

        Debug.Log(ans);
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
        //Debug.Log("Color" + color);
        //Debug.Log("H(x)=" + m_heuristic.GetScoreOfBoard(field, color == 1 ? 2 : 1));
        //DumpField();
        m_heuristic.GetScoreOfBoard(field, color == 1 ? 2 : 1);
        if (m_heuristic.GetLastError() > 1)
        {
            Debug.Log("[win] Return from heuristic");
            //return true;
        }

        int count_hor = 0, count_ver = 0, count_dia_1 = 0, count_dia_2 = 0;
        int border_left = min(column, 3);
        int border_right = min(6 - column, 3);
        int border_down = min(5 - row, 3);
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
            }
            else
            {
                break;
            }
        }

        while (border_right > 0)
        {
            if (field[column + border_right, row] == color)
            {
                count_hor++;
                border_right--;
            }
            else
            {
                break;
            }
        }

        while (border_up > 0)
        {
            if (field[column, row - border_up] == color)
            {
                count_ver++;
                border_up--;
            }
            else
            {
                break;
            }
        }

        while (border_down > 0)
        {
            if (field[column, row + border_down] == color)
            {
                count_ver++;
                border_down--;
            }
            else
            {
                break;
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
                break;
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
                break;
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
                break;
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
                break;
            }
        }

        //Debug.Log(count_dia_1 + "," + count_dia_2 + "," + count_hor + "," + count_ver);

        if (count_dia_1 >= 3 || count_dia_2 >= 3 || count_hor >= 3 || count_ver >= 3)
        {
            Debug.Log("Found a win");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Return (utility, action_causing_this_utility), the second parameter may be -1, which means the action is missing.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="currentDepth">Start from 1, please. Equals to how many MinimaxValue instances exists currently in this thread.</param>
    /// <param name="currentColor"></param>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <param name="lastAction">The action that results in STATE. Use (-1,-1) as default parameter.</param>
    /// <returns></returns>
    private (int, int) MinimaxValue(int[,] state, int currentDepth, int currentColor, int alpha, int beta,
        (int, int) lastAction)
    {
        (int, int) ans;
        ans.Item2 = -1;
        if (currentDepth > searchDepth)
        {
            // Terminal state: max search depth reached
            // No action is performed, so the action should be -1.
            ans.Item1 = m_heuristic.GetScoreOfBoard(state, playerNum);
            return ans;
        }

        if (!lastAction.Equals((-1, -1)))
        {
            // Terminal state: Already win/lose.
            // Note that MinimaxAICheckWin uses different parameter order.
            field = state;
            if (MinimaxAICheckWin(lastAction.Item2, lastAction.Item1))
            {
                ans.Item1 = m_heuristic.GetScoreOfBoard(state, playerNum);
                return ans;
            }
        }

        if (currentDepth % 2 == 1)
        {
            // Max agent.
            return MinimaxMaxValue(state, currentDepth, currentColor, alpha, beta);
        }

        return MinimaxMinValue(state, currentDepth, currentColor, alpha, beta);
    }

    private (int, int) MinimaxMaxValue(int[,] state, int currentDepth, int currentColor, int alpha, int beta)
    {
        List<(int, int)> actions = m_gameController.GetPossibleDetailedMoves(state);
        (int, int) v = (Int32.MinValue, -1);
        if (actions.Count > 0)
        {
            // action.Item1 is the column, and it is our action.
            v.Item2 = actions[0].Item1;
        }

        foreach (var action in actions)
        {
            // Modify state.
            int[,] stateCopy = state.Clone() as int[,];
            stateCopy[action.Item1, action.Item2] = currentColor;
            (int, int) tmpValue =
                MinimaxValue(stateCopy, currentDepth + 1, currentColor == 1 ? 2 : 1, alpha, beta, action);
            if (currentDepth == 1)
            {
                Debug.Log("Action " + action + ": minimax value = " + tmpValue);
            }

            if (v.Item1 < tmpValue.Item1)
            {
                // action.Item1 is the column, and it is our action.
                v.Item2 = action.Item1;
                v.Item1 = tmpValue.Item1;
                if (currentDepth == 1)
                {
                    Debug.Log("Updated v: " + v);
                }
            }

            if (enableProne && v.Item1 >= beta)
            {
                // Prone!
                return v;
            }

            // Update alpha
            alpha = Math.Max(alpha, v.Item1);
        }

        return v;
    }

    private (int, int) MinimaxMinValue(int[,] state, int currentDepth, int currentColor, int alpha, int beta)
    {
        List<(int, int)> actions = m_gameController.GetPossibleDetailedMoves(state);
        (int, int) v = (Int32.MaxValue, -1);
        foreach (var action in actions)
        {
            int[,] stateCopy = state.Clone() as int[,];
            stateCopy[action.Item1, action.Item2] = currentColor;
            (int, int) tmpValue =
                MinimaxValue(stateCopy, currentDepth + 1, currentColor == 1 ? 2 : 1, alpha, beta, action);
            if (v.Item1 > tmpValue.Item1)
            {
                v.Item2 = action.Item1;
                v.Item1 = tmpValue.Item1;
            }

            if (enableProne && v.Item1 <= alpha)
            {
                // Prone!
                return v;
            }

            beta = Math.Min(beta, v.Item1);
        }

        return v;
    }
}