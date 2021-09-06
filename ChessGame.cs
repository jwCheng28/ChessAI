using System;
using System.Collections.Generic;
using System.Collections;
using ChessPieces;

namespace ChessGame
{
    public class Board
    {
        private ChessPiece[,] board;
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
            board = new ChessPiece[8, 8];
            int row = 0, col = 0;
            foreach (char piece in FEN)
            {
                if (piece.Equals('/'))
                {
                    row++;
                    col = 0;
                    continue;
                } 
                else if (Char.IsNumber(piece))
                {
                    int spaces = piece - '0';
                    for (int i = 0; i < spaces; ++i)
                    {
                        board[row, col] = new ChessPiece(PieceAttributes.Empty, PieceAttributes.Empty);
                        col++;
                    }
                }
                else
                {
                    ChessPiece currentChessPiece = GetChessPieceFromFEN(piece);
                    int currentColor = currentChessPiece.pieceColor;
                    int currentRank = currentChessPiece.pieceRank;
                    ((Hashtable) pieceCount[currentColor])[currentRank] = (int) ((Hashtable) pieceCount[currentColor])[currentRank] + 1;
                    board[row, col] = currentChessPiece;
                    col++;
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
            for (int row = 0; row < 8; ++row)
            {
                for (int col = 0; col < 8; ++col)
                {
                    if (board[row, col].pieceRank == PieceAttributes.Empty)
                    {
                        spaceCount++;
                    } 
                    else 
                    {
                        if (spaceCount > 0)
                        {
                            FEN.Add((char) ('0' + spaceCount));
                        }
                        char piece = (char) pieceReverseTranslation[board[row, col].pieceRank];
                        if (board[row, col].pieceColor == PieceAttributes.White)
                        {
                            piece = Char.ToUpper(piece);
                        }
                        FEN.Add(piece);
                        spaceCount = 0;
                    }
                }
                if (spaceCount > 0)
                {
                    FEN.Add((char) ('0' + spaceCount));
                }
                if (row != 7)
                {
                    FEN.Add('/');
                }
                spaceCount = 0;
            }
            previousPositionFEN = currentPositionFEN;
            currentPositionFEN = string.Join("", FEN);
        }

        public ChessPiece[,] GetBoard()
        {
            return board;
        }
        
        public void SetChessPiece(Location pieceLocation, ChessPiece targetPiece)
        {
            int pieceRow = pieceLocation.row, pieceColumn = pieceLocation.column;

            int indexColor = board[pieceRow, pieceColumn].pieceColor, indexRank = board[pieceRow, pieceColumn].pieceRank;
            int targetColor = targetPiece.pieceColor, targetRank = targetPiece.pieceRank;

            if (indexRank != PieceAttributes.Empty)
            {
                ((Hashtable) pieceCount[indexColor])[indexRank] = (int) ((Hashtable) pieceCount[indexColor])[indexRank] - 1;
            }
            if (targetRank != PieceAttributes.Empty)
            {
                ((Hashtable) pieceCount[targetColor])[targetRank] = (int) ((Hashtable) pieceCount[targetColor])[targetRank] + 1;
            }
            board[pieceRow, pieceColumn] = targetPiece;
            UpdateFEN();
        }

        public string GetFEN()
        {
            return currentPositionFEN;
        }

        public void DisplayBoard()
        {
            Console.WriteLine("\n------------------------");
            for (int row = 0; row < 64; ++row)
            {
                for (int col = 0; col < 64; ++col)
                {
                    Console.Write($" {board[row, col].pieceRank} ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("\n------------------------");
        }

        public void DisplayFEN()
        {
            Console.Write("\n   ");
            for (int col = 0; col < 8; ++col)
            {
                Console.Write($" {col} ");
            }
            Console.Write("\n   ");
            for (int col = 0; col < 8; ++col)
            {
                Console.Write("---");
            }
            int row = 0;
            Console.Write($"\n{row}| ");
            row++;
            foreach (char piece in currentPositionFEN)
            {
                if (piece.Equals('/'))
                {
                    Console.WriteLine("");
                    Console.Write($"{row}| ");
                    row++;
                }
                else if (Char.IsDigit(piece))
                {
                    int spaces = piece - '0';
                    for (int i = 0; i < spaces; ++i)
                    {
                        Console.Write(" - ");
                    }
                }
                else
                {
                    Console.Write(" " + piece + " ");
                }
            }
            Console.WriteLine("\n   ------------------------");
        }

        public bool ValidMove(Location pieceLocation, Location targetLocation, bool surpressMessage)
        {
            int pieceRow = pieceLocation.row, pieceColumn = pieceLocation.column;
            int targetRow = targetLocation.row, targetColumn = targetLocation.column;
            if (board[pieceRow, pieceColumn].pieceRank == PieceAttributes.Empty)
            {
                if (surpressMessage == false)
                {
                    Console.WriteLine("There's no piece at that position");
                }
                return false;
            }
            List<Location> movePath = PieceMovement.ValidMove(board[pieceRow, pieceColumn], pieceLocation, targetLocation);
            if (board[pieceRow, pieceColumn].pieceColor == board[targetRow, targetColumn].pieceColor || movePath.Count == 0)
            {
                if (surpressMessage == false)
                {
                    Console.WriteLine("Invalid Move");
                }
                return false;
            }
            for (int i = 1; i < movePath.Count-1; ++i)
            {
                int row = movePath[i].row, column = movePath[i].column;
                if (board[row, column].pieceRank != PieceAttributes.Empty)
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

        public bool MovePiece(Location pieceLocation, Location targetLocation, bool surpressMessage=false)
        {
            if (ValidMove(pieceLocation, targetLocation, surpressMessage))
            {
                int pieceRow = pieceLocation.row, pieceColumn = pieceLocation.column;
                int targetRow = targetLocation.row, targetColumn = targetLocation.column;
                if (board[targetRow, targetColumn].pieceRank != PieceAttributes.Empty)
                {
                    int targetColor = board[targetRow, targetColumn].pieceColor;
                    int targetRank = board[targetRow, targetColumn].pieceRank;
                    ((Hashtable) pieceCount[targetColor])[targetRank] = (int) ((Hashtable) pieceCount[targetColor])[targetRank] - 1;
                }
                board[targetRow, targetColumn] = board[pieceRow, pieceColumn];
                board[pieceRow, pieceColumn] = new ChessPiece(PieceAttributes.Empty, PieceAttributes.Empty);
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
