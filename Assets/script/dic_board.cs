using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class dic_board :SingletonMonoBehaviour<dic_board>
{
    private char[,] cells = new char[3, 3];
    private const char Empty = ' ';
    private const char PlayerX = 'X';
    private const char PlayerO = 'O';

    public char[,] GetCells() => cells;

    public dic_floor cur_floor;
    public dic_floor[] floor_list;

    void Start()
    {
        for (int i = 0; i < 3; i++)
         for (int j = 0; j < 3; j++)
        {
            cells[i, j] = Empty;
            int id = i *3 + j;
             dic_floor tmp = floor_list[id];
             tmp.SetAs(Empty);
             tmp.id = id;
        }
    }

    public bool IsMoveValid(int row, int col)
    {
        return cells[row, col] == Empty;
    }

    public void ClearAll(){
        for (int i = 0; i < 3; i++)
         for (int j = 0; j < 3; j++)
        {
            cells[i, j] = Empty;
            int id = i *3 + j;
             dic_floor tmp = floor_list[id];
             tmp.SetAs(Empty);
        }
}
    public void MakeMove(int row, int col, char player)
    {
        cells[row, col] = player;
        int id = row *3 + col;
        dic_floor tmp = floor_list[id];
        tmp.SetAs(player);
    }

    //  public void PlayerMove(char player)
    // {
    //     int id = cur_floor.id;
    //     int row = id
    //     cells[row, col] = player;
    // }
    public int get_cur_id(){
        int res = -1;
        if (cur_floor){
            res = cur_floor.id;
        }
        return res;

    }
    public bool CheckWin(char player)
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            if (cells[i, 0] == player && cells[i, 1] == player && cells[i, 2] == player)
                return true;
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (cells[0, i] == player && cells[1, i] == player && cells[2, i] == player)
                return true;
        }

        // Check diagonals
        if (cells[0, 0] == player && cells[1, 1] == player && cells[2, 2] == player)
            return true;
        if (cells[0, 2] == player && cells[1, 1] == player && cells[2, 0] == player)
            return true;

        return false;
    }

    public bool IsBoardFull()
    {
        return !cells.Cast<char>().Any(c => c == Empty);
    }

    public List<(int, int)> GetPossibleMoves()
    {
        var moves = new List<(int, int)>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (cells[i, j] == Empty)
                    moves.Add((i, j));
            }
        }
        return moves;
    }

    // Minimax algorithm implementation
    private int Minimax(int depth, char player, bool isMaximizing)
    {
        if (CheckWin(player)){
            if(depth < 2)
            Debug.Log( player + "wins in "+depth.ToString());
            return 1; // Player wins
        }


        // 不可能出现下了一步对手反而赢了的情况, 必须等对手下
        if (CheckWin(player == PlayerX ? PlayerO : PlayerX))
            return -1; // Opponent wins
        if (IsBoardFull())
        {
            if(depth < 2)
            Debug.Log( player + "draw in "+depth.ToString());
            return 0; // Draw
        }

        // player = player == PlayerX ? PlayerO : PlayerX;
        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            foreach (var move in GetPossibleMoves())
            {
                MakeMove(move.Item1, move.Item2, player);
                int score = Minimax(depth + 1, player, false);
                MakeMove(move.Item1, move.Item2, Empty); // Undo move
            if(depth < 2)
                Debug.Log( move.Item1.ToString() + move.Item2.ToString() + player + score.ToString() + "in "+depth.ToString());

                bestScore = Mathf.Max(score, bestScore);
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            char op_player = player == PlayerX ? PlayerO : PlayerX;
            foreach (var move in GetPossibleMoves())
            {
                MakeMove(move.Item1, move.Item2, op_player);
                int score = Minimax(depth + 1, player, true);
                MakeMove(move.Item1, move.Item2, Empty); // Undo move
            if(depth < 2)
             Debug.Log( move.Item1.ToString() + move.Item2.ToString() + player + score.ToString() + "in "+depth.ToString());
                bestScore = Mathf.Min(score, bestScore);
            }
            return bestScore;
        }
    }

    public (int, int)? GetBestMove(char player)
    {
        int bestScore = int.MinValue;
        (int, int)? bestMove = null;

        foreach (var move in GetPossibleMoves())
        {
            MakeMove(move.Item1, move.Item2, player);
            // int score = Minimax(0, player == PlayerX ? PlayerO : PlayerX, false);
            int score = Minimax(0, player, false);
            MakeMove(move.Item1, move.Item2, Empty); // Undo move
            Debug.Log( move.Item1.ToString() + move.Item2.ToString() + player + score.ToString());
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }
}