using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu.Entities
{
    public class Room
    {
        private int mX;
        private int mY;
        private string mName;
        private string mNumber;

        public Room(string name, string number, int x, int y)
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

        public string Number
        {
            get { return this.mNumber; }
            set { this.mNumber = value; }
        }
    }
}
