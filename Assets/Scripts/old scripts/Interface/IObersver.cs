using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARMon {
    public interface IObersver {
        bool IsOccupy { get; set; }
        void StatusUpdate(Vector3 pos,Direction dir, SpawngridConfig conf, List<IObersver> obersvers);
    }
}