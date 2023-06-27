using UnityEngine;
using TMPro;

public class UIAttractorName : MonoBehaviour {

	[SerializeField]
	TextMeshProUGUI display;

    [SerializeField] private Swarm swarm;
    Swarm.AttractorName lastAttractor;

    void Awake() {
        display.SetText(((int)swarm.attractorName+1) + " - " + swarm.attractorName.ToString());
        lastAttractor = swarm.attractorName;
    }

    void Update() {
        if (swarm.attractorName != lastAttractor) {
            display.SetText(((int)swarm.attractorName+1) + " - " + swarm.attractorName.ToString());
            lastAttractor = swarm.attractorName;
        }
    }
}