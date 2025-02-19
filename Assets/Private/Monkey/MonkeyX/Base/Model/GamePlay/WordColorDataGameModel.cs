using System.Collections.Generic;
using UnityEngine;

public class WordColorDataGameModel
{
    public string Text { get; private set; }
    public string Highlight { get; private set; }

    public WordColorDataGameModel(string text, string highlight)
    {
        Text = text;
        Highlight = highlight;
    }
}
