using TMPro;
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
    private string[] sudokusAnswers;
    private string currentSudoku;
    private string currentSudokuAnswer;
    //solvedSudoku no longer gets used since we use currentSudokuAnswer so either we need to remove one and make sure only one exists.
    private int[,] solvedSudoku = new int[9,9];

    public string keyPressed { get; set; } = null;

    [Header("Tile States")]
    public Tile.State hintState;
    public Tile.State emptyState;
    public Tile.State selectedState;
    public Tile.State sameNumberState;
    public Tile.State incorrectState;
    public Tile.State correctState;

    [Header("UI")]
    public Button CheckAnswerButton;
    public Button NewGameButton;
    public TextMeshProUGUI WinOrLossText;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
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
            if (keyPressed != null && currentTile.isChangeble())
            {
                WinOrLossText.gameObject.SetActive(false);
                currentTile.setNumber(keyPressed);
            }
        }
        keyPressed = null;
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("sudokus") as TextAsset;
        sudokus = textFile.text.Split('\n');
        textFile = Resources.Load("sudokusAnswers") as TextAsset;
        sudokusAnswers = textFile.text.Split('\n');
    }

    private void SetBoard()
    {
        WinOrLossText.gameObject.SetActive(false);
        int selected = UnityEngine.Random.Range(0, sudokus.Length);
        currentSudoku = sudokus[selected];
        currentSudoku = currentSudoku.Trim();
        currentSudokuAnswer = sudokusAnswers[selected];
        currentSudokuAnswer = currentSudokuAnswer.Trim();

        Debug.Log("Length of answer is " + currentSudokuAnswer.Length);
        Debug.Log("Current answer:\n" +
            currentSudokuAnswer[..9] + "\n" +
            currentSudokuAnswer[9..18] + "\n" +
            currentSudokuAnswer.Substring(18, 9) + "\n" +
            currentSudokuAnswer.Substring(27, 9) + "\n" +
            currentSudokuAnswer.Substring(36, 9) + "\n" +
            currentSudokuAnswer.Substring(45, 9) + "\n" +
            currentSudokuAnswer.Substring(54, 9) + "\n" +
            currentSudokuAnswer.Substring(63, 9) + "\n" +
            currentSudokuAnswer[72..]);
        /*
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
        */
        /*
        currentSudoku =
            "023597684" +
            "546318279" +
            "897246531" +
            "784962315" +
            "251783946" +
            "639451827" +
            "312675498" +
            "968134752" +
            "475829163";
        */

        int curr = 0;
        
        for (int j = 0; j < solvedSudoku.GetLength(0); j++)
        {
            for (int k = 0; k < solvedSudoku.GetLength(1); k++)
            {
                solvedSudoku[j, k] = currentSudoku[curr++] - '0';
            }
        }
        //Debug.Log("setting up board)");
        int i = 0;
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                if (currentSudoku[i] != '0')
                {
                    tile.setNumber(currentSudoku[i++].ToString());
                    tile.SetState(hintState);
                }
                else
                {
                    tile.SetState(emptyState);
                    i++;
                }
                
            }
        }
        /*
        Debug.Log("Board is set up");
        Debug.Log("starting to solve");
        //current solver is not fast enough so you sometimes have to wait 20 - 60 seconds in order for the board to be made.
        sudokuSolver(solvedSudoku, 0, 0);
        Debug.Log("Finished solving");
        */
        /*
        string answer = "";

        for (int j = 0; j < solvedSudoku.GetLength(0); j++)
        {
            for (int k = 0; k < solvedSudoku.GetLength(1); k++)
            {
                answer += solvedSudoku[j,k].ToString();
            }
        }
        */
    }

    public void newGame()
    {
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                tile.setNumber("");
                tile.SetState(emptyState);
            }
        }

        SetBoard();
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

    //This should not only check to see if the game is completed but also lock in any correct answers
    //It may also be good to change the state to show that it is a number that you had put in rather than one that you started with
    //May also be a good idea to change the state of the incorrect numbers like making the text red or something to show it should not go there.
    public void checkSolved()
    {
        solved = true;

        int curr = 0;
        for (int i = 0; i < solvedSudoku.GetLength(0); ++i)
        {
            for (int j = 0; j < solvedSudoku.GetLength(1); ++j)
            {
                if (rows[i].tiles[j].isChangeble())
                {
                    if (rows[i].tiles[j].number == null)
                    {
                        solved = false;
                    }
                    else if (rows[i].tiles[j].number != currentSudokuAnswer[curr].ToString())
                    {
                        rows[i].tiles[j].SetState(incorrectState);
                        solved = false;
                    }
                    else if (rows[i].tiles[j].number == currentSudokuAnswer[curr].ToString())
                    {
                        rows[i].tiles[j].SetState(correctState);
                    }
                }
                curr++;
            }
        }

                /*
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
                */

        if (solved)
        {
            WinOrLossText.text = ("Congrats you solved the puzzle!");
            WinOrLossText.color = Color.green;
            WinOrLossText.gameObject.SetActive(true);
            //Debug.Log("This puzzle has been solved!");
        }
        else
        {
            WinOrLossText.text = ("I'm sorry but this puzzle is incorrect. :'(");
            WinOrLossText.color = Color.red;
            WinOrLossText.gameObject.SetActive(true);
            //Debug.Log("This puzzle has not been solved :(");
        }
        
        //return solved;
    }
}
