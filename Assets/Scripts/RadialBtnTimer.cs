using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;

/// <summary>
/// Radial timer disabling button while waiting
/// </summary>
public class RadialBtnTimer : MonoBehaviour
{
    [SerializeField] private Image radialImage;
    [SerializeField] private float radialSpeed = 1f;

    private static float t = 0.0f;
    private bool started = false;


    private MMTouchButton button;
private void Awake() {
    button = GetComponent<MMTouchButton>();
}

    /// <summary>
    /// Starts the radial timer
    /// </summary>
    public void StartTimer()
    {
        radialImage.fillAmount = 1f;
        t = 0f;
        started = true;
        button.DisableButton();
    }

    /// <summary>
    /// if timer over, enable button, otherwise increasinly show radial button
    /// </summary>
    public void Update()
    {
        if (started && radialImage.fillAmount > 0)
        {
            radialImage.fillAmount = Mathf.Lerp(1f, 0f, t);
            t += radialSpeed * Time.deltaTime;
        } else {
            button.EnableButton();
        }         
    }
}
