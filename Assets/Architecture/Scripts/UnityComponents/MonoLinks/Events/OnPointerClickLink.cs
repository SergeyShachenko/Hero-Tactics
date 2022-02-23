using Components.Events.Physics;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityComponents.MonoLinks.Events
{
    public class OnPointerClickLink : PhysicsLinkBase, IPointerClickHandler
    {
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            
            World.NewEntity().Get<OnPointerClickEvent>() = new OnPointerClickEvent
            {
                GameObjSender = gameObject,
                Sender = entitySender
            };
            
            //Debug.Log("OnPointerClick");
        }
    }
}