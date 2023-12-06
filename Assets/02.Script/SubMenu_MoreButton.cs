using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubMenu_MoreButton : MonoBehaviour
{
    public enum State
    {
        Opened, Closed
    }

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private Sprite openSprite;

    [SerializeField]
    private Sprite closeSprite;

    private ReactiveProperty<State> state = new ReactiveProperty<State>(State.Closed);

    [SerializeField]
    private GameObject expandObject;

    [SerializeField]
    private List<UiRedDotBase> redDotBase;

    //[SerializeField]
    //private RectTransform batterySafeObject;

    private void Awake()
    {
        expandObject.SetActive(false);

        Subscribe();
    }

    private void Subscribe()
    {
        state.AsObservable().Subscribe(WhenStateChanged).AddTo(this);
    }

    public void WhenStateChanged(State state)
    {
        expandObject.SetActive(state == State.Opened);
        icon.sprite = state == State.Opened ? closeSprite : openSprite;
        buttonText.SetText(state == State.Opened ? "닫기" : "메뉴");

        if (state == State.Opened)
        {
            redDotBase.ForEach(e => e?.GoTargetButton());
        }
        else
        {   
            redDotBase.ForEach(e => e?.GoMoreButton());
        }
    }

    public void OnClickExpandButton()
    {
        state.Value = state.Value == State.Opened ? State.Closed : State.Opened;
    }


}
