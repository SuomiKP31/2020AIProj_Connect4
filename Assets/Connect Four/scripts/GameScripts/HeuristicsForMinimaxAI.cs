using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicsForMinimaxAI : BaseHeuristic
{
    // private int column = 7;
    // private int row = 6;
    private int[,] m_field = null;
    private int m_playerNum = 0;
    private int lastError = 0;

    
    public override int GetLastError()
    {
        return lastError;
    }

    public override int GetScoreOfBoard(int[,] field, int playerNum)
    {
        lastError = 0;
        // This trivial evaluation function will assume the field is 7x6, and line length = 4
        m_field = field;
        m_playerNum = playerNum;
        int res = 0;
        //Evaluation of Horizontal Lines
        for (int c = 0; c < 4; c++)
        {
            for (int r = 5; r >= 0; r--)
            {
                int value = EvaluateHorizontal(c, r);
                if (value == -1)
                {
                    break;
                }

                res += value;
            }
        }

        //Evaluation of Vertical Lines
        for (int c = 0; c < 7; c++)
        {
            for (int r = 5; r > 2; r--)
            {
                int value = EvaluateVertical(c, r);
                if (value == -1)
                {
                    break;
                }

                res += value;
            }
        }

        //Evaluation of Diagonal Lines A (Point to upper-left)
        for (int c = 3; c < 7; c++)
        {
            for (int r = 5; r > 2; r--)
            {
                int value = EvaluateDiagonalA(c, r);
                if (value == -1)
                {
                    break;
                }

                res += value;
            }
        }

        //Evaluation of Diagonal Lines B (Point to upper-right)
        for (int c = 0; c < 4; c++)
        {
            for (int r = 5; r > 2; r--)
            {
                int value = EvaluateDiagonalB(c, r);
                if (value == -1)
                {
                    break;
                }

                res += value;
            }
        }

        if (lastError != 2 && lastError != 3)
        {
            lastError = 1;
        }

        return res;
    }

    /// <summary>
    /// Given column/row tag, evaluate a horizontal line's factor
    /// </summary>
    /// <param name="c"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    private int EvaluateHorizontal(int c, int r)
    {
        int ourPieces = 0;
        int advPieces = 0;
        for (int i = c; i < c + 4; i++)
        {
            int pieceNum = m_field[i, r];
            if (pieceNum == m_playerNum)
            {
                ourPieces++;
            }
            else if (pieceNum != 0)
            {
                advPieces++;
            }
        }

        if (ourPieces != 0)
        {
            if (ourPieces == 4)
            {
                lastError = 2;
                return WinFac;
            }

            if (advPieces != 0)
            {
                return BlockFac;
            }

            return GoodFac * ourPieces;
        }
        else
        {
            if (advPieces == 4)
            {
                lastError = 3;
                return LoseFac;
            }

            if (advPieces == 0)
            {
                return -1; // Found complete empty row, need to do optimization
            }

            return DangerFac * advPieces;
        }
    }

    private int EvaluateVertical(int c, int r)
    {
        int ourPieces = 0;
        int advPieces = 0;
        for (int i = r; i > r - 4; i--)
        {
            int pieceNum = m_field[c, i];
            if (pieceNum == m_playerNum)
            {
                ourPieces++;
            }
            else if (pieceNum != 0)
            {
                advPieces++;
            }
        }

        if (ourPieces != 0)
        {
            if (ourPieces == 4)
            {
                lastError = 2;
                return WinFac;
            }

            if (advPieces != 0)
            {
                return BlockFac;
            }

            return GoodFac * ourPieces;
        }
        else
        {
            if (advPieces == 4)
            {
                lastError = 3;
                return LoseFac;
            }

            if (advPieces == 0)
            {
                return -1; // Found complete empty row, need to do optimization
            }

            return DangerFac * advPieces;
        }
    }

    private int EvaluateDiagonalA(int c, int r)
    {
        int ourPieces = 0;
        int advPieces = 0;
        for (int i = 0; i < 4; i++)
        {
            int pieceNum = m_field[c - i, r - i];
            if (pieceNum == m_playerNum)
            {
                ourPieces++;
            }
            else if (pieceNum != 0)
            {
                advPieces++;
            }
        }

        if (ourPieces != 0)
        {
            if (ourPieces == 4)
            {
                lastError = 2;
                return WinFac;
            }

            if (advPieces != 0)
            {
                return BlockFac;
            }

            return GoodFac * ourPieces;
        }
        else
        {
            if (advPieces == 4)
            {
                lastError = 3;
                return LoseFac;
            }

            if (advPieces == 0)
            {
                return -1; // Found complete empty row, need to do optimization
            }

            return DangerFac * advPieces;
        }
    }

    private int EvaluateDiagonalB(int c, int r)
    {
        int ourPieces = 0;
        int advPieces = 0;
        for (int i = 0; i < 4; i++)
        {
            int pieceNum = m_field[c + i, r - i];
            if (pieceNum == m_playerNum)
            {
                ourPieces++;
            }
            else if (pieceNum != 0)
            {
                advPieces++;
            }
        }

        if (ourPieces != 0)
        {
            if (ourPieces == 4)
            {
                lastError = 2;
                return WinFac;
            }

            if (advPieces != 0)
            {
                return BlockFac;
            }

            return GoodFac * ourPieces;
        }
        else
        {
            if (advPieces == 4)
            {
                lastError = 3;
                return LoseFac;
            }

            if (advPieces == 0)
            {
                return -1; // Found complete empty row, need to do optimization
            }

            return DangerFac * advPieces;
        }
    }
}