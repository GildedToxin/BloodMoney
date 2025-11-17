using JetBrains.Annotations;
using UnityEngine;
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


    public GameObject BrainPrefab;
    public Transform brainSpawnLocation;

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
            case "Limbs":
                IsLimbsHighlighted = true;
                foreach (GameObject limb in Limbs)
                {
                    limb.layer = LayerMask.NameToLayer("Highlight");
                }
                break;
            case "Bones":
                IsBonesHighlighted = true;
                break;
            case "Blood":
                IsBloodHighlighted = true;
                break;
            case "SkullMinigame":
                Instantiate(BrainPrefab, brainSpawnLocation.position, brainSpawnLocation.rotation);
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
        // Update mini game text if held item hasnt been used
        
    }
    public void OnLookExit()
    {
       
    }
}
