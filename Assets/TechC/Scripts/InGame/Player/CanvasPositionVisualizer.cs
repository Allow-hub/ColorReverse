using UnityEngine;

namespace TechC
{
    public class CanvasPositionTransfer : MonoBehaviour
    {
        [SerializeField] private RectTransform sourceUIElement; // 一枚目のCanvas上のUI要素
        [SerializeField] private Canvas sourceCanvas;          // 一枚目のCanvas（普通のCanvas）
        [SerializeField] private Canvas targetCanvas;          // 二枚目のCanvas（World Canvas）
        [SerializeField] private RectTransform targetUIElement; // 二枚目のCanvas上のUI要素

        private void Update()
        {
            if (sourceUIElement == null || sourceCanvas == null || targetCanvas == null || targetUIElement == null)
                return;

            // 一枚目のCanvasのUI要素のスクリーン座標を取得
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(sourceCanvas.worldCamera, sourceUIElement.position);

            // スクリーン座標をWorld Canvasのワールド座標に変換
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                targetCanvas.GetComponent<RectTransform>(),
                screenPoint,
                targetCanvas.worldCamera,
                out Vector3 worldPosition
            ))
            {
                // 計算したワールド座標を二枚目のUI要素に適用
                targetUIElement.position = worldPosition;
            }
        }
    }
}
