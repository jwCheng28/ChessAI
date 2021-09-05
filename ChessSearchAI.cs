using System;
using System.Collections.Generic;
using System.Collections;
using ChessPieces;
using ChessGame;

namespace Player
{
    public static class ChessSearchAI
    {
        private static int OpponentColor(int myColor)
        {
            return myColor == PieceAttributes.White ? PieceAttributes.Black : PieceAttributes.White;
        }

        private static List<int> searchBestMove(Board board, int currentTurnColor, int searchDepth, int currentMoveStart, int currentMoveEnd)
        {
            if (searchDepth == 0)
            {
                return new List<int>() {board.EvaluatePositionScore(), currentMoveStart, currentMoveEnd};
            }

            List<int> bestMove = currentTurnColor == PieceAttributes.Black ? new List<int>() {Int32.MaxValue, -1, -1} : new List<int>() {Int32.MinValue, -1, -1};
            for (int i = 0; i < 64; ++i)
            {
                ChessPiece currentPiece = board.GetBoard()[i];
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
                    ChessPiece targetPiece = board.GetBoard()[j];
                    bool valid = board.MovePiece(i, j, true);
                    if (valid == false)
                    {
                        continue;
                    }
                    List<int> currentBestMove = searchBestMove(board, OpponentColor(currentTurnColor), searchDepth-1, i, j);
                    board.SetChessPiece(i, currentPiece);
                    board.SetChessPiece(j, targetPiece);

                    if ((currentTurnColor == PieceAttributes.Black && currentBestMove[0] < bestMove[0]) ||
                       (currentTurnColor == PieceAttributes.White && currentBestMove[0] > bestMove[0]))
                    {
                        bestMove[0] = currentBestMove[0];
                        bestMove[1] = i;
                        bestMove[2] = j;
                    }
                }
            }
            return bestMove;
        }

        public static List<int> GetBestMove(Board board, int botColor, int searchDepth)
        {
            return searchBestMove(board, botColor, searchDepth, 0, 0).GetRange(1, 2);
        }
    }
}
