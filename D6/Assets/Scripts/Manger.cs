using System;
using UnityEngine;

public class Manger : MonoBehaviour
{
    [SerializeField] private Dice _dicePrefab;
    [Range(-10, 10)] [SerializeField] private float _diceMinX;
    [Range(-10, 10)] [SerializeField] private float _diceMaxX;
    [Range(-10, 10)] [SerializeField] private float _diceMinY;
    [Range(-10, 10)] [SerializeField] private float _diceMaxY;
    [SerializeField] private Vector2Int _diceRowColumn;

    private Dice[] _dice;
    
    private void Awake()
    {
        _dice = new Dice[_diceRowColumn.x * _diceRowColumn.y];
        
        for (var x = 0; x < _diceRowColumn.x; x++)
        {
            var xCord = (float)x / _diceRowColumn.x; 
            
            for (var y = 0; y < _diceRowColumn.y; y++)
            {
                var yCord = (float)y / _diceRowColumn.y; 

                var newDice = Instantiate(_dicePrefab,
                    new Vector3(Mathf.Lerp(_diceMinX, _diceMaxX, xCord),
                    Mathf.Lerp(_diceMinY, _diceMaxY, yCord)), Quaternion.identity);
                _dice[x * _diceRowColumn.y + y] = newDice;
            }
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Roll Dice"))
        {
            foreach (var die in _dice)
            {
                die.Roll();
            }
        }
    }
}
