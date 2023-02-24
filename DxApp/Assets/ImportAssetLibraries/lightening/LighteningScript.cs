using UnityEngine;
using System.Collections;

public class LighteningScript : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject target;

    [Header("Material Settings")]
    public Material mat;
    private Material _mat;
    public int columns = 2;
    public int rows = 8;
    public float framesPerSecond = 10f;
    public bool randomize = false;

    [Header("Line Renderer Settings")]
    public int points = 5;
    public float pointsDisplacement = 0.1f;
    public float lineScale = 1f;
    public bool distanceBasedDisplacement = true;
    public bool zDisplacement = false;

    private int max;
    [HideInInspector]
    public int index = 0; //the current frame to display
    private bool run;
    private LineRenderer rend;
    private Vector2[] offsets;
    private Vector2 size;

    void Start()
    {
        Initialize();
        StartCoroutine(updateTiling());
    }

    void OnDisable()
    {
        if (rend != null)
        {
            rend.enabled = false;
        }

        columns = 2;
        rows = 8;
        run = false;
    }

    void OnEnable()
    {
        if (rend != null)
        {
            rend.enabled = true;
            Initialize();
            StartCoroutine(updateTiling());
        }
        Invoke("ChangeLighting", 0.2f);
    }

    private void ChangeLighting()
    {
        columns = 5;
        rows = 1;
    }
    public void Initialize()
    {
        //max = rows * columns;
        // add Line renderer
        if (rend == null)
        {
            rend = gameObject.GetComponent<LineRenderer>();
            if (rend == null)
            {
                rend = gameObject.AddComponent<LineRenderer>();
            }
        }

        rend.positionCount = points;
        _mat = Instantiate<Material>(mat);
        //apply material
        rend.material = _mat;
        //set the tile size of the texture
        size = new Vector2(1f / columns, 1f / rows);
        _mat.SetTextureScale("_MainTex", size);
        //get offsets array (tile offsets for randomization)
        GetRandomOffsets();
        run = true;

        rend.sortingLayerName = "Foreground";
    }

    private void GetRandomOffsets()
    {
        // get positions of all tiles
        int i = 0;
        offsets = new Vector2[rows * columns];
        while (i < max)
        {
            //split into x and y indexes
            Vector2 offset = new Vector2((float)i / columns - (i / columns), //x index
                                         (i / columns) / (float)rows);          //y index

            offsets[i] = offset;
            i++;
        }
    }

    public void UpdateMaterial(int rows, int cols, Material newMaterial)
    {
        mat = Instantiate<Material>(newMaterial);
        this.rows = rows;
        this.columns = cols;
        Initialize();
    }

    // this can be done on update or fixed update
    // coroutine ensures easier delay (fps) management 
    private IEnumerator updateTiling()
    {
        while (run)
        {
            //move to the next index
            index++;
            if (index >= rows * columns)
                index = 0;

            rend.positionCount = points;
            rend.startWidth = lineScale;
            rend.endWidth = lineScale;
            rend.SetPosition(0, transform.position);
            rend.SetPosition(points - 1, target.transform.position);

            // if more than 2 points than we can randomize point positions
            if (points >= 3)
            {
                for (int i = 1; i < points - 1; i++)
                {
                    float scale = (float)i / (points - 1);
                    var pos = Vector3.Lerp(transform.position, target.transform.position, scale);

                    // random values depend on distance to target
                    if (distanceBasedDisplacement)
                    {
                        float distance = Vector3.Distance(transform.position, target.transform.position);
                        distance = distance * pointsDisplacement / points;
                        pos.y += Random.Range(-distance, distance);
                        pos.x += Random.Range(-distance, distance); //uncomment this line if you want X displacement
                        if (zDisplacement)
                        {
                            pos.z += Random.Range(-distance, distance);
                        }
                    }
                    else
                    {
                        pos.y += Random.Range(-pointsDisplacement, pointsDisplacement);
                        pos.x += Random.Range(-pointsDisplacement, pointsDisplacement);
                        if (zDisplacement)
                        {
                            pos.z += Random.Range(-pointsDisplacement, pointsDisplacement);
                        }
                    }
                    rend.SetPosition(i, pos);
                }
            }
            // this picks random tile animation for each frame and randomly flips tile vertically or horizontally
            if (randomize)
            {
                if (Random.Range(0, 1) == 0)
                {
                    size.y = size.y * -1;
                    if (Random.Range(0, 1) == 0)
                        size.x = size.x * -1;
                }
                Vector2 offset = offsets[Random.Range(0, max)];
                _mat.SetTextureOffset("_MainTex", offset);
            }
            else
            {
                Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
                                             (index / columns) / (float)rows);          //y index

                _mat.SetTextureOffset("_MainTex", offset);
            }
            rend.sortingLayerName = "New Layer 2";
            rend.sortingOrder = 500;
            if (run)
                yield return new WaitForSeconds(1f / framesPerSecond);
        }
    }
}