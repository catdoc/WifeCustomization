using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendShapeSlider : MonoBehaviour
{
    Slider slider;
    [Header("别加后缀！！！")]
    public string BlendShapeName;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(value => WifeCustomization.Instance.ChangeBlendShapeValue(BlendShapeName, value));
    }
}
