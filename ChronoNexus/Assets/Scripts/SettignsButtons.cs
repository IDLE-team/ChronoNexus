using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettignsButtons : MonoBehaviour
{
    private List<ButtonClickedSetting> buttons = new List<ButtonClickedSetting>();

    private void OnDrawGizmos()
    {
        buttons = GetComponentsInChildren<ButtonClickedSetting>().ToList();
    }
    private void Start()
    {
        if (buttons.Count <= 0)
        {
            buttons = GetComponentsInChildren<ButtonClickedSetting>().ToList();
        }
    }

    public bool RecolorAllButtons(bool clicked)
    {
        if (!clicked)
        {
            foreach (ButtonClickedSetting item in buttons)
            {
                item.PaintButton(false);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
