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
            int startRow, startCol, endRow, endCol;
            for (int i = 0; i < 5; ++i)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter piece row");
                startRow = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter piece col");
                startCol = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter target row");
                endRow = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter target col");
                endCol = Convert.ToInt32(Console.ReadLine());

                board.MovePiece(new Location(startRow, startCol), new Location(endRow, endCol));

                Console.WriteLine("");
                board.DisplayFEN();

                Console.WriteLine("Input Search Depth");
                int sd = Convert.ToInt32(Console.ReadLine());
                ChessSearchAI.GetBestMove(board, PieceAttributes.Black, sd);
                // Console.WriteLine($"Position Score: {board.EvaluatePositionScore()}");
            }
        }
    }
}
