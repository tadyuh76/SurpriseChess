using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SurpriseChess
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
            // Tạo string phần bảng của FEN
            string fen = GenerateBoardFEN(board);

            // Thêm trạng thái lượt người chơi hiện tại
            fen += $" {(gameState.CurrentPlayerColor == PieceColor.White ? 'w' : 'b')}";

            // Thêm trạng thái quyền nhập thành
            fen += $" {GetCastlingRights(gameState)}";

            // Thêm vị trí en passant được
            fen += gameState.EnPassantPosition == null ? " -" : $" {PositionToFEN(gameState.EnPassantPosition)}";

            // Thêm số đếm HalfMove và FullMove
            fen += $" {gameState.HalfMoveClock} {gameState.FullMoveNumber}";

            return fen;
        }

        private static string GenerateBoardFEN(IBoardView board)
        {
            string fen = "";
            for (int row = 0; row < 8; row++)
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
                if (row < 7) fen += "/";  // thêm "/" giữa các dòng bàn cờ
            }
            return fen;
        }

        private static string GetCastlingRights(GameState gameState)
        {
            string castlingRights = "";
            if (gameState.CanCastle[PieceColor.White][CastleDirection.KingSide]) castlingRights += "K";
            if (gameState.CanCastle[PieceColor.White][CastleDirection.QueenSide]) castlingRights += "Q";
            if (gameState.CanCastle[PieceColor.Black][CastleDirection.KingSide]) castlingRights += "k";
            if (gameState.CanCastle[PieceColor.Black][CastleDirection.QueenSide]) castlingRights += "q";
            return castlingRights == "" ? "-" : castlingRights;
        }

        public static string PositionToFEN(Position position)
        {

            char file = (char)('a' + position.Col);  // Chuyển đổi cột từ 0-7 sang a-h
            char rank = (char)('1' + (7 - position.Row));  // Chuyển đổi dòng từ 0-7 sang 1-8
            return $"{file}{rank}";
        }

        public static void LoadPositionFromFEN(string fen, ReplayBoard board)
        {
            string[] parts = fen.Split(' ');
            string positionPart = parts[0];
            string[] ranks = positionPart.Split('/');

            for (int row = 0; row < ranks.Length; row++)
            {
                int col = 0;
                foreach (char symbol in ranks[row])
                {
                    if (char.IsDigit(symbol))
                    {
                        col += symbol - '0'; // Ô bàn cờ trống
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
                _ => throw new ArgumentException("Ký hiệu quân cờ không hợp lệ")
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
                    throw new ArgumentException($"ID trận {matchId} không thể tìm được trong file.");
                }

                return fenList;
            }
        }

        public static Position FENToPosition(string fenSquare)
        {
            char file = fenSquare[0]; // Phần chữ (a-h)
            char rank = fenSquare[1]; // Phần số (1-8)
            int col = file - 'a';  // Chuyển đổi file (cột) từ'a'-'h' sang 0-7
            int row = 7 - (rank - '1');  // Chuyển đổi rank (dòng) từ '1'-'8' sang 0-7

            return new Position(row, col);
        }
    }
}