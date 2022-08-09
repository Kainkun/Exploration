using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiToolModule
{
    protected readonly PlayerMultiTool parentMultiTool;
    protected GameObject moduleGameObject;
    protected abstract string YarnName { get; }
    protected abstract string PrefabName { get; }

    protected MultiToolModule(PlayerMultiTool parentMultiTool)
    {
        this.parentMultiTool = parentMultiTool;
    }

    public bool Unlock()
    {
        if (PrefabName == null)
            return false;
        
        if (YarnAccess.TryGetValue(YarnName, out bool result) && result)
            return false;
        YarnAccess.SetValue(YarnName, true);
        moduleGameObject = Object.Instantiate(Resources.Load<GameObject>("MultiToolModules/" + PrefabName),
            parentMultiTool.baseMesh.transform);
        return true;
    }

    public abstract void Intro();
    public abstract void Outro();
    public abstract void Update();

    protected T RaycastGet<T>() where T : Component
    {
        Transform t = PlayerManager.playerCamera.transform;
        if (Physics.Raycast(t.position, t.forward, out RaycastHit raycastHit, 4f, parentMultiTool.raycastLayerMask,
                QueryTriggerInteraction.Ignore))
        {
            T component = raycastHit.transform.GetComponent<T>();
            if (component)
                return component;
        }

        return null;
    }
}


public class NoneModule : MultiToolModule
{
    public NoneModule(PlayerMultiTool parentMultiTool) : base(parentMultiTool)
    {
    }

    protected override string YarnName => null;
    protected override string PrefabName => null;
    public override void Intro()
    {
        parentMultiTool.baseMesh.transform.localPosition = new Vector3(0,0.1f,0);
        parentMultiTool.baseMesh.transform.localEulerAngles = new Vector3(-20,0,0);
    }

    public override void Outro()
    {
        parentMultiTool.baseMesh.transform.localPosition = Vector3.zero;
        parentMultiTool.baseMesh.transform.localEulerAngles = Vector3.zero;
    }

    public override void Update()
    {
        
    }
}


public class TrashCollectorModule : MultiToolModule
{
    protected override string YarnName => "hasTrashCollector";
    protected override string PrefabName => "TrashCollectorModule";


    public override void Intro()
    {
        TrashCollectable.isDisplayingBounds = true;
        moduleGameObject.transform.GetChild(0).localScale = new Vector3(0.66f, 2f, 0.66f);
    }

    public override void Outro()
    {
        TrashCollectable.isDisplayingBounds = false;
        moduleGameObject.transform.GetChild(0).localScale = new Vector3(0.66f, 0.66f, 0.66f);
    }

    public override void Update()
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

    public TrashCollectorModule(PlayerMultiTool parentMultiTool) : base(parentMultiTool)
    {
    }
}


public class EssenceCollectorModule : MultiToolModule
{
    protected override string YarnName => "hasEssenceCollector";
    protected override string PrefabName => "EssenceCollectorModule";

    public override void Intro()
    {
        moduleGameObject.transform.GetChild(0).localScale = new Vector3(0.66f, 2f, 0.66f);
    }

    public override void Outro()
    {
        moduleGameObject.transform.GetChild(0).localScale = new Vector3(0.66f, 0.66f, 0.66f);
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastGet<EssenceCollectable>()?.Collect();
        }
    }

    public EssenceCollectorModule(PlayerMultiTool parentMultiTool) : base(parentMultiTool)
    {
    }
}