using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiQuickButtons : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> remainText;

    [SerializeField]
    private GameObject selectPopup;

    [SerializeField]
    private List<TextMeshProUGUI> selectText;

    [SerializeField]
    private Image currentSelectPotionIcon;

    [SerializeField]
    private List<Sprite> potionIcons;

    [SerializeField]
    private TextMeshProUGUI currentSelectPotionDescription;

    [SerializeField]
    private Image potionCoolTimeMask;

    public void ShowSelectPopup()
    {
        SoundManager.Instance.PlayButtonSound();
        selectPopup.SetActive(true);
        selectPopup.transform.parent = InGameCanvas.Instance.transform;
        selectPopup.transform.SetAsLastSibling();
    }

    private void Start()
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            
        }
        else
        {
            Subscribe();
            
        }
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Potion_0).AsObservable().Subscribe(e =>
        {
            if (ServerData.equipmentTable.TableDatas[EquipmentTable.Potion].Value == 0)
            {
                SetCurrentSelectPotionAmount((int)e);
            }

            remainText[0].SetText(e.ToString());
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Potion_1).AsObservable().Subscribe(e =>
        {
            if (ServerData.equipmentTable.TableDatas[EquipmentTable.Potion].Value == 1)
            {
                SetCurrentSelectPotionAmount((int)e);
            }

            remainText[1].SetText(e.ToString());

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Potion_2).AsObservable().Subscribe(e =>
        {
            if (ServerData.equipmentTable.TableDatas[EquipmentTable.Potion].Value == 2)
            {
                SetCurrentSelectPotionAmount((int)e);
            }

            remainText[2].SetText(e.ToString());
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.Potion].AsObservable().Subscribe(WhenSelectedIdxChanged).AddTo(this);

        //자동사용

        //플레이어 체력에 변화가 있을때
        PlayerStatusController.Instance.hp.AsObservable().Subscribe(currentHp =>
        {
            AutoPotion();
        }).AddTo(this);

        //자동사용 퍼센트 바뀌었을때
        SettingData.PotionUseHpOption.AsObservable().Subscribe(e =>
        {
            AutoPotion();
        }).AddTo(this);

        //포션 구매했을때
        ServerData.goodsTable.GetTableData(GoodsTable.Potion_0).AsObservable().Pairwise((pre, cur) => cur > pre).Subscribe(e => { AutoPotion(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.Potion_1).AsObservable().Pairwise((pre, cur) => cur > pre).Subscribe(e => { AutoPotion(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.Potion_2).AsObservable().Pairwise((pre, cur) => cur > pre).Subscribe(e => { AutoPotion(); }).AddTo(this);
    }

    private void AutoPotion()
    {
        if (GameManager.contentsType != GameManager.ContentsType.NormalField) return;

        float currentHp = (float)PlayerStatusController.Instance.hp.Value;
        float maxhp = (float)PlayerStatusController.Instance.maxHp.Value;
        float optionValue = GameBalance.potion_Option[SettingData.PotionUseHpOption.Value];
        float currentHpRatio = currentHp / maxhp;
        if (currentHpRatio < optionValue)
        {
            UsePotion(false);
        }
    }

    private void SetCurrentSelectPotionAmount(int amount)
    {
        currentSelectPotionDescription.SetText($"{amount}개");
    }

    public void WhenSelectedIdxChanged(int idx)
    {
        currentSelectPotionIcon.sprite = potionIcons[idx];

        int remainCount = 0;

        if (idx == 0)
        {
            remainCount = (int)ServerData.goodsTable.GetTableData(GoodsTable.Potion_0).Value;
        }
        else if (idx == 1)
        {
            remainCount = (int)ServerData.goodsTable.GetTableData(GoodsTable.Potion_1).Value;
        }
        else if (idx == 2)
        {
            remainCount = (int)ServerData.goodsTable.GetTableData(GoodsTable.Potion_2).Value;

        }

        SetCurrentSelectPotionAmount(remainCount);

        for (int i = 0; i < selectText.Count; i++)
        {
            if (i == idx)
            {
                selectText[i].SetText("등록됨");
                selectText[i].color = Color.white;
            }
            else
            {
                selectText[i].SetText("등록");
                selectText[i].color = Color.white;
            }
        }

    }

    public void OnClickSelect(int idx)
    {
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.Potion, idx);
    }

    public void UsePotion()
    {

        UsePotion(true);
    }

    public void UsePotion(bool showAlarmText)
    {
        if (GameManager.contentsType != GameManager.ContentsType.NormalField &&
            GameManager.contentsType != GameManager.ContentsType.FireFly)
        {
            if (showAlarmText)
            {
                PopupManager.Instance.ShowAlarmMessage("이곳에서는 뿌리식물을 사용할 수 없습니다.");
            }
            return;
        }

        if (potionDelay != null)
        {
            //포션 딜레이중
            return;
        }

        if (PlayerStatusController.Instance.IsPlayerDead())
        {
            if (showAlarmText)
            {
                PopupManager.Instance.ShowAlarmMessage("플레이어가 사망했습니다.");
            }
            return;
        }

        int currentSelectIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Potion].Value;

        if (HasPotion(currentSelectIdx) == false)
        {
            if (showAlarmText)
            {
                PopupManager.Instance.ShowAlarmMessage("뿌리식물이 부족합니다");
            }
            return;
        }

        potionDelay = StartCoroutine(PotionDelayRoutine());

        RecoverHpMp(currentSelectIdx);
    }

    private Coroutine potionDelay;
    private IEnumerator PotionDelayRoutine()
    {
        float tick = 0f;

        while (tick < GameBalance.potionUseDelay)
        {
            tick += Time.deltaTime;
            potionCoolTimeMask.fillAmount = 1f - tick / GameBalance.potionUseDelay;
            yield return null;
        }

        potionCoolTimeMask.fillAmount = 0f;
        potionDelay = null;

        //포션 딜레이 끝났을때
        AutoPotion();
    }

    private bool HasPotion(int idx)
    {
        int currentPotionAmount = 0;

        if (idx == 0)
        {
            currentPotionAmount = (int)ServerData.goodsTable.GetTableData(GoodsTable.Potion_0).Value;
        }
        else if (idx == 1)
        {
            currentPotionAmount = (int)ServerData.goodsTable.GetTableData(GoodsTable.Potion_1).Value;
        }
        else
        {
            currentPotionAmount = (int)ServerData.goodsTable.GetTableData(GoodsTable.Potion_2).Value;
        }

        return currentPotionAmount > 0;
    }

    private static string PotionUseSoundKey = "Potion";
    private void RecoverHpMp(int idx)
    {
        SoundManager.Instance.PlaySound(PotionUseSoundKey);

        float recoverAmount = PotionBalance.recover_Potion[idx];

        float hpRecoverAmount = (float)PlayerStatusController.Instance.maxHp.Value * recoverAmount;
        float mpRecoverAmount = (float)PlayerStatusController.Instance.maxMp.Value * recoverAmount;

        PlayerStatusController.Instance.UpdateHp(hpRecoverAmount);
        PlayerStatusController.Instance.UpdateMp(mpRecoverAmount);

        //포션 차감
        ServerData.goodsTable.GetTableData(GoodsTable.GetPosionKey(idx)).Value--;
    }
}
