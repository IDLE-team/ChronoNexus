using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonClickedSetting))]
public class ButtonChangeFPS : MonoBehaviour
{
    public int fpsNumber = 60;
    private Button _buttonClicked;
    private ButtonClickedSetting _buttonSet;
    private void Start()
    {
        _buttonSet = gameObject.GetComponent<ButtonClickedSetting>();
        _buttonClicked = gameObject.GetComponent<Button>();
        _buttonClicked.onClick.AddListener(Clicked);
    }
    public void Clicked()
    {
        Application.targetFrameRate = fpsNumber;
        _buttonSet.Clicked();
    }
}
