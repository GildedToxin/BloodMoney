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
    public GameObject[] Hands;
    public GameObject[] BodyHands;

    public List<GameObject> handPrefabs;
    public List<Transform> handSpawnLocations;

    public GameObject bloodPrefab;
    public Transform bloodSpawnLocation;

    public GameObject eyePrefab;
    public Transform rightEyeSpawnLocation;
    public Transform leftEyeSpawnLocation;

    public int eyesHarvested = 0;


    public bool hasShownScreen = false;
    public bool hasShownScreen2 = false;

    public GameObject BonePrefab;
    public Transform BoneSpawnLocation;
    public GameObject Chest;




    public void Update()
    {
        if (isLookedAt)
        {
            string organ = GameManager.Instance.GetOrganFromSlot(FindAnyObjectByType<InventoryController>().selectedIndex.Value - 1);
            if(FindAnyObjectByType<HUDManager>() ==null)
                return;

            if (organ == "")
                FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
            else
                FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);

            FindAnyObjectByType<HUDManager>().UpdateCrossHairText($"Press E to harvest {organ}");
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
        if(GameManager.Instance.isInHotelRoom == false)
            return;

        switch (organ) {
            case "Limbs":
                IsLimbsHighlighted = true;
                foreach(GameObject limb in Limbs)
                {
                    if (limb == null) continue;

                    limb.SetActive(true);
                    limb.layer= LayerMask.NameToLayer("Highlight");
                }
                break;
            case "Bones":
                IsBonesHighlighted = true;
                foreach (GameObject bone in Bones)
                {
                    bone.SetActive(true);
                    bone.layer = LayerMask.NameToLayer("Highlight");
                }
                break;
            case "Blood":
                IsBloodHighlighted = true;
                    Blood.layer = LayerMask.NameToLayer("Highlight");
                break;
            case "Brain":
                IsBrainHighlighted = true;
                    Brain.layer = LayerMask.NameToLayer("Highlight");
                break;
            case "Hands":
                foreach (GameObject hand in Hands)
                {
                    if (hand == null) continue;

                    hand.SetActive(true);
                    hand.layer = LayerMask.NameToLayer("Highlight");
                }
                IsFingersHighlighted = true;
                break;
            case "Eyes":
                foreach (GameObject eye in Eyes)
                {
                    eye.SetActive(true);
                    eye.layer = LayerMask.NameToLayer("Highlight");
                }
                IsEyesHighlighted = true;
                break;

        }
    }
    public void SpawnOrgan(string sceneOrgan, int quality)
    {
        switch (sceneOrgan)
        {
            case "LimbMiniGame":

                if(BodyLimbs[limbsHarvested * 2] != null)
                    Destroy(BodyLimbs[limbsHarvested * 2]);

                if(BodyLimbs[(limbsHarvested * 2) + 1] != null)
                    Destroy(BodyLimbs[(limbsHarvested * 2) + 1]);

                Destroy(Limbs[limbsHarvested]);

                if(Hands[limbsHarvested] != null)
                    Destroy(Hands[limbsHarvested]);

                var limb = Instantiate(limbPrefabs[limbsHarvested], limbSpawnLocations[limbsHarvested].position, limbSpawnLocations[limbsHarvested].rotation);
                limb.GetComponent<OrganManager>().currentHealth = quality;
                if (handsHarvested > limbsHarvested)
                    limb.GetComponent<LimbManager>().isHandHarvested = true;
                break;
            case "BoneMiniGame":
               var bone = Instantiate(BonePrefab, BoneSpawnLocation.position, BoneSpawnLocation.rotation);
                bone.GetComponent<OrganManager>().currentHealth = quality;
                break;
            case "BloodMiniGame":
                var blood = Instantiate(bloodPrefab, bloodSpawnLocation.position, bloodSpawnLocation.rotation);
                blood.GetComponent<OrganManager>().currentHealth = quality;
                break;
            case "SkullMinigame":
                Instantiate(brainPrefab, brainSpawnLocation.position, brainSpawnLocation.rotation);
                Destroy(BodyBrain);
                Destroy(headTop);
                var brain = Instantiate(brainPrefab, brainSpawnLocation.position, brainSpawnLocation.rotation);
                brain.GetComponent<OrganManager>().currentHealth = quality;
                break;
            case "HandMinigame":

                Destroy(BodyHands[handsHarvested]);
                Destroy(Hands[handsHarvested]);

                Instantiate(handPrefabs[handsHarvested], handSpawnLocations[handsHarvested].position, handSpawnLocations[handsHarvested].rotation);

                var hand = Instantiate(handPrefabs[handsHarvested], handSpawnLocations[handsHarvested].position, handSpawnLocations[handsHarvested].rotation);
                hand.GetComponent<OrganManager>().currentHealth = quality;
                break;
            case "EyeBallMinigame":

                var eye = gameObject;
                if(eyesHarvested == 0)
                eye = Instantiate(eyePrefab, leftEyeSpawnLocation.position, leftEyeSpawnLocation.rotation);

                if(eyesHarvested == 1)
                eye = Instantiate(eyePrefab, rightEyeSpawnLocation.position, rightEyeSpawnLocation.rotation);

                eye.GetComponent<OrganManager>().currentHealth = quality;

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
            if (limb == null) continue;

            limb.layer = LayerMask.NameToLayer("Default");
            limb.SetActive(false);
        }
        foreach (GameObject hand in Hands)
        {
            if (hand == null) continue;

            hand.layer = LayerMask.NameToLayer("Default");
            hand.SetActive(false);
        }
        foreach (GameObject eye in Eyes)
        {
            eye.layer = LayerMask.NameToLayer("Default");
            eye.SetActive(false);
        }
        foreach (GameObject bone in Bones)
        {
            bone.layer = LayerMask.NameToLayer("Default");
            bone.SetActive(false);
        }
        if(Brain != null)
            Brain.layer = LayerMask.NameToLayer("Default");
        Blood.layer = LayerMask.NameToLayer("Default");
    }

    public void OnLookEnter()
    {
        isLookedAt = true;
      //  FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(true);
      //  FindAnyObjectByType<HUDManager>().UpdateCrossHairText("");

    }
    public void OnLookExit()
    {
        isLookedAt = false;
        FindAnyObjectByType<HUDManager>().CrossHairText.transform.parent.parent.gameObject.SetActive(false);
    }
}
