using System;
using Unity.Mathematics;

namespace Ugolki
{

    public enum MoveType
    {
        Shift,
        Jump,
    }

public struct Move:IEquatable<Move>
{

    public readonly Location from;
    public readonly Location to;
    public readonly MoveType moveType;

        public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Move) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((from != null ? @from.GetHashCode() : 0) * 397) ^ (to != null ? to.GetHashCode() : 0);
        }
    }



    public Move(Location from, Location to)
    {
        this.from = from;
        this.to = to;
        this.moveType = (Move.Distance(from, to) == 1) ? MoveType.Shift : MoveType.Jump;
    }

    public static bool operator ==(Move move1,  Move move2)
    {

        //if (move1 is null && move2 is null) return true;
        //if (move1 is null || move2 is null) return false;

        return (move1.@from == move2.@from && move1.to == move2.to);

    }

    public static bool operator !=(Move move1, Move move2)
    {
        return !(move1 == move2);
    }

    public bool Equals(Move other)
    {
        return other != null && (other.@from == @from) && (other.to == this.to);
    }


    public static int Distance(Location a, Location b)
    {
        return math.abs(a.x - b.x) + math.abs(a.y - b.y);
    }
}



}

