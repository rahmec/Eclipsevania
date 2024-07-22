using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverOffset = new Vector3(0, 10, 0);  // Offset quando il mouse è sopra il bottone
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isHovered = false;
    public float speed = 5f; // Velocità del movimento
    public float highlightAlpha = 2f;  // Alpha value when highlighted
    public float normalAlpha = 0.1f;  // Alpha value when not highlighted
    private TMP_Text buttonText;

    void Start()
    {
        originalPosition = transform.localPosition;  // Salva la posizione originale del bottone
        targetPosition = originalPosition;
        buttonText = GetComponentInChildren<TMP_Text>();
        SetTextAlpha(normalAlpha);
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPosition = originalPosition + hoverOffset;  // Imposta la posizione di destinazione
        isHovered = true;
        SetTextAlpha(highlightAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPosition = originalPosition;  // Ripristina la posizione di destinazione
        isHovered = false;
        SetTextAlpha(normalAlpha);
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = buttonText.color;
        color.a = alpha;
        buttonText.color = color;
    }
}

