using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Model.ObjectTypes;

namespace MyGame.Model
{
    public static class MapCreator
    {
        private static Dictionary<int, IMapObject> MapObjects;
        private static Dictionary<int, ISolidObject> SolidObjects;
        private static Dictionary<int, IGravityObject> GravityObjects;
        private static Dictionary<int, IAliveObject> AliveObjects;

        private static readonly int TileSize = 120;
        private static readonly char[,] Map = new char[13, 15];
        public static int CurrentId { get; private set; }
        public static int PlayerId { get; private set; }
        public static bool IsPlayerPlaced { get; private set; }
        
        public static void ConnectMapCreator(
            Dictionary<int, IMapObject> mapObjects,
            Dictionary<int, ISolidObject> solidObjects,
            Dictionary<int, IGravityObject> gravityObjects,
            Dictionary<int, IAliveObject> aliveObjects)
        {
            MapObjects = mapObjects;
            SolidObjects = solidObjects;
            GravityObjects = gravityObjects;
            AliveObjects = aliveObjects;
            CurrentId = 1;
        }

        public static void CreateFirstMap()
        {
            Map[0, 4] = 'P';
            Map[0, 2] = 'G';
            Map[2, 3] = 'G';
            
            Map[1, 4] = 'G';
            Map[4, 2] = 'E';
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
                    Map[x, y] = 's';
                }
                Map[0, y] = 'S';
                Map[Map.GetLength(0) - 1, y] = 'S';
            }
            GenerateAllObjects();
        }

        private static void GenerateAllObjects()
        {
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    if (Map[x, y] != '\0')
                    {
                        IMapObject generatedObject = GenerateObject(Map[x, y], x, y);
                        MapObjects.Add(CurrentId, generatedObject);
                        if (generatedObject is ISolidObject solidObj)
                            SolidObjects.Add(CurrentId, solidObj);
                        if (generatedObject is IGravityObject gravityObj)
                            GravityObjects.Add(CurrentId, gravityObj);
                        if (generatedObject is IAliveObject aliveObj)
                            AliveObjects.Add(CurrentId, aliveObj);
                        if (Map[x, y] == 'P' && !IsPlayerPlaced)
                        {
                            PlayerId = CurrentId;
                            IsPlayerPlaced = true;
                        }
                        CurrentId++;
                    }
                }
            }
        }

        private static IMapObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * TileSize;
            float y = yTile * TileSize;

            IMapObject generatedObject = null;

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

            else if (sign == 'S')
            {
                generatedObject = Factory.CreateStone(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }

            else if (sign == 's')
            {
                generatedObject = Factory.CreateStoneNoSolid(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }

            return generatedObject;
        }
    }
}
