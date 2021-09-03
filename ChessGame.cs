using System;
using System.Collections.Generic;
using System.Collections;

namespace ChessGame
{
    public class PieceAttributes
    {
        public const int Empty = 0, Pawn = 1, Knight = 2, Bishop = 3, Rook = 4, Queen = 5, King = 6;
        public const int Black = 1, White = 2;
    }

    public struct ChessPiece
    {
        public int pieceRank;
        public int pieceColor;

        public ChessPiece(int pieceRank, int pieceColor)
        {
            this.pieceRank = pieceRank;
            this.pieceColor = pieceColor;
        }
    }

    public class Board
    {
        private List<ChessPiece> board;
        private string currentPositionFEN;

        public Board() : this("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")
        {
        }

        public Board(string FEN)
        {
            currentPositionFEN = FEN;
            board = new List<ChessPiece>(64);
            int currentPosition = 0;
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
            Hashtable pieceTranslation = new Hashtable() {
                {'p', PieceAttributes.Pawn}, {'n', PieceAttributes.Knight}, {'b', PieceAttributes.Bishop},
                {'r', PieceAttributes.Rook}, {'q', PieceAttributes.Queen}, {'k', PieceAttributes.King}
            };
            chessPiece.pieceRank = (int) pieceTranslation[Char.ToLower(piece)];
            return chessPiece;
        }

        public void DisplayBoard()
        {
            for (int i = 0; i < 64; i++) 
            {
                if (i%8 == 0)
                {
                    Console.WriteLine("");
                }
                Console.Write($"{board[i].pieceRank} ");
            }
        }

    }


}
