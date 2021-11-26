using UnityEngine;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] RectTransform contentView;
    [SerializeField] int linesToIncreaseContent;
    private int linesCreated = 0;

    public void IncreaseContentSize()
    {
        contentView.sizeDelta += new Vector2(0, 300f);
    }

    public void ParseLineOnConsole(string debugLine)
    {
        if(linesCreated < linesToIncreaseContent)
            linesCreated++;
        else
        {
            IncreaseContentSize();
            linesCreated = 0;
        }

        mainText.text += debugLine + '\n' + '\n';
    }
}