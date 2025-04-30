using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeamLongestPEriod.Models;

namespace TeamLongestPEriod.Helpers
{
    public class DataHelper
    {
        public static Tuple<List<Employee>, string> ReadCsvFile(Stream fileStream)
        {
            var employees = new List<Employee>();
            var errorMessage = string.Empty;

            try
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    var content = streamReader.ReadToEnd();
                    var test = streamReader.ReadLine();
                    string[] lines = content.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var employeeData = line.Split(',');
                        var dateTo = DateTime.Now;

                        if (employeeData[3] != null)
                        {
                            // Check if DateTo is NULL 
                            if (!employeeData[3].Trim().Equals("NULL", StringComparison.CurrentCultureIgnoreCase))
                            {
                                DateTime? parsedDateTo = DateParser.ParseDateString(employeeData[3].Trim().ToString());
                                dateTo = parsedDateTo != null ? parsedDateTo.Value : DateTime.MinValue;
                            }
                            else
                            {
                                dateTo = DateTime.Now;
                            }
                        }

                        DateTime? parsedDateFrom = DateParser.ParseDateString(employeeData[2].Trim().ToString());
                        var dateFrom = parsedDateFrom != null ? parsedDateFrom.Value : DateTime.MinValue;

                        var employee = new Employee
                        {
                            EmployeeID = employeeData[0] != null ? int.Parse(employeeData[0].ToString()) : 0,
                            ProjectID = employeeData[1] != null ? int.Parse(employeeData[1].Trim().ToString()) : 0,
                            DateFrom = dateFrom,
                            DateTo = dateTo
                        };

                        // If any date could not be parsed, show an error message because days worked could not be 
                        if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
                        {
                            errorMessage = string.Format("The file contains incorrect date formats. Please fix them and try again. \r\n Employee ID: {0}, PRoject ID: {1}", employee.EmployeeID, employee.ProjectID);
                            return Tuple.Create(employees, errorMessage);
                        }

                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return Tuple.Create(employees, errorMessage);
        }

        public static DataTable GetTeams(List<Employee> employees)
        {
            var addedPairs = new List<Team>();
            var dtTeams = new DataTable();
            dtTeams.Columns.Add("Employee ID #1"); 
            dtTeams.Columns.Add("Employee ID #2"); 
            dtTeams.Columns.Add("Project ID"); 
            dtTeams.Columns.Add("Days Worked");

            var projectsGroupsKeys = employees.GroupBy(emp => emp.ProjectID).Where(g => g.Count() > 1).Select(t => t.Key);

            foreach (var key in projectsGroupsKeys)
            {
                var teamEmployees = employees.Where(p => p.ProjectID == key).ToArray();

                object syncLock = new object();
                Parallel.ForEach(teamEmployees, teamEmployee => 
                {
                    lock (syncLock)
                    {
                        var pairMembers = teamEmployees.Where(te => te.EmployeeID != teamEmployee.EmployeeID);

                        foreach (var pairMember in pairMembers)
                        {
                            if (addedPairs.SingleOrDefault(t => t.ProjectID == teamEmployee.ProjectID && t.Employee1ID == pairMember.EmployeeID && t.Employee2ID == teamEmployee.EmployeeID) != null
                                    || addedPairs.SingleOrDefault(t => t.ProjectID == teamEmployee.ProjectID && t.Employee2ID == pairMember.EmployeeID && t.Employee1ID == teamEmployee.EmployeeID) != null)
                                continue;

                            var startDate = teamEmployee.DateFrom > pairMember.DateFrom ? teamEmployee.DateFrom : pairMember.DateFrom;
                            var endDate = teamEmployee.DateTo > pairMember.DateTo ? pairMember.DateTo : teamEmployee.DateTo;

                            // If startDate is greater than the endDate, the employees have worked on the same project but not together in the same time
                            if (startDate.Date < endDate.Date)
                            {
                                var daysWorked = endDate.Subtract(startDate).Days;

                                var team = new Team
                                {
                                    ProjectID = key,
                                    Employee1ID = teamEmployee.EmployeeID,
                                    Employee2ID = pairMember.EmployeeID,
                                    DaysWorked = daysWorked
                                };

                                var addedProject = addedPairs.SingleOrDefault(ap => ap.ProjectID == key);
                                if (addedProject != null)
                                {
                                    if (addedProject.DaysWorked < daysWorked)
                                    {
                                        addedPairs.Remove(addedProject);
                                        addedPairs.Add(team);
                                    }
                                }
                                else
                                {
                                    addedPairs.Add(team);
                                }
                            }
                        }
                    }
                });
            }

            foreach (var pair in addedPairs)
            {
                var row = dtTeams.Rows.Add();
                row["Project ID"] = pair.ProjectID;
                row["Employee ID #1"] = pair.Employee1ID;
                row["Employee ID #2"] = pair.Employee2ID;
                row["Days Worked"] = pair.DaysWorked;
            }

            return dtTeams;
        }
    }
}