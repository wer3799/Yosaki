using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using static NetworkManager;
using Photon.Pun;

public class PartyRaidOtherPlayerView_Ingame : MonoBehaviour
{
    [SerializeField]
    private List<UiTopRankerCell> playerView_Room;

    [SerializeField]
    private List<Transform> flippedObject;

    [SerializeField]
    private float yOffset;

    private Dictionary<int, PlayerInfo> roomPlayerDatas;
    private List<int> keys;

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        roomPlayerDatas = PartyRaidManager.Instance.NetworkManager.RoomPlayerDatas;

        keys = roomPlayerDatas.Keys.ToList();

        for (int i = 0; i < playerView_Room.Count; i++)
        {
            if (i < keys.Count)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber != keys[i])
                {
                    playerView_Room[i].gameObject.SetActive(true);

                    var playerInfo = roomPlayerDatas[keys[i]];

                    playerView_Room[i].Initialize(playerInfo.nickName, string.Empty, playerInfo.costumeId, playerInfo.petId, playerInfo.weaponId, playerInfo.magicBookId, playerInfo.gumgi, playerInfo.guildName, playerInfo.mask, playerInfo.horn,playerInfo.suhoAnimal,playerInfo.guildIcon);
                }
                else
                {
                    playerView_Room[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerView_Room[i].gameObject.SetActive(false);
            }
        }
    }

    private float teleportDistance = 5f;

    private void Update()
    {
        if (SettingData.showOtherPlayer.Value == 1)
        {
            for (int i = 0; i < playerView_Room.Count; i++)
            {
                if (i < roomPlayerDatas.Count)
                {
                    if (PhotonNetwork.LocalPlayer.ActorNumber != keys[i])
                    {
                        playerView_Room[i].gameObject.SetActive(true);

                        Vector3 playerServerPos = roomPlayerDatas[keys[i]].currentPos + Vector3.down * yOffset;

                        float distance = Vector3.Distance(playerView_Room[i].transform.position, playerServerPos);

                        if (distance < teleportDistance)
                        {
                            playerView_Room[i].transform.position = Vector3.Lerp(playerView_Room[i].transform.position, playerServerPos, Time.deltaTime * 3f);
                        }
                        else
                        {
                            playerView_Room[i].transform.position = playerServerPos;
                        }


                        if (roomPlayerDatas[keys[i]].currentPos.x >= playerView_Room[i].transform.position.x)
                        {
                            flippedObject[i].transform.localScale = new Vector3(-Mathf.Abs(flippedObject[i].transform.localScale.x), flippedObject[i].transform.localScale.y, flippedObject[i].transform.localScale.z);
                        }
                        else
                        {
                            flippedObject[i].transform.localScale = new Vector3(Mathf.Abs(flippedObject[i].transform.localScale.x), flippedObject[i].transform.localScale.y, flippedObject[i].transform.localScale.z);
                        }

                        if (roomPlayerDatas[keys[i]].endGame || roomPlayerDatas[keys[i]].retireGame)
                        {
                            playerView_Room[i].gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        playerView_Room[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    playerView_Room[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < playerView_Room.Count; i++)
            {
                if (playerView_Room[i].gameObject.activeInHierarchy == true)
                {
                    playerView_Room[i].gameObject.SetActive(false);
                }
            }
        }


    }

}
