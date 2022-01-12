using System;
using System.Collections.Generic;
using General.Components.Battle;
using General.UnityComponents.MonoLinks.Base;

namespace General.Components.Links
{
    [Serializable] public struct BattlefieldLink
    {
        public BattleSide BattleSide;
        public List<Warrior> Warriors;
        public List<MonoEntity> LinkedBattlefields;
    }
}