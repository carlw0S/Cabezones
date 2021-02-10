using UnityEngine;
using UnityEngine.UI;
 
public class FPSCounter : MonoBehaviour
{
    private Text FPSCounterText;
    private float deltas = 0;
    private int count = 0;

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
        int fps = (int)(1f / (deltas / count));
        FPSCounterText.text = fps.ToString() + " FPS";
        deltas = 0f;
        count = 0;
    }

    void Update()
    {
        deltas += Time.unscaledDeltaTime;
        count += 1;
    }
}