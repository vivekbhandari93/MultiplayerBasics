using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{

    [SerializeField] private TextMeshProUGUI displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;
    

    [SyncVar(hook =nameof(HandleDisplayNameUpdated))] 
    [SerializeField] private string displayName = "Missing Name";
    
    [SyncVar(hook =nameof(HandleDisplayColorUpdated))] 
    [SerializeField] private Color displayColor = Color.black;


    #region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }
    
    private void HandleDisplayColorUpdated(Color oldColor , Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("My New Name");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion

}
