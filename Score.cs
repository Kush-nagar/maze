using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text text;

    public void SetText(int value){
        text.text = value.ToString();
    }
}
