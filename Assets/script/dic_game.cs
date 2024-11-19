using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class dic_game : SingletonMonoBehaviour<dic_game>
{
    public dic_board board;
    // public Button[] buttons; // Assuming you have 9 buttons for the board
    // public TextMeshPro statusText;
    public enum GameState { None, OnUI, OnMove, OnAI, OnPlay }
    public GameState CurrentGameState = GameState.OnUI;

    // public piece cur_piece = null;

    public GameObject statusText;

    public char First_piece = 'X';
    public GameObject First_piece_text;
    public GameState First_player = GameState.OnMove;
    public GameObject First_player_text;

    private char currentPlayer = 'X';
    void Start()
    {
        // board = new dic_board();
        // Initialize buttons and their event listeners here
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space) && currentPlayer == 'O') // Press Space to make AI move
    //     {
    //         MakeAIMove();
    //     }
    // }

    public void MakePlayerMove(int index)
    {
        int row = index / 3;
        int col = index % 3;

        if (board.IsMoveValid(row, col))
        {
            board.MakeMove(row, col, currentPlayer);

            // Update UI here

            if (board.CheckWin(currentPlayer))
            {
                // statusText.text = "Player " + currentPlayer + " wins!";
                set_text("Player wins!");
                CurrentGameState = GameState.OnUI;

                // Disable buttons or end game logic here
            }
            else if (board.IsBoardFull())
            {
                // statusText.text = "It's a draw!";
                set_text("It's a draw!");
                CurrentGameState = GameState.OnUI;

                // Disable buttons or end game logic here
            }
            else
            {
                set_text("AI turn!");
                currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
                CurrentGameState = GameState.OnAI;

                // Enable/Disable AI move logic here if needed
            }
        }
    }

    public void MakeAIMove()
    {
        var bestMove = board.GetBestMove(currentPlayer);
        if (bestMove.HasValue)
        {
            int row = bestMove.Value.Item1;
            int col = bestMove.Value.Item2;

            board.MakeMove(row, col, currentPlayer);

            // Update UI here

            if (board.CheckWin(currentPlayer))
            {
                // statusText.text = "AI wins!";
                set_text("AI wins!");
                CurrentGameState = GameState.OnUI;
                // Disable buttons or end game logic here
            }
            else if (board.IsBoardFull())
            {
                // statusText.text = "It's a draw!";
                set_text("It's a draw!");
                CurrentGameState = GameState.OnUI;
                // Disable buttons or end game logic here
            }
            else
            {
                set_text("Your turn!");
                CurrentGameState = GameState.OnMove;
                currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
            }
        }
    }

    public void set_text(string text)
    {
        statusText.GetComponent<TMP_Text>().text = text;
    }


    // IEnumerator shot_func()
    // {
    //     // CurrentGameState = GameState.None;
    //     // trigger_shatter();
    //     // set_states(GameState.OnPlay);
    //     // out_states(GameState.OnMove);
    //     cur_piece.set_func();
    //     CurrentGameState = GameState.OnPlay;
    //     Sound_Man.Instance.PlayOther();
    //     // cur_piece = null;
    //     yield return new WaitForSeconds(0.1f); // 等待指定时间  

    //     // bool is_win = board.CheckWin()
    //     // bool is_draw = false;
    //     // if( !is_win){
    //     //     is_draw = board.IsBoardFull()
    //     // }

    // }

    // Update is called once per frame
    void Update()
    {

        if (CurrentGameState == GameState.OnMove)
        {


            if (Input.GetMouseButtonDown(0))
            {
                if (board.get_cur_id() >= 0)
                {
                    MakePlayerMove(board.get_cur_id());
                }
                else
                {
                    Debug.Log("can't build");
                    // check_mouse_item();
                }
            }

        }
        if (CurrentGameState == GameState.OnAI)
        {
            MakeAIMove();

        }
    }

    public void StartGame()
    {
        board.ClearAll();
        currentPlayer = First_piece;
        CurrentGameState = First_player;
        if (CurrentGameState == GameState.OnMove)
            set_text("Your turn!");

    }
        
    public void toggle_first_piece()
    {
        First_piece =  First_piece == 'O' ? 'X' : 'O';
        First_piece_text.GetComponent<TMP_Text>().text = First_piece.ToString();
    }
    public void toggle_first_player()
    {
        if (First_player == GameState.OnAI)
        {
            First_player = GameState.OnMove;
            First_player_text.GetComponent<TMP_Text>().text = "Player";
        }
        else
        {
            First_player = GameState.OnAI;
            First_player_text.GetComponent<TMP_Text>().text = "AI";

        }
    }
}
