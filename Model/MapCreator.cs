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
using KnightLegends.Model.Objects.MapObjects;
using KnightLegends.Model.ObjectTypes;

namespace KnightLegends.Model
{
    public static class MapCreator
    {
        private static Dictionary<int, IMapObject> MapObjects;
        private static Dictionary<int, ISolidObject> SolidObjects;
        private static Dictionary<int, NoSolidObject> NoSolidObjects;
        private static Dictionary<int, IGravityObject> GravityObjects;
        private static Dictionary<int, IAliveObject> AliveObjects;
        private static Dictionary<int, BackgroundObject> BackgroundObjects;

        private static readonly int TileSize = 120;
        private static char[,] Map;
        public static int CurrentId { get; private set; }
        public static int PlayerId { get; private set; }
        public static int PortalId { get; private set; }
        public static int CloudsId { get; private set; }
        public static bool IsPlayerPlaced { get; private set; }
        
        public static void ConnectMapCreator(
            Dictionary<int, IMapObject> mapObjects,
            Dictionary<int, ISolidObject> solidObjects,
            Dictionary<int, NoSolidObject> noSolidObjects,
            Dictionary<int, IGravityObject> gravityObjects,
            Dictionary<int, IAliveObject> aliveObjects,
            Dictionary<int, BackgroundObject> backgroundObjects)
        {
            MapObjects = mapObjects;
            SolidObjects = solidObjects;
            NoSolidObjects = noSolidObjects; 
            GravityObjects = gravityObjects;
            AliveObjects = aliveObjects;
            BackgroundObjects = backgroundObjects;
            CurrentId = 1;
        }

        public static void CreateFirstMap()
        {
            IsPlayerPlaced = false;
            Map = new char[,]
            {
                 /*0*/{'M', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'E', '0', '0', '0', '0', 'E', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'E', 'e', '0'},/*0*/
                 /*1*/{'0', '0', 'c', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'E', 'E', '0'},/*1*/
                 /*2*/{'C', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'H', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'E', 'E', '0'},/*2*/
                 /*3*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 't', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'H', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*3*/
                 /*4*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'E', '0', '0', '0', '0', '0', '0', '0', '0', 'H', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 't', '0', '0', '0', '0', '0', '0'},/*4*/
                 /*5*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 't', '0', '0', 'T', 'E', '0', 'G', 'G', 'G', 'G', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 't', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'H', '0', '0', '0'},/*5*/
                 /*6*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'G', 'G', 's', 's', 's', 's', 'G', 'G', 'G', 'G', '0', '0', '0', '0', '0', 'T', '0', '0', '0', '0', '0', '0', '0', '0', 'G', 'G', 'G', 'G', '0', '0', '0', '0', '0', '0', '0', '0'},/*6*/
                 /*7*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 'G', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 'G', 'G', 'G', 'G', '0', '0', '0', '0', 'p', '0', '0', 'G', 'G', 'G', 's', 's', 's', 's', 'G', 'G', 'G', '0', '0', '0', '0', '0'},/*7*/
                 /*8*/{'0', 'T', '0', '0', '0', '0', '0', 'T', '0', 'E', 'G', 'G', 'G', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 'G', 'G', 'G', 'G', 'D'},/*8*/
                 /*9*/{'D', '0', '0', '0', '0', 'P', '0', '0', '0', 'G', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 'G'},/*9*/
                /*10*/{'G', 'G', 'G', '0', '0', '0', 'G', 'G', 'G', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 'S'},/*10*/
                /*11*/{'S', 's', 's', 'G', 'G', 'G', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 's', 'S'},/*11*/
              /*s 12*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 12*/
              /*s 13*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 13*/
              /*s 14*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 14*/
              /*s 15*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 15*/
              /*s 16*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 16*/
              /*s 17*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 17*/
              /*s 18*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 18*/
              /*s 19*/{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'},/*s 19*/
            };
            for (int y = 12; y < Map.GetLength(0) - 1; y++)
            {
                for (int x = 1; x < Map.GetLength(1); x++)
                {
                    Map[y, x] = 's';
                }
                Map[y, 0] = 'S';
                Map[y, Map.GetLength(1) - 1] = 'S';
            }
            GenerateAllObjects();
        }

        private static void GenerateAllObjects()
        {
            for (int y = 0; y < Map.GetLength(0); y++)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                {
                    if (Map[y, x] != '0')
                    {
                        IMapObject generatedObject = GenerateObject(Map[y, x], x, y);
                        MapObjects.Add(CurrentId, generatedObject);
                        if (generatedObject is ISolidObject solidObj)
                            SolidObjects.Add(CurrentId, solidObj);
                        if (generatedObject is IGravityObject gravityObj)
                            GravityObjects.Add(CurrentId, gravityObj);
                        if (generatedObject is IAliveObject aliveObj)
                            AliveObjects.Add(CurrentId, aliveObj);
                        if (generatedObject is BackgroundObject backgroundObj)
                            BackgroundObjects.Add(CurrentId, backgroundObj);
                        if (generatedObject is NoSolidObject noSolidObj)
                            NoSolidObjects.Add(CurrentId, noSolidObj);
                        if (Map[y, x] == 'P' && !IsPlayerPlaced)
                        {
                            PlayerId = CurrentId;
                            IsPlayerPlaced = true;
                        }
                        if (Map[y, x] == 'p')
                        {
                            PortalId = CurrentId;
                        }
                        if (Map[y, x] == 'C')
                        {
                            CloudsId = CurrentId;
                        }
                        CurrentId++;
                    }
                }
            }
        }

        private static IMapObject GenerateObject(char sign, int yTile, int xTile)
        {
            float x = yTile * TileSize;
            float y = xTile * TileSize;

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
            else if (sign == 'e')
            {
                generatedObject = Factory.CreateAngryEnemy(
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
            else if (sign == 'p')
            {
                generatedObject = Factory.CreatePortal(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }
            else if (sign == 'c')
            {
                generatedObject = Factory.CreateSun(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }
            else if (sign == 'C')
            {
                generatedObject = Factory.CreateClouds(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }
            else if (sign == 'M')
            {
                generatedObject = Factory.CreateMountains(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }
            else if (sign == 'H')
            {
                generatedObject = Factory.CreateJapanHouse(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }
            else if (sign == 't')
            {
                generatedObject = Factory.CreateSakura(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }
            else if (sign == 'T')
            {
                generatedObject = Factory.CreateTree(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }
            else if (sign == 'D')
            {
                generatedObject = Factory.CreateDeathTable(
                    x + TileSize / 2,
                    y + TileSize / 2);
            }

            return generatedObject;
        }
    }
}
