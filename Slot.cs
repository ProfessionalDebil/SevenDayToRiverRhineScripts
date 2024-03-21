using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    private bool isDisabled;
    public int index;
    private Image spriteContainer;
    private Slider sliderContainer;
    private Image sliderContainerFill;

    public void Awake() {
        if (spriteContainer == null) {
            GetImage();
        }
        if (sliderContainer == null) {
            GetSlider();
        }
    }
    public virtual void Disable() {
        isDisabled = true;
    }

    private void GetImage() {
        spriteContainer = GetComponentInChildren<Image>(true);
    }

    public void UpdateSlider(float newValue) {
        sliderContainer.value = newValue;
        OnSliderChanged(newValue);
    }

    private void GetSlider() {
        Debug.Log("A");
        sliderContainer = GetComponentInChildren<Slider>(true);
        sliderContainer.gameObject.SetActive(false);
        sliderContainerFill = sliderContainer.transform.Find("Fill Area/Fill").GetComponent<Image>();
    }

    public void SetActiveSlider(bool value) {
        sliderContainer.gameObject.SetActive(value);
    }

    public void OnSliderChanged(float newVal) {
        /*
        60-100%, green
        35-60%, yellow
        0-35%, red

        0, 255, 0
        0, 255, 255
        255, 0, 0
        */

        Color fillColor = new Color(0, 1f, 0);
        if (newVal <= 35f) {
            fillColor = new Color(1f, 0, 0);
        }
        else if (newVal <= 60f) {
            fillColor = new Color(0, 1f, 1f);
        }
        sliderContainerFill.color = fillColor;
    }

    public void SetIcon(Sprite newIcon) {
        spriteContainer.sprite = newIcon;
    }


    public bool IsDisabled() {
        return isDisabled;
    }

    public void Enable() {
        isDisabled = false;
    }

    public void _Disable() {
        isDisabled = true;
    }
}