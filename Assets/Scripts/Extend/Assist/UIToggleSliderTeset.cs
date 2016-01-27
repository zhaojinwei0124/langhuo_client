using UnityEngine;
using System.Collections;

public class UIToggleSliderTeset : MonoBehaviour {

    public GameObject target;
    public int setCount;

    void Start()
    {
        UIToggleSlider.AttachTo(target, setCount);
    }
}
