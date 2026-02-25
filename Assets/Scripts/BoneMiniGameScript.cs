using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoneMiniGameScript : MonoBehaviour
{
    [SerializeField] private List<int> boneOrder = new List<int> { 1, 2, 3, 4, 5, 6 };
    [SerializeField] private List<GameObject> boneObjects = new List<GameObject>();
    [SerializeField] private int selectedBoneIndex = 0;
    private BoneCuttingMiniGameScript cuttingScript;
    [SerializeField] public int currentBone = 1; // Which bone in the order the player is at (starts at 1 ends at 6)
    public bool startGame = false;
    private bool gameStarted = false;
    private float boneDamage = 10f; // how much damage the bone takes when selecting the wrong bone

    public LayerMask hitLayers; // Use organs layer
    private float maxDistance = Mathf.Infinity;

    void Start()
    {
        cuttingScript = this.gameObject.GetComponent<BoneCuttingMiniGameScript>();
    }

    void Update()
    {
        if (startGame == true)
        {
            RandomizeBoneOrder();
        }

        // Raycast out from camera from mouse cursor to highlight bones
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayers) && selectedBoneIndex != currentBone)
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }


        // Actions that should happen when left clicking (use to select bone)
        if (Input.GetMouseButtonDown(0) && selectedBoneIndex != currentBone)
        {
            SelectBone();
        }
        if (selectedBoneIndex == currentBone)
        {
            StartCuttingGame();
        }
    }

    private void RandomizeBoneOrder()
    {
        for (int i = boneOrder.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = boneOrder[i];
            boneOrder[i] = boneOrder[j];
            boneOrder[j] = temp;
        }
        startGame = false;
    }

    private void SelectBone()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayers))
        {
            int i;
            for (i = boneObjects.Count - 1; i > 0; i--)
            {
                if (hit.collider.gameObject == boneObjects[i] && boneOrder[i] == currentBone)
                {
                    selectedBoneIndex =+ 1;
                    return;
                }
            }

            if (selectedBoneIndex != currentBone)
                Debug.Log("Incorrect Bone");
        }
    }

    private void StartCuttingGame()
    {
        cuttingScript.startGame = true;
    }
}
