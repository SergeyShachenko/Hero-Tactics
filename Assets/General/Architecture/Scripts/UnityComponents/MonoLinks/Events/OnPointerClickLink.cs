using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace General.UnityComponents.MonoLinks.Events
{
    public class OnPointerClickLink : PhysicsLinkBase, IPointerClickHandler
    {
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            
            World.NewEntity().Get<OnPointerClickEvent>() = new OnPointerClickEvent
            {
                Sender = gameObject,
                EntitySender = entitySender
            };
            
            //Debug.Log("OnPointerClick");
        }
    }
}