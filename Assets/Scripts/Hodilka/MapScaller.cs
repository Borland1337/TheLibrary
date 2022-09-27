using UnityEngine;

public class MapScaller : MonoBehaviour
{
    public Transform rectTransform;
    public float MaxScale;
    public float MinScale;
    private float initialDistance;
    private Vector3 initialScale;

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled
               || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return;
            }

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = rectTransform.transform.localScale;
            }
            else
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                if (Mathf.Approximately(initialDistance, 0)) return;
                var factor = currentDistance / initialDistance;
                Zoom(initialScale * factor);
            }
        }
    }

    private void Zoom(Vector3 newScale)
    {
        if (newScale.x <= MaxScale && newScale.x > MinScale)
        {
            rectTransform.localScale = newScale;
        }
    }
}