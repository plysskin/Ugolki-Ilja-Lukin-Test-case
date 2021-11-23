using UnityEngine;
using Ugolki;

internal interface IPlayer
{
    void GameUpdated(IGameModel game);
    bool YourTurn(Side yourSide);

}

public interface IPlayerController
{
    public void MakeMove(Move move);
}


class PlayerController :IPlayer,IPlayerController
{
    private IGameViewController _gameView;
    private IGameModel _game;
    private Side _mySide;

    public PlayerController(IGameViewController gameView)
    {
        _gameView = gameView;
    }

    public void GameUpdated(IGameModel game)
    {
        _game = game;
    }

    public bool YourTurn(Side yourSide)
    {
        _mySide = yourSide;
        _gameView.ActivateController(this);
        return true;
    }


    public void MakeMove(Move move)
    {
        _game.TryMove(move, _mySide);

    }
}