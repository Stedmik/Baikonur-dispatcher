using UnityEngine;
using TMPro;

public class TMPSizeFit : MonoBehaviour
{
    TMP_Text text;
    RectTransform rectTransform;
    

    void Start()
    {
        text = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
        text.ForceMeshUpdate();
        float neededHight = text.preferredHeight;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, neededHight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
