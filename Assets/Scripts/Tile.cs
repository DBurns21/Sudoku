using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    public class State
    {
        public Color fillColor;
    }

    private Board board;

    public State state { get; private set; }
    public char number { get; private set; }
    public bool changeable { get; set; } = true;

    private TextMeshProUGUI text;
    private Image fill;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        fill = GetComponent<Image>();
        board = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>();
    }

    public void setNumber(char number)
    {
        this.number = number;
        text.text = number.ToString();
    }

    public void SetState(State state)
    {
        this.state = state;
        fill.color = state.fillColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // This code will execute when the UI Image is clicked
        Debug.Log("Key was clicked");
        
        board.keyPressed = text.text.ToLower();
        // Example: Call another function, change a variable, etc.
        // MyCustomAction(); 
    }
}
