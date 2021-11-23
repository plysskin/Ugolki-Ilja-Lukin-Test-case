using System.Collections.Generic;
using Ugolki;

public abstract class MoveRules
{
    internal abstract bool ValidateMove(Board board, Move move, Side side);
    internal abstract List<Move> GetValidMoves(Board board, Side side, List<Location> tokenLocations);


}