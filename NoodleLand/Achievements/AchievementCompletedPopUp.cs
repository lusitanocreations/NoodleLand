using System;
using System.Collections;
using NoodleLand.Data.Achievements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoodleLand.Achievements
{
  public class AchievementCompletedPopUp : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI achievementNameText;
    [SerializeField] private Image achievementIconImage;
    private Vector3 originalPos;
    [SerializeField] private Vector3 moveTo;


    private void Start()
    {
      RectTransform tr = (RectTransform) transform;

      originalPos = tr.anchoredPosition3D;

    }

    void Animate()
    {
      RectTransform tr = (RectTransform) transform;

      LeanTween.move(tr, moveTo, 1).setOnComplete(() =>
      {
        StartCoroutine(MoveBack(tr));
      });
    }

    IEnumerator MoveBack(RectTransform tr)
    {
      yield return new WaitForSeconds(3f);
      LeanTween.move(tr, originalPos, 3f).setOnComplete(() =>
      {
        gameObject.SetActive(false);
      });
    }
    public void Set(Achievement achievement)
    {
      gameObject.SetActive(true);
      this.achievementNameText.text = achievement.AchievementName;
      this.achievementIconImage.sprite = achievement.Image;
      Animate();  
    }
    
    
  }
}
