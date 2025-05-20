using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Model
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated;

        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public Dictionary<int, ISolid> SolidObjects { get; set; }
        public Dictionary<int, IGravity> GravityObjects { get; set; }

        private int CurrentId;
        private char[,] Map = new char[13, 9];
        private readonly int TileSize = 120;
        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            SolidObjects = new Dictionary<int, ISolid>();
            GravityObjects = new Dictionary<int, IGravity>();
            Map[0, 2] = 'G';
            Map[2, 3] = 'G';
            Map[0, 4] = 'P';
            Map[1, 4] = 'G';
            Map[4, 4] = 'E';
            Map[6, 2] = 'E';
            Map[8, 2] = 'E';
            Map[10, 2] = 'E';
            Map[12, 2] = 'E';
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                Map[x, 5] = 'G';
            }
            for (int y = 6; y < Map.GetLength(1); y++)
            {
                for (int x = 1; x < Map.GetLength(0) - 1; x++)
                {
                    Map[x, y] = 'd';
                }
                Map[0, y] = 'D';
                Map[Map.GetLength(0) - 1, y] = 'D';
            }

            CurrentId = 1;
            bool isPlacedPlayer = false;

            for (int y = 0; y < Map.GetLength(1); y++)
            {
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    if (Map[x, y] != '\0')
                    {
                        IObject generatedObject = GenerateObject(Map[x, y], x, y);
                        Objects.Add(CurrentId, generatedObject);
                        if (generatedObject is ISolid solidObj)
                            SolidObjects.Add(CurrentId, solidObj);
                        if (generatedObject is IGravity gravityObj)
                            GravityObjects.Add(CurrentId, gravityObj);
                        if (Map[x, y] == 'P' && !isPlacedPlayer)
                        {
                            PlayerId = CurrentId;
                            isPlacedPlayer = true;
                        }
                        CurrentId++;
                    }
                }
            }

            Updated.Invoke(this, new GameplayEventArgs()
            {
                Objects = Objects,
                POVShift = new Vector2(
                    Objects[PlayerId].Pos.X,
                    Objects[PlayerId].Pos.Y)
            });
        }

        private IObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * TileSize;
            float y = yTile * TileSize;

            IObject generatedObject = null;

            if (sign == 'P')
            {
                generatedObject = Factory.CreateMainCharacter(
                    x: x + TileSize / 2,
                    y: y + TileSize / 2,
                    speed: new Vector2(0, 0));
            }
            else if (sign == 'E')
            {
                generatedObject = Factory.CreateEnemy(
                    x: x + TileSize / 2,
                    y: y + TileSize / 2,
                    speed: new Vector2(0, 0));
            }

            else if (sign == 'G')
            {
                generatedObject = Factory.CreateGrass(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }

            else if (sign == 'D')
            {
                generatedObject = Factory.CreateDirt(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }

            else if (sign == 'd')
            {
                generatedObject = Factory.CreateDirtNoSolid(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }

            return generatedObject;
        }




        public void Update(GameTime gameTime)
        {
            Vector2 playerInitPos = Objects[PlayerId].Pos;
            foreach (var gravityObject in GravityObjects.Values)
            {
                if (gravityObject is Enemy enemy)
                {
                    enemy.JumpAttempt();
                    enemy.ChangeDirection(Objects[PlayerId].Pos);
                }
            }

            UpdateGravityObjectsSpeed();
            CollisionCalculater.ActivateCollisionCalculater(Objects, SolidObjects, GravityObjects);


            Vector2 playerShift = Objects[PlayerId].Pos - playerInitPos;
            Updated.Invoke(this, new GameplayEventArgs
            {
                Objects = Objects,
                POVShift = playerShift
            });
        }


        public void UpdateGravityObjectsSpeed()
        {
            foreach (var gravityObject in GravityObjects.Values)
            {
                if (!gravityObject.isGrounded)
                {
                    gravityObject.verticalSpeed += gravityObject.gravity;
                    gravityObject.Speed = new Vector2(gravityObject.Speed.X, gravityObject.verticalSpeed);
                }
                else
                {
                    gravityObject.verticalSpeed = 0;
                }
            }
        }

        public void ChangePlayerSpeed(IGameplayModel.Direction direction)
        {
            MainCharacter player = Objects[PlayerId] as MainCharacter;
            switch (direction)
            {
                case IGameplayModel.Direction.right:
                    {
                        if (!CollisionCalculater.CheckRightSide(player))
                            player.Speed += new Vector2(1, 0);
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        if (!CollisionCalculater.CheckLeftSide(player))
                            player.Speed += new Vector2(-1, 0);
                        break;
                    }
                case IGameplayModel.Direction.up:
                    {
                        if (CollisionCalculater.CheckIfGrounded(player))
                            player.JumpAttempt();
                        break;
                    }
                case IGameplayModel.Direction.leftUp:
                    {
                        if (!CollisionCalculater.CheckLeftSide(player))
                            player.Speed += new Vector2(-1, 0);
                        if (CollisionCalculater.CheckIfGrounded(player))
                            player.JumpAttempt();
                        break;
                    }
                case IGameplayModel.Direction.rightUp:
                    {
                        if (!CollisionCalculater.CheckRightSide(player))
                            player.Speed += new Vector2(1, 0);
                        if (CollisionCalculater.CheckIfGrounded(player))
                            player.JumpAttempt();
                        break;
                    }
            }
        }
    }
}