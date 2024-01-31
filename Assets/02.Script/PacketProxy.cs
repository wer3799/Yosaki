using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketProxy : SingletonMono<PacketProxy>
{
    private new void Awake()
    {
        base.Awake();
        if (SendQueue.IsInitialize == false)
        {
            // SendQueue 초기화
            SendQueue.StartSendQueue(true, ExceptionHandler);
        }
    }
    private void Update()
    {
        SendQueue.Poll();        
        Backend.AsyncPoll();

    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            // 게임이 Pause 되었을 때
            Debug.Log("SendQueuePause");
            SendQueue.PauseSendQueue();
        }
        else if (pause == false)
        {
            Debug.Log("SendQueueResume");
            // 게임이 다시 진행 되었을 때
            SendQueue.ResumeSendQueue();
        }
    }

    void ExceptionHandler(Exception e)
    {
        Debug.LogError($"Packet proxy error {e}");
        // 예외 처리,
    }

    private void OnApplicationQuit()
    {
        Debug.LogError("StopSendQueue");
        SendQueue.StopSendQueue();
    }
}
