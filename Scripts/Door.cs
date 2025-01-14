// Manages the functionality of doors to transition between rooms.
public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;  // Reference to the previous room (for transitions)
    [SerializeField] private Transform nextRoom;  // Reference to the next room (for transitions)
    [SerializeField] private CameraController cam;  // Camera controller for room transition effects

    // Trigger transition between rooms when player collides with the door
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)  // If the player is moving to the next room
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);  // Activate the next room
                previousRoom.GetComponent<Room>().ActivateRoom(false);  // Deactivate the previous room
            }
            else  // If the player is moving to the previous room
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);  // Activate the previous room
                nextRoom.GetComponent<Room>().ActivateRoom(false);  // Deactivate the next room
            }
        }
    }
}
