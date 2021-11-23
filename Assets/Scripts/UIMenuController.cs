using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ugolki;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private Canvas gameOver;

    private string _blackPlayer = "Computer";
    private string _whitePlayer = "Human";
    private string _rules = "AllJumps";
    // Start is called before the first frame update

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked(GameObject Canvas)
    {
        Canvas.SetActive(false);

        Debug.Log($"StartingGame, rules are {_rules}, White: {_whitePlayer}, Black: {_blackPlayer}");
        GameObject.FindObjectOfType<GameLauncher>()?.StartGame(_whitePlayer, _blackPlayer, _rules);
    }

    public void SetBlackPlayer(string type)
    {
        Debug.Log($"Black is now {type}");
        _blackPlayer = type;
    }
    public void SetWhitePlayer(string type)
    {
        Debug.Log($"White is now {type}");
        _whitePlayer = type;
    }

    public void SetRules(string type)
    {
        Debug.Log($"Rules is is now {type}");
        _rules = type;
    }

    public void GameOver(GameState gameState)
    {
        gameOver.GetComponentInChildren<Text>().text = (gameState == GameState.BlackWin) ? "Black WIN!" : "White WIN!";
        gameOver.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        gameOver.gameObject.SetActive(false);
        gameObject.SetActive(true);

    }

}
