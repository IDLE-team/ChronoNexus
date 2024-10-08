using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelDescriptionHolder : MonoBehaviour
{
    [SerializeField] private Slider _difficultyFillSlider;

    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _levelDescription;

    [SerializeField] private RewardGoalHolder _rewardsDisplay;

    [SerializeField] private Button _startLevel;

    [SerializeField] private SceneLoader _sceneLoader;

    [SerializeField] private float _typeSpeed;

    private Tween typeWriter;
    private string _text;

    private void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    private void DisplayText(string textOld, string textNew, TextMeshProUGUI textMesh)
    {
        _text = textOld;

        typeWriter = DOTween.To(() => _text, x => _text = x, textNew, _typeSpeed).OnUpdate(() =>
        {
            textMesh.text = _text;
        });
    }

    public void DisplayData(LevelData levelData)
    {
        DisplayText(_levelName.text, levelData.levelName, _levelName);
        DisplayText(_levelDescription.text, levelData.levelDescription, _levelDescription);
        
        gameObject.transform.DOScale(Vector3.one, 0.4f);
   
        _rewardsDisplay.SetRewards(levelData);
        switch (levelData.difficulty)
        {
            case 1:
                _difficultyFillSlider.DOValue(1,0.4f).SetEase(Ease.InQuad);
                break;
            case 2:
                _difficultyFillSlider.DOValue(2, 0.4f).SetEase(Ease.InQuad);
                break;
            case 3:
                _difficultyFillSlider.DOValue(3, 0.4f).SetEase(Ease.InQuad);
                break;
            case 4:
                _difficultyFillSlider.DOValue(4, 0.4f).SetEase(Ease.InQuad);
                break;
            case 5:
                _difficultyFillSlider.DOValue(5, 0.4f).SetEase(Ease.InQuad);
                break;
        }

        _sceneLoader.SetScene(levelData.sceneStringName);

    }

    private void OnDisable()
    {
        gameObject.transform.DOScale(Vector3.zero, 0.3f);
    }


}
