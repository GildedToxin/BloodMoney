using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomerRequestUI : MonoBehaviour
{
    public List<GameObject> icons = new List<GameObject>();
    public List<string> textOptions = new List<string>();
    public TextMeshProUGUI text;
    public string currentText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // string currentText = textOptions[Random.Range(0, textOptions.Count)];
       // text.text = currentText;
    }

    public void SetText(string newText) {
        text.text = newText;
    }
    public string GetText(OrganType organ)
    {
        var text = "";
        var organString = organ.ToString().ToLower();

        if(organString == "blood") 
            organString = "blood vial";

        int rand = Random.Range(0, 5); // 0,1,2,3,4

        string ana = organString == "eye" ? "an" : "a"; 

        switch (rand)
        {
            case 0:
                text = $"I need {ana} {organString}… fresh";
                break;

            case 1:
                text = $"I’m here for {ana} {organString}";
                break;

            case 2:
                text = $"You got {ana} {organString} for me?";
                break;

            case 3:
                text = $"I need {ana} {organString}. Clean";
                break;

            case 4:
                text = $"One {organString}. No defects";
                break;
        }



        return text;
    }

    public void SetTextSell(OrganType organ)
    {
        var text = "";
        var organString = organ.ToString().ToLower();

        if (organString == "blood")
            organString = "blood vial";

        var itemData = Resources.Load<Item>($"items/{organ.ToString()}");
       

        text = $"Press E to sell the {organString} for ${itemData.price}";


        this.text.text = text;
    }
    public void GetSellText()
    {




    }
    public void SetIcon(OrganType organ)
    {

        foreach(var icon in icons)
        {
            icon.SetActive(false);
        }
        switch (organ)
        {
            case OrganType.Blood:
                icons[0].SetActive(true);
                break;
            case OrganType.Bone:
                icons[1].SetActive(true);
                break;
            case OrganType.Brain:
                icons[2].SetActive(true);
                break;
            case OrganType.Eye:
                icons[3].SetActive(true);
                break;
            case OrganType.Limb:
                icons[4].SetActive(true);
                break;
            case OrganType.Hand:
                icons[5].SetActive(true);
                break;
        }
    }
}
