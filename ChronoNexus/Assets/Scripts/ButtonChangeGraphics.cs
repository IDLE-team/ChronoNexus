using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonClickedSetting))]
public class ButtonChangeGraphics : MonoBehaviour
{
    private Button _buttonClicked;
    private ButtonClickedSetting _buttonSet;
    private void Start()
    {
        _buttonClicked = gameObject.GetComponent<Button>();
        _buttonClicked.onClick.AddListener(Clicked);
    }
    public void Clicked()
    {
        _buttonSet.Clicked();
        {
            // FUNCTION FOR GRAPHICS CHANGE // // ‘”Õ ÷»ﬂ ƒÀﬂ »«Ã≈Õ≈Õ»ﬂ √–¿‘€
        }
    }
}
