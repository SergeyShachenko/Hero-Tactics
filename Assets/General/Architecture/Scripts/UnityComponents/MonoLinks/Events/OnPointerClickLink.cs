﻿using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine.EventSystems;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnPointerClickLink : PhysicsLinkBase, IPointerClickHandler
    {
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Entity.Get<OnPointerClickEvent>() = new OnPointerClickEvent {GameObject = gameObject};
        }
    }
}