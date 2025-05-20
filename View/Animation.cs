using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.View
{
    public static class Animation
    {
        public static Vector2 AnimateObject(int widthObj, int heightObj, int widthImage, int heightImage, Vector2 imagePos, Vector2 possitionDifference)
        {
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
                            return new Vector2(widthObj, heightObj);
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
                            return new Vector2(widthObj, 0);
                        return new Vector2(imagePos.X + widthObj, 0);
                    }
                }
                return new Vector2(0, imagePos.Y);
            }
        }
    }
}
