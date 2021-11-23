using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ugolki;

public class TokenPrefab : MonoBehaviour
{
    [SerializeField]
    private Color _blackColor;
    [SerializeField]
    private Color _whiteColor;
    private SpriteRenderer _renderer;
    private Side _side;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSide(Side side)
    {
        if (_renderer==null) _renderer = GetComponent<SpriteRenderer>();

        _renderer.color = (side == Side.Black ? _blackColor : _whiteColor);
        _renderer.enabled = (side != Side.None);
    }

    
}
