using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell 
{
    public enum State
    {
        Empty,
        Occupied,
        Blocked
    }

    private int _width;
    private int _height;
    private int _x, _y;
    private int _debugVal;

    public State CurrentState { get; set; }

    public int GetX => _x;
    public int GetY => _y;
    public int GetWidth => _width;
    public int GetHeight => _height;

    public int DebugVal
    {
        get => _debugVal;
        set
        {
            _debugVal = value;
        }
    }

    public Cell (int width, int height, int x, int y)
    {
        _width = width;
        _height = height;
        _x = x;
        _y = y;
        _debugVal = 0;
    }

    public override string ToString()
    {
        return "X: " + _x + " Y: " + _y + " DebugVal: " + _debugVal;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * _width, 0,  y * _height);
    }

    public void Draw90()
    {
        Debug.DrawLine(GetWorldPosition(_x, _y), GetWorldPosition(_x, _y + 1), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(_x, _y), GetWorldPosition(_x + 1, _y), Color.white, 100f);
    }
}
