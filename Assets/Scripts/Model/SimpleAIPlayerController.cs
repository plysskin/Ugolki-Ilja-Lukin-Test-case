using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace Ugolki
{
    class SimpleAIPlayerController : IPlayer, IPlayerController

    {
    private IGameModel _game;
    private List<Location> _myWiningLocations;
    private readonly IGameViewController _gameView;
    private Side _mySide;

    public void GameUpdated(IGameModel game)
    {
        _game = game;

    }

    public SimpleAIPlayerController(IGameViewController gameView)
    {
        _gameView = gameView;
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public bool YourTurn(Side mySide)
    {
        _mySide = mySide;
        _myWiningLocations = mySide == Side.Black
            ? _game.GetBoardSettings().winBlackLocations
            : _game.GetBoardSettings().winWhiteLocations;

        var allAvailableMoves = _game.GetValidMoves(mySide, _game.GetTokensList(mySide));
        var movesWeight = new Dictionary<Move, float>();
        
        allAvailableMoves.ForEach(move => movesWeight[move]=EvaluateMove(move));



        var maxWeight = movesWeight.Max((pair => pair.Value));
        var bestMove = movesWeight.First(pair => pair.Value == maxWeight).Key;

        _gameView.DelayedMove(this, bestMove, mySide);

        return true;
    }

    private float EvaluateMove(Move move)
    {

        return MinimumDistanceToWiningLocation(move.from) - MinimumDistanceToWiningLocation(move.to);
    }

    private float MinimumDistanceToWiningLocation(Location location)
    {
        return _myWiningLocations.Min(winLoc => Move.Distance(winLoc, location)+(Random.value-0.5f));
    }

    public void MakeMove(Move move)
    {
        _game.TryMove(move, _mySide);
    }

    }
}