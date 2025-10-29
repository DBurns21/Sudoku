using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class key : MonoBehaviour, IPointerClickHandler
{

    private Board board;

    public char number { get; }

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        board = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // This code will execute when the UI Image is clicked
        //Debug.Log("Key " + text.text + " was clicked");
        if (board.currentTile != null)
        {
            board.keyPressed = text.text;
        } 
    }
}
