using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.View;
using static MyGame.Model.Direction;

namespace MyGame.Model
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated;

        public int PlayerId { get; set; }
        public Dictionary<int, IMapObject> Objects { get; set; }
        public Dictionary<int, ISolidObject> SolidObjects { get; set; }
        public Dictionary<int, IGravityObject> GravityObjects { get; set; }
        public Dictionary<int, IAliveObject> AliveObjects { get; set; }
        public Dictionary<int, IAttackObject> AttackObjects { get; set; }

        private bool IsPlayerPlaced = false;

        private int CurrentId;
        private Vector2 PlayerShift;

        public void Initialize()
        {
            Objects = new Dictionary<int, IMapObject>();
            SolidObjects = new Dictionary<int, ISolidObject>();
            GravityObjects = new Dictionary<int, IGravityObject>();
            AliveObjects = new Dictionary<int, IAliveObject>();
            AttackObjects = new Dictionary<int, IAttackObject>();
            PlayerControl.ConnectPlayerControl(Objects, AttackObjects);
            AttacksControl.ConnectAttacksControl(
                AliveObjects, GravityObjects, SolidObjects, AttackObjects, Objects);
            CollisionCalculater.ConnectCollisionCalculater(Objects, SolidObjects, GravityObjects);
            MapCreator.ConnectMapCreator(Objects, SolidObjects, GravityObjects, AliveObjects);

            MapCreator.CreateFirstMap();
            CurrentId = MapCreator.CurrentId;
            PlayerId = MapCreator.PlayerId;
            IsPlayerPlaced = MapCreator.IsPlayerPlaced;

            Updated.Invoke(this, new GameplayEventArgs()
            {
                Objects = Objects,
                POVShift = new Vector2(
                    Objects[PlayerId].Pos.X,
                    Objects[PlayerId].Pos.Y)
            });
        }

        

        public void Update(GameTime gameTime)
        {
            if (IsPlayerPlaced)
            {
                Vector2 playerInitPos = Objects[PlayerId].Pos;
                foreach (var aliveObject in AliveObjects.Values)
                {
                    if (aliveObject is Enemy enemy)
                    {
                        enemy.JumpAttempt();
                        enemy.ChangeDirection(Objects[PlayerId].Pos);
                        if (enemy.TryAttack())
                        {
                            IMapObject generatedObject;
                            if (enemy.Direction == right)
                                generatedObject = Factory.CreateEnemyAttack(enemy.Pos.X + enemy.Width, enemy.Pos.Y - enemy.Height / 4, right);
                            else
                                generatedObject = Factory.CreateEnemyAttack(enemy.Pos.X - 128, enemy.Pos.Y - enemy.Height / 2, left);
                            Objects.Add(CurrentId, generatedObject);
                            AttackObjects.Add(CurrentId, generatedObject as IAttackObject);
                            CurrentId++;
                        }
                    }
                }

                UpdateGravityObjectsSpeed();
                CollisionCalculater.ActivateCollisionCalculater();
                AttacksControl.ActivateAttacksControl();

                if (Objects.ContainsKey(PlayerId))
                    PlayerShift = Objects[PlayerId].Pos - playerInitPos;
                else
                {
                    IsPlayerPlaced = false;
                    PlayerShift = new Vector2(0, 0);
                }
            }
            else
            {

            }

            Updated.Invoke(this, new GameplayEventArgs
            {
                Objects = Objects,
                POVShift = PlayerShift
            });
        }

        public void UpdateGravityObjectsSpeed()
        {
            foreach (var gravityObject in GravityObjects.Values)
            {
                gravityObject.UpdateGravity();
            }
        }

        public void ControlPlayer(ControlsEventArgs e)
        {
            if (IsPlayerPlaced)
            {
                var PCAId = PlayerControl.BeginPlayerControl(PlayerId, CurrentId, e);
                PlayerId = PCAId[0];
                CurrentId = PCAId[1];
            }
        }


    }
}