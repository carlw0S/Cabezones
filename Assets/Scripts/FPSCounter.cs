using UnityEngine;
using UnityEngine.UI;
 
public class FPSCounter : MonoBehaviour
{
    private Text FPSCounterText;

    void Awake()
    {
        FPSCounterText = GetComponent<Text>();
    }

    void Start()
    {
        InvokeRepeating("ShowFPS", 0.5f, 0.5f);
    }

    private void ShowFPS()
    {
        int fps = (int)(1f / Time.unscaledDeltaTime);
        FPSCounterText.text = fps.ToString() + " FPS";
    }
}