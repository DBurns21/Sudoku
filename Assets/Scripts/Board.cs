using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    private Row[] rows;
    private Key[] keys;

    public Tile currentTile { get; set; } = null;

    private string[] sudokus;
    private string currentSudoku;
    private string solvedSudoku;

    public string keyPressed { get; set; } = null;

    [Header("Tile States")]
    public Tile.State emptyState;
    public Tile.State selectedState;
    public Tile.State sameNumberState;
    public Tile.State incorrectState;
    public Tile.State correctState;

    [Header("UI")]
    public Button tryAgainButton;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        //keys = GameObject.FindGameObjectWithTag("Keyboard").GetComponentsInChildren<Key>();
    }

    private void Start()
    {
        LoadData();
        SetBoard();
    }

    private void Update()
    {
        if (currentTile != null)
        {
            if (keyPressed != null && currentTile.changeable)
            {
                currentTile.setNumber(keyPressed);
            }
        }
        keyPressed = null;
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("sudokus") as TextAsset;
        sudokus = textFile.text.Split('\n');
    }

    private void SetBoard()
    {
        currentSudoku = sudokus[UnityEngine.Random.Range(0, sudokus.Length)];
        currentSudoku = currentSudoku.Trim();

        int i = 0;
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                if (currentSudoku[i] != '.')
                {
                    tile.setNumber(currentSudoku[i++].ToString());
                    tile.changeable = false;
                }
                else
                {
                    i++;
                }
                
            }
        }
    }
}
