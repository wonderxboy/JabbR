using System;
using System.Collections.Generic;

namespace JabbR.Models
{
    public class Employee
    {
        public Employee()
        {
            //this.AccessRequests = new List<AccessRequest>();
            //this.Applications = new List<Application>();
            //this.Applications1 = new List<Application>();
            //this.AppRequests = new List<AppRequest>();
            //this.SecurityInstances = new List<SecurityInstance>();
            this.Employees = new List<Employee>();
        }

        public int DirectoryId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string UserId { get; set; }
        public string EmployeeStatus { get; set; }
        public string RecordType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Nullable<int> BudgetId { get; set; }
        public string JobItemNo { get; set; }
        public string JobTitle { get; set; }
        public string WorkPhone { get; set; }
        public string WorkExtension { get; set; }
        public string EngineerNumber { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string DivisionDistrictIndicator { get; set; }
        public string CrewNumber { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public Nullable<int> ReportsToId { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
        public Nullable<System.DateTime> DeleteFlagDate { get; set; }
        public Nullable<bool> TerminalServices { get; set; }
        public bool Enabled { get; set; }
        public Nullable<System.DateTime> DateEnabled { get; set; }
        public Nullable<System.DateTime> DateDisabled { get; set; }
        public System.DateTime LastChangeDate { get; set; }
        public string LastChangeUser { get; set; }
        //public virtual ICollection<AccessRequest> AccessRequests { get; set; }
        //public virtual ICollection<Application> Applications { get; set; }
        //public virtual ICollection<Application> Applications1 { get; set; }
        //public virtual ICollection<AppRequest> AppRequests { get; set; }
        //public virtual Budget Budget { get; set; }
        //public virtual Section Section { get; set; }
        //public virtual ICollection<SecurityInstance> SecurityInstances { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual Employee ReportsTo { get; set; }
    }
}
