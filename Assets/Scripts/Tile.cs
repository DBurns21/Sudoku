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
        public Color textColor;
        public bool changeable;
    }

    private Board board;

    public State state { get; private set; }
    public string number { get; private set; }
    private bool changeable;

    private TextMeshProUGUI text;
    private Image fill;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        fill = GetComponent<Image>();
        board = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<Board>();
    }

    public void setNumber(string number)
    {
        this.number = number;
        text.text = number.ToString();
    }

    public void SetState(State state)
    {
        this.state = state;
        fill.color = state.fillColor;
        text.color = state.textColor;
        changeable = state.changeable;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Tile " + text.text + " was clicked");
        board.selectTile(this); 
    }

    public bool isChangeble()
    {
        return changeable;
    }
}
