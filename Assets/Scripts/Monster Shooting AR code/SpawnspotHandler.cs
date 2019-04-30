using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARMon
{
    public class SpawnspotHandler : ISubject
    {
        private List<IObersver> observers = new List<IObersver>();
        public List<IObersver> Obersvers { get { return observers; } }
        public void Notfiy(Vector3 pos, Direction dir, SpawngridConfig conf)
        {
            foreach (IObersver observer in observers)
            {
                observer.StatusUpdate(pos, dir, conf, observers);
            }
        }

        public void Subscribe(IObersver obersver)
        {
            if (!observers.Contains(obersver))
            {
                observers.Add(obersver);
            }
        }

        public void Unsubscribe(IObersver obersver)
        {
            observers.Remove(obersver);
            obersver = null;
        }

        public void Subscribe(List<IObersver> newobersver)
        {

        }
    }
}
