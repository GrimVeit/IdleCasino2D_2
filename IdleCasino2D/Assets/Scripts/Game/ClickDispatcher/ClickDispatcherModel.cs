using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDispatcherModel
{
    private IEnumerator clickRoutine;

    public void Activate()
    {
        if (clickRoutine != null) Coroutines.Stop(clickRoutine);

        clickRoutine = ClickCoro();
        Coroutines.Start(clickRoutine);
    }

    public void Deactivate()
    {
        if (clickRoutine != null)
        {
            Coroutines.Stop(clickRoutine);
            clickRoutine = null;
        }
    }

    private IEnumerator ClickCoro()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);
                if (hit.collider != null && hit.collider.TryGetComponent<IClick>(out var clickItem))
                {
                    clickItem.Click();
                }
            }

            yield return null;
        }
    }
}
