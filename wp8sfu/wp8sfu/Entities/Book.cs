using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu.Entities
{
    public class Book
    {
        private string mClassNumber;
        private string mClassName;
        private string mTitle;
        private string mAuthor;
        private string mStatus;
        private string mIsbn;
        private float mNewPrice;
        private float mUsedPrice;

        public Book(string className, string classNumber, string title, string author, string status, string isbn, float newPrice, float usedPrice)
        {
            this.ClassName = className;
            this.ClassNumber = classNumber;
            this.Title = title;
            this.Author = author;
            this.Status = status;
            this.Isbn = isbn;
            this.NewPrice = newPrice;
            this.UsedPrice = usedPrice;
        }

        public string ClassName
        {
            get { return this.mClassName; }
            set { this.mClassName = value; }
        }

        public string ClassNumber
        {
            get { return this.mClassNumber; }
            set { this.mClassNumber = value; }
        }

        public string Title
        {
            get { return this.mTitle; }
            set { this.mTitle = value; }
        }

        public string Author
        {
            get { return this.mAuthor; }
            set { this.mAuthor = value; }
        }

        public string Status
        {
            get { return this.mStatus; }
            set { this.mStatus = value; }
        }

        public string Isbn
        {
            get { return this.mIsbn; }
            set { this.mIsbn = value; }
        }

        public float NewPrice
        {
            get { return this.mNewPrice; }
            set { this.mNewPrice = value; }
                
        }

        public float UsedPrice
        {
            get { return this.mUsedPrice; }
            set { this.mUsedPrice = value; }
        }
    }
}
