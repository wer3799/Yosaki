using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Serialization;

public class UiReArrangePass : MonoBehaviour
{
    private enum CombinedPassList
    {
        GoodsPass,
        AdvancedGoodsPass,
        GrowthPass,
        FoxPass,
        PetPass,
        TrainingPass,
    }

    private enum PassList
    {
        SoulForestPass,
        SwordPass,
        PeachPass,
        GuimoonPass,
        SoulPass,
        MeditationPass,
        SealSwordEvolutionPass,
        suhoPass,
        FoxFirePass,
        SealSwordPass,
        DosulPass,
    }
    
    [SerializeField] private CombinedPassList combinedPassList;

    
    
    private void OnEnable()
    {
        this.gameObject.SetActive(RewardCheck());
    }

    private bool RewardCheck()
    {
        switch(combinedPassList)
        {
            case CombinedPassList.GoodsPass:
                return IsReceivablePassReward(PassList.SoulForestPass) ||
                       IsReceivablePassReward(PassList.SwordPass) ||
                       IsReceivablePassReward(PassList.PeachPass);
            case CombinedPassList.AdvancedGoodsPass:
                return IsReceivablePassReward(PassList.GuimoonPass) ||
                       IsReceivablePassReward(PassList.SoulPass) ||
                       IsReceivablePassReward(PassList.MeditationPass)||
                       IsReceivablePassReward(PassList.SealSwordEvolutionPass);            
            case CombinedPassList.GrowthPass:
                break;
            case CombinedPassList.FoxPass:
                break;
            case CombinedPassList.PetPass:
                break;
            case CombinedPassList.TrainingPass:
                return IsReceivablePassReward(PassList.suhoPass) ||
                       IsReceivablePassReward(PassList.FoxFirePass) ||
                       IsReceivablePassReward(PassList.SealSwordPass)||
                       IsReceivablePassReward(PassList.DosulPass);   
            default:
                return true;
        }
        return true;
    }
    public List<int> GetSplitData(string[] key)
    {
        List<int> returnValues = new List<int>();

        //var splits = ServerData.coldSeasonPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < key.Length; i++)
        {
            if (int.TryParse(key[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }
    private bool IsReceivablePassReward(PassList passList)
    {
        var adRewardCount = -1;
        var freeRewardCount = -1;
        var totalRewardCount = 0;
        var buyPass = false;
        switch (passList)
        {
            case PassList.SoulForestPass:
                totalRewardCount = TableManager.Instance.SoulForestPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiSoulForestPassBuyButton.seasonPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.SoulForestdAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.SoulForestFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.SwordPass:
                totalRewardCount = TableManager.Instance.SwordPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiSwordPassBuyButton.seasonPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.SwordAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.SwordFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.PeachPass:
                
                totalRewardCount = TableManager.Instance.PeachPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiPeachPassBuyButton.seasonPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.GuimoonPass:
                totalRewardCount = TableManager.Instance.GuimoonPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiGuimoonPassBuyButton.seasonPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.guimoonAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.guimoonFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.SoulPass:
                totalRewardCount = TableManager.Instance.SoulPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiSoulPassBuyButton.seasonPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;            
            case PassList.MeditationPass:
                totalRewardCount = TableManager.Instance.MeditationPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiMeditationPassBuyButton.seasonPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.meditationAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.meditationFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;            
            case PassList.SealSwordEvolutionPass:
                totalRewardCount = TableManager.Instance.SealSwordEvolutionPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiSealSwordEvolutionPassBuyButton.seasonPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.sealswordEvolutionAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.sealswordEvolutionFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.suhoPass:
                totalRewardCount = TableManager.Instance.suhoPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[UiSuhoPassBuyButton.suhoPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.suhoAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.suhoFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.FoxFirePass:
                totalRewardCount = TableManager.Instance.FoxFirePass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[PassBuyButton.foxfirePassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.foxfireAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.foxfireFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.SealSwordPass:
                totalRewardCount = TableManager.Instance.SealSwordPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[PassBuyButton.sealswordPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.sealSwordAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.sealSwordFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            case PassList.DosulPass:
                totalRewardCount = TableManager.Instance.DosulPass.dataArray.Length;
                
                buyPass = ServerData.iapServerTable.TableDatas[PassBuyButton.dosulPassKey].buyCount.Value > 0;
                if (buyPass)
                {
                    adRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.dosulAd].Value.Split(',').Length;

                    //받을게 있으면 true
                    if (totalRewardCount >= adRewardCount)
                    {
                        return true;
                    }
                }
                freeRewardCount = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.dosulFree].Value.Split(',').Length;
                //받을게 있으면 true
                if (totalRewardCount >= freeRewardCount)
                {
                    return true;
                }
                break;
            default:
                return true;
        }

        return false;
    }
}
