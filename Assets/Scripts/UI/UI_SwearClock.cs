using UnityEngine;
using UnityEngine.UI;

public class UI_SwearClock : MonoBehaviour
{
    public Image image;
    private void Awake()
    {
        GenericMsg.AddReceiver
            (GenericSign.backMenu, ResetFill)
            (GenericSign.level_swear_end, ResetFill)
            (GenericSign.level_swear, ResetFill);
        GenericMsg<float>.AddReceiver(GenericSign.level_swear, UpdateFill);
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.backMenu, ResetFill)
            (GenericSign.level_swear_end, ResetFill)
            (GenericSign.level_swear, ResetFill);
        GenericMsg<float>.DelReceiver(GenericSign.level_swear, UpdateFill);
    }

    private void ResetFill()
    {
        image.fillAmount = 0;
    }
    private void UpdateFill(float fillAmount)
    {
        image.fillAmount = fillAmount / 100;
    }
}
