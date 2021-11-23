using System.Collections.Generic;
using Ugolki;

public  class BoardSettings
{
    public readonly int maxX;
    public readonly int maxY;
    public readonly List<Location> startingBlackLocations;
    public readonly List<Location> startingWhiteLocations;
    public readonly List<Location> winBlackLocations;
    public readonly List<Location> winWhiteLocations;
    public readonly int startingLocationSideLength;


    public BoardSettings(int maxX, int maxY, int startingLocationSideLength,int movesToLeaveStartingSquare=80)
    {

        this.maxX = maxX;
        this.maxY = maxY;
        this.startingLocationSideLength = startingLocationSideLength;

        this.startingWhiteLocations = GenerateStartingLocations(Side.White);
        this.startingBlackLocations = GenerateStartingLocations(Side.Black);
        this.winBlackLocations = startingWhiteLocations;
        this.winWhiteLocations = startingBlackLocations;
        this.movesToLeaveStartingSquare = movesToLeaveStartingSquare;


    }

    public int movesToLeaveStartingSquare { get; set; }

    private List<Location> GenerateStartingLocations(Side side)
    {
        int xStart, yStart;
        int length = startingLocationSideLength;
        List<Location> result = new List<Location>() { };
        if (side == Side.White)
        {
            xStart = 0;
            yStart = 0;
        }
        else
        {
            xStart = maxX - length;
            yStart = maxY - length;
        }

        for (int xIndex = xStart; xIndex < xStart + length; xIndex++)
        {
            for (int yIndex = yStart; yIndex < yStart + length; yIndex++)
            {
                result.Add(new Location(xIndex, yIndex));
            }
        }



        return result;
    }

}