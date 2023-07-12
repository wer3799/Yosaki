using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class KingTrialSort : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;

    [SerializeField] private SeletableTab _seletableTab;
    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
    }

    void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).AsObservable().Subscribe(
            e =>
            {
                if (e > 0)
                {
                    for (int i = 0; i < e; i++)
                    {
                        _gameObjects[(int)i].transform.SetAsLastSibling();
                    }
                }

                if (e >= GameBalance.sinsunKingGraduate)
                {
                    _seletableTab.OnSelect(0);
                }
                else
                {
                    _seletableTab.OnSelect((int)e);
                }
            }).AddTo(this);
    }
}
