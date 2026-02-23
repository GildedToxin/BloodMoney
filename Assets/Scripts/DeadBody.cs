using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class DeadBody : MonoBehaviour, IPlayerLookTarget
{
    public bool IsBloodHarvested { get; set; }
    public bool IsBloodHighlighted { get; set; }

    public bool IsBrainHarvested { get; set; }
    public bool IsBrainHighlighted { get; set; }

    public bool IsLimbsHarvested { get; set; }
    public bool IsLimbsHighlighted { get; set; }

    public bool IsFingersHarvested { get; set; }
    public bool IsFingersHighlighted { get; set; }

    public bool IsEyesHarvested { get; set; }
    public bool IsEyesHighlighted { get; set; }

    public bool IsBonesHarvested { get; set; }
    public bool IsBonesHighlighted { get; set; }

    public GameObject Brain;
    public GameObject[] Limbs;
    public GameObject[] Bones;
    public GameObject Blood;
    public GameObject[] Fingers;
    public GameObject[] Eyes;


    public GameObject brainPrefab;
    public Transform brainSpawnLocation;

    public GameObject headTop;

    public List<GameObject> limbPrefabs;
    public List<Transform> limbSpawnLocations;

    public bool isLookedAt = false;

    public GameObject BodyBrain;
    public GameObject[] BodyLimbs;

    public int limbsHarvested = 0;
    public int handsHarvested = 0;


    public GameObject bloodPrefab;
    public Transform bloodSpawnLocation;


    public bool hasShownScreen = false;
    public bool hasShownScreen2 = false;

    public void Update()
    {
        if (isLookedAt)
        {
            string organ = GameManager.Instance.GetOrganFromSlot(FindAnyObjectByType<InventoryController>().selectedIndex.Value - 1);
            if(FindAnyObjectByType<HUDManager>() ==null)
                return;

            if (organ == "")
                FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
            else
                FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);

            FindAnyObjectByType<HUDManager>().UpdateCrossHairText($"Press E to Harvest {organ}");
            if (Input.GetKeyDown(KeyCode.E))
                GameManager.Instance.StartMiniGame();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.currentDay == 0 && !hasShownScreen && FindAnyObjectByType<FirstDayManager>().currentScreen == 8)
        {
            var fdm = FindAnyObjectByType<FirstDayManager>();
            fdm.currentScreen++;
            fdm.isShowingScreen = true;
            fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
            hasShownScreen = true;
            StartCoroutine(fdm.WaitForNextScreen());
        }
    }
    public void Highlight(string organ)
    {
        RemoveHighlight();
        switch (organ) {
            case "Limbs":
                IsLimbsHighlighted = true;
                foreach(GameObject limb in Limbs)
                {
                    limb.SetActive(true);
                    limb.layer= LayerMask.NameToLayer("Highlight");
                }
                break;
            case "Bones":
                IsBonesHighlighted = true;
                break;
            case "Blood":
                IsBloodHighlighted = true;
                    Blood.layer = LayerMask.NameToLayer("Highlight");
                break;
            case "Brain":
                IsBrainHighlighted = true;
                    Brain.layer = LayerMask.NameToLayer("Highlight");
                break;
            case "Fingers":
                IsFingersHighlighted = true;
                break;
            case "Eyes":
                IsEyesHighlighted = true;
                break;

        }
    }
    public void SpawnOrgan(string sceneOrgan)
    {
        switch (sceneOrgan)
        {
            case "LimbMiniGame":

                Destroy(BodyLimbs[limbsHarvested * 2]);
                Destroy(BodyLimbs[(limbsHarvested * 2) + 1]);

                Instantiate(limbPrefabs[limbsHarvested], limbSpawnLocations[limbsHarvested].position, limbSpawnLocations[limbsHarvested].rotation);
                break;
            case "Bones":
                //IsBonesHighlighted = true;
                break;
            case "BloodMiniGame":
                Instantiate(bloodPrefab, bloodSpawnLocation.position, bloodSpawnLocation.rotation);
                break;
            case "SkullMinigame":
                Instantiate(brainPrefab, brainSpawnLocation.position, brainSpawnLocation.rotation);
                Destroy(BodyBrain);
                Destroy(headTop);
                break;
            case "Fingers":
                //IsFingersHighlighted = true;
                break;
            case "Eyes":
                //IsEyesHighlighted = true;
                break;

        }

        if (GameManager.Instance.currentDay == 0 && !hasShownScreen2 && FindAnyObjectByType<FirstDayManager>().currentScreen == 10)
        {
            var fdm = FindAnyObjectByType<FirstDayManager>();
            fdm.currentScreen++; // 11
            fdm.isShowingScreen = true;
            fdm.tutorialScreens[fdm.currentScreen].SetActive(true);
            hasShownScreen2 = true;
            StartCoroutine(fdm.WaitForNextScreen());
        }
    }
    public void RemoveHighlight()
    {
        IsLimbsHighlighted = false;
        IsFingersHighlighted = false;
        IsEyesHighlighted = false;
        IsBrainHighlighted = false;
        IsBonesHighlighted = false;
        IsBloodHighlighted = false;

        foreach (GameObject limb in Limbs)
        {
            limb.layer = LayerMask.NameToLayer("Default");
            limb.SetActive(false);
        }


        Brain.layer = LayerMask.NameToLayer("Default");
        Blood.layer = LayerMask.NameToLayer("Default");
    }

    public void OnLookEnter()
    {
        isLookedAt = true;
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);
    }
    public void OnLookExit()
    {
        isLookedAt = false;
        FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
    }
}
