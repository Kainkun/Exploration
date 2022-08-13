using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class MultiToolModule
{
    protected readonly PlayerMultiTool parentMultiTool;
    protected GameObject moduleGameObject;
    protected Animator animator;
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
        moduleGameObject.transform.localPosition = Vector3.zero;
        moduleGameObject.transform.localRotation = Quaternion.identity;
        animator = moduleGameObject.GetComponent<Animator>();
        return true;
    }

    public abstract void Intro();
    public abstract void Outro();
    public abstract void Update();
    public abstract void UsePrimary();

    protected T RaycastGet<T>() where T : Component
    {
        Transform t = PlayerCamera.Singleton.virtualCamera.transform;
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
    private static readonly int IsUp = Animator.StringToHash("isUp");

    public NoneModule(PlayerMultiTool parentMultiTool) : base(parentMultiTool)
    {
        animator = parentMultiTool.animator;
    }

    protected override string YarnName => null;
    protected override string PrefabName => null;

    public override void Intro()
    {
        animator.SetBool(IsUp, false);
        // parentMultiTool.baseMesh.transform.localPosition = new Vector3(0, 0.1f, 0);
        // parentMultiTool.baseMesh.transform.localEulerAngles = new Vector3(-20, 0, 0);
    }

    public override void Outro()
    {
        animator.SetBool(IsUp, true);
        // parentMultiTool.baseMesh.transform.localPosition = Vector3.zero;
        // parentMultiTool.baseMesh.transform.localEulerAngles = Vector3.zero;
    }

    public override void Update()
    {
    }

    public override void UsePrimary()
    {
    }
}


public class TrashCollectorModule : MultiToolModule
{
    protected override string YarnName => "hasTrashCollector";
    protected override string PrefabName => "TrashCollectorModule";

    private TrashCollectable currentTrash;
    private TrashBin currentTrashBin;

    public static bool hoveringTrashBin;
    private static readonly int Use = Animator.StringToHash("use");
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    public override void Intro()
    {
        TrashCollectable.isDisplayingBounds = true;
        //moduleGameObject.transform.GetChild(0).localScale = new Vector3(0.66f, 2f, 0.66f);
        animator.SetBool(IsOpen, true);
    }

    public override void Outro()
    {
        TrashCollectable.isDisplayingBounds = false;
        //moduleGameObject.transform.GetChild(0).localScale = new Vector3(0.66f, 0.66f, 0.66f);
        hoveringTrashBin = false;
        animator.SetBool(IsOpen, false);
    }

    public override void Update()
    {
        TrashCollectable previousTrash = currentTrash;
        currentTrash = RaycastGet<TrashCollectable>();
        if (previousTrash != currentTrash && previousTrash != null)
            previousTrash.displayBounds.Unhighlight();
        if (currentTrash != null)
            currentTrash.displayBounds.Highlight();


        currentTrashBin = RaycastGet<TrashBin>();
        hoveringTrashBin = currentTrashBin != null;
    }

    public override void UsePrimary()
    {
        animator.SetTrigger(Use);

        if (currentTrash != null)
            currentTrash.Collect();

        if (currentTrashBin)
        {
            if (YarnAccess.TryGetValue("trashCount", out float trashCount) && trashCount > 0) ;
            {
                TrashBin.Deposit(trashCount);
                YarnAccess.SetValue("trashCount", 0);
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
    }

    public override void UsePrimary()
    {
        RaycastGet<EssenceCollectable>()?.Collect();
    }

    public EssenceCollectorModule(PlayerMultiTool parentMultiTool) : base(parentMultiTool)
    {
    }
}