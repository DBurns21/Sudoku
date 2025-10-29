using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private bool solved;

    private Row[] rows;
    private Key[] keys;

    public Tile currentTile { get; set; } = null;

    private string[] sudokus;
    private string currentSudoku;
    private int[,] solvedSudoku = new int[9,9];

    public string keyPressed { get; set; } = null;

    [Header("Tile States")]
    public Tile.State emptyState;
    public Tile.State selectedState;
    public Tile.State sameNumberState;
    public Tile.State incorrectState;
    public Tile.State correctState;

    [Header("UI")]
    public Button CheckAnswerButton;

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
        currentSudoku.Replace('.', '0');

        currentSudoku = 
            "023097600" +
            "546018279" +
            "000200001" +
            "084062300" +
            "251783900" +
            "009051800" +
            "012075008" +
            "968034752" +
            "475800063";

        int curr = 0;

        for (int j = 0; j < solvedSudoku.GetLength(0); j++)
        {
            for (int k = 0; k < solvedSudoku.GetLength(1); k++)
            {
                solvedSudoku[j, k] = currentSudoku[curr++] - '0';
            }
        }

        int i = 0;
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                if (currentSudoku[i] != '0')
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

        sudokuSolver(solvedSudoku, 0, 0);

        string answer = "";

        for (int j = 0; j < solvedSudoku.GetLength(0); j++)
        {
            for (int k = 0; k < solvedSudoku.GetLength(1); k++)
            {
                answer += solvedSudoku[j,k].ToString();
            }
        }
    }

    private bool isSafe(int[,] mat, int row, int col, int num)
    {
        for (int x = 0; x < 9; ++x)
        {
            if (mat[row, x] == num)
            {
                return false;
            }
        }

        for (int x = 0; x < 9; x++)
        {
            if (mat[x, col] == num)
            {
                return false;
            }
        }

        int startRow = row - (row % 3);
        int startCol = col - (col % 3);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mat[i + startRow, j + startCol] == num)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool sudokuSolver(int[,] mat, int row, int col)
    {
        if (row == 8 && col == 9)
        {
            return true;
        }

        if (col == 9)
        {
            row++;
            col = 0;
        }

        if (mat[row, col] != 0)
        {
            return sudokuSolver(mat, row, col + 1);
        }

        for (int num = 1; num <= 9; num++)
        {
            if (isSafe(mat, row, col, num))
            {
                mat[row, col] = num;
                if (sudokuSolver(mat, row, col + 1))
                {
                    return true;
                }
                mat[row, col] = 0;
            }
        }

        return false;
    }

    public void checkSolved()
    {
        solved = true;

        for (int i = 0; i < solvedSudoku.GetLength(0); ++i)
        {
            for (int j = 0; j < solvedSudoku.GetLength(1); ++j)
            {
                if (rows[i].tiles[j].number == null) {
                    solved = false;
                }
                else if (solvedSudoku[i, j] != int.Parse(rows[i].tiles[j].number))
                {
                    solved = false;
                }
            }
        }

        /*
        if (solved)
        {
            Debug.Log("This puzzle has been solved!");
        }
        else
        {
            Debug.Log("This puzzle has not been solved :(");
        }
        */
        //return solved;
    }
}
