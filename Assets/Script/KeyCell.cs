using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class KeyCell : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private KeyChange keyEvent;
    private char letter;
    private Image tile;

    void Start()
    {
        letter = GetComponentInChildren<Text>().text.ToCharArray()[0];
        tile = GetComponent<Image>();
        keyEvent.OnKeyChange += ReColor;
    }

    void ReColor()
    {
        ColorType ct = KeyManager.CheckKeyColor(letter);
        tile.sprite = sprites[(int)ct - 1];
    }
}
