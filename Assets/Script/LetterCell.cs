using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LetterCell : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private char letter = '0';
    private Text text;
    private Image tile;
    private Animator animtor;
    void Start()
    {
        text = GetComponentInChildren<Text>();
        tile = GetComponentInChildren<Image>();
        animtor = GetComponent<Animator>();
        ResetCell();
    }

    public void ChangeLetter(char l)
    {
        letter = l;
        text.text = letter.ToString();
        ChangeTile(ColorType.White);
        animtor.SetTrigger("Pop");
    }

    public void Flip(ColorType ct)
    {
        StartCoroutine(ChangeTile(ct, 0.4f));
        animtor.SetTrigger("Flip");
    }

    public void Shake()
    {
        animtor.SetTrigger("Shake");
    }

    public void ResetCell(bool flip = false)
    {
        if (flip)
        {
            // animtor.SetTrigger("Pop");
            animtor.SetTrigger("Flip");
        }
        ResetLetter();
    }

    void ResetLetter()
    {
        text.text = "";
        ChangeTile(ColorType.Null);
    }


    void ChangeTile(ColorType ct)
    {
        tile.sprite = sprites[(int)ct];
    }

    private IEnumerator ChangeTile(ColorType ct, float delay)
    {
        yield return new WaitForSeconds(delay);
        tile.sprite = sprites[(int)ct];
    }

}
