using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHeuristic : MonoBehaviour
{
    // Heuristic Class is used in AIs who need to evaluate the board state.
    
    // A block is a line where both players pieces are present. It's considered good in defensive strategy.
    [SerializeField]
    protected int BlockFac = 5; 
    
    // A good line is a line where only your pieces are present. Each your piece will provide a score of GoodFac. A line
    // which occupied all by your pieces grants you a win, so that will give you 10000 score
    [SerializeField]
    protected int GoodFac = 15;
    [SerializeField]
    protected int WinFac = 10000;
    // A dangerous line, in contrast, is a line where only your adversary's pieces are present, so it will provide negative
    // scores. Same as above, a full dangerous line means a lost, so it should give you a -6000 score
    [SerializeField]
    protected int DangerFac = -20;
    [SerializeField]
    protected int LoseFac = -6000;
    
    /// <summary>
    /// Basic Function of any Heuristic functions. Takes the board and player tag as input, output a evaluation as a int
    /// </summary>
    /// <param name="field"> The chessboard you want to evaluate, typically a 7 column x 6 row int 2D array</param>
    /// <param name="playerNum"> What number is representing your player, typically 1(Blue) or 2(Red). (0 is for empty position)</param>
    /// <returns></returns>
    public virtual int GetScoreOfBoard(int[,] field, int playerNum)
    {
        return 0;
    }
}
