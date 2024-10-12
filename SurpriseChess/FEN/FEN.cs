using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SurpriseChess.FEN
{
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
            int halfMoveClock = 0, fullMoveClock = 0;
            // Generate the board part of the FEN
            string fen = "";
            for (int row = 7; row >= 0; row--)
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
                if (row > 0) fen += "/";  // Add a slash between rows
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
            if (gameState.CurrentPlayerColor == PieceColor.White)
            {
                halfMoveClock++;
            }

            fen += $" {halfMoveClock}";
            fen += $" {++fullMoveClock}";

            return fen;
        }

        public static Position FENToPosition(string fenSquare)
        {
            char file = fenSquare[0]; // The letter part (a-h)
            char rank = fenSquare[1]; // The number part (1-8)
            int col = file - 'a';  // Convert file (column) from 'a'-'h' to 0-7
            int row = rank - '1';  // Convert rank (row) from '1'-'8' to 0-7

            return new Position(row, col);
        }

        public static string PositionToFEN(Position position)
        {
            char file = (char)('a' + position.Col);  // Convert column from 0-7 to 'a'-'h'
            char rank = (char)('1' + position.Row);  // Convert row from 0-7 to '1'-'8'

            return $"{file}{rank}";
        }

        public static void LoadPositionFromFEN(string fen, ReplayBoard board)
        {
            string[] parts = fen.Split(' ');
            string positionPart = parts[0];
            string[] ranks = positionPart.Split('/');

            for (int row = 0; row < 8; row++)
            {
                int col = 0;
                foreach (char symbol in ranks[7 - row])
                {
                    if (char.IsDigit(symbol))
                    {
                        col += symbol - '0'; // Empty squares
                    }
                    else
                    {
                        board.SetPieceAt(new Position(row, col), CreatePieceFromFEN(symbol));
                        col++;
                    }
                }
            }
        }

        private static Piece CreatePieceFromFEN(char symbol)
        {
            PieceColor color = char.IsUpper(symbol) ? PieceColor.White : PieceColor.Black;
            PieceType type = GetPieceTypeFromSymbol(char.ToUpper(symbol));
            return PieceFactory.Create(color, type);
        }

        private static PieceType GetPieceTypeFromSymbol(char symbol)
        {
            return symbol switch
            {
                'P' => PieceType.Pawn,
                'N' => PieceType.Knight,
                'B' => PieceType.Bishop,
                'R' => PieceType.Rook,
                'Q' => PieceType.Queen,
                'K' => PieceType.King,
                _ => throw new ArgumentException("Invalid piece symbol")
            };
        }

        public static class FENParser
        {
            public static List<string> LoadFENFromFileByMatchId(string filePath, int matchId)
            {
                var fenList = new List<string>();
                bool isInDesiredMatch = false;

                using (var reader = new StreamReader(filePath))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Match ID:"))
                        {
                            int currentMatchId = int.Parse(line.Split(": ")[1]);
                            isInDesiredMatch = (currentMatchId == matchId);
                        }

                        if (isInDesiredMatch && line.StartsWith("History:"))
                        {
                            while ((line = reader.ReadLine()) != null && !line.StartsWith("Match ID:"))
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                {
                                    fenList.Add(line.Trim());
                                }
                            }
                            break;
                        }
                    }
                }

                if (!isInDesiredMatch)
                {
                    throw new ArgumentException($"Match ID {matchId} not found in the file.");
                }

                return fenList;
            }
        }
    }
}