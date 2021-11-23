using System;
using System.Collections;
using System.Collections.Generic;
using Ugolki;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public interface ICellPrefab
{
    internal void SetToken(Side side);
    internal void SetLocation(Location location);
    internal void AllowMoveHere(bool Allow);

    void Select(bool selected);
    void Deselect();
    void SpawnToken(TokenPrefab tokenPrefab);
}



public class CellPrefab : MonoBehaviour,ICellPrefab 
{
    public Action<Location> OnClicked;
    private bool MoveAllowed;


    [SerializeField] private Color allowMoveCollor;
    [SerializeField] private Color EmptyColorLight;
    [SerializeField] private Color EmptyColorDark;
    [SerializeField] private Color BlackColor;
    [SerializeField] private Color WhiteColor;
    private Color _defaultColor;
    private Color _currentColor;
    private TokenPrefab _token;


    private Location _location;
    private SpriteRenderer _renderer;
    private Color _storedColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider.gameObject == gameObject)
            {
                HandleClick();
            }
        }
    }

    private void HandleClick()
    {
        OnClicked?.Invoke(_location);
    }

    private void ChangeColor(Color color)
    {
        if (_renderer ==null)
        {
            _renderer = GetComponent<SpriteRenderer>();

        }
        _currentColor = color;
        _renderer.color = color;
    }


    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1);
    }
    

    void ICellPrefab.SetToken(Side side)
    {
        _token.SetSide(side);
        ChangeColor(_defaultColor);
    }

    void ICellPrefab.SetLocation(Location location)
    {
        _location = location;
        _defaultColor = (_location.x + _location.y) % 2 == 0 ? EmptyColorDark: EmptyColorLight;
        _storedColor = _defaultColor;
    }

    void ICellPrefab.AllowMoveHere(bool Allow)
    {
        if (Allow)
        {
            _storedColor = _currentColor;
            ChangeColor(allowMoveCollor);
        }
        else
        {
            ChangeColor(_storedColor);
        }
        
       
    }


    public void Select(bool selected=true)
    {
        //здесь можно рисовать рамку.
    }

    public void Deselect()
    {
        Select(false);
    }

    public void SpawnToken(TokenPrefab tokenPrefab)
    {
        _token = GameObject.Instantiate(tokenPrefab, transform);
        _token.SetSide(Side.None);
    }
}

