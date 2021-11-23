using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ugolki;
using Unity.VisualScripting;
using UnityEngine;

public enum GameViewState
{
    not_active = 0,
    awaiting,
    selected,
}

public interface IGameViewController
{
    public void SetGame(IGameModel game);
    public void ActivateController(IPlayerController playerController);
    public void DelayedMove(IPlayerController player, Move move, Side side, float delay = 0.8f);

}




public class GameView : MonoBehaviour, IGameViewController

{
//    private IGameModel _gameModel;

    [SerializeField]
    private CellPrefab cellPrefab;

    [SerializeField] 
    private TokenPrefab _TokenPrefab;

    [SerializeField] 
    private UIMenuController m_MenuController;

    private IGameModel _currentGame;
    private Dictionary<Location, ICellPrefab> _boardView;
    private GameViewState _gameViewState;
    private List<Move> _validMoves;
    private Location _selectedToken;
    private IPlayerController _playerController;
    private float _timeToDelayedMove;
    private Move _delayedMove;


    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (_timeToDelayedMove>0)
        {
            _timeToDelayedMove -= Time.deltaTime;
            if (_timeToDelayedMove <= 0)
            {
                _timeToDelayedMove = 0;
                _playerController.MakeMove(_delayedMove);

            }
        }
    }


    public void SetGame(IGameModel game)
    {
        if (!(_currentGame is null)) _currentGame.boardUpdated -= OnGameUpdated;
        BuildBoard(game.GetBoardSettings());
        _currentGame = game;
        _currentGame.boardUpdated += OnGameUpdated;
        OnGameUpdated(game);
        
    }
    public void ActivateController(IPlayerController playerController)
    {
        _gameViewState = GameViewState.awaiting;
        _playerController = playerController;
    }

    public void DelayedMove(IPlayerController player, Move move, Side side, float delay = 0.8f)
    {
        _timeToDelayedMove = delay;
        _delayedMove = move;
        _playerController = player;
    }




    private void BuildBoard(BoardSettings boardSettings)
    {
        _boardView = new Dictionary<Location, ICellPrefab>() { }; 
        for (int xIndex = 0; xIndex < boardSettings.maxX; xIndex++)
        {
            for (int yIndex = 0; yIndex < boardSettings.maxY; yIndex++)
            {
                var cellLocation = new Location(xIndex, yIndex);
                var cellGameObject= Instantiate(cellPrefab,transform);
                ((ICellPrefab)cellGameObject).SpawnToken(_TokenPrefab);
                ((ICellPrefab)cellGameObject).SetLocation(cellLocation);
                _boardView.Add(cellLocation, cellGameObject);
                cellGameObject.transform.position = new Vector3(xIndex * (9 / boardSettings.maxX) - 4f,
                    yIndex * (9 / boardSettings.maxY) - 4f); //TODO: избавиться от магических чисел

                ((ICellPrefab) cellGameObject).SetToken(Side.None);
                cellGameObject.OnClicked += CellClicked; 
            }
        }
    }

    // Update is called once per frame

    void OnGameUpdated(IGameModel game)
    {
        var boardSettings = game.GetBoardSettings();
        for (int xIndex = 0; xIndex < boardSettings.maxX; xIndex++)
        {
            for (int yIndex = 0; yIndex < boardSettings.maxY; yIndex++)
            {
                var cellLocation = new Location(xIndex, yIndex);
                _boardView[cellLocation].SetToken(game.GetTokenAt(cellLocation));
            }
        }
        if (game.GetGameState() != GameState.Going) GameOver();
    }

    private void GameOver()
    {
        DestroyBoard();
        m_MenuController.GameOver(_currentGame.GetGameState());
        gameObject.SetActive(false);



    }

    private void DestroyBoard()
    {
        foreach (var keyValuePair in _boardView)
        {
            
            GameObject.Destroy((keyValuePair.Value as CellPrefab)?.gameObject);
        }
        _boardView = null;
    }



    private void CellClicked(Location location)
    {
        switch (_gameViewState) 
        {
            case GameViewState.awaiting:
                TrySelect(location);
                break;
            case GameViewState.not_active:
                break;
            case GameViewState.selected:
                TryMoveTo(location);
                break;
        }
    }

    private bool TryMoveTo(Location location)
    {
        if (_currentGame.GetTokenAt(location)==_currentGame.GetActivePlayer())
        {
            Deselect();
            TrySelect(location);
            return false;
        }

        var move = new Move(_selectedToken, location);
        if (!_validMoves.Contains(move)) return false;
        _gameViewState = GameViewState.not_active;
        _playerController.MakeMove(move);
        return true;
    }

    private void Deselect()
    {
        foreach (var validMove in _validMoves)
        {
            _boardView[validMove.to].AllowMoveHere(false);
        }
    }

    private bool TrySelect(Location location)
    {
        var token = _currentGame.GetTokenAt(location);
        if (_currentGame.GetActivePlayer() != token) return false;
        
        var validMoves = _currentGame.GetValidMoves(token, new List<Location>() {location});
        
        if (!validMoves.Any()) return false;
        _validMoves = validMoves;
        _gameViewState = GameViewState.selected;
        _boardView[location].Select(true);
        _selectedToken = location;
        validMoves.ForEach(move =>{_boardView[move.to].AllowMoveHere(true);} );

        //foreach (var validMove in validMoves)
        //{
        //    _boardView[validMove.to].AllowMoveHere(true);
        //}

        return true;
    }


    private void OnDestroy()
    {
        if (!(_currentGame is null)) _currentGame.boardUpdated -= OnGameUpdated;
    }

}