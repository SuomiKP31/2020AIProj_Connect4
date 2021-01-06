using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestAction : MonoBehaviour
{
    int[,] field = new int [7, 6];
    public static TestAction m_instance;
    public int color = 0;

    [SerializeField] private BaseHeuristic m_testingHeur;
    private List<int> m_factorList;

    private const int BLOCK = 0;
    private const int GOOD = 1;
    private const int WIN = 2;
    private const int DANGER = 3;
    private const int LOSE = 4;
    
    private void Awake()
    {
        if(m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        m_instance = this;
    }
    public static TestAction GetTestManager()
    {
        return m_instance;
    }
    public int changeValue(int row, int column)
    {
        field[row, column] = color;
        return field[row, column];
    }
    public int min(int a, int b)
    {
        return a < b ? a : b;
    }
    public bool MinimaxAICheckWin(int row, int column)
    {
    
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
            return true;
        }

        return false;
    }
    /// <summary>
    /// Callback function to calculate test board's heuristic score.
    /// Note that the test should pass if and only if the board is physically legit(i.e. no floating pieces)
    /// </summary>
    /// <returns></returns>
    public void TestHeuristic()
    {
        if (!IsPhysicalLegit())
        {
            Debug.LogError("This Board is not physically legit, please adjust.");
        }
        m_factorList = m_testingHeur.GetFactorList(); //BlockFac, GoodFac, WinFac, DangerFac, LoseFac
        List<int> FactorCounters = new List<int> {0,0,0,0,0}; // This count the factors in the order above
        List<int> LineCounters = new List<int> {0,0,0,0,0}; // This count the connect-4 lines in the order of {block,
                                                            // good, win, danger, lose}.
        // Count the horizontal lines
        for (int c = 0; c < 4; c++)
        {
            for (int r = 5; r >= 0; r--)
            {
                int[] pieces = {field[c, r],field[c+1, r],field[c+2, r],field[c+3, r]};
                CountAndEvaluate(pieces, ref FactorCounters, ref LineCounters);
            }
        }
        // Count the vertical lines
        for (int c = 0; c < 7; c++)
        {
            for (int r = 5; r > 2; r--)
            {
                int[] pieces = {field[c, r],field[c, r-1],field[c, r-2],field[c, r-3]};
                CountAndEvaluate(pieces, ref FactorCounters, ref LineCounters);
            }
        }
        // Count diagonal A(upper-left)
        for (int c = 3; c < 7; c++)
        {
            for (int r = 5; r > 2; r--)
            {
                int[] pieces = {field[c, r],field[c-1, r-1],field[c-2, r-2],field[c-3, r-3]};
                CountAndEvaluate(pieces, ref FactorCounters, ref LineCounters);
            }
        }
        // Count diagonal B(upper-right)
        for (int c = 0; c < 4; c++)
        {
            for (int r = 5; r > 2; r--)
            {
                int[] pieces = {field[c, r],field[c+1, r-1],field[c+2, r-2],field[c+3, r-3]};
                CountAndEvaluate(pieces, ref FactorCounters, ref LineCounters);
            }
        }
        
        Debug.Log(convCounterToString(LineCounters));
        var heuScore = m_testingHeur.GetScoreOfBoard(field, color);
        int testScore = 0;
        for (int i = 0; i < 5; i++)
        {
            testScore += FactorCounters[i] * m_factorList[i];
        }

        if (testScore == heuScore)
        {
            Debug.Log("Heuristic Function Implementation Verified.");
        }
        else
        {
            Debug.LogWarning("The heuristic function might be wrong");
        }
    }

    private void CountAndEvaluate(int[] pieces, ref List<int> FactorCounters, ref List<int> LineCounters)
    {
        var ourPiece = 0;
        var advPiece = 0;
        for (var i = 0; i < 4; i++)
        {
            if (pieces[i] == color)
            {
                ourPiece++;
            }
            else if(pieces[i] == 0)
            {
                // Do nothing
            }
            else
            {
                advPiece++;
            }
        }
        // Empty
        if (ourPiece + advPiece == 0)
        {
            return;
        }
        // Win
        if (ourPiece == 4)
        {
            FactorCounters[WIN] += 1;
            LineCounters[WIN] += 1;
            return;
        }
        // Lose
        if(advPiece == 4)
        {
            FactorCounters[LOSE] += 1;
            LineCounters[LOSE] += 1;
            return;
        }
        // Block - Nobody can win from this line
        if (ourPiece != 0 && advPiece != 0)
        {
            FactorCounters[BLOCK] += 1;
            LineCounters[BLOCK] += 1;
            return;
        }
        // Danger or Good
        if (ourPiece != 0)
        {
            FactorCounters[GOOD] += ourPiece;
            LineCounters[GOOD] += 1;
        }

        if (advPiece != 0)
        {
            FactorCounters[DANGER] += advPiece;
            LineCounters[DANGER] += 1;
        }
    }
    /// <summary>
    /// Judge if the current board is physically legit
    /// </summary>
    /// <returns>True if no floating pieces present</returns>
    private bool IsPhysicalLegit()
    {
        for (int c = 0; c < 7; c++)
        {
            for (int r = 5; r > 0; r--)
            {
                if (field[c, r] == 0)
                {
                    if (field[c, r - 1] != 0)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private string convCounterToString(List<int> counter)
    {
        string counterstr = String.Empty;
        for (int i = 0; i < 5; i++)
        {
            counterstr += counter[i].ToString() + " ";
        }

        return counterstr;
    }
}
