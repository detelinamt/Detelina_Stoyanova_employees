using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamLongestPEriod.Models
{
    public class Employee
    {
        private string employeeID;

        private int? projectID;

        private DateTime? dateFrom;

        private DateTime? dateTo;

        public string EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }
        public int? ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }
        public DateTime? DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }
        public DateTime? DateTo
        {
            get { return dateTo; }
            set { dateTo = value; }
        }
    }
}