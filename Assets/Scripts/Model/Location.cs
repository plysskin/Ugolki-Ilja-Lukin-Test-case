using System;
namespace Ugolki
{
public struct Location:IEquatable<Location>
{
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            if (ReferenceEquals(this, obj)) return true;
            return false;
        }
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Location) obj);
    }

    public int x;
    public int y;

    public Location(int x=0, int y=0)
    {
        this.x = x;
        this.y = y;
    }
    public override int GetHashCode()
    {
        return x * 1000 + y;

    }

    public static bool operator ==( Location location1,  Location location2)
    {
        //if (location1 is null && location2 is null) return true;
        //if (location1 is null || location2 is null) return false;

        return  ( location1.x == location2.x && location1.y == location2.y);
    }

    public static bool operator !=(Location move1, Location move2)
    {
        return !(move1 == move2);
    }

    public bool Equals( Location other)
    {
        return x == other.x && y == other.y;
    }

    public override string ToString()
    {
        return x.ToString() + "," + y.ToString();
    }
}

}