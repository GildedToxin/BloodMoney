using UnityEngine;

public class BoneMiniGameScript : MonoBehaviour
{
    private int[] boneOrder = { 1, 2, 3, 4, 5, 6 };
    [SerializeField] private int selectedBoneIndex = 0;
    public bool startGame = false;

    void Update()
    {
        if (startGame == true)
        {
            RandomizeBoneOrder();
        }
    }

    private void RandomizeBoneOrder()
    {
        for (int i = boneOrder.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = boneOrder[i];
            boneOrder[i] = boneOrder[j];
            boneOrder[j] = temp;
        }
        startGame = false;
    }
}
