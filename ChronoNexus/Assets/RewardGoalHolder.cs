using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardGoalHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _materialText;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private TextMeshProUGUI _gunID;

    [SerializeField] private GameObject _moneyRew;
    [SerializeField] private GameObject _materialRew;
    [SerializeField] private GameObject _expRew;
    [SerializeField] private GameObject _gunRew;
    public void SetRewards(LevelData levelData)
    {

        if (levelData.moneyReward > 0)
        {
            _moneyRew.SetActive(true);
            _moneyText.text = levelData.moneyReward.ToString();
        }
        else
        {
            _moneyRew.SetActive(false);
        }

        if (levelData.materialReward > 0)
        {
            _materialRew.SetActive(true);
            _materialText.text = levelData.materialReward.ToString();
        }
        else
        {
            _materialRew.SetActive(false);
        }

        if (levelData.expReward > 0)
        {
            _expRew.SetActive(true);
            _expText.text = levelData.expReward.ToString();
        }
        else
        {
            _expRew.SetActive(false);
        }

        if (levelData.gunRewardID >= 0)
        {
            _gunRew.SetActive(true);
            _gunID.text = "1";
        }
        else
        {
            _gunRew.SetActive(false);
        }
    }
}
