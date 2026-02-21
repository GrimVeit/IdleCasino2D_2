using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRootView : MonoBehaviour
{
    [SerializeField] private Canvas canvasMain;
    [SerializeField] private List<MovePanel> loadScreens = new List<MovePanel>();
    [SerializeField] private Transform uiSceneContainer;

    public IEnumerator ShowLoadingScreen(int index)
    {
        loadScreens[index].ActivatePanel();
        yield return new WaitForSeconds(0.3f);
    }

    public IEnumerator HideLoadingScreen(int index)
    {
        loadScreens[index].DeactivatePanel();
        yield return new WaitForSeconds(0.3f);
    }

    public void AttachSceneUI(GameObject sceneUI, Camera camera)
    {
        ClearSceneUI();

        canvasMain.renderMode = RenderMode.ScreenSpaceCamera;
        canvasMain.worldCamera = camera;

        sceneUI.transform.SetParent(uiSceneContainer, false);
        sceneUI.transform.localScale = Vector3.one;

        RectTransform rectTransform = sceneUI.GetComponent<RectTransform>();

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private void ClearSceneUI()
    {
        for (int i = 0; i < uiSceneContainer.childCount; i++)
        {
            Destroy(uiSceneContainer.GetChild(i).gameObject);
        }
    }
}
