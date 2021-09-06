//using UnityEngine;
//using UnityEngine.Events;

//public abstract class AGameEventListener<T> : MonoBehaviour where T : IGameEventListener
//{
//    public GameEvent Event;
//    public UnityEvent Response;

//    public virtual void OnEnable()
//    {
//        Event.RegisterListener(this);
//    }

//    public virtual void OnDisable()
//    {
//        Event.UnregisterListener(this);
//    }
//}
