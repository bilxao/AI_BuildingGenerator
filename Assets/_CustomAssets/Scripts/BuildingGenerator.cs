using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public GameObject[] buildingBlocks;
    public int width = 5;
    public int height = 10;
    public float blockSize = 1.0f;

    public void GenerateBuilding(int newWidth, int newHeight)
    {
        ClearBuilding();  // Remove old building
        width = newWidth;
        height = newHeight;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * blockSize, y * blockSize, 0);
                GameObject block = Instantiate(buildingBlocks[Random.Range(0, buildingBlocks.Length)], position, Quaternion.identity);
                block.transform.parent = transform;
            }
        }
    }

    void ClearBuilding()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
