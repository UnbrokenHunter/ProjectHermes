using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
* v1.0  list of pixel objects that form a shadow beneath the player.
* Created by Adam Younis 
* Yt: http://youtube.com/AdamCYounis
* Tw: http://twitter.com/AdamCYounis
* Tch: http://twitch.tv/AdamCYounis
*/
public class PlatformShadow : MonoBehaviour {
    public SpriteRenderer pixelSprite; //the gameobject that the shadow list uses as a template
    public float baseWidth = 0.2f; //the worldspace width of the shadow
    public float maxHeight = 0.5f; //the max jump height of the character, defines range for changes to size and colour of shadow.
    [Range(0, 1)]
    public float shrinkFactor = 0.25f; //the smallest size that the shadow can shrink,relative to the base size.
    [Range(0, 3)]
    public int chamfer = 1;//the number of pixels we remove from the bottom row's radius to achieve visual roundness.
    public Color darkestShade = Color.black;//the colour of the shadow when the player is on the ground
    public Color lightestShade = Color.black; //the colour of the shadow when the player is at max jump height

    public LayerMask collisionLayers; //which layers are considered "ground?"

    public float edgeDropTolerance = 0.03f;//the distance between two pixels we consider to be an "edge"
    public bool continueOffEdge;//should the shadow continue off ledges?

    public bool drawHeight = true; //draw the height gizmo?
    public bool drawWidth = true; //draw the width gizmo?

    //common measurements
    float halfWidth => baseWidth / 2;
    int pixelWidth => Mathf.CeilToInt(baseWidth * ppu);
    int halfPixelWidth => pixelWidth / 2;
    Vector2 basePos => (Vector2)transform.position;
    float ppu => pixelSprite.sprite.pixelsPerUnit;
    List<ShadowPixel> pixels;

    // Start is called before the first frame update
    void Start() {
        SetupPixelObjects();
    }

    //setup all of the pixel sprites
    void SetupPixelObjects() {
        pixels = new List<ShadowPixel>();
        Vector2 startPoint = Vector2.left * baseWidth * 0.5f;
        //for each pixel, up to the total width of the body
        for (int i = 0; i < pixelWidth; i++) {
            Vector2 offset = startPoint + (Vector2.right * i) / ppu;

            //instantiate left to right
            SpriteRenderer sr = GameObject.Instantiate(pixelSprite, transform).GetComponent<SpriteRenderer>();
            sr.gameObject.name = i.ToString();
            sr.transform.localPosition = offset;
            sr.gameObject.SetActive(true);

            //add it to the list
            pixels.Add(new ShadowPixel(offset, sr));
        }

    }
    // Update is called once per frame
    void LateUpdate() {
        SetBaseShadow();
        if (!continueOffEdge) {
            CullShadow();
        }
    }

    void SetBaseShadow() {
        float centrePixelPoint = halfPixelWidth - 0.5f;
        Vector2 rayStart = (Vector2.up * halfWidth); //starts the ray from a little higher than the shadow, for slope support
        for (int i = 0; i < pixelWidth; i++) { //for each of the pixels

            ShadowPixel sp = pixels[i];
            //calculate a raycast towards the ground
            RaycastHit2D hit = Physics2D.Raycast(basePos + sp.initLocalPos + rayStart, Vector2.down, maxHeight * 2, collisionLayers);

            float dist = hit.distance - rayStart.magnitude;//how far was the ground from our feet?

            if (hit.collider != null) {//disable if it hit nothing
                Vector2 direction = Vector2.right * transform.lossyScale.x;
                sp.transform.localPosition = sp.initLocalPos * direction + (Vector2.down * dist);
                sp.renderer.enabled = true;
            }

            //determine how close to the edge of the shadow this pixel is...
            float distanceFromShadowEdge = centrePixelPoint - Mathf.Abs(centrePixelPoint - i); //half pixel offset
            bool isOutsideMinRange = (halfPixelWidth - distanceFromShadowEdge) / halfPixelWidth > shrinkFactor;

            //determine whether to this falls outside of the radius, based on raycast length
            float pixelShrinkDistance = Mathf.RoundToInt(Map(dist, 0, maxHeight, 0, baseWidth - (baseWidth * shrinkFactor)) * ppu);
            bool isOutsideRadius = isOutsideMinRange && distanceFromShadowEdge < pixelShrinkDistance;

            //disable this pixel if we don't meet the conditions
            if (hit.collider == null || hit.distance == 0 || isOutsideRadius) {
                sp.renderer.enabled = false;
            } else { //otherwise, style the pixel appropriately
                float lerp = Map(dist, 0f, maxHeight, 0, 1, true); //set lerp
                sp.renderer.color = Color.Lerp(darkestShade, lightestShade, lerp); //set colour

                float yHeight = distanceFromShadowEdge - pixelShrinkDistance < chamfer ? 1 : 2;
                sp.transform.localScale = new Vector3(1, yHeight);//set line height
            }
        }
    }

    void CullShadow() {

        bool shouldCull = false;//once this is true, hide all other pixels
        float firstY = pixels[0].pos.y;
        float lastY = pixels[pixels.Count - 1].pos.y;

        //determine which direction we want to parse the list, depending on the slope direction.
        bool leftToRight = pixels[0].pos.y > pixels[pixels.Count - 1].pos.y;

        //iterator instance variables
        int startIndex = leftToRight ? 0 : pixels.Count - 1;
        int endIndex = leftToRight ? pixels.Count - 1 : 0;
        int increment = leftToRight ? 1 : -1;

        //iterate through all of the pixels comparing two at a time,
        for (int i = startIndex; i != endIndex; i += increment) {
            int j = i + increment;

            if (!shouldCull) {
                //make sure we're not checking outside the bounds
                if (j >= 0 && j < pixels.Count && pixels[i].renderer.enabled) {
                    //compare two pixels
                    float currentY = pixels[i].pos.y;
                    float nextY = pixels[j].pos.y;
                    //if we see a significant drop between them, disable all subsequent pixels
                    shouldCull = Mathf.Abs(currentY - nextY) > edgeDropTolerance;
                }
            }

            if (shouldCull) {//disable all subsequent pixels if we found a drop
                pixels[i].renderer.enabled = false;
            }
        }
    }
    //map value from one range to another
    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false) {
        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        return clamp ? Mathf.Clamp(val, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }

    public void OnDrawGizmos() {
        if (drawWidth) {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position + Vector3.left * halfWidth, transform.position + Vector3.right * halfWidth);
        }

        if (drawHeight) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxHeight);
        }
    }
}

//simple struct to contain the initial values of the shadow pixels
public struct ShadowPixel {
    public SpriteRenderer renderer;//this pixel's sprite
    public Transform transform;//this pixel's transform
    public Vector2 initLocalPos;//the initial position relative to the shadow base
    public Vector3 pos => transform.position;

    public ShadowPixel(Vector2 _initLocalPos, SpriteRenderer _renderer) {
        initLocalPos = _initLocalPos;
        renderer = _renderer;
        transform = renderer.transform;
    }

}
