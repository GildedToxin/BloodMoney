using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class LimbEndScreen : MonoBehaviour
{
    public List<GameObject> limbs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(float score)
    {
        int count = 0;

        foreach(GameObject limb in limbs)
        {
            limb.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(score).ToString() + "%";
            count++;
        }
    }
}
