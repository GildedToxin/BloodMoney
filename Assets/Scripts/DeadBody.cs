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

    public List<GameObject> limbPrefabs;
    public List<Transform> limbSpawnLocations;

    public bool isLookedAt = false;

    public void Update()
    {
        if (isLookedAt)
        {
            string organ = GameManager.Instance.GetOrganFromSlot(FindAnyObjectByType<InventoryController>().selectedIndex.Value - 1);
            if(organ == "")
                FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(false);
            else
                FindAnyObjectByType<HUDManager>().CrossHairText.SetActive(true);

            FindAnyObjectByType<HUDManager>().UpdateCrossHairText($"Press E to Harvest {organ}");
            if (Input.GetKeyDown(KeyCode.E))
                GameManager.Instance.StartMiniGame();

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
                    limb.layer= LayerMask.NameToLayer("Highlight");
                }
                break;
            case "Bones":
                IsBonesHighlighted = true;
                break;
            case "Blood":
                IsBloodHighlighted = true;
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
                Instantiate(limbPrefabs[0], limbSpawnLocations[0].position, limbSpawnLocations[0].rotation);
                Instantiate(limbPrefabs[1], limbSpawnLocations[1].position, limbSpawnLocations[1].rotation);
                break;
            case "Bones":
                IsBonesHighlighted = true;
                break;
            case "Blood":
                IsBloodHighlighted = true;
                break;
            case "SkullMinigame":
                Instantiate(brainPrefab, brainSpawnLocation.position, brainSpawnLocation.rotation);
                break;
            case "Fingers":
                IsFingersHighlighted = true;
                break;
            case "Eyes":
                IsEyesHighlighted = true;
                break;

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
        }


        Brain.layer = LayerMask.NameToLayer("Default");
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
