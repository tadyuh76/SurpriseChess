using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SurpriseChess;

internal class ChessController
{
    private int cursorX = 0, cursorY = 7;
    private readonly ChessModel model;
    private readonly ChessView view;

    public static int squareWidth = 4;
    public ChessController(ChessModel model, ChessView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Run()
    {
        model.NewGame(GameMode.PlayerVsPlayer);
        while(true)
        {
            view.DrawBoard(model.Board, model.SelectedPosition, model.HighlightedMoves, cursorX, cursorY);
            ListenKeyStroke();
        }
        
    }

    public void ListenKeyStroke()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();

        if (keyInfo.Key == ConsoleKey.LeftArrow && cursorX > 0)
            cursorX -= 1;
        else if (keyInfo.Key == ConsoleKey.RightArrow && cursorX < 7)
            cursorX += 1;
        else if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            if (cursorY > 0) cursorY--;
        }
        else if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            if (cursorY < 7) cursorY++;
        }
        else if (keyInfo.Key == ConsoleKey.Enter)
            HandleBoardClick(new Position(cursorY, cursorX));
        //else if (keyInfo.Key == ConsoleKey.D)
        //    debugInteract();
        //else if (keyInfo.Key == ConsoleKey.Escape)
        //    cancel();
    }

    public void HandleBoardClick(Position clickedSquare)
    {
        // If the player clicks on one of the highlighted moves, move there
        if (model.HighlightedMoves.Contains(clickedSquare))
        {
            model.HandleMoveTo(clickedSquare);
            return;
        }

        // If the player clicks on one of their pieces, select it
        Piece? clickedPiece = model.Board.GetPieceAt(clickedSquare);
        if (clickedPiece?.Color == model.GameState.CurrentPlayerColor)
        {
            model.Select(clickedSquare);
        }
        else  // Deselect whatever's selected
        {
            model.Deselect();
        }
    }
}
