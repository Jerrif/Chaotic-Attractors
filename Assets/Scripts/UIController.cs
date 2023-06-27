using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour {

    [SerializeField] TextMeshProUGUI uiFrameRateCounter;
    [SerializeField] TextMeshProUGUI uiAttractorName;
    [SerializeField] TextMeshProUGUI uiControls;

    void OnToggleUi() {
        uiFrameRateCounter.enabled = !uiFrameRateCounter.enabled;
        uiAttractorName.enabled = !uiAttractorName.enabled;
        uiControls.enabled = !uiControls.enabled;
    }

}
