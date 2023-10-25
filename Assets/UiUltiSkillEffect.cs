using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUltiSkillEffect : SingletonMono<UiUltiSkillEffect>
{
    [SerializeField]
    private List<GameObject> ultSkillEffect;

    [SerializeField] private List<Animator> _animators;

    public void ShowUltSkillEffect(int idx)
    {
        if (SettingData.showVisionSkill.Value == 0) return;

        if (PlayerMoveController.Instance.MoveDirection == MoveDirection.Left)
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }


        if (idx == 46)
        {
            var index = 0;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 47)
        {
            var index = 1;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 48)
        {
            var index = 2;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 49)
        {
            var index = 3;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 55)
        {
            var index = 4;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 95)
        {
            var index = 5;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 141)
        {
            var index = 6;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }


        if (idx == 142)
        {
            var index = 7;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 157)
        {
            var index = 8;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 158)
        {
            var index = 9;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 159)
        {
            var index = 10;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 160)
        {
            var index = 11;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 172)
        {
            var index = 12;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }

        if (idx == 173)
        {
            var index = 13;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }    
        
        if (idx == 180)
        {
            var index = 14;
            ultSkillEffect[index].SetActive(true);
            _animators[index].Play("UltSkill0", 0, 0f);
        }
    }

    public void OffAllUltSkillEffect()
    {
        for (var i = 0; i < ultSkillEffect.Count; i++)
        {
            ultSkillEffect[i].gameObject.SetActive(false);
        }
    }
}