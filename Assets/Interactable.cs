using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void PrimaryInteract();

}

public interface IInteractable2 : IInteractable
{
    public void SecondaryInteract();

}

public interface IInteractable3 : IInteractable2
{
    public void TertiaryInteract();
}