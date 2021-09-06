using System;
using System.Collections.Generic;
using ChessGame;
using ChessPieces;
using Player;

namespace ChessAI
{
    class Program
    {
        static void PlayTest(string startFEN, int testTurns=5, int aiColor=PieceAttributes.Black, int aiSearchDepth=2)
        {
            Board board = new Board(startFEN);
            board.DisplayFEN();
            int startRow, startCol, endRow, endCol;
            for (int i = 0; i < testTurns; ++i)
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

                List<Location> aiMove = ChessSearchAI.GetBestMove(board, aiColor, aiSearchDepth);
                board.MovePiece(aiMove[0], aiMove[1]);
                Console.WriteLine("Opponent has made a move");
                Console.WriteLine("");
                board.DisplayFEN();
                Console.WriteLine($"Position Score: {board.EvaluatePositionScore()}");
            }
        }

        static void BestNextMove()
        {
            string FEN;
            int colorToMove, searchDepth;
            Console.WriteLine("Please input FEN of the current position");
            FEN = Console.ReadLine();
            Console.WriteLine("Please input the color to move (0-Black, 1-White)");
            colorToMove = Math.Abs(Convert.ToInt32(Console.ReadLine())) % 2 + 1;
            Console.WriteLine("Please input move search depth (how many moves to look ahead)");
            searchDepth = Math.Abs(Convert.ToInt32(Console.ReadLine()));
            
            Board board = new Board(FEN);
            Console.WriteLine("Current Position");
            board.DisplayFEN();
            Console.WriteLine("");
            List<Location> bestMove = ChessSearchAI.GetBestMove(board, colorToMove, searchDepth);
            board.MovePiece(bestMove[0], bestMove[1]);
            string resultFEN = board.GetFEN();
            string color = colorToMove == PieceAttributes.Black ? "Black" : "White";
            Console.WriteLine($"{color}'s Best Move (search depth = {searchDepth}) is (r{bestMove[0].row}, c{bestMove[0].column}) to (r{bestMove[1].row}, c{bestMove[1].column})");
            board.DisplayFEN();
            Console.WriteLine("");
            Console.WriteLine($"Best Move FEN = {resultFEN}");
        }

        static void Main(string[] args)
        {
            // PlayTest("rnb1k2r/pp3ppp/4pq2/2pp4/3P4/2NB1N2/PPP2PPP/R2QK2R");
            BestNextMove();
        }
    }
}
