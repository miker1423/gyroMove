using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gyroMove
{
    class Globo
    {
        Texture2D texture;
        Rectangle pos;
        bool isExploded;

        public Globo(Texture2D text, Rectangle rect)
        {
            this.isExploded = false;
            this.texture = text;
            this.pos = rect;
        }

        public void drawGlobo(SpriteBatch b)
        {
            b.Draw(this.texture, this.pos, Color.White);
        }

        public Rectangle getRect()
        {
            return this.pos;
        }

        public bool isDestroyed()
        {
            return this.isExploded;
        }

        public void destroy()
        {
            this.isExploded = true;
        }

    }
}
