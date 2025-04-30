using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamLongestPEriod.Models
{
    public class Team
    {
        private int employee1ID;

        private int employee2ID;

        private int projectID;

        private int daysWorked;

        public int Employee1ID
        {
            get { return employee1ID; }
            set { employee1ID = value; }
        }

        public int Employee2ID
        {
            get { return employee2ID; }
            set { employee2ID = value; }
        }

        public int ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }
        public int DaysWorked
        {
            get { return daysWorked; }
            set { daysWorked = value; }
        }
    }
}