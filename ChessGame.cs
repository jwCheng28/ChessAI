using System;
using System.Collections.Generic;
using System.Collections;
using ChessPieces;

namespace ChessGame
{
    public class Board
    {
        private List<ChessPiece> board;
        private string currentPositionFEN;
        private static readonly Hashtable pieceTranslation = new Hashtable() {
                {'p', PieceAttributes.Pawn}, {'n', PieceAttributes.Knight}, {'b', PieceAttributes.Bishop},
                {'r', PieceAttributes.Rook}, {'q', PieceAttributes.Queen}, {'k', PieceAttributes.King}
        };
        private static readonly Hashtable pieceReverseTranslation = new Hashtable() {
            {PieceAttributes.Pawn, 'p'}, {PieceAttributes.Knight, 'n'}, {PieceAttributes.Bishop, 'b'},
            {PieceAttributes.Rook, 'r'}, {PieceAttributes.Queen, 'q'}, {PieceAttributes.King, 'k'}
        };

        public Board() : this("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")
        {
        }

        public Board(string FEN)
        {
            currentPositionFEN = FEN;
            board = new List<ChessPiece>(64);
            foreach (char piece in FEN)
            {
                if (piece.Equals('/'))
                {
                    continue;
                } 
                else if (Char.IsNumber(piece))
                {
                    int spaces = piece - '0';
                    for (int i = 0; i < spaces; ++i)
                    {
                        board.Add(new ChessPiece(PieceAttributes.Empty, PieceAttributes.Empty));
                    }
                }
                else
                {
                    board.Add(GetChessPieceFromFEN(piece));
                }
            }
        }

        private ChessPiece GetChessPieceFromFEN(char piece)
        {
            ChessPiece chessPiece = new ChessPiece();
            chessPiece.pieceColor = Char.IsUpper(piece) ? PieceAttributes.White : PieceAttributes.Black;
            chessPiece.pieceRank = (int) pieceTranslation[Char.ToLower(piece)];
            return chessPiece;
        }

        private void UpdateFEN()
        {
            List<char> FEN = new List<char>();
            int spaceCount = 0;
            for (int i = 0; i < 64; ++i)
            {
                if (i != 0 && i%8 == 0)
                {
                    if (spaceCount > 0)
                    {
                        FEN.Add((char) ('0' + spaceCount));
                    }
                    FEN.Add('/');
                    spaceCount = 0;
                }
                if (board[i].pieceRank == PieceAttributes.Empty)
                {
                    spaceCount++;
                } 
                else 
                {
                    if (spaceCount > 0)
                    {
                        FEN.Add((char) ('0' + spaceCount));
                    }
                    char piece = (char) pieceReverseTranslation[board[i].pieceRank];
                    if (board[i].pieceColor == PieceAttributes.White)
                    {
                        piece = Char.ToUpper(piece);
                    }
                    FEN.Add(piece);
                    spaceCount = 0;
                }
            }
            currentPositionFEN = string.Join("", FEN);
        }

        public void DisplayBoard()
        {
            Console.WriteLine("\n------------------------");
            for (int i = 0; i < 64; ++i) 
            {
                if (i%8 == 0)
                {
                    Console.WriteLine("");
                }
                Console.Write($" {board[i].pieceRank} ");
            }
            Console.WriteLine("\n------------------------");
        }

        public void DisplayFEN()
        {
            Console.WriteLine("\n------------------------");
            foreach (char piece in currentPositionFEN)
            {
                if (piece.Equals('/'))
                {
                    Console.WriteLine("");
                }
                else if (Char.IsDigit(piece))
                {
                    int spaces = piece - '0';
                    for (int i = 0; i < spaces; ++i)
                    {
                        Console.Write("   ");
                    }
                }
                else
                {
                    Console.Write(" " + piece + " ");
                }
            }
            Console.WriteLine("\n------------------------");
        }

        public bool ValidMove(int piecePosition, int targetPosition)
        {
            if (board[piecePosition].pieceRank == PieceAttributes.Empty)
            {
                Console.WriteLine("There's no piece at that position");
                return false;
            }
            if (PieceMovement.ValidMove(board[piecePosition], piecePosition, targetPosition))
            {
                board[targetPosition] = board[piecePosition];
                board[piecePosition] = new ChessPiece(PieceAttributes.Empty, PieceAttributes.Empty);
                UpdateFEN();
                return true;
            }
            Console.WriteLine("Move Invalid");
            return false;
        }

    }

}
