using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private Board board;
    private Camera mainCamera;

    private (int, int) position;
    private bool _alive = false;
    private bool settingUp;


    private Material material;


    // Start is called before the first frame update
    void Awake()
    {

        // If the object you want is the one this script is attached to:
        Renderer renderer = GetComponent<Renderer>();

        // This creates a special material just for this one object. If you change this material it will only affect this object:
        material = renderer.material;


        // if (_alive)
        // {
        //     material.color = Color.green;
        // }
        // else
        // {
        //     material.color = Color.black;
        // }
    }

    void Update()
    {
        if (settingUp)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == transform.name)
                    {
                        setAlive(!_alive);
                        board.setAlive(position, _alive);
                        Debug.Log($"{position} {_alive}");

                    }
                }
            }
        }
    }

    public void FinishSetUp()
    {
        settingUp = false;
    }

    public void StartSetUp()
    {
        settingUp = true;
    }

    public void setCamera(Camera cameraMain)
    {
        mainCamera = cameraMain;
    }

    public bool Alive
    {
        get { return Alive; }
    }

    public void setAlive(bool alive)
    {
        _alive = alive;

        if (_alive)
        {
            material.color = Color.green;
        }
        else
        {
            material.color = Color.black;
        }
    }

    public void setBoard(Board newBoard)
    {
        board = newBoard;
    }

    public void setPosition((int, int) pos)
    {
        position = pos;
    }
}
