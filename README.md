# ChessAI
This project is for me to play around & learn about how to create a chess AI using minimax and alpha-beta pruning. 
In the future, I might create a GUI for this project; but as of now, the goal is that the user will simply paste in the FEN of the current position and the best move will be calculated.

## How to Run
Note: This project currently is at most halfway done and not optimized!

To run the current progress just clone and run `dotnet run` in the terminal (Need .NET 5.0)

## Current Progress
 - Able to generate a board from FEN
 - Able to move chess pieces correctly
 - Implemented the Chess AI with Minimax (Currently can only use a max search depth of 3 to compute the result in a reasonable amount of time)
 - !Doing Right Now! : Optimization with alpha-beta pruning

## How it Currently Looks Like
![alt text](https://github.com/jwCheng28/ChessAI/blob/main/img/test.png?raw=true)

## Unimplemented Chess Rules (as of now)
 - Can't Castle
 - Pawns currently don't "take" diagonally
 - No En Passant
 - Doesn't detect checks; King can get taken
