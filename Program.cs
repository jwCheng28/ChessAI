using System;
using System.Collections.Generic;
using ChessGame;
using ChessPieces;
using Player;

namespace ChessAI
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board("rnb1k2r/pp3ppp/4pq2/2pp4/3P4/2NB1N2/PPP2PPP/R2QK2R");
            // Board board = new Board();
            board.DisplayFEN();
            int start, end;
            for (int i = 0; i < 5; ++i)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter piece location");
                start = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter target location");
                end = Convert.ToInt32(Console.ReadLine());
                board.MovePiece(start, end);
                Console.WriteLine("");
                board.DisplayFEN();
                List<int> bestOpponentMove = ChessSearchAI.GetBestMove(board, PieceAttributes.Black, 2);
                Console.WriteLine($"AI Move: {bestOpponentMove[0]} to {bestOpponentMove[1]}");
                // Console.WriteLine($"Position Score: {board.EvaluatePositionScore()}");
            }
        }
    }
}
