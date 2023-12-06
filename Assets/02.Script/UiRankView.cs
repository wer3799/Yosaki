using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiRankView : MonoBehaviour
{
    public enum RankType
    {
        Normal, GangChul, ChunMa
    }

    [SerializeField]
    private TextMeshProUGUI text1;

    [SerializeField]
    private TextMeshProUGUI text2;

    [SerializeField]
    private TextMeshProUGUI text3;

    [SerializeField]
    private TextMeshProUGUI guildName;

    [SerializeField]
    private List<GameObject> rankList;

    public static int rank1Count = 0;

    public void Initialize(string text1, string text2, string text3, int rank, int costumeId, int petId, int weaponId, int magicBookId, int gumgiIdx, string guildName, int maskIdx, int hornIdx,int suhoAnimal,RankType rankType = RankType.Normal)
    {
        this.text1.SetText(text1);
        this.text2.SetText(text2);
        this.text3.SetText(text3);

        this.guildName.gameObject.SetActive(string.IsNullOrEmpty(guildName) == false);
        this.guildName.SetText($"({guildName})");

        this.text1.gameObject.SetActive(rank != 1 && rank != 2 && rank != 3);

        rankList[0].SetActive(rank == 1);
        rankList[1].SetActive(rank == 2);
        rankList[2].SetActive(rank == 3);


        if (rankType == RankType.Normal)
        {
            UiTopRankerCell rankerCell = null;

            if (rank == 1)
            {
                if (rank1Count == 0)
                {
                    rankerCell = UiTopRankerView.Instance.RankerCellList[0];
                }
                else if (rank1Count == 1)
                {
                    rankerCell = UiTopRankerView.Instance.RankerCellList[1];
                }
                else if (rank1Count == 2)
                {
                    rankerCell = UiTopRankerView.Instance.RankerCellList[2];
                }

                rank1Count++;
            }
            else if (rank == 2)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[1];
            }
            else if (rank == 3)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[2];
            }
            else if (rank == 4)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[3];
            }
            else if (rank == 5)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[4];
            }
            else if (rank == 6)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[5];
            }
            else if (rank == 7)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[6];
            }
            else if (rank == 8)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[7];
            }
            else if (rank == 9)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[8];
            }
            else if (rank == 10)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[9];
            }
            else if (rank == 11)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[10];
            }
            else if (rank == 12)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[11];
            }
            else if (rank == 13)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[12];
            }
            else if (rank == 14)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[13];
            }
            else if (rank == 15)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[14];
            }

            if (rankerCell != null)
            {
                rankerCell.gameObject.SetActive(true);
                rankerCell.Initialize(text2, text3, costumeId, petId, weaponId, magicBookId, gumgiIdx, guildName, maskIdx, hornIdx,suhoAnimal,-1);
            }
        }
        else if (rankType == RankType.GangChul)
        {
            UiTopRankerCell rankerCell = null;

            if (rank == 1)
            {
                if (rank1Count == 0)
                {
                    rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[0];
                }
                else if (rank1Count == 1)
                {
                    rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[1];
                }
                else if (rank1Count == 2)
                {
                    rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[2];
                }

                rank1Count++;
            }
            else if (rank == 2)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[1];
            }
            else if (rank == 3)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[2];
            }
            else if (rank == 4)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[3];
            }
            else if (rank == 5)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[4];
            }
            else if (rank == 6)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[5];
            }
            else if (rank == 7)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[6];
            }
            else if (rank == 8)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[7];
            }
            else if (rank == 9)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[8];
            }
            else if (rank == 10)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[9];
            }
            else if (rank == 11)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[10];
            }
            else if (rank == 12)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[11];
            }
            else if (rank == 13)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[12];
            }
            else if (rank == 14)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[13];
            }
            else if (rank == 15)
            {
                rankerCell = UiTopRankerView_GangChul.Instance.RankerCellList[14];
            }

            if (rankerCell != null)
            {
                rankerCell.gameObject.SetActive(true);
                rankerCell.Initialize(text2, text3, costumeId, petId, weaponId, magicBookId, gumgiIdx, guildName, maskIdx, hornIdx,suhoAnimal,-1);
            }
        }
        else if (rankType == RankType.ChunMa)
        {
            UiTopRankerCell rankerCell = null;

            if (rank == 1)
            {
                if (rank1Count == 0)
                {
                    rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[0];
                }
                else if (rank1Count == 1)
                {
                    rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[1];
                }
                else if (rank1Count == 2)
                {
                    rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[2];
                }
                else if (rank1Count == 4)
                {
                    rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[3];
                }

                rank1Count++;
            }
            else if (rank == 2)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[1];
            }
            else if (rank == 3)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[2];
            }
            else if (rank == 4)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[3];
            }
            else if (rank == 5)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[4];
            }
            else if (rank == 6)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[5];
            }
            else if (rank == 7)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[6];
            }
            else if (rank == 8)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[7];
            }
            else if (rank == 9)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[8];
            }
            else if (rank == 10)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[9];
            }
            else if (rank == 11)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[10];
            }
            else if (rank == 12)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[11];
            }
            else if (rank == 13)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[12];
            }
            else if (rank == 14)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[13];
            }
            else if (rank == 15)
            {
                rankerCell = UiTopRankerView_Chunma.Instance.RankerCellList[14];
            }

            if (rankerCell != null)
            {
                rankerCell.gameObject.SetActive(true);
                rankerCell.Initialize(text2, text3, costumeId, petId, weaponId, magicBookId, gumgiIdx, guildName, maskIdx,hornIdx,suhoAnimal,-1);
            }
        }


    }
}
