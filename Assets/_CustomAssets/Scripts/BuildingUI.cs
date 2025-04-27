using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    public BuildingGenerator generator;
    public Slider widthSlider;
    public Slider heightSlider;

    public void GenerateBuilding()
    {
        int width = (int)widthSlider.value;
        int height = (int)heightSlider.value;
        generator.GenerateBuilding(width, height);
    }
}
