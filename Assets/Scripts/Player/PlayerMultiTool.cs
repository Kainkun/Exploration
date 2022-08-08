using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMultiTool : MonoBehaviour
{
    public enum MultiToolMode
    {
        TrashCollector,
        EssenceCollector
    }

    public LayerMask multiToolRaycast;
    public MultiToolMode currentMultiToolMode;
    private int multiToolCount = 0;
    public Transform muzzle;
    public GameObject trashCollector;
    public GameObject essenceCollector;
    public GameObject mesh;

    public void Activate()
    {
        YarnAccess.SetValue("hasMultiTool", true);
        mesh.SetActive(true);
    }

    public void UnlockTrashCollector()
    {
        if (YarnAccess.TryGetValue("hasTrashCollector", out bool result) && result)
            return;

        multiToolCount++;
        currentMultiToolMode = MultiToolMode.TrashCollector;
        trashCollector.SetActive(true);
        YarnAccess.SetValue("hasTrashCollector", true);
    }

    public void UnlockEssenceCollector()
    {
        if (YarnAccess.TryGetValue("hasEssenceCollector", out bool result) && result)
            return;

        multiToolCount++;
        currentMultiToolMode = MultiToolMode.EssenceCollector;
        essenceCollector.SetActive(true);
        YarnAccess.SetValue("hasEssenceCollector", true);
    }

    void Update()
    {
        if (!YarnAccess.TryGetValue("hasMultiTool", out bool multiToolResult) || multiToolResult == false)
            return;

        if (multiToolCount >= 2)
        {
            if (Input.mouseScrollDelta.y > 0)
                currentMultiToolMode = (MultiToolMode)(((int)currentMultiToolMode + 1) % multiToolCount);
            else if (Input.mouseScrollDelta.y < 0)
                currentMultiToolMode = (MultiToolMode)Mathf.Abs(((int)currentMultiToolMode - 1) % multiToolCount);
        }

        switch (currentMultiToolMode)
        {
            case MultiToolMode.TrashCollector:
                if (YarnAccess.TryGetValue("hasTrashCollector", out bool trashResult) && trashResult == true)
                    HandleTrashCollector();
                break;

            case MultiToolMode.EssenceCollector:
                if (YarnAccess.TryGetValue("hasEssenceCollector", out bool essenceResult) && essenceResult == true)
                    HandleEssenceCollector();
                break;
        }
    }

    T RaycastGet<T>() where T : Component
    {
        Transform t = PlayerManager.playerCamera.transform;
        if (Physics.Raycast(t.position, t.forward, out RaycastHit raycastHit, 4f, multiToolRaycast,
                QueryTriggerInteraction.Ignore))
        {
            T component = raycastHit.transform.GetComponent<T>();
            if (component)
                return component;
        }

        return null;
    }

    void HandleTrashCollector()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastGet<TrashCollectable>()?.Collect();
            TrashBin trashBin = RaycastGet<TrashBin>();
            if (trashBin)
            {
                if (YarnAccess.TryGetValue("trashCount", out float trashCount) && trashCount > 0) ;
                {
                    TrashBin.DepositStatic(trashCount);
                    YarnAccess.SetValue("trashCount", 0);
                }
            }
        }
    }


    void HandleEssenceCollector()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastGet<EssenceCollectable>()?.Collect();
        }
    }
}