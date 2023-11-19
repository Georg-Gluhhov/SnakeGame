using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snäke
{
    class DynamicPointCreator
    {
        int mapWidht;
        int mapHeight;
        Random random = new Random();

        public DynamicPointCreator(int mapWidth, int mapHeight)
        {
            this.mapHeight = mapHeight;
            this.mapWidht = mapWidth;

        }
        public Point CreatePoint(char sym)
        {
            int x = random.Next(2, mapWidht - 2);
            int y = random.Next(2, mapHeight - 2);
            return new Point(x, y, sym);
        }
    }

}
