using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiTutorialAlarmObject : MonoBehaviour
{
    [SerializeField]
    private TutorialStep tutorialStep;

    public GameObject rootObject;

    private void Start()
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            if (rootObject != null)
            {
                rootObject.SetActive(false);
            }
        }
        else
        {
            Subscribe();
        }
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).AsObservable().Subscribe(e =>
        {
            var step = (TutorialStep)ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).Value;
            rootObject.SetActive(step == tutorialStep && UiTutorialManager.Instance.HasClearFlag(step) == false);
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialClearFlags).AsObservable().Subscribe(e =>
        {
            var step = (TutorialStep)ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).Value;
            rootObject.SetActive(step == tutorialStep && UiTutorialManager.Instance.HasClearFlag(step) == false);
        }).AddTo(this);
    }
}
