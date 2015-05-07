using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gyroMove
{
    class Globos
    {
        ContentManager content;
        private Globo[] ballonsArray;

        public Globos(ContentManager cont)
        {
            this.ballonsArray = new Globo[6];
            this.content = cont;
        }

        public void initGlobos()
        {
            for (int i = 0; i < this.ballonsArray.Length; i++)
            {
                Random rnd = new Random();
                int posX = rnd.Next(0, 600);
                int posY = rnd.Next(0, 400);
                this.ballonsArray[i] = new Globo(this.content.Load<Texture2D>("globo" + (i + 1)), new Rectangle(posX, posY, 100,100));
            }
        }

        public void setGlobos()
        {
            this.ballonsArray = this.numberOfBallons();
        }

        public void drawGlobos(SpriteBatch b)
        {
            foreach (Globo globo in ballonsArray)
            {
                if (!globo.isDestroyed())
                {
                    globo.drawGlobo(b);
                }
            }
        }

        public Globo[] getBallons()
        {
            return this.ballonsArray;
        }

        public Globo[] numberOfBallons()
        {
            int c = 0;

            foreach (Globo globo in this.ballonsArray)
            {
                if (!(globo.isDestroyed()))
                {
                    c++;
                }
            }

            Globo[] ballons = new Globo[c];
            int k = 0;
            for (int j = 0; j < ballons.Length; j++)
            {
                for (int i = (j + k); i < this.ballonsArray.Length; i++)
                {
                    if (!(this.ballonsArray[i].isDestroyed()))
                    {
                        ballons[j] = this.ballonsArray[i];
                        break;
                     }
                    k++;
                }
            }
            return ballons;
        }

    }
}
