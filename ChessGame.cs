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
        private string previousPositionFEN;

        private static readonly Hashtable pieceTranslation = new Hashtable() {
            {'p', PieceAttributes.Pawn}, {'n', PieceAttributes.Knight}, {'b', PieceAttributes.Bishop},
            {'r', PieceAttributes.Rook}, {'q', PieceAttributes.Queen}, {'k', PieceAttributes.King}
        };
        private static readonly Hashtable pieceReverseTranslation = new Hashtable() {
            {PieceAttributes.Pawn, 'p'}, {PieceAttributes.Knight, 'n'}, {PieceAttributes.Bishop, 'b'},
            {PieceAttributes.Rook, 'r'}, {PieceAttributes.Queen, 'q'}, {PieceAttributes.King, 'k'}
        };

        private Hashtable blackPieceCount;       
        private Hashtable whitePieceCount;
        private Hashtable pieceCount;

        public Board() : this("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")
        {
        }

        public Board(string FEN)
        {
            InitializeChessBoard(FEN);
        }

        private void InitializeChessBoard(string FEN)
        {
            blackPieceCount = new Hashtable() {
                {PieceAttributes.Pawn, 0}, {PieceAttributes.Knight, 0}, {PieceAttributes.Bishop, 0},
                {PieceAttributes.Rook, 0}, {PieceAttributes.Queen, 0}, {PieceAttributes.King, 0}
            };
            whitePieceCount = new Hashtable() {
                {PieceAttributes.Pawn, 0}, {PieceAttributes.Knight, 0}, {PieceAttributes.Bishop, 0},
                {PieceAttributes.Rook, 0}, {PieceAttributes.Queen, 0}, {PieceAttributes.King, 0}
            };
            pieceCount = new Hashtable() {
                {PieceAttributes.Black, blackPieceCount},
                {PieceAttributes.White, whitePieceCount}
            };
            previousPositionFEN = FEN;
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
                    ChessPiece currentChessPiece = GetChessPieceFromFEN(piece);
                    int currentColor = currentChessPiece.pieceColor;
                    int currentRank = currentChessPiece.pieceRank;
                    ((Hashtable) pieceCount[currentColor])[currentRank] = (int) ((Hashtable) pieceCount[currentColor])[currentRank] + 1;
                    board.Add(currentChessPiece);
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
            previousPositionFEN = currentPositionFEN;
            currentPositionFEN = string.Join("", FEN);
        }

        public List<ChessPiece> GetBoard()
        {
            return board;
        }
        
        public void SetChessPiece(int pieceIndex, ChessPiece targetPiece)
        {
            board[pieceIndex] = targetPiece;
        }

        public string GetFEN()
        {
            return currentPositionFEN;
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

        public bool ValidMove(int piecePosition, int targetPosition, bool surpressMessage)
        {
            if (board[piecePosition].pieceRank == PieceAttributes.Empty)
            {
                if (surpressMessage == false)
                {
                    Console.WriteLine("There's no piece at that position");
                }
                return false;
            }
            List<int> movePath = PieceMovement.ValidMove(board[piecePosition], piecePosition, targetPosition);
            if (board[piecePosition].pieceColor == board[targetPosition].pieceColor || movePath.Count == 0)
            {
                if (surpressMessage == false)
                {
                    Console.WriteLine("Invalid Move");
                }
                return false;
            }
            for (int i = 1; i < movePath.Count-1; ++i)
            {
                if (board[movePath[i]].pieceRank != PieceAttributes.Empty)
                {
                    if (surpressMessage == false)
                    {
                        Console.WriteLine("Invalid Move");
                    }
                    return false;
                }
            }
            return true;
        }

        public bool MovePiece(int piecePosition, int targetPosition, bool surpressMessage=false)
        {
            if (ValidMove(piecePosition, targetPosition, surpressMessage))
            {
                if (board[targetPosition].pieceRank != PieceAttributes.Empty)
                {
                    int targetColor = board[targetPosition].pieceColor;
                    int targetRank = board[targetPosition].pieceRank;
                    ((Hashtable) pieceCount[targetColor])[targetRank] = (int) ((Hashtable) pieceCount[targetColor])[targetRank] - 1;
                }
                board[targetPosition] = board[piecePosition];
                board[piecePosition] = new ChessPiece(PieceAttributes.Empty, PieceAttributes.Empty);
                UpdateFEN();
                return true;
            }
            return false;
        }

        public int EvaluatePositionScore()
        {
            int positionScore = 0;
            foreach (DictionaryEntry pieceRemain in blackPieceCount)
            {
                positionScore -= (int) PieceAttributes.pieceValue[pieceRemain.Key] * (int) pieceRemain.Value;
            }
            foreach (DictionaryEntry pieceRemain in whitePieceCount)
            {
                positionScore += (int) PieceAttributes.pieceValue[pieceRemain.Key] * (int) pieceRemain.Value;
            }
            return positionScore;
        }
    }
}
