using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue", order = 1)]
public class DialoguePresent : ScriptableObject
{
    public string ID;

    public List<DialogueStep> dialogue;
}

[System.Serializable]
public class DialogueStep
{
    public Sprite background;
    public string personName;
    public string talk;
}
