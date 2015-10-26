using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
    //public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
    //public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
    private float leftBounds = 0f;
    private float rightBounds = 0f;
    private float topBounds = 0;

    private Transform player;		// Reference to the player's transform.
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    void Awake()
    {
        // Setting up the reference.
        //If you are just starting out in the lab, this won't yet be set (there is no player) and it will
        //show an error inthe console window. Ignore that, it will resolve when you add the player to the scene.
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Determine the amount we'll clamp the camera by
        leftBounds = GameObject.FindGameObjectWithTag("BorderLeft").transform.position.x;
        rightBounds = GameObject.FindGameObjectWithTag("BorderRight").transform.position.x;
        topBounds = GameObject.FindGameObjectWithTag("BorderTop").transform.position.y;
        float size = Camera.main.orthographicSize;
        float aspect = (float)Screen.width/Screen.height;

        //aspect  * size gives width. We know leftBounds in this case is negative so we'll add the negative number.
        minX = aspect*size + leftBounds;
        //The right hand side the camera position can't go past.
        maxX = rightBounds - aspect*size;
        //Ex: camera is at 6     Orthographic size is 21.6  top border at 28. So Top - size = camera position in middle.
        minY = topBounds - size;
        maxY = minY;
    }


    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }


    void FixedUpdate()
    {
        TrackPlayer();
    }


    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;


        // If the player has moved beyond the x margin...
        if (CheckXMargin())
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

        // If the player has moved beyond the y margin...
        if (CheckYMargin())
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        //
        //targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetX = Mathf.Clamp(targetX, minX, maxX);
        //targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
        targetY = Mathf.Clamp(targetY, minY, maxY);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
