using System;
using ChessGame;

namespace ChessAI
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.DisplayFEN();
            int start, end;
            for (int i = 0; i < 5; ++i)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter piece location");
                start = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter target location");
                end = Convert.ToInt32(Console.ReadLine());
                board.ValidMove(start, end);
                Console.WriteLine("");
                board.DisplayFEN();
            }
        }
    }
}
