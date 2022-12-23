using System.Collections.Generic;
using UnityEngine;


public class Board
{
    public Board(int boardLength)
    {
        _length = parseLength(boardLength);
    }
    private int _length;
    public int Length
    {
        get
        {
            return _length;
        }
        set
        {
            _length = parseLength(value);
        }
    }

    private Dictionary<(int, int), bool> map = new Dictionary<(int, int), bool>();
    private List<(int, int)> positions = new List<(int, int)>();

    // public void Reset() 
    // {
    //     map.Clear();
    //     positions.Clear();
    // }

    private int parseLength(int len)
    {
        if (len % 2 == 0)
        {
            return len + 1;
        }
        else
        {
            return len;
        }
    }

    public List<(int, int)> getNeighbors((int, int) position)
    {
        List<(int, int)> list = new List<(int, int)>();

        int i = position.Item1;
        int j = position.Item2;
        
        list.Add((i - 1, j));
        list.Add((i + 1, j));
        list.Add((i, j - 1));
        list.Add((i, j + 1));
        list.Add((i - 1, j - 1));
        list.Add((i - 1, j + 1));
        list.Add((i + 1, j - 1));
        list.Add((i + 1, j + 1));
        
        return list;
    }

    public void ReSize(int newLength)
    {
        _length = parseLength(newLength);
        Generate();
    }

    public void Generate()
    {
        positions.Clear();

        Dictionary<(int, int), bool> _map = new Dictionary<(int, int), bool>();

        for (int row = 0; row < _length; row++)
        {
            for (int column = 0; column < _length; column++)
            {
                (int, int) pos = (row, column);
                _map[pos] = false;
                positions.Add(pos);
            }
        }

        map.Clear();
        map = _map;
    }

    public (float, float) Translate((int, int) pos)
    {
        float middle = _length * 0.5f;
        float row = middle - pos.Item1;
        float column = middle - pos.Item2;

        return (row, column);
    }

    public bool getState((int, int) position)
    {
        return map[position];
    }

    public bool exists((int, int) pos) 
    {
        return map.ContainsKey(pos);
    }

    public void setAlive((int, int) pos, bool alive)
    {
        map[pos] = alive;
    }

    public List<(int, int)> getPositions()
    {
        return positions;
    }
}