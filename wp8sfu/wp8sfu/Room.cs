using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu
{
    public class Room
    {
        private int mX;
        private int mY;
        private string mName;
        private int mNumber;

        public Room(string name, int number, int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Name = name;
            this.Number = number;
        }

        public int X
        {
            get { return this.mX; }
            set { this.mX = value; }
        }

        public int Y
        {
            get { return this.mY; }
            set { this.mY = value; }

        }

        public string Name
        {
            get { return this.mName; }
            set { this.mName = value; }
        }

        public int Number
        {
            get { return this.mNumber; }
            set { this.mNumber = value; }
        }
    }
}
