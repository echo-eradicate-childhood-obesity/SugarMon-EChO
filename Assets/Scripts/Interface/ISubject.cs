using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARMon
{
    public interface ISubject
    {
        void Subscribe(IObersver obersver);
        void Unsubscribe(IObersver obersver);
        void Notfiy(Vector3 pos, Direction dir, SpawngridConfig conf);
    }
}
