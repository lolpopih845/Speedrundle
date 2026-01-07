using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextAsset wordFile;
    [SerializeField] private TextAsset wordFileDup;
    [SerializeField] private LetterCell[] lcs;
    [SerializeField] private Text debugWordText;
    [SerializeField] private KeyChange keyEvent;
    [SerializeField] private Animator panel;
    [SerializeField] private Text title1;
    [SerializeField] private Text title2;
    [SerializeField] private Text title3;
    static string[] words;
    static string[] wordsfull;
    private int attempt = 0;
    private string selected_word;
    private LinkedList<char> inputted_word = new();
    private bool active = false;
    private bool debugWord;
    private void Awake()
    {
        words ??= wordFile.text.Split('\n');
        wordsfull ??= wordFileDup.text.Split('\n');

    }
    private IEnumerator Shuffling()
    {
        active = false;
        for (int i = 0; i < 30; i++)
        {
            lcs[attempt * 5 + i].ResetCell(true);
            selected_word = words[UnityEngine.Random.Range(0, words.Length)];
            debugWordText.text = "Answer: " + selected_word;
            yield return new WaitForSeconds(0.05f);
        }
        if (!debugWord) debugWordText.text = "Answer: DISABLE";
        active = true;
    }

    void TryInput(char key)
    {
        if (!active) return;
        if (inputted_word.Count < 5)
        {
            lcs[attempt * 5 + inputted_word.Count].ChangeLetter(key);
            inputted_word.AddLast(key);
        }
        else
        {
            lcs[attempt * 5 + inputted_word.Count - 1].Shake();
        }
    }

    void TrySubmit()
    {
        if (!active) return;
        if (inputted_word.Count < 5)
        {
            for (int i = 0; i < 5; i++)
            {
                lcs[attempt * 5 + i].Shake();
                // Popup Text
            }
        }
        else
        {
            string in_word = string.Join("", inputted_word).ToLower().Trim();
            for (int i = 0; i < wordsfull.Length; i++)
            {
                if (in_word.Equals(wordsfull[i].Trim()))
                {
                    StartCoroutine(CheckAnswer());
                    return;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                lcs[attempt * 5 + i].Shake();
                // Popup Text
            }
        }
    }

    void TryDel()
    {
        if (!active) return;
        if (inputted_word.Count > 0)
        {
            lcs[attempt * 5 + inputted_word.Count - 1].ResetCell();
            inputted_word.RemoveLast();
        }
    }
    void HandleWin()
    {
        attempt++;
        active = false;
        title1.text = "Lucky Boi!";
        title1.color = new Color(0.24f, 1, 0.24f, 1);
        title2.text = "Attempt Used:";
        title3.text = attempt.ToString() + "/6";
        panel.SetBool("active", true);
    }
    void HandleLose()
    {
        active = false;
        title1.text = "Bummer!";
        title1.color = new Color(1, 0.24f, 0.24f, 1);
        title2.text = "The word:";
        title3.text = selected_word;
        panel.SetBool("active", true);
    }

    IEnumerator CheckAnswer()
    {
        active = false;
        int score = 0;
        char[] word = selected_word.ToCharArray();
        for (int i = 0; i < 5; i++)
        {
            char curr_char = inputted_word.First.Value;
            for (int j = 0; j < 5; j++)
            {
                if (curr_char == char.ToUpper(word[j]))
                {
                    if (i == j)
                    {
                        score++;
                        KeyManager.AddGreen(curr_char);
                        lcs[attempt * 5 + i].Flip(ColorType.Green);
                        goto EndLoop;
                    }
                    else
                    {
                        KeyManager.AddYellow(curr_char);
                        lcs[attempt * 5 + i].Flip(ColorType.Yellow);
                        goto EndLoop;
                    }
                }
            }
            KeyManager.AddGrey(curr_char);
            lcs[attempt * 5 + i].Flip(ColorType.Grey);
        EndLoop:
            inputted_word.RemoveFirst();
            yield return new WaitForSeconds(0.4f);
            if (score >= 5) HandleWin();
        }
        keyEvent.ChangeKey();
        attempt++;
        active = true;
        if (attempt == 6) HandleLose();
    }

    public void OnKeyBoardPress(string key)
    {
        if (key == "Del") TryDel();
        else if (key == "Enter") TrySubmit();
        else TryInput(key[0]);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    public void Setup()
    {
        attempt = 0;
        panel.SetBool("active", false);
        KeyManager.ResetKey();
        keyEvent.ChangeKey();
        StartCoroutine(Shuffling());
    }

    public void ToggleDebugWord()
    {
        debugWord = !debugWord;
        if (debugWord) debugWordText.text = "Answer: " + selected_word;
        else debugWordText.text = "Answer: DISABLE";
    }
}
