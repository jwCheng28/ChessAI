using System;
using System.Collections.Generic;
using System.Collections;

namespace ChessPieces
{
    public static class PieceAttributes
    {
        public const int Empty = 0;
        public const int Pawn = 1, Knight = 2, Bishop = 3, Rook = 4, Queen = 5, King = 6;
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

    public class PieceMovement
    {
        static private bool PawnMove(int pieceColor, int currentPosition, int targetPosition)
        {
            if (pieceColor == PieceAttributes.Black)
            {
                return (targetPosition - currentPosition) == 8;
            }
            else if (pieceColor == PieceAttributes.White)
            {
                return (targetPosition - currentPosition) == -8;
            }
            return false;
        }

        static private bool KnightMove(int currentPosition, int targetPosition)
        {
            HashSet<int> possiblePositionDeltas = new HashSet<int>() {-17, -15, -10, -6, 6, 10, 15, 17};
            return possiblePositionDeltas.Contains(targetPosition - currentPosition);
        }

        static private bool BishopMove(int currentPosition, int targetPosition)
        {
            int delta = targetPosition - currentPosition;
            return delta % 7 == 0 || delta % 9 == 0; 
        }

        static private bool RookMove(int currentPosition, int targetPosition)
        {
            return (targetPosition - currentPosition) % 8 == 0 || targetPosition/8 == currentPosition/8;
        }

        static private bool QueenMove(int currentPosition, int targetPosition)
        {
            return BishopMove(currentPosition, targetPosition) || RookMove(currentPosition, targetPosition);
        }

        static private bool KingMove(int currentPosition, int targetPosition)
        {
            HashSet<int> possiblePositionDeltas = new HashSet<int>() {-9, -8, -7, -1, 1, 7, 8, 9};
            return possiblePositionDeltas.Contains(targetPosition - currentPosition);
        }

        static public bool ValidMove(ChessPiece currentPiece, int currentPosition, int targetPosition)
        {
            if (currentPiece.pieceRank == PieceAttributes.Pawn)
            {
                return PawnMove(currentPiece.pieceColor, currentPosition, targetPosition);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Knight)
            {
                return KnightMove(currentPosition, targetPosition);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Bishop)
            {
                return BishopMove(currentPosition, targetPosition);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Rook)
            {
                return RookMove(currentPosition, targetPosition);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Queen)
            {
                return QueenMove(currentPosition, targetPosition);
            }
            else if (currentPiece.pieceRank == PieceAttributes.King)
            {
                return KingMove(currentPosition, targetPosition);
            }
            else
            {
                return false;
            }
        }
    }
}
