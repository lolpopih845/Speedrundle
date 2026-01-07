using System;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyChange", menuName = "Scriptable Objects/KeyChange")]
public class KeyChange : ScriptableObject
{
    public event Action OnKeyChange;

    public void ChangeKey()
    {
        OnKeyChange?.Invoke();
    }
}
