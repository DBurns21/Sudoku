using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color fillColor;
    }

    public State state { get; private set; }
    public char number { get; private set; }
    public bool changeable { get; private set; }

    private TextMeshProUGUI text;
    private Image fill;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        fill = GetComponent<Image>();
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
}
