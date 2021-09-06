using System;
using System.Collections.Generic;
using System.Collections;
using ChessPieces;
using ChessGame;

namespace Player
{
    public static class ChessSearchAI
    {
        private static int color;
        private static int bestScore;
        private static int initialSearchDepth;
        private static Location bestStartLocation;
        private static Location bestEndLocation;

        private static int OpponentColor(int myColor)
        {
            return myColor == PieceAttributes.White ? PieceAttributes.Black : PieceAttributes.White;
        }

        private static int searchBestMove(Board board, int currentTurnColor, int searchDepth)
        {
            if (searchDepth == 0)
            {
                return board.EvaluatePositionScore();
            }

            int bestCurrentScore = currentTurnColor == PieceAttributes.Black ? Int32.MaxValue : Int32.MinValue;
            for (int i = 0; i < 64; ++i)
            {
                int row = i / 8, col = i % 8;
                ChessPiece currentPiece = board.GetBoard()[row, col];
                if (currentPiece.pieceColor != currentTurnColor)
                {
                    continue;
                }
                for (int j = 0; j < 64; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    int targetRow = j / 8, targetColumn = j % 8;
                    ChessPiece targetPiece = board.GetBoard()[targetRow, targetColumn];
                    bool valid = board.MovePiece(new Location(row, col), new Location(targetRow, targetColumn), true);
                    if (valid == false)
                    {
                        continue;
                    }
                    int moveEvaluation = searchBestMove(board, OpponentColor(currentTurnColor), searchDepth-1);
                    board.SetChessPiece(new Location(row, col), currentPiece);
                    board.SetChessPiece(new Location(targetRow, targetColumn), targetPiece);
                    if ((currentTurnColor == PieceAttributes.Black && moveEvaluation < bestCurrentScore) ||
                        (currentTurnColor == PieceAttributes.White && moveEvaluation > bestCurrentScore))
                    {
                        bestCurrentScore = moveEvaluation;
                    }

                    if (searchDepth == initialSearchDepth &&
                         ((color == PieceAttributes.Black && bestCurrentScore < bestScore) ||
                          (color == PieceAttributes.White && bestCurrentScore > bestScore))
                       )
                    {
                        bestScore = bestCurrentScore;
                        bestStartLocation = new Location(row, col);
                        bestEndLocation = new Location(targetRow, targetColumn);
                    }
                }
            }
            return bestCurrentScore;
        }
        
        public static List<Location> GetBestMove(Board board, int botColor, int searchDepth)
        {
            bestScore = botColor == PieceAttributes.Black ? Int32.MaxValue : Int32.MinValue;
            initialSearchDepth = searchDepth;
            color = botColor;
            searchBestMove(board, botColor, searchDepth);
            Console.WriteLine($"Score: {bestScore} From ({bestStartLocation.row}, {bestStartLocation.column}) to ({bestEndLocation.row}, {bestEndLocation.column})");
            return new List<Location>() {bestStartLocation, bestEndLocation};
        }
    }
}
