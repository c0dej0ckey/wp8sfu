using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Course
    {
        private string mClassName;
        private string mSection;
        private string mCredits;
        private string mStatus;
        private string mInstructor;
        private string mType;
        private List<CourseOffering> mCourseOfferings;

        public Course(string className, string section, string credits, string status, string instructor, string type)
        {
            this.ClassName = className;
            this.Section = section;
            this.Status = status;
            this.Credits = credits;
            this.Instructor = instructor;
            this.Type = type;
            this.CourseOfferings = new List<CourseOffering>();
        }

        [JsonProperty]
        public string ClassName
        {
            get { return this.mClassName; }
            set { this.mClassName = value; }
        }

        [JsonProperty]
        public string Section
        {
            get { return this.mSection; }
            set { this.mSection = value; }
        }

        [JsonProperty]
        public string Credits
        {
            get { return this.mCredits; }
            set { this.mCredits = value; }
        }

        [JsonProperty]
        public string Status
        {
            get { return this.mStatus; }
            set { this.mStatus = value; }
        }

        [JsonProperty]
        public string Instructor
        {
            get { return this.mInstructor; }
            set { this.mInstructor = value; }
        }

        [JsonProperty]
        public string Type
        {
            get { return this.mType; }
            set { this.mType = value; }
        }

        [JsonProperty]
        public List<CourseOffering> CourseOfferings
        {
            get { return this.mCourseOfferings; }
            set { this.mCourseOfferings = value; }
        }

        public void AddCourseOffering(CourseOffering courseOffering)
        {
            mCourseOfferings.Add(courseOffering);
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CourseOffering
    {
        private string mStartTime;
        private string mEndTime;
        private string mLocation;
        private string mDays;
        private string mDate;

        public CourseOffering(string start, string end, string loc, string days, string date)
        {
            this.StartTime = start;
            this.EndTime = end;
            this.Location = loc;
            this.Days = days;
            this.Date = date;
        }

        [JsonProperty]
        public string StartTime
        {
            get { return this.mStartTime; }
            set { this.mStartTime = value; }
        }

        [JsonProperty]
        public string EndTime
        {
            get { return this.mEndTime; }
            set { this.mEndTime = value; }
        }

        [JsonProperty]
        public string Location
        {
            get { return this.mLocation; }
            set { this.mLocation = value; }
        }

        [JsonProperty]
        public string Days
        {
            get { return this.mDays; }
            set { this.mDays = value; }
        }

        [JsonProperty]
        public string Date
        {
            get { return this.mDate; }
            set { this.mDate = value; }
        }
    }
}
