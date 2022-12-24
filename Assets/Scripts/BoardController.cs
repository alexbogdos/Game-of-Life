using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardController : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private GameObject tile;
    [SerializeField] private Transform grid;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float offsetFactor;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float alteranteY;
    [SerializeField] private TMP_Text counter;
    [SerializeField] private float roundDuration;



    private Board board;

    private bool settingUp = false;
    private bool started = false;

    private int gen = 1;
    private int lastLenght;
    private float previousTime = 0f;
    private float timeNow = 0f;

    private Dictionary<(int, int), TileController> tileControllers = new Dictionary<(int, int), TileController>();

    public void Reset()
    {
        board = new Board(length);
        board.Generate();
        BuildBoard();


        lastLenght = length = board.Length;
        previousTime = 0f;
        gen = 1;
        settingUp = false;
        started = false;

    }

    void Awake()
    {
        board = new Board(99);
        board.Generate();
        BuildBoard(active: false);
        board.ReSize(length);


        lastLenght = length = board.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveCamera();
        BuildBoard();
    }


    // Update is called once per frame
    void Update()
    {
        if (started || settingUp)
        {
            return;
        }

        if (lastLenght != length && (3 <= length && length < 100))
        {
            board.ReSize(length);
            MoveCamera();
            board.Generate();
            BuildBoard();

            lastLenght = length = board.Length;
        }
    }

    void FixedUpdate()
    {
        if (settingUp || !started)
        {
            return;
        }

        timeNow = Time.time;
        float dt = timeNow - previousTime;
        if (dt >= roundDuration)
        {
            // Start Game Code

            List<(int, int)> dying = new List<(int, int)>();
            List<(int, int)> born = new List<(int, int)>();

            foreach ((int, int) pos in board.getPositions())
            {
                bool state = board.getState(pos);
                List<(int, int)> neighbors = board.getNeighbors(pos);
                int count = 0;



                foreach ((int, int) neighbor in neighbors)
                {


                    if (board.exists(neighbor) == false)
                    {
                        continue;
                    }


                    if (board.getState(neighbor))
                    {
                        count += 1;
                    }
                }

                // Case from rules 1. and 2.
                if (board.getState(pos) && (count <= 1 || count >= 4))
                {// Mark alive cell for death
                    dying.Add(pos);
                }

                // Case from rule 4.
                else if (!board.getState(pos) && count == 3)
                {
                    // Mark dead cell for birth
                    born.Add(pos);
                }

                // Excecute actions according to each
                // cell's category
            }


            foreach ((int, int) pos in dying)
            {
                board.setAlive(pos, false);
                tileControllers[pos].setAlive(false);
            }
            foreach ((int, int) pos in born)
            {
                board.setAlive(pos, true);
                tileControllers[pos].setAlive(true);
            }


            // End Game Code

            gen += 1;
            updateCounter();
            previousTime = timeNow;
        }

    }

    public void setLength(int value)
    {
        length = value;
    }

    public void setRoundDuration(float value)
    {
        roundDuration = value;
    }

    public float getRoundDuration()
    {
        return roundDuration;
    }


    private void updateCounter()
    {
        string text = "Generation: ";
        if (gen < 10)
        {
            text += $"   {gen}";
        }
        else if (gen < 100)
        {
            text += $"  {gen}";
        }
        else if (gen < 1000)
        {
            text += $" {gen}";
        }
        else
        {
            text += $"{gen}";
        }

        counter.text = text;
    }

    public void StartGame()
    {
        started = true;
    }
    public void FinishSetUp()
    {
        settingUp = false;
        foreach ((int, int) pos in board.getPositions())
        {
            tileControllers[pos].FinishSetUp();
        }
    }

    public void StartSetUp()
    {
        foreach ((int, int) pos in board.getPositions())
        {
            tileControllers[pos].StartSetUp();
        }
        timeNow = Time.time;
        settingUp = true;
    }


    void MoveCamera()
    {
        float p = (board.Length -1) * 0.5f;
        float h = board.Length * offsetFactor;
        if (board.Length <= 20) 
        {
            h *= 1.1f;
        }
        Vector3 newPos = new Vector3(p, h, p);
        cameraTransform.position = newPos; // new Vector3(cameraTransform.position.x, board.Length * offsetFactor, cameraTransform.position.z);
    }

    void BuildBoard(bool active = true)
    {
        foreach ((int, int) pos in tileControllers.Keys)
        {
            if (pos.Item1 >= board.Length || pos.Item2 >= board.Length)
            {
                tileControllers[pos].gameObject.SetActive(false);
            }
        }


        int count = 0;
        foreach ((int, int) pos in board.getPositions())
        {
            if (tileControllers.ContainsKey(pos))
            {
                tileControllers[pos].gameObject.SetActive(true);
                tileControllers[pos].setBoard(board);
                tileControllers[pos].setAlive(board.getState(pos));
                tileControllers[pos].FinishSetUp();
                count += 1;
                continue;
            }

            (float, float) translated = board.Translate(pos);

            float yPos = 0;
            if (count % 2 == 0)
            {
                yPos = alteranteY;
            }
            Vector3 localPos = new Vector3(translated.Item1, yPos / 100, translated.Item2);

            GameObject newTile = Instantiate<GameObject>(tile);
            newTile.transform.localPosition = localPos;
            newTile.transform.SetParent(grid);

            if (active == false)
            {
                newTile.SetActive(false);
            }

            TileController tileController = newTile.GetComponent<TileController>();
            tileController.setAlive(board.getState(pos));
            tileController.setCamera(mainCamera);
            tileController.setBoard(board);
            tileController.setPosition(pos);
            newTile.name = $"({pos.Item1}, {pos.Item2})";


            tileControllers[pos] = tileController;

            count += 1;
        }
    }
}

