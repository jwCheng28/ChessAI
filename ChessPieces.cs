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

    public static class PieceMovement
    {
        static private List<int> PawnMove(int pieceColor, int currentPosition, int targetPosition)
        {
            if ((pieceColor == PieceAttributes.Black && (targetPosition - currentPosition) == 8) ||
                (pieceColor == PieceAttributes.White && (targetPosition - currentPosition) == -8))
            {
                return new List<int>() {currentPosition, targetPosition};
            }
            return new List<int>() {};
        }

        static private List<int> KnightMove(int currentPosition, int targetPosition)
        {
            HashSet<int> possiblePositionDeltas = new HashSet<int>() {-17, -15, -10, -6, 6, 10, 15, 17};
            if (possiblePositionDeltas.Contains(targetPosition - currentPosition))
            {
                return new List<int>() {currentPosition, targetPosition};
            }
            return new List<int>() {};
        }

        static private List<int> BishopMove(int currentPosition, int targetPosition)
        {
            int delta = targetPosition - currentPosition;
            int start = Math.Min(targetPosition, currentPosition);
            int end = Math.Max(targetPosition, currentPosition);
            List<int> path = new List<int>();
            if (delta % 7 == 0)
            {
                for (int i = start; i <= end; i += 7)
                {
                    path.Add(i);
                }
            }
            else if (delta % 9 == 0)
            {
                for (int i = start; i <= end; i += 9)
                {
                    path.Add(i);
                }
            }
            return path;
        }

        static private List<int> RookMove(int currentPosition, int targetPosition)
        {
            int start = Math.Min(targetPosition, currentPosition);
            int end = Math.Max(targetPosition, currentPosition);
            List<int> path = new List<int>();
            if (currentPosition % 8 == targetPosition % 8)
            {
                for (int i = start; i <= end; i+=8)
                {
                    path.Add(i);
                }
            }
            else if (currentPosition/8 == targetPosition/8)
            {
                for (int i = start; i <= end; ++i)
                {
                    path.Add(i);
                }
            }
            return path;
        }

        static private List<int> QueenMove(int currentPosition, int targetPosition)
        {
            List<int> bishopPath = BishopMove(currentPosition, targetPosition);
            List<int> rookPath = RookMove(currentPosition, targetPosition);
            return bishopPath.Count > 0 ? bishopPath : rookPath;
        }

        static private List<int> KingMove(int currentPosition, int targetPosition)
        {
            HashSet<int> possiblePositionDeltas = new HashSet<int>() {-9, -8, -7, -1, 1, 7, 8, 9};
            if (possiblePositionDeltas.Contains(targetPosition - currentPosition))
            {
                return new List<int>() {currentPosition, targetPosition};
            }
            return new List<int>() {};
        }

        static public List<int> ValidMove(ChessPiece currentPiece, int currentPosition, int targetPosition)
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
                return new List<int>() {};
            }
        }
    }
}
