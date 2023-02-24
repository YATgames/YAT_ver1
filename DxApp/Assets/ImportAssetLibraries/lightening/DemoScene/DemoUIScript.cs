using UnityEngine;
using UnityEngine.UI;

public class DemoUIScript : MonoBehaviour
{
    public LighteningScript lighteningScript;
    public void Start()
    {
        framesPerSecondText.text = "FPS: " + lighteningScript.framesPerSecond.ToString();
        pointsText.text = "POINTS: " + lighteningScript.points.ToString();
        scaleText.text = "SCALE: " + lighteningScript.lineScale.ToString();
        displacementText.text = "DISPLACEMENT: " + lighteningScript.pointsDisplacement.ToString();
    }

    public GameObject sourceLightningForInitialization;

    public void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (sourceLightningForInitialization != null)
            {
                //example of a lightning initialization
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newObj = Instantiate(sourceLightningForInitialization);
                var target = newObj.transform.Find("Target");
                var host = newObj.transform.Find("Host");
                LighteningScript hostScript = host.GetComponent<LighteningScript>();
                hostScript.index = 0;
                hostScript.randomize = true;
                host.transform.position = new Vector3(worldPos.x, worldPos.y, host.transform.position.z);
            }
        }
    }

    public Text framesPerSecondText;
    public void framesPerSecond(float value)
    {
        lighteningScript.framesPerSecond = value;
        framesPerSecondText.text = "FPS: " + ((int)value).ToString();
    }

    public Text pointsText;
    public void pointsInLine(float value)
    {
        lighteningScript.points = (int)value;
        pointsText.text = "POINTS: " + ((int)value).ToString();
    }

    public Text displacementText;
    public void displacement(float value)
    {
        lighteningScript.pointsDisplacement = value;
        displacementText.text = "DISPLACEMENT: " + value.ToString();
    }

    public Text scaleText;
    public void lineScale(float value)
    {
        lighteningScript.lineScale = value;
        scaleText.text = "SCALE: " + value.ToString();
    }

    public void randomize(bool value)
    {
        lighteningScript.randomize = value;
    }

    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    public Material mat5;
    public Material mat6;
    public Material mat7;
    public Material mat8;
    public Material mat9;


    private int _materialChange = 0;
    public void materialChange(float value)
    {
        int val = (int)value;
        if (_materialChange == val)
            return;

        _materialChange = val;

        switch (val)
        {
            case 1:
                handleMaterialChange("2048x2048", 8, 2, "Version 1", mat1);
                break;
            case 2:
                handleMaterialChange("2048x2048", 8, 2, "Version 2", mat2);
                break;
            case 3:
                handleMaterialChange("2048x2048", 8, 2, "Version 3", mat3);
                break;
            case 4:
                handleMaterialChange("2048x4096", 16, 2, "Version 1", mat4);
                break;
            case 5:
                handleMaterialChange("2048x4096", 16, 2, "Version 2", mat5);
                break;
            case 6:
                handleMaterialChange("2048x4096", 16, 2, "Version 3", mat6);
                break;
            case 7:
                handleMaterialChange("4096x4096", 16, 4, "Version 1", mat7);
                break;
            case 8:
                handleMaterialChange("4096x4096", 16, 4, "Version 2", mat8);
                break;
            case 9:
                handleMaterialChange("4096x4096", 16, 4, "Version 3", mat9);
                break;
            default:
                break;
        }

    }

    public Text sizeText;
    public Text rowsColumnsText;
    public Text animationText;
    public void handleMaterialChange(string textureSize, int rows, int cols, string textureAnimV, Material mat)
    {
        sizeText.text = "SIZE: " + textureSize;
        animationText.text = "ANIMATION: " + textureAnimV;
        rowsColumnsText.text = "ROWS & COLUMNS: " + rows.ToString() + "x" + cols.ToString();
        lighteningScript.UpdateMaterial(rows, cols, mat);
    }
}
