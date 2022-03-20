using Components.Events.Physics;
using Leopotam.Ecs;
using UnityComponents.MonoLinks.Base;
using UnityEngine.EventSystems;

namespace UnityComponents.MonoLinks.Events
{
    public sealed class OnPointerClickLink : PhysicsLinkBase, IPointerClickHandler
    {
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var entitySender = gameObject.GetComponent<MonoEntity>().GetEntity();
            
            World.NewEntity().Get<OnPointerClickEvent>() = new OnPointerClickEvent
            {
                SenderGameObj = gameObject,
                Sender = entitySender
            };
            
            //Debug.Log("OnPointerClick");
        }
    }
}