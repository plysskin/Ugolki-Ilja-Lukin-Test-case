using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using  Ugolki;

public class GameLauncher : MonoBehaviour
{
    
    private float moveCooldown = 1;
    private IGameModel gameModel;
    public GameView gameView;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hi!");
    }

    public void StartGame(string whitePlayerType, string blackPlayerType, string rules) 
    {
        var boardSettings = new BoardSettings(maxX:8, maxY:8, startingLocationSideLength:3, movesToLeaveStartingSquare:100);



        var whitePlayerController = CreatePlayerController(whitePlayerType, gameView);
        var blackPlayerController = CreatePlayerController(blackPlayerType, gameView);
        var rulesSet = SelectRules(rules);


        gameModel = new UgolkiGameModel(rulesSet, whitePlayerController, blackPlayerController, boardSettings);
        gameView.SetGame(gameModel);
        (gameView as GameView)?.gameObject.SetActive(true);
    }

    private MoveRules SelectRules(string rules)
    {
        switch (rules)
        {
            case "CheckersJumps":
                return new JumpCheckersMoveRules();
                
            case "AllJumps":
                return new JumpQueenMoveRules();
                
            case "NoJumps":
                return new SimpleMoveRules();

        }
        throw new ArgumentException($"Unsupported Ruled {rules}");

    }

    private IPlayer CreatePlayerController(string playerType, IGameViewController gameView)
    {
        if (playerType == "Human")
            return new PlayerController(gameView);
        else if (playerType == "Computer")
            return new SimpleAIPlayerController(gameView);
        else
            throw new ArgumentException($"Unsupported Player Type {playerType}");
    }




    void Update()
    {

    }

}