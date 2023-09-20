using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Spine.Unity;
using CodeStage.AntiCheat.ObscuredTypes;

public class PlayerPet : SingletonMono<PlayerPet>
{
    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private Transform playerPos;

    private ObscuredFloat moveSpeed = 0f;

    private Transform target;

    private Dictionary<int, DropItem> dropItems;

    private ReactiveProperty<PetTableData> petTableData = new ReactiveProperty<PetTableData>();

    private CompositeDisposable petTimerDisposable = new CompositeDisposable();

    [SerializeField]
    private SkeletonAnimation skeletonAnimation;

    [SerializeField]
    private GameObject awakeEffect;

    private new void OnDestroy()
    {
        base.OnDestroy();
        petTimerDisposable.Dispose();
    }

    private new void Awake()
    {
        base.Awake();
        Initialize();
        Subscribe();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveRoutine());
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].AsObservable().Subscribe(e =>
        {
            //디폴트
            if (e == -1) return;

            WhenCostumeChanged(e);
            WhenPetEquipIdxChanged(e);
        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.petAwake].AsObservable().Subscribe(e =>
        {
            awakeEffect.SetActive(e == 1);
        }).AddTo(this);
    }

    private void WhenCostumeChanged(int idx)
    {
        skeletonAnimation.ClearState();
        skeletonAnimation.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];
        skeletonAnimation.Initialize(true);

        if (idx <= 14)
        {
            skeletonAnimation.transform.localScale = new Vector3(2.6f, 2.6f, 2.6f);
            skeletonAnimation.transform.localPosition = new Vector3(0.05f, -1.24f, 0f);
        }
        else if (idx == 15)
        {
            skeletonAnimation.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            skeletonAnimation.transform.localPosition = new Vector3(0.05f, 0.6f, 0f);
        }
        else if (idx == 16 || idx == 19)
        {
            skeletonAnimation.transform.localScale = new Vector3(0.42f, 0.42f, 0.42f);
            skeletonAnimation.transform.localPosition = new Vector3(0.26f, 0.58f, 0.0f);
        }
        else if (idx == 17)
        {
            skeletonAnimation.transform.localScale = new Vector3(0.65f, 0.65f, 0.45f);
            skeletonAnimation.transform.localPosition = new Vector3(0.3f, 0.58f, 0.0f);
        }
        //개
        else if (idx == 20)
        {
            skeletonAnimation.transform.localScale = new Vector3(-0.6f, 0.6f, 0.4f);
            skeletonAnimation.transform.localPosition = new Vector3(0.3f, 0.96f, 0.0f);
        }
        //고양이
        else if (idx == 21)
        {
            skeletonAnimation.transform.localScale = new Vector3(0.6f, 0.6f, 0.4f);
            skeletonAnimation.transform.localPosition = new Vector3(0.3f, 0.96f, 0.0f);
        }
        else if (idx >= 22 && idx <= 23)
        {
            skeletonAnimation.transform.localScale = new Vector3(0.6f, 0.6f, 0.4f);
            skeletonAnimation.transform.localPosition = new Vector3(0.3f, 0.96f, 0.0f);
        }
        else if (idx >= 24 && idx <= 27)
        {
            skeletonAnimation.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            skeletonAnimation.transform.localPosition = new Vector3(0.129f, -0.582f, 0.0f);
        }
        else if (idx >= 28 && idx <= 31)
        {
            skeletonAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
            skeletonAnimation.transform.localPosition = new Vector3(0.129f, 1.11f, 0.0f);
        }
        else if (idx >= 32 && idx <= 35)
        {
            skeletonAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
            skeletonAnimation.transform.localPosition = new Vector3(0.129f, 1.42f, 0.0f);
        }
        else if (idx >= 53 && idx <= 54)
        {
            skeletonAnimation.transform.localScale = new Vector3(2f, 2f, 1f);
            skeletonAnimation.transform.localPosition = new Vector3(0.129f, -0.05f, 0.0f);
        }
        else if (idx >= 55 && idx <= 56)
        {
            skeletonAnimation.transform.localScale = new Vector3(2f, 2f, 1f);
            skeletonAnimation.transform.localPosition = new Vector3(0.129f, 0.77f, 0.0f);
        }
        else
        {
            skeletonAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
            skeletonAnimation.transform.localPosition = new Vector3(0.129f, 1.42f, 0.0f);
        }
    }
    //

    public void WhenPetEquipIdxChanged(int idx)
    {
        petTableData.Value = TableManager.Instance.PetDatas[idx];

        var petServerData = ServerData.petTable.TableDatas[petTableData.Value.Stringid];

        moveSpeed = petTableData.Value.Movespeed;

        //미보유펫
        if (petServerData.hasItem.Value == 0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        //펫 보유중일때

        //무료펫
        if (petTableData.Value.Price == -1)
        {
            //시간이 남아있을때만
            if (petServerData.remainSec.Value > 0)
            {
                this.gameObject.SetActive(true);
                SubscribeFreePet();

                //타이머 시작
                StartFreePetTimer();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        //유료펫은 보유중이면 바로 활성화
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    private void StartFreePetTimer()
    {
        StopPetTimer();

        timerRoutine = StartCoroutine(FreePetTimerRoutine());
    }

    private void StopPetTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
        }
    }

    private Coroutine timerRoutine = null;

    private IEnumerator FreePetTimerRoutine()
    {
        WaitForSeconds ws = new WaitForSeconds(1.0f);
        int updateInterval = 5;
        int updateTick = 0;

        string key = TableManager.Instance.PetTable.dataArray[0].Stringid;
        var remainSec = ServerData.petTable.TableDatas[key].remainSec;

        while (remainSec.Value > 0)
        {
            yield return ws;
            remainSec.Value--;
            updateTick++;
#if UNITY_EDITOR
            Debug.LogError($"Free pet remain Sec {remainSec.Value}");
#endif
            if (updateTick >= updateInterval)
            {
                updateTick = 0;
                //서버 싱크
                ServerData.petTable.UpdateData(key);
            }
        }
    }


    private void SubscribeFreePet()
    {
        petTimerDisposable.Clear();

        var petServerData = ServerData.petTable.TableDatas[petTableData.Value.Stringid];
        petServerData.remainSec.AsObservable().Subscribe(WhenPetRemainSecDecrease).AddTo(petTimerDisposable);
    }

    private void WhenPetRemainSecDecrease(int remainSec)
    {
        if (remainSec == 0)
        {
            this.gameObject.SetActive(false);
            //
            PopupManager.Instance.ShowAlarmMessage("하수인이 들어갔습니다.");
        }
    }


    private void Initialize()
    {
        this.transform.parent = null;
        dropItems = BattleObjectManager.Instance.dropItemProperty.Pool.OutPool;
    }

    private void FindTarget()
    {
        var e = dropItems.GetEnumerator();

        float neariestDist = float.MaxValue;

        while (e.MoveNext())
        {
            float dist = Vector3.Distance(this.transform.position, e.Current.Value.transform.position);
            if (dist < neariestDist)
            {
                neariestDist = dist;
                target = e.Current.Value.transform;
            }
        }

    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            FindTarget();

            if (target == null || target.gameObject.activeInHierarchy == false)
            {
                this.transform.position = Vector2.Lerp(this.transform.position, targetPos.transform.position, Time.deltaTime * moveSpeed * 0.5f);

                if (playerPos.position.x > this.transform.position.x)
                {
                    this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
                }
                else
                {
                    this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
                }
            }
            else
            {
                Vector3 moveDir = target.transform.position - this.transform.position;

                this.transform.position += moveDir.normalized * Time.deltaTime * moveSpeed;

                if (moveDir.x > 0)
                {
                    this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
                }
                else
                {
                    this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
                }
            }

            yield return null;
        }
    }


}
