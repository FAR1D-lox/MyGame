using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightLegends.Model;
using static KnightLegends.Model.Direction;

namespace KnightLegends.View
{
    public static class Animation
    {
        //0, 1, 2, 3, 4, 5, 6, 7 - что-то на подобии строчек. Высота каждой строчки идёт сверху вниз и равна высоте объекта.
        public static Vector2 AnimateObjectMove(int widthObj, int heightObj,
            int widthImage, Vector2 imagePos, Vector2 possitionDifference)
        {
            if (imagePos == new Vector2(int.MinValue, int.MinValue) ||
                imagePos.Y == heightObj * 2 || imagePos.Y == heightObj * 3)
            {
                return new Vector2(0, 0);
            }
            if (imagePos.Y == heightObj * 4 || imagePos.Y == heightObj * 5 ||
                imagePos.Y == heightObj * 6 || imagePos.Y == heightObj * 7)
            {
                imagePos.Y -= heightObj * 4;
            }
            
            if (possitionDifference.Y != 0)
            {
                if (possitionDifference.X > 0)
                    return new Vector2(0, 0);
                return new Vector2(0, heightObj);
            }
            else
            {
                if (possitionDifference.X < 0)
                {
                    if (imagePos.Y == 0)
                        return new Vector2(0, heightObj);
                    else if (imagePos.Y == heightObj)
                    {
                        if (imagePos.X == widthImage - widthObj)
                            return new Vector2(0, heightObj);
                        return new Vector2(imagePos.X + widthObj, heightObj);
                    }
                }
                else if (possitionDifference.X > 0)
                {
                    if (imagePos.Y == heightObj)
                        return new Vector2(0, 0);
                    else if (imagePos.Y == 0)
                    {
                        if (imagePos.X == widthImage - widthObj)
                            return new Vector2(0, 0);
                        return new Vector2(imagePos.X + widthObj, 0);
                    }
                }
                return new Vector2(0, imagePos.Y);
            }
        }

        public static Vector2 AnimateObjectAttacking(int widthObj, int heightObj,
            int widthImage, Vector2 imagePos, Direction direction)
        {
            if (imagePos == new Vector2(int.MinValue, int.MinValue) ||
                imagePos.Y == 0 || imagePos.Y == heightObj)
            {
                return new Vector2(0, heightObj * 2);
            }
            if (imagePos.Y == heightObj * 4 || imagePos.Y == heightObj * 5 ||
                imagePos.Y == heightObj * 6 || imagePos.Y == heightObj * 7)
            {
                imagePos.Y -= heightObj * 4;
            }
            if (direction == left)
            {
                if (imagePos.Y == heightObj * 2)
                    return new Vector2(0, heightObj * 3);
                else if (imagePos.Y == heightObj * 3)
                {
                    if (imagePos.X == widthImage - widthObj)
                        return new Vector2(0, heightObj * 3);
                    return new Vector2(imagePos.X + widthObj, heightObj * 3);
                }
            }
            else if (direction == right)
            {
                if (imagePos.Y == heightObj * 3)
                    return new Vector2(0, heightObj * 2);
                else if (imagePos.Y == heightObj * 2)
                {
                    if (imagePos.X == widthImage - widthObj)
                        return new Vector2(0, heightObj * 2);
                    return new Vector2(imagePos.X + widthObj, heightObj * 2);
                }
            }
            return new Vector2(0, imagePos.Y);
        }

        public static Vector2 AnimateHurtObject(Vector2 imagePos, int heightObj)
        {
            return new Vector2(imagePos.X, imagePos.Y + heightObj * 4);
        }

        public static Vector2 AnimateButton(
            int widthObj, bool cursorHover)
        {
            if (cursorHover)
                return new Vector2(widthObj, 0);
            return new Vector2(0, 0);
        }
    }
}
