using System.Drawing;
﻿using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    // reference ZumaMonster
    public class HellLord2 : MonsterObject
    {

        public bool Stoned = true; // seated

        protected override bool CanMove
        {
            get
            {
                return base.CanMove && !Stoned;
            }
        }
        protected override bool CanAttack
        {
            get
            {
                return base.CanAttack && !Stoned;
            }
        }
        protected internal HellLord2(MonsterInfo info) : base(info)
        {
        }

        public void StepDown()
        {
            if (!Stoned) return;

            Stoned = false;
            Broadcast(new S.ObjectShow { ObjectID = ObjectID });
            ActionTime = Envir.Time + 1000;
        }

        protected override void ProcessAI()
        {
            if (!Dead && Envir.Time > ActionTime)
            {
                bool stoned = !FindNearby(2);

                if (Stoned && !stoned)
                {
                    StepDown();
                }
            }

            base.ProcessAI();
        }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return !Stoned && base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(HumanObject attacker)
        {
            return !Stoned && base.IsAttackTarget(attacker);
        }


        public override Packet GetInfo()
        {
            var packet = (S.ObjectMonster)base.GetInfo();
            packet.Extra = Stoned;
            return packet;
        }

    }
}
