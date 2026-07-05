using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;



namespace Server.MirObjects.Monsters
{

    public class ChieftainSword : MonsterObject
    {
        private long _mapFireTime=0;

        protected internal ChieftainSword(MonsterInfo info)
            : base(info)
        {
        }

        private void SpawnFireWall()
        {
            List<MapObject> targets = FindAllTargets(Info.ViewRange, CurrentLocation);
            if (targets.Count == 0) return;

            // RangeAttack1
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (j == 1 && i == 1)
                        continue;

                    Point location = new Point(CurrentLocation.X + (j - 1) * 7,
                                             CurrentLocation.Y + (i - 1) * 7);

                    SpellObject spellObj = null;

                    spellObj = new SpellObject
                    {
                        Spell = Spell.ChieftainSwordMapFire,
                        Value = Envir.Random.Next(Stats[Stat.MinDC], Stats[Stat.MaxDC]),
                        ExpireTime = Envir.Time + 7000,
                        TickSpeed = 1000,
                        Caster = this,
                        CurrentLocation = location,
                        CurrentMap = CurrentMap
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + 1500, spellObj);
                    CurrentMap.ActionList.Add(action);
                }
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange())
            {
                Attack();

                if (Target != null && Target.Dead)
                {
                    FindTarget();
                }

                return;
            } else if (Envir.Time > _mapFireTime)
            {
                SpawnFireWall();
                _mapFireTime = Envir.Time + 8000;

                return;
            }

            MoveTo(Target.CurrentLocation);
        }
    }
    }