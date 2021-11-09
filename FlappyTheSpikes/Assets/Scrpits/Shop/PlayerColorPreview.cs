using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorPreview : MonoBehaviour
{
    public Material playerMatAndTrail;
    Light playerEmmision;
    Color playerColor;

    void Start()
    {
        playerEmmision = GetComponentInChildren<Light>();

        playerColor = Color.magenta;

        if (playerEmmision != null)
        {
            if (playerColor == Color.black)
                playerEmmision.color = Color.white;
            else
                playerEmmision.color = playerColor;
        }
        if (playerMatAndTrail != null)
            playerMatAndTrail.SetColor("_EmissionColor", playerColor);
    }

    public void UpdatePreviewColor(Color theColor)
    {
        playerColor = theColor;

        if (playerEmmision != null)
        {
            if (playerColor == Color.black)
                playerEmmision.color = Color.white;
            else
                playerEmmision.color = playerColor;
        }
        if (playerMatAndTrail != null)
            playerMatAndTrail.SetColor("_EmissionColor", playerColor);
    }
}
