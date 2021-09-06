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
        // Standard Valuation (from Wikipedia); King is technically INF but using 100 makes thing easier
        public static readonly Hashtable pieceValue = new Hashtable() {
            {PieceAttributes.Pawn, 1}, {PieceAttributes.Knight, 3}, {PieceAttributes.Bishop, 3},
            {PieceAttributes.Rook, 5}, {PieceAttributes.Queen, 9}, {PieceAttributes.King, 100}
        };
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

    public struct Location
    {
        public int row;
        public int column;

        public Location(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }

    public static class PieceMovement
    {
        static private List<Location> PawnMove(int pieceColor, Location currentLocation, Location targetLocation)
        {
            int currentRow = currentLocation.row, currentColumn = currentLocation.column;
            int targetRow = targetLocation.row, targetColumn = targetLocation.column;
            if (currentColumn == targetColumn && 
                 ((pieceColor == PieceAttributes.Black && (targetRow - currentRow) == 1) ||
                  (pieceColor == PieceAttributes.White && (targetRow - currentRow) == -1))
               )
            {
                return new List<Location>() {currentLocation, targetLocation};
            }
            return new List<Location>() {};
        }

        static private List<Location> KnightMove(Location currentLocation, Location targetLocation)
        {
            int currentRow = currentLocation.row, currentColumn = currentLocation.column;
            int targetRow = targetLocation.row, targetColumn = targetLocation.column;
            int[,] possibleMovements = new int[,] {
                {-1, -2}, {-2, -1}, {-2, 1}, {-1, 2},
                {1, -2}, {2, -1}, {2, 1}, {1, 2}
            };
            for (int i = 0; i < possibleMovements.GetLength(0); ++i)
            {
                int yMove = possibleMovements[i, 0];
                int xMove = possibleMovements[i, 1];
                if (targetRow-currentRow == yMove && targetColumn-currentColumn == xMove)
                {
                    return new List<Location>() {currentLocation, targetLocation};
                }
            }
            return new List<Location>() {};
        }

        static private List<Location> BishopMove(Location currentLocation, Location targetLocation)
        {
            int currentRow = currentLocation.row, currentColumn = currentLocation.column;
            int targetRow = targetLocation.row, targetColumn = targetLocation.column;
            int[,] moveDirections = new int[,] {
                {-1, -1}, {1, 1},
                {-1, 1}, {1, -1}
            };
            List<Location> path = new List<Location>();
            for (int i = 0; i < moveDirections.GetLength(0); ++i)
            {
                int currentYDirection = moveDirections[i, 0], currentXDirection = moveDirections[i, 1];
                int yDelta = targetRow - currentRow, xDelta = targetColumn - currentColumn;
                int yDeltaAbs = Math.Abs(yDelta), xDeltaAbs = Math.Abs(xDelta);
                if (yDelta != xDelta)
                {
                    continue;
                }
                int unitYDirection = yDelta / yDeltaAbs, unitXDirection = xDelta/ xDeltaAbs;
                if (unitYDirection == currentYDirection && unitXDirection == currentXDirection)
                {
                    for (int curY = currentRow, curX = currentColumn; 
                         curY != targetRow && curX != targetColumn; 
                         curY += unitYDirection, curX += unitXDirection)
                    {
                        path.Add(new Location(curY, curX));
                    }
                    path.Add(targetLocation);
                    break;
                }
            }
            return path;
        }

        static private List<Location> RookMove(Location currentLocation, Location targetLocation)
        {
            int currentRow = currentLocation.row, currentColumn = currentLocation.column;
            int targetRow = targetLocation.row, targetColumn = targetLocation.column;
            List<Location> path = new List<Location>();
            if (currentRow == targetRow)
            {
                int startColumn = Math.Min(targetColumn, currentColumn);
                int endColumn = Math.Max(targetColumn, currentColumn);
                for (; startColumn <= endColumn; ++startColumn)
                {
                    path.Add(new Location(currentRow, startColumn));
                }
            }
            else if (currentColumn == targetColumn)
            {
                int startRow = Math.Min(targetRow, currentRow);
                int endRow = Math.Max(targetRow, currentRow);
                for (; startRow <= endRow; ++startRow)
                {
                    path.Add(new Location(startRow, currentColumn));
                }
            }
            return path;
        }

        static private List<Location> QueenMove(Location currentLocation, Location targetLocation)
        {
            List<Location> bishopPath = BishopMove(currentLocation, targetLocation);
            List<Location> rookPath = RookMove(currentLocation, targetLocation);
            return bishopPath.Count > 0 ? bishopPath : rookPath;
        }

        static private List<Location> KingMove(Location currentLocation, Location targetLocation)
        {
            int currentRow = currentLocation.row, currentColumn = currentLocation.column;
            int targetRow = targetLocation.row, targetColumn = targetLocation.column;
            int rowDeltaAbs = Math.Abs(targetRow - currentRow), columnDeltaAbs = Math.Abs(targetColumn - currentColumn);
            if (rowDeltaAbs <= 1 && columnDeltaAbs <= 1)
            {
                return new List<Location>() {currentLocation, targetLocation};
            }
            return new List<Location>();
        }

        static public List<Location> ValidMove(ChessPiece currentPiece, Location currentLocation, Location targetLocation)
        {
            if (currentPiece.pieceRank == PieceAttributes.Pawn)
            {
                return PawnMove(currentPiece.pieceColor, currentLocation, targetLocation);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Knight)
            {
                return KnightMove(currentLocation, targetLocation);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Bishop)
            {
                return BishopMove(currentLocation, targetLocation);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Rook)
            {
                return RookMove(currentLocation, targetLocation);
            }
            else if (currentPiece.pieceRank == PieceAttributes.Queen)
            {
                return QueenMove(currentLocation, targetLocation);
            }
            else if (currentPiece.pieceRank == PieceAttributes.King)
            {
                return KingMove(currentLocation, targetLocation);
            }
            else
            {
                return new List<Location>() {};
            }
        }
    }
}
