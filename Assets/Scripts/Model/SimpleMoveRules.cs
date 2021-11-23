using System.Collections.Generic;
using Ugolki;

public class SimpleMoveRules : MoveRules
{
    internal override bool ValidateMove(Board board, Move move, Side side)
    {
        if (!board.IsLocationValid(move.to)) return false;              // вне доски ходить нель€
        if (board.GetTokenAt(move.from) != side) return false;          // чужим ходить нельз€
        if (board.GetTokenAt(move.to) != Side.None) return false;       // на чужой токен ходить нельз€
        if ( Move.Distance(move.from,move.to)!=1) return false;    //ƒалеко ходить нельз€

        return true;
    }

    internal override List<Move> GetValidMoves(Board board, Side side, List<Location> tokenLocations)
    {
        var moveList = new List<Move>() ;
        foreach (var tokenLocation in tokenLocations)
        {
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x + 1, tokenLocation.y),moveList);
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x - 1, tokenLocation.y), moveList);
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x, tokenLocation.y+1 ), moveList);
            TryAddShift(board, side, tokenLocation, new Location(tokenLocation.x, tokenLocation.y-1), moveList);
        }

        return moveList;
    }

    private void TryAddShift(Board board, Side side, Location from, Location to, List<Move> movesList)
    {
        var move = new Move(from, to);
        if (ValidateMove(board,move,side))
            movesList.Add(move);
    }
}