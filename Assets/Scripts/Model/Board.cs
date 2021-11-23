using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Ugolki;

internal interface IBoard
{
    Side GetTokenAt(Location location);
    bool IsLocationValid(Location location);
    void UpdateBoard(List<Location> whiteTokens, List<Location> blackTokens);
    public bool WinLocationBlocked(Side side, List<Location> winLocations);
    public bool AllWinLocationOccupied(Side side, List<Location> winLocations);


}

internal class Board : IBoard
{
    private Side[,] _board;
    public readonly BoardSettings boardSettings;


    public Board(BoardSettings boardSettings)
    {
        _board = new Side[boardSettings.maxX, boardSettings.maxY];
        this.boardSettings = boardSettings;
        ClearBoard();
    }

    public Side GetTokenAt(Location location) => _board[location.x, location.y];

    public bool IsLocationValid(Location location)
    {
        return location.x < boardSettings.maxX && location.x >= 0 && location.y < boardSettings.maxY && location.y >= 0;
    }

    public void UpdateBoard(List<Location> whiteTokens, List<Location> blackTokens)
    {
        _board = new Side[boardSettings.maxX, boardSettings.maxY] ;
        ClearBoard();
        PlaceTokens(whiteTokens, Side.White);
        PlaceTokens(blackTokens,Side.Black);
    }



    private void ClearBoard(Side side=Side.None)
    {
        for (int xIndex = 0; xIndex < boardSettings.maxX; xIndex++)
        {
            for (int yIndex = 0; yIndex < boardSettings.maxY; yIndex++)
            {
                _board[xIndex, yIndex] = side;
            }
        }
    }

    private void PlaceTokens(List<Location> locations, Side side)
    {
        foreach (var location in locations)
        {
            _board[location.x, location.y] = side;
        }
    }

    public override string ToString() //for debug purposes only
    {
        StringBuilder result = new StringBuilder(" GameState: \n");
        string symbol = null;
        for (int xIndex = 0; xIndex < boardSettings.maxX; xIndex++)
            result.Append(xIndex.ToString());
        result.Append("\n");
            for (int xIndex = 0; xIndex < boardSettings.maxX; xIndex++)
        {
            for (int yIndex = 0; yIndex < boardSettings.maxY; yIndex++)
            {
                
                switch (_board[xIndex, yIndex])
                {
                    case Side.None: symbol = ".";
                        break;
                    case Side.Black: symbol = "O";
                        break;
                    case Side.White: symbol = "X";
                        break;
                }
                result.Append(symbol) ;
                
            }
            result.Append("\n");
        }
        return result.ToString();

    }


    public bool WinLocationBlocked(Side side, List<Location> winLocations)
    {
        foreach (var winLoc in winLocations)
        {
            if (GetTokenAt(winLoc) == side) return true;
        }
        return false;

    }

    public bool AllWinLocationOccupied(Side side, List<Location> winLocations) //TODO: переделать в LINQ
    {
        foreach (var winLoc in winLocations)
        {
            if (GetTokenAt(winLoc) != side) return false;
        }
        return true;
    }
}