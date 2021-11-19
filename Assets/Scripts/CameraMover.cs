using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _treshold = 30f;
    [Tooltip("Max X camera position, Min x = -MaxM")]
    [SerializeField] private float _maxX = 16.5f;

    private int _screenWidth;

    private void Awake()
    {
        _screenWidth = Screen.width;
    }

    private void LateUpdate()
    {
        float mouseX = Input.mousePosition.x;
        Vector2Int moveDirection = new Vector2Int();

        if (mouseX < _treshold)
            moveDirection.x = -1;
        else if (mouseX > _screenWidth - _treshold)
            moveDirection.x = 1;

        if (moveDirection.x != 0)
            Move(moveDirection);
    }

    private void Move(Vector2Int direction)
    {
        Vector3 newPos = transform.position;
        float x = newPos.x + _speed * Time.deltaTime * direction.x;

        newPos.x = Mathf.Clamp(x, _maxX * -1, _maxX);

        transform.position = newPos;
    }
}
