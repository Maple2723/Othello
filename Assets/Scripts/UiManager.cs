using Cysharp.Threading.Tasks;
using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI blackCountText;
    [SerializeField] private TextMeshProUGUI whiteCountText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI pathText;

    public static UiManager Instance { get; private set; }
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }

    void Update()
    {
        
    }

    public void UpdateStoneCountText()
    {
        string blackText = "";
        string whiteText = "";


        blackText = "<color=black><size=200%>●</size></color>";
        whiteText = "<color=white><size=200%>●</size></color>";

        if (GameManager.Instance.playerTurn == GameManager.TurnState.Black)
        {
            blackText += " <color=white><size=70%>YOU</size><size=60%> (Black)  </size></color> ";
            whiteText += " <color=white><size=70%>NPC</size><size=60%> (white)    </size></color>";
        }
        else
        {
            blackText += " <color=white><size=70%>NPC</size><size=60%> (Black)  </size></color> ";
            whiteText += " <color=white><size=70%>YOU</size><size=60%> (white)    </size></color>";
        }

        blackCountText.text = blackText+="<size=130%>"+GameManager.Instance.blackStoneCount.ToString()+"</size >";
        whiteCountText.text = whiteText+="<size=130%>"+GameManager.Instance.whiteStoneCount.ToString()+"</size >";
    }

    public void UpdateTurnText()
    {
        string enemyTurnColor = "";
        string playerTurnColor = "";

        if (GameManager.Instance.playerTurn == GameManager.TurnState.Black)
        {
            playerTurnColor = " <color=black><size=200%>●</size></color>";
            enemyTurnColor = " <color=white><size=200%>●</size></color>";
        }
        else
        {
            playerTurnColor = " <color=white><size=200%>●</size></color>";
            enemyTurnColor = " <color=black><size=200%>●</size></color>";
        }

        if (GameManager.Instance.currenTurn == GameManager.Instance.playerTurn)
        {
            turnText.text = playerTurnColor + "<size=120%>Your turn</size>";
        }
        else
        {
            turnText.text = enemyTurnColor + "<size=120%>NPC turn</size>";
        }
        
    }

    public async UniTaskVoid ShowPathTextAsync()
    {
        pathText.text = "Path";

        await UniTask.Delay(500);

        pathText.text = "";
    }
}
