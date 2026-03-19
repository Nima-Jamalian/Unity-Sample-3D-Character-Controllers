using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ballScoreText,GreenCubeText,YellowCubeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBallScoreUI(int score)
    {
        ballScoreText.text = score.ToString();
    }

    public void UpdateGreenCubeScoreUI(int score)
    {
        GreenCubeText.text = score.ToString();
    }

    public void UpdateYellowCubeScoreUI(int score)
    {
        YellowCubeText.text = score.ToString();
    }
}
