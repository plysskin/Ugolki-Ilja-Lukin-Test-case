using System;
using System.Collections.Generic;
using Ugolki;
using Unity.VisualScripting;
using UnityEngine;

public interface IGameModel
{
    //Board GetBoard();
    public List<Move> GetValidMoves(Side side);
    public List<Move> GetValidMoves(Side side, List<Location> tokens);
    bool ValidateMove(Move move, Side side);
    bool TryMove(Move move, Side side);
    event Action<IGameModel> boardUpdated;
    List<Move> GetMovesHistory();
    Side GetActivePlayer();
    List<Location> GetTokensList(Side side);
    Side GetTokenAt(int x, int y);
    Side GetTokenAt(Location location);
    BoardSettings GetBoardSettings();
    GameState GetGameState();

}


internal class UgolkiGameModel : IGameModel
{

    public event Action<IGameModel> boardUpdated;
    public event Action<IGameModel> gameFinished;
    IPlayer WhitePlayer { set; get; }
    IPlayer BlackPlayer { set; get; }
    public GameState GameState { get; }

    private Side activePlayer { get; set; }
    private readonly MoveRules MoveRules;
    private readonly Board _board;
    private readonly List<Location> _whiteTokens;
    private readonly List<Location> _blackTokens;
    private readonly List<Move> _moveHistory;
    private GameState _gameState;


    public List<Move> GetMovesHistory() => _moveHistory;
    
    public Side GetActivePlayer() => activePlayer;
    public List<Location> GetTokensList(Side side)
    {
        if (side == Side.None) throw new ArgumentException("can't get empty squares like that");
        return (side == Side.Black) ? _blackTokens : _whiteTokens;
    }

        

    public UgolkiGameModel(MoveRules moveRules, IPlayer white, IPlayer black, BoardSettings boardSettings)
    {
        _moveHistory = new List<Move>() {};
        MoveRules = moveRules;
        WhitePlayer = white;
        BlackPlayer = black;

        boardUpdated += WhitePlayer.GameUpdated;
        boardUpdated += BlackPlayer.GameUpdated;
        _board = new Board(boardSettings);
        activePlayer = Side.White;
        _whiteTokens = CopyLocationsList(boardSettings.startingWhiteLocations);
        _blackTokens = CopyLocationsList(boardSettings.startingBlackLocations);
        _board.UpdateBoard(_whiteTokens,_blackTokens);
        _gameState = GameState.Going;
        boardUpdated?.Invoke(this);

        NotifySubscribers();
    }

    private void NotifySubscribers()
    {
        if (activePlayer == Side.White) WhitePlayer.YourTurn(activePlayer);
        if (activePlayer == Side.Black) BlackPlayer.YourTurn(activePlayer);
        boardUpdated?.Invoke(this);

    }


    public List<Move> GetValidMoves(Side side) => MoveRules.GetValidMoves(_board, side, activePlayer == Side.Black ? _blackTokens : _whiteTokens);
    public List<Move> GetValidMoves(Side side, List<Location> tokens) => MoveRules.GetValidMoves(_board, side, tokens);


    public bool ValidateMove(Move move, Side side)
    {
        if (side != activePlayer) return false;

        return MoveRules.ValidateMove(_board, move, side);
    }

    public bool TryMove(Move move, Side side)
    {
        if (!ValidateMove(move, side)) return false;
        ApplyMove(move);
        return true;
    }

    private void ApplyMove(Move move)
    {
        MoveTokens(_board, activePlayer == Side.Black ? _blackTokens : _whiteTokens, move);
        var winingSide = CheckWinConditions();

        if (winingSide != Side.None)
        {
//            Debug.Log($" {winingSide} wins!"); 
            _gameState = winingSide == Side.Black ? GameState.BlackWin : GameState.WhiteWin;


        }

        activePlayer = (1 - activePlayer);
        NotifySubscribers();
    }

    private void MoveTokens(Board board, List<Location> tokens, Move move)
    {
        if (_gameState != GameState.Going) return;//Игра уже окончена


        tokens.Remove(move.@from);
        tokens.Add(move.to);
        _moveHistory.Add(move);
        _board.UpdateBoard(_whiteTokens, _blackTokens);

    }

    public Side GetTokenAt(int x, int y) => _board.GetTokenAt(new Location(x,y));

    public Side GetTokenAt(Location location) => _board.GetTokenAt(location);
    public BoardSettings GetBoardSettings() => _board.boardSettings;
    public GameState GetGameState() => _gameState;
    
    private List<Location> CopyLocationsList(List<Location> locations)
    {
        var result = new List<Location>() { };
        foreach (var location in locations)
        {
            result.Add(location);
        }

        return result;
    }

    public Side CheckWinConditions()
    {
        if (_board.AllWinLocationOccupied(Side.White, _board.boardSettings.winWhiteLocations)) return Side.White;
        if (_board.AllWinLocationOccupied(Side.Black, _board.boardSettings.winBlackLocations)) return Side.Black;
        if (GetMovesHistory().Count >= _board.boardSettings.movesToLeaveStartingSquare)
        {
            if (_board.WinLocationBlocked(Side.Black, _board.boardSettings.winWhiteLocations)) return Side.White;
            if (_board.WinLocationBlocked(Side.White, _board.boardSettings.winBlackLocations)) return Side.Black;
        }

        return Side.None;
    }


}

public enum Side
{
    White=0,
    Black=1,
    None = 3,
}

public enum GameState
{
    Going,
    WhiteWin,
    BlackWin,
}