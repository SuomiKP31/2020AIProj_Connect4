using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAction : MonoBehaviour
{
    int[,] field = new int [7, 6];
    public static TestAction m_instance;
    public int color = 0;
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
}
