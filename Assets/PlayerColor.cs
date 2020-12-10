using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerColor : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnColorChanged))]
    public Color PlayerCol;

    [Command]
    public void CmdSetupColor(Color playerColor)
    {
        PlayerCol = playerColor;
    }

    public void OnColorChanged(Color oldColor, Color newColor)
    {
        GetComponentInChildren<SpriteRenderer>().color = newColor;
    }
}