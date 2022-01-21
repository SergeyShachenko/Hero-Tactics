﻿using General.Components.Events;
using Leopotam.Ecs;
using UnityEngine.EventSystems;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnPointerClickLink : PhysicsLinkBase, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            _entity.Get<OnPointerClickEvent>() = new OnPointerClickEvent {GameObject = gameObject};
        }
    }
}