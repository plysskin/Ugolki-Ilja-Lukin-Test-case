
using System.Collections.Generic;
using Ugolki;
using UnityEngine;

public class JumpCheckersMoveRules : MoveRules
{
    internal override bool ValidateMove(Board board, Move move, Side side)
    {
        if (!board.IsLocationValid(move.to)) return false;              // вне доски ходить нельзя
        if (board.GetTokenAt(move.from) != side) return false;          // чужим ходить нельзя
        if (board.GetTokenAt(move.to) != Side.None) return false;       // на любой токен ходить нельзя
        if (Move.Distance(move.from, move.to) != 1 && move.moveType == MoveType.Shift) return false;    //Далеко шифтить нельзя
        //TODO: валидировать прыжки

        return true;
    }

    internal override List<Move> GetValidMoves(Board board, Side side, List<Location> tokenLocations)
    {
        var moveList = new List<Move>();
        foreach (var tokenLocation in tokenLocations)
        {
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x + 1, tokenLocation.y), moveList);
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x - 1, tokenLocation.y), moveList);
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x, tokenLocation.y + 1), moveList);
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x, tokenLocation.y - 1), moveList);
            AddJumps(board, side, tokenLocation, moveList);
        }
        return moveList;
    }

    private void AddJumps(Board board, Side side, Location tokenLocation, List<Move> moveList)
    {
       var jumpDestinationsList = new List<Location>() { }; 
       TryAddJumpsRecursive( board,  side,  tokenLocation,  jumpDestinationsList, firstJump:true);
       var randomJump = new Move(tokenLocation, new Location((int)Random.value * 7, (int) (7 * Random.value)));
       foreach (var jumpDestination in jumpDestinationsList)
       {
           moveList.Add(new Move(tokenLocation,jumpDestination));
       }

    }

    private bool TryAddJumpsRecursive(Board board, Side side, Location fromLocation, List<Location> jumpDestinationsList,bool firstJump=false)
    {

        if (!board.IsLocationValid(fromLocation)) return false;
        if (board.GetTokenAt(fromLocation)!=Side.None && !firstJump) return false;
        CheckDirection(board,side,1, 1, fromLocation, jumpDestinationsList);
        CheckDirection(board, side, -1, 1, fromLocation, jumpDestinationsList);
        CheckDirection(board, side, 1, -1, fromLocation, jumpDestinationsList);
        CheckDirection(board, side, -1, -1, fromLocation, jumpDestinationsList);
        return true;
    }

    private void CheckDirection(Board board, Side side, int xDir, int yDir, Location fromLocation, List<Location> jumpDestinationsList)
    {
        Location over = new Location(fromLocation.x + 1*xDir, fromLocation.y + 1*yDir);
        Location to = new Location(fromLocation.x + 2*xDir, fromLocation.y + 2*yDir);

        if (JumpPossible(board, fromLocation, over, to, jumpDestinationsList))
        {
            jumpDestinationsList.Add(to);
            TryAddJumpsRecursive(board, side, to, jumpDestinationsList);
        }

    }

    private bool JumpPossible(Board board, Location _from, Location over, Location to, List<Location> jumpList) //won't check if locations are in line
    {
        if (!board.IsLocationValid(_from) || !board.IsLocationValid(to)) return false;
        if (board.GetTokenAt(over) == Side.None) return false;
        if (board.GetTokenAt(to) != Side.None) return false;
        if (jumpList.Contains(to)) return false;
        return true;
    }


    private void TryAddShift(Board board, Side side, Location from, Location to, List<Move> movesList)
    {
        var move = new Move(from, to);
        if (ValidateMove(board, move, side))
            movesList.Add(move);
    }
}

