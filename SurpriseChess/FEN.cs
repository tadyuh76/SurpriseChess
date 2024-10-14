using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpriseChess;

public static class FEN
{
    private static readonly Dictionary<(PieceColor, PieceType), string> PieceChar = new()
    {
        [(PieceColor.White, PieceType.Pawn)] = "P",
        [(PieceColor.White, PieceType.Knight)] = "N",
        [(PieceColor.White, PieceType.Bishop)] = "B",
        [(PieceColor.White, PieceType.Rook)] = "R",
        [(PieceColor.White, PieceType.Queen)] = "Q",
        [(PieceColor.White, PieceType.King)] = "K",
        [(PieceColor.Black, PieceType.Pawn)] = "p",
        [(PieceColor.Black, PieceType.Knight)] = "n",
        [(PieceColor.Black, PieceType.Bishop)] = "b",
        [(PieceColor.Black, PieceType.Rook)] = "r",
        [(PieceColor.Black, PieceType.Queen)] = "q",
        [(PieceColor.Black, PieceType.King)] = "k"
    };

    public static string GetFEN(IBoardView board, GameState gameState)
    {
        // Generate the board part of the FEN
        string fen = "";
        for (int row = 0; row < 8; row++) // Rows from 0 to 7 (1 to 8 from bottom to top)
        {
            int emptyCount = 0;
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board.GetPieceAt(new Position(row, col));
                if (piece == null)
                {
                    emptyCount++;
                }
                else
                {
                    if (emptyCount > 0)
                    {
                        fen += emptyCount.ToString();
                        emptyCount = 0;
                    }
                    fen += PieceChar[(piece.Color, piece.Type)];
                }
            }
            if (emptyCount > 0)
            {
                fen += emptyCount.ToString();
            }
            if (row < 7) fen += "/";  // Add a slash between rows except after the last row
        }

        // Add active color
        fen += $" {(gameState.CurrentPlayerColor == PieceColor.White ? 'w' : 'b')}";

        // Add castling rights
        string castlingRights = "";
        if (gameState.CanCastle[PieceColor.White][CastleDirection.KingSide]) castlingRights += "K";
        if (gameState.CanCastle[PieceColor.White][CastleDirection.QueenSide]) castlingRights += "Q";
        if (gameState.CanCastle[PieceColor.Black][CastleDirection.KingSide]) castlingRights += "k";
        if (gameState.CanCastle[PieceColor.Black][CastleDirection.QueenSide]) castlingRights += "q";
        if (castlingRights == "") castlingRights = "-";
        fen += $" {castlingRights}";

        // Add en passant position
        fen += gameState.EnPassantPosition == null ? " -" : $" {PositionToFEN(gameState.EnPassantPosition)}";

        // Add arbitrary values for halfmove clock and fullmove number
        fen += " 0 1";

        return fen;
    }

    public static Position FENToPosition(string fenSquare)
    {
        char file = fenSquare[0]; // The letter part (a-h)
        char rank = fenSquare[1]; // The number part (1-8)
        int col = file - 'a';  // Convert file (column) from 'a'-'h' to 0-7
        int row = 8 - (rank - '0'); // Convert rank (row) from '1'-'8' to 7-0

        return new Position(row, col);
    }

    public static string PositionToFEN(Position position)
    {
        char file = (char)('a' + position.Col);  // Convert column from 0-7 to 'a'-'h'
        char rank = (char)('1' + (7 - position.Row));  // Convert row from 0-7 to '8'-'1'

        return $"{file}{rank}";
    }
}






