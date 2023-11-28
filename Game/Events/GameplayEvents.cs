using System.Net.Mail;
using JetBoxer2D.Engine.Events;
using JetBoxer2D.Engine.Interfaces;

namespace JetBoxer2D.Game.Events;

public class GameplayEvents : BaseGameStateEvent
{
    public class PlayerShoots : GameplayEvents
    {
    }

    public class EnemyHitBy : GameplayEvents
    {
        public IDamageable HitBy { get; private set; }

        public EnemyHitBy(IDamageable gameObject)
        {
            HitBy = gameObject;
        }
    }

    public class EnemyLostLife : GameplayEvents
    {
        public int CurrentLife { get; private set; }

        public EnemyLostLife(int currentLife)
        {
            CurrentLife = currentLife;
        }
    }
}