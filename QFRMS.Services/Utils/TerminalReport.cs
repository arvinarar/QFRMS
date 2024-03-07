using OfficeOpenXml.Attributes;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Utils
{
    public class TerminalReport
    {
        [Description("Region")]
        public string? I_Region { get; set; }

        [Description("Province")]
        public string? I_Province { get; set; }

        [Description("Congressional District")]
        public string? I_District { get; set; }

        [Description("Municipality City")]
        public string? I_City { get; set; }

        [Description("Name of Provider")]
        public string? I_Name { get; set; }

        [Description("Complete Address")]
        public string? I_Address { get; set; }

        [Description("Type of Provider")]
        public string? I_ProviderType { get; set; }

        [Description("Classification of Provider")]
        public string? I_ProviderClassification { get; set; }

        [Description("Industry Sector of Qualification")]
        public string? C_Sector { get; set; }

        [Description("TVET Program Registration Status")]
        public string? C_Status { get; set; }

        [Description("Qualification Program Title")]
        public string? C_ProgramTitle { get; set; }

        [Description("Cluster")]
        public string? U_Cluster { get; set; }

        [Description("CTPR")]
        public string? C_COPRNo { get; set; }

        [Description("Training Calendar Code")]
        public string? U_CalendarCode { get; set; }

        [Description("Delivery Mode")]
        public string? C_DeliveryMode { get; set; }

        [Description("Last Name")]
        public string? S_LastName { get; set; }

        [Description(   "First Name")]
        public string? S_FirstName { get; set; }

        [Description("Middle Name")]
        public string? S_MiddleName { get; set; }

        [Description("Extension Name")]
        public string? S_ExtensionName { get; set; }

        [Description("ULI")]
        public string? S_ULI { get; set; }

        [Description("Contact Number")]
        public string? S_ContactNo { get; set; }

        [Description("Email Address")]
        public string? S_Email { get; set; }

        [Description("Street No and Street address")]
        public string? S_StreetNo { get; set; }

        [Description("Barangay")]
        public string? S_Barangay { get; set; }

        [Description("Municipality City")]
        public string? S_MunicipalityCity { get; set; }

        [Description("District")]
        public string? S_District { get; set; }

        [Description("Province")]
        public string? S_Province { get; set; }

        [Description("Sex")]
        public string? S_Sex { get; set; }

        [Description("Date of Birth")]
        public string? S_BirthDate { get; set; }

        [Description("Age")]
        public string? S_Age { get; set; }

        [Description("Civil Status")]
        public string? S_CivilStatus { get; set; }

        [Description("Highest Grade Completed")]
        public string? S_HighestGrade { get; set; }

        [Description("Nationality")]
        public string? S_Nationality { get; set; }

        [Description("Classification of Clients")]
        public string? C_ClientClassification { get; set; }

        [Description("Training Status")]
        public string? S_TrainingStatus { get; set; }

        [Description("Type of Scholarships")]
        public string? C_ScholarshipType { get; set; }

        [Description("Voucher Number")]
        public string? B_VoucherNo { get; set; }

        [Description("Date Started")]
        public string? B_DateStart { get; set; }

        [Description("Date Finished")]
        public string? B_DateEnd { get; set; }

        [Description("Date Assessed")]
        public string? U_DateAssessed { get; set; }

        [Description("Assessment Results")]
        public string? U_AssessmentResult { get; set; }

        [Description("Employment Status Before the Training")]
        public string? S_ESBT { get; set; }

        [Description("Date Of Employment")]
        public string? S_DateEmployed { get; set; }

        [Description("Occupation")]
        public string? D_Occupation { get; set; }

        [Description("Name of Employer")]
        public string? D_EmployerName { get; set; }

        [Description("Address")]
        public string? D_EmployerAddress { get; set; }

        [Description("Classification")]
        public string? D_Classification { get; set; }

        [Description("Salary")]
        public string? D_Salary { get; set; }
    }
}
