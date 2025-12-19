using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

using System.Linq;
using System.Web;
using System.Data;
using System.Drawing.Printing;

namespace AttendanceManagementSystem.Models
{
    public class TADContext : DbContext
    {
        public TADContext()
           : base("DMSConnection")
        {
        }
        public DbSet<ClientInfo> ClientInfos { get; set; }
        public DbSet<Monitoring_UnitNo> Monitoring_UnitNos { get; set; }

        [Table("ClientInfo")]
        public class ClientInfo
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int id { get; set; }
            public string firstname { get; set; }
            public string middlename { get; set; }
            public string lastname { get; set; }
            public string addedby { get; set; }
            public string updatedby { get; set; }
            public DateTime? dateadded { get; set; }
            public DateTime? dateupdated { get; set; }
            public int status { get; set; }
        }
        [Table("Monitoring_UnitNo")]
        public class Monitoring_UnitNo
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int id { get; set; }
            public string unit_no { get; set; }
            public string status { get; set; }
            public string addedby { get; set; }
            public string updatedby { get; set; }
            public DateTime? dateadded { get; set; }
            public DateTime? dateupdated { get; set; }
        }

    }

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Docs> Docss { get; set; }
        public DbSet<ClientInfoStatus> Account { get; set; }
        public DbSet<PreBIR> Prebir { get; set; }
        public DbSet<Project> project { get; set; }
        public DbSet<Location> location { get; set; }
        public DbSet<RDLocation> rdlocation { get; set; }
        public DbSet<AssessorLocation> asseslocation { get; set; }
        public DbSet<Financing> financings { get; set; }
        public DbSet<Client> client { get; set; }
        public DbSet<ClientDocs> clientdocs { get; set; }
        public DbSet<BIR> bir { get; set; }
        public DbSet<Pre_ROD> prerod { get; set; }
        public DbSet<ROD> rods { get; set; }
        public DbSet<PRE_ASSESSOR> preassessors { get; set; }
        public DbSet<Assessor> assessors { get; set; }
        public DbSet<TurnOver> turnovers { get; set; }
        public DbSet<Document> documents { get; set; }
        public DbSet<ProjectDocs> projectdocs { get; set; }
        public DbSet<Cancel> cancels { get; set; }
        public DbSet<PullOut> pullouts { get; set; }
        public DbSet<Remarks> remarks { get; set; }
        public DbSet<ExportExcel> exportExcels { get; set; }
        public DbSet<Year> years { get; set; }
        public DbSet<AgingReport> agingreports { get; set; }
        public DbSet<TotalDaysCount> totaldayscounts { get; set; }
        
        public DbSet<Ziongoal> Ziongoals { get; set; }
        public DbSet<Zionbrother> Zionbrothers { get; set; }
        public DbSet<ZionMajorActivity> ZionMajorActivitys { get; set; }
        public DbSet<ZionP0sitionHolder> ZionP0sitionHolders { get; set; }
        public DbSet<ZionAttendance> ZionAttendances { get; set; }
        public DbSet<ZionGroupName> ZionGroupNames { get; set; }
        public DbSet<ZionGrouped> ZionGroupeds { get; set; }
        
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class DirectoryPicture
    {
        public HttpPostedFileBase imageforpreview { get; set; } //directory image
    }

    [Table("employees")]
    public class Employee
    {
        [Key]
        public string idno { get; set; }
        public string fname { get; set; }
        public string company { get; set; }
        public string gender { get; set; }
        public DateTime? date_hired { get; set; }
        public string emp_status { get; set; }
        public string dept { get; set; }
        public string email { get; set; }
        public string designation { get; set; }
        public DateTime? date_created { get; set; }
        public string created_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
    }

    [Table("Users")]
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string fname { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string email { get; set; }
        public int accesstype { get; set; }        
        public DateTime? datecreated { get; set; }
        public string createdby { get; set; }
        public DateTime? dateupdated { get; set; }
        public string updatedby { get; set; }
        public int status { get; set; }
        public byte[] photopicture { get; set; }
    }

    [Table("tbl_Docs")]
    public class Docs
    {
        [Key]
        public int id { get; set; }
        public string developer { get; set; }
        public string create_by { get; set; }
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{mm/dd/yyyy}")]
        public DateTime? date_created { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("Client_Status")]
    public class ClientInfoStatus
    {
        [Key]
        public int id { get; set; }
        public string account { get; set; }
        public string generatedcode { get; set; }
        public string project { get; set; }
        public string company_name { get; set; }
        public string location { get; set; }
        public string rd_location { get; set; }
        public string ass_location { get; set; }
        public string financing { get; set; }
        public DateTime? notice_of_fullpayment_date { get; set; }
        public string unit_no { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? dateupdate_prebir { get; set; }
        public string updated_by_prebir { get; set; }
        public DateTime? dateupdate_bir { get; set; }
        public string updated_by_bir { get; set; }
        public DateTime? dateupdate_prerod { get; set; }
        public string updated_by_prerod { get; set; }
        public DateTime? dateupdate_rod { get; set; }
        public string updated_by_rod { get; set; }
        public DateTime? dateupdate_preassessor { get; set; }
        public string updated_by_preassessor { get; set; }
        public DateTime? dateupdate_assessor { get; set; }
        public string updated_by_assessor { get; set; }
        public string dateupdate_TurnOver { get; set; }
        public string updated_by_TurnOver { get; set; }
        public DateTime? date_Turn_over_Done { get; set; }
        public string date_Turn_over_by_Done { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_PRE_BIR")]
    public class PreBIR
    {
        [Key]        
        public int id { get; set; }        
        public string name { get; set; }
        public string generatedcode { get; set; }
        public string block { get; set; }
        public DateTime? unit_no { get; set; }
        public string tcp { get; set; }
        public string title_no { get; set; }
        public string title_no_hide { get; set; }
        public string title_no_hide_one { get; set; }
        public string title_no_hide_two { get; set; }
        public string title_no_hide_tree { get; set; }
        public string title_no_hide_four { get; set; }
        public string tax_dec_no { get; set; }
        public string tax_dec_no_hide { get; set; }
        public string tax_dec_no_hide_one { get; set; }
        public string tax_dec_no_hide_two { get; set; }
        public string tax_dec_no_hide_tree { get; set; }
        public string tax_dec_no_hide_four { get; set; }
        public DateTime? doas { get; set; }
        public DateTime? dst { get; set; }
        public DateTime? notarized_doas { get; set; }
        public DateTime? transfer_tax { get; set; }
        public DateTime? ewt { get; set; }
        public DateTime? cts { get; set; }
        public DateTime? receipts { get; set; }
        public int status { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public string remarks { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public DateTime? date_approve { get; set; }
        public string approve_by { get; set; }
    }

    [Table("tbl_Project")]
    public class Project
    {
        [Key]
        public int id { get; set; }
        public string project { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_Location")]
    public class Location
    {
        [Key]
        public int id { get; set; }
        public string location { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_RD_location")]
    public class RDLocation
    {
        [Key]
        public int id { get; set; }
        public string rd_location { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_Assessor_location")]
    public class AssessorLocation
    {
        [Key]
        public int id { get; set; }
        public string assessor_location { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_Financing")]
    public class Financing
    {
        [Key]
        public int id { get; set; }
        public string financing { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_Client")]
    public class Client
    {
        [Key]
        public int id { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string unit_no { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? dateupdate { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("master_DOCS")]
    public class ClientDocs
    {
        [Key]
        public int id { get; set; }
        public int customer_id { get; set; }
        public string generatedcode { get; set; }
        public int status { get; set; }
        public int idpre_bir { get; set; }
        public int idbir { get; set; }
        public int idpre_rod { get; set; }
        public int idrod { get; set; }
        public int idpre_assessor { get; set; }
        public int idassessor { get; set; }
        public DateTime? date_added { get; set; }
        public string added_by { get; set; }
        public DateTime? date_update { get; set; }
        public string updated_by { get; set; }
    }

    [Table("tbl_BIR")]
    public class BIR
    {
        [Key]
        public int id { get; set; }
        public string account { get; set; }
        public string generatedcode { get; set; }
        public DateTime? submission { get; set; }
        public DateTime? released { get; set; }
        public string bir_location { get; set; }
        public string rod_location { get; set; }
        public string assessor_location { get; set; }
        public string ecar_no { get; set; }
        public string ecar_no_hide { get; set; }
        public string ecar_no_hide_one { get; set; }
        public string ecar_no_hide_two { get; set; }
        public string ecar_no_hide_tree { get; set; }
        public string ecar_no_hide_four { get; set; }
        public DateTime? ecar_expirry_date { get; set; }
        public string added_by { get; set; }
        public DateTime? dateadded { get; set; }
        public int status { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public string remarks { get; set; }
    }

    [Table("tbl_PRE_ROD")]
    public class Pre_ROD
    {
        [Key]
        public int id { get; set; }
        public string accountname { get; set; }
        public string generatedcode { get; set; }
        public DateTime? title_request_ate { get; set; }
        public DateTime? title_availability_date { get; set; }
        public DateTime? date_completed { get; set; }
        public string addedby { get; set; }
        public DateTime? dateadded { get; set; }
        public int ecar { get; set; }
        public int doas { get; set; }
        public int dor { get; set; }
        public int sec_cert { get; set; }
        public int td_ctc { get; set; }
        public int spa { get; set; }
        public int t_tax { get; set; }
        public int lma { get; set; }
        public int title { get; set; }
        public int tc { get; set; }
        public int bir { get; set; }
        public int sec_cert_bylaws_sec_reg { get; set; }
        public int status { get; set; }
        public string remarks { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
    }

    [Table("tbl_ROD")]
    public class ROD
    {
        [Key]
        public int id { get; set; }
        public string account { get; set; }
        public string generatedcode { get; set; }
        public DateTime? date_submitted { get; set; }
        public DateTime? date_released { get; set; }
        public DateTime? mortgage_sub { get; set; }
        public DateTime? mortgage_rel { get; set; }
        public string bir_location { get; set; }
        public string rod_location { get; set; }
        public string assessor_location { get; set; }
        public string epeb_no { get; set; }
        public string eped_no_hide { get; set; }
        public string eped_no_hide_one { get; set; }
        public string eped_no_hide_two { get; set; }
        public string eped_no_hide_tree { get; set; }
        public string eped_no_hide_four { get; set; }
        public string title_no { get; set; }
        public string title_no_hide { get; set; }
        public string title_no_hide_one { get; set; }
        public string title_no_hide_two { get; set; }
        public string title_no_hide_tree { get; set; }
        public string title_no_hide_four { get; set; }
        public string addedby { get; set; }
        public DateTime? dateadded { get; set; }        
        public int status { get; set; }
        public DateTime? date_update { get; set; }
        public string updated_by { get; set; }   
        public string remarks { get; set; }
    }

    [Table("tbl_PRE_ASSESSOR")]
    public class PRE_ASSESSOR
    {
        [Key]
        public int id { get; set; }
        public string account { get; set; }
        public string generatedcode { get; set; }
        public DateTime? date_completed { get; set; }
        public int ecar { get; set; }
        public int tax_dec { get; set; }
        public int tax_clearance { get; set; }
        public int doas { get; set; }
        public int title { get; set; }
        public int transfer_tax { get; set; }
        public int sworn_statement { get; set; }
        public int sec_cert { get; set; }
        public int picture { get; set; }
        public int management_cer { get; set; }
        public int cover_page { get; set; }
        public int others { get; set; }
        public string remarks { get; set; }
        public int status { get; set; }
        public DateTime? date_added { get; set; }
        public string added_by { get; set; }
        public DateTime? date_update { get; set; }
        public string updated_by { get; set; }
    }

    [Table("tbl_ASSESSOR")]
    public class Assessor
    {
        [Key]
        public int id { get; set; }
        public string account { get; set; }
        public string generatedcode { get; set; }
        public string tax_dec_no { get; set; }
        public string bir_location { get; set; }
        public string rod_location { get; set; }
        public string assessor_location { get; set; }
        public string tax_dex_no_land { get; set; }
        public string tax_dex_no_land_two { get; set; }
        public string tax_dex_no_land_tree { get; set; }
        public string tax_dex_no_land_four { get; set; }

        public string land_unit_no { get; set; }
        public string tax_dex_no_building { get; set; }
        public string tax_dex_no_building_two { get; set; }
        public string tax_dex_no_building_tree { get; set; }
        public string tax_dex_no_building_four { get; set; }

        public DateTime? date_released { get; set; }
        public int status { get; set; }
        public string added_by { get; set; }
        public DateTime? datea_dded { get; set; }
        public string remarks { get; set; }
    }

    [Table("tbl_Turn_Over")]
    public class TurnOver
    {
        [Key]
        public int id { get; set; }
        public string account { get; set; }
        public string unit_no { get; set; }
        public string project { get; set; }
        public string generatedcode { get; set; }
        public string document_name { get; set; }
        public string title_no { get; set; }
        public string tax_dec_no { get; set; }
        public DateTime? date_forwarded { get; set; }
        public string received_by { get; set; }
        public string turn_over_by { get; set; }
        public string remarks { get; set; }
        public string doc_path { get; set; }
        public DateTime? date_added { get; set; }
        public string added_by { get; set; }
        public int status { get; set; }
    }

    [Table("Document")]
    public class Document
    {
        [Key]
        public int id { get; set; }
        public string accountname { get; set; }
        public string generatedcode { get; set; }
        public string documentname { get; set; }
        public string document_file_name { get; set; }
        public string project { get; set; }
        public string unitno { get; set; }
        public int status { get; set; }
        public string doc_path { get; set; }
        public string remarks { get; set; }
        public DateTime? dateadded { get; set; }
        public string addedby { get; set; }
        public string updatedby { get; set; }
        public DateTime? dateupdated { get; set; }
        public string file_extension { get; set; }
        public int filestatus { get; set; }
    }

    [Table("tbl_ProjectDocs")]
    public class ProjectDocs
    {
        [Key]
        public int id { get; set; }
        public string project { get; set; }
        public string documents { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public DateTime? dateupdate { get; set; }
        public string updated_by { get; set; }
        public DateTime? deletedate { get; set; }
        public string deletedby { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_Cancel")]
    public class Cancel
    {
        [Key]
        public int id { get; set; }
        public string generatedcode { get; set; }
        public string accountname { get; set; }
        public string document_attach { get; set; }
        public string remarks { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_Pull_Out")]
    public class PullOut
    {
        [Key]
        public int id { get; set; }
        public string generatedcode { get; set; }
        public string accountname { get; set; }
        public string document_attach { get; set; }
        public string remarks { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_Remarks")]
    public class Remarks
    {
        [Key]
        public int id { get; set; }
        public string remarks { get; set; }
        public string block_prebir { get; set; }
        public string datesub_bir { get; set; }
        public string titlerequestdate_prerod { get; set; }
        public string epedno_rod { get; set; }
        public string datesub_preasses { get; set; }
        public string converttaxdec_ass { get; set; }
        public DateTime? dateadded { get; set; }
        public string added_by { get; set; }   
        public DateTime? dateupdated { get; set; }
        public string updated_by { get; set; }
        public string generatedcode { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_ExportExcel")]
    public class ExportExcel
    {
        [Key]
        public int id { get; set; }
        public string generatedcode { get; set; }
        public string customername { get; set; }
        public string unitno { get; set; }
        public string project { get; set; }
        public string financing { get; set; }
        public DateTime? notice_of_fullpayment_date { get; set; }
        public string fullpamentdate { get; set; }
        public string tcp { get; set; }
        public string titleno { get; set; }
        public string taxdecno { get; set; }
        public string doasdate { get; set; }
        public string dstrequestdate { get; set; }
        public string notarizeddoasdate { get; set; }
        public string transfertaxrequdate { get; set; }
        public string ewtdate { get; set; }
        public string ctsdate { get; set; }
        public string datereceiptreceived { get; set; }
        public string remarks1 { get; set; }
        public string titlerequestdate { get; set; }
        public string titleavailabilitydate { get; set; }
        public string datecompleted { get; set; }
        public string remarks2 { get; set; }
        public string datesubmitted { get; set; }
        public string remarks3 { get; set; }
        public string documentstatus { get; set; }        
        public int status { get; set; }        
    }

    [Table("tbl_Years")]
    public class Year
    {
        [Key]
        public string year { get; set; }
    }

    [Table("tbl_Aging_Report")]
    public class AgingReport
    {
        [Key]
        public int id { get; set; }
        public string account { get; set; }
        public string generatedcode { get; set; }
        public string unitno { get; set; }
        public string financing { get; set; }
        public string month_notice_of_fullpayment_date { get; set; }
        public string year_notice_of_fullpayment_date { get; set; }
        public DateTime? notice_of_fullpayment_date { get; set; }

        public string monthsubpro { get; set; }
        public string yearsubpro { get; set; }
        public DateTime? fullpayment_date_prebir { get; set; }

        public string yearsubmissiondateprojected30 { get; set; }
        public string monthsubmissiondateprojected30 { get; set; }
        public DateTime? submissiondateprojected30 { get; set; }

        public string monthsubactbir { get; set; }
        public string yearsubactbir { get; set; }
        public DateTime? submission_date_bir { get; set; }

        public string monthbirreaprojected90 { get; set; }
        public string yearbirbirreaprojected90 { get; set; }
        public DateTime? birrealeasedprojected90 { get; set; }

        public string monthreabir { get; set; }
        public string yearreabir { get; set; }
        public DateTime? released_date_bir { get; set; }

        public string mo_prerodsubprojected45 { get; set; }
        public string yr_prerodsubprojected45 { get; set; }
        public DateTime? prerodsubprojected45 { get; set; }

        public string mo_rodsubprojected60 { get; set; }
        public string yr_rodsubprojected60 { get; set; }
        public DateTime? rodsubprojected60 { get; set; }

        public string mo_preasssubprojected15 { get; set; }
        public string yr_preasssubprojected15 { get; set; }
        public DateTime? preasssubprojected15 { get; set; }

        public string mo_assrelprojected60 { get; set; }
        public string yr_assrelprojected60 { get; set; }
        public DateTime? assrelprojected60 { get; set; }

        public string monthsubrod { get; set; }
        public string yearsubrod { get; set; }
        public DateTime? date_submitted_rod { get; set; }

        public string monthrearod { get; set; }
        public string yearrearod { get; set; }
        public DateTime? date_released_rod { get; set; }

        public string monthsubpreass { get; set; }
        public string yearsubprea { get; set; }
        public DateTime? date_submitted_preassessors { get; set; }

        public string monthreaass { get; set; }
        public string yearreaass { get; set; }
        public DateTime? date_released_assessors { get; set; }
        public int status { get; set; }
    }

    [Table("tbl_TotalDaysCount")]
    public class TotalDaysCount
    {
        [Key]
        public int id { get; set; }
        public int ninetydays { get; set; }
        public int oneeighty { get; set; }
        public int treesixfive { get; set; }
        public int sevintreeziro { get; set; }
        public int sevintreeone { get; set; }
    }

    [Table("zion_goal")]
    public class Ziongoal
    {
        [Key]
        public int id { get; set; }
        public string onex_attendace { get; set; }
        public string fourx_attendace { get; set; }
        public string tithes { get; set; }
        public string baptism { get; set; }
        public DateTime? date_created { get; set; }
        public string created_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public int status { get; set; }
    }

    [Table("zion_member")]
    public class Zionbrother
    {
        [Key]
        public int id { get; set; }
        public string generatedcode { get; set; }
        public string full_name { get; set; }
        public string life_number { get; set; }
        public string gender { get; set; }
        public DateTime? date_created { get; set; }
        public string created_by { get; set; }
        public DateTime? date_updated { get; set; }
        public string updated_by { get; set; }
        public int status { get; set; }
    }

    [Table("zion_position_holder")]
    public class ZionP0sitionHolder
    {
        [Key]
        public int id { get; set; }
        public string generated_code { get; set; }
        public int member_id { get; set; }
        public string full_name { get; set; }
        public string life_number { get; set; }
        public string position { get; set; }
        public DateTime? date_created { get; set; }
        public string created_by { get; set; }
        public DateTime? date_update { get; set; }
        public string update_by { get; set; }
        public DateTime? date_deleted { get; set; }
        public string deleted_by { get; set; }
        public int status { get; set; }
    }

    [Table("zion_attendance")]
    public class ZionAttendance
    {
        [Key]
        public int id { get; set; }
        public string generated_code { get; set; }
        public string grouped_gencode { get; set; }
        public string members_gencode { get; set; }
        public DateTime? date { get; set; }
        public decimal total_members { get; set; }

        public string first_week { get; set; }
        public decimal t_day_worship_first_week { get; set; }
        public decimal s_m_worship_first_week { get; set; }
        public decimal s_a_worship_first_week { get; set; }
        public decimal s_e_worship_first_week { get; set; }

        public string second_week { get; set; }
        public decimal t_day_worship_second_week { get; set; }
        public decimal s_m_worship_second_week { get; set; }
        public decimal s_a_worship_second_week { get; set; }
        public decimal s_e_worship_second_week { get; set; }

        public string therd_week { get; set; }
        public decimal t_day_worship_therday_week { get; set; }
        public decimal s_m_worship_therday_week { get; set; }
        public decimal s_a_worship_therday_week { get; set; }
        public decimal s_e_worship_therday_week { get; set; }

        public string forth_week { get; set; }
        public decimal t_day_worship_forth_week { get; set; }
        public decimal s_m_worship_forth_week { get; set; }
        public decimal s_a_worship_forth_week { get; set; }
        public decimal s_e_worship_forth_week { get; set; }

        public string fifth_week { get; set; }
        public decimal t_day_worship_fifth_week { get; set; }
        public decimal s_m_worship_fifth_week { get; set; }
        public decimal s_a_worship_fifth_week { get; set; }
        public decimal s_e_worship_fifth_week { get; set; }

        public DateTime? date_created { get; set; }
        public string created_by { get; set; }
        public DateTime? date_update { get; set; }
        public string update_by { get; set; }
        public DateTime? date_delete { get; set; }
        public string deleted_by { get; set; }
        public int status { get; set; }
    }

    public class AttendanceWithCount
    {
        public string GroupedGencode { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    [Table("zion_mojor_activity")]
    public class ZionMajorActivity
    {
        [Key]
        public int id { get; set; }
        public string activity_code { get; set; }
        public string activity_title { get; set; }
        public string activity_purpose { get; set; }
        public DateTime? date_created { get; set; }
        public string created_by { get; set; }
    }

    [Table("zion_group_name")]
    public class ZionGroupName
    {
        [Key]
        public int id { get; set; }
        public string gencode { get; set; }
        public string group_name { get; set; }
        public string leader_gencode { get; set; }
        public int total_members { get; set; }
        public DateTime? date_create { get; set; }
        public string created_by { get; set; }
        public DateTime? date_update { get; set; }
        public string update_by { get; set; }
        public DateTime? date_delete { get; set; }
        public string deleted_by { get; set; }
        public int status { get; set; }
    }

    [Table("zion_grouped")]
    public class ZionGrouped
    {
        [Key]
        public int id { get; set; }
        public string generatedcode { get; set; }
        public string group_name_gencode { get; set; }
        public DateTime? date_create { get; set; }
        public string created_by { get; set; }
        public DateTime? date_update { get; set; }
        public string updated_by { get; set; }
        public DateTime? date_delete { get; set; }
        public string deleted_by { get; set; }
        public int status { get; set; }
    }

    public class GetMonthAndYear
    {
        public int Year { get; set; }
        public string Months { get; set; }
        public int NoticeOfFullpaymentDate { get; set; }
        public int FullpaymentDatePrebir { get; set; }
        public int SubmissionDateProjected30 { get; set; }
        public int SubmissionDateBir { get; set; }
        public int BirReleasedProjected90 { get; set; }
        public int ReleasedDateBir { get; set; }
        public int PreRodSubProjected45 { get; set; }
        public int DateSubmittedRod { get; set; }
        public int RodSubProjected60 { get; set; }
        public int DateReleasedRod { get; set; }
        public int PreAssSubProjected15 { get; set; }
        public int DateSubmittedPreAssessors { get; set; }
        public int AssRelProjected60 { get; set; }
        public int DateReleasedAssessors { get; set; }
        public int TotalMonth { get; set; }
    }

    public class AgingTable
    {
        public string ACCOUNT_NAME { get; set; }
        public string UNIT_NO { get; set; }
        public string FINANCING { get; set; }

        public DateTime? PRE_BIR_PROJECTED { get; set; }
        public DateTime? PRE_BIR_ACTUAL { get; set; }
        public string PRE_BIR_STATUS { get; set; }

        public DateTime? BIR_SUBMISSION_PRJECTED { get; set; }
        public DateTime? BIR_SUBMISSION_ACTUAL { get; set; }
        public string BIR_SUBMISSION_STATUS { get; set; }

        public DateTime? BIR_RELEASE_PROJECTED { get; set; }
        public DateTime? BIR_RELEASE_ACTUAL { get; set; }
        public string BIR_RELEASE_STATUS { get; set; }

        public DateTime? PRE_ROD_SUBMISION_PROJECTED { get; set; }
        public DateTime? PRE_ROD_SUBMISION_ACTUAL { get; set; }
        public string PRE_ROD_SUBMISSION_STATUS { get; set; }

        public DateTime? ROD_RELEASE_PROJECTED { get; set; }
        public DateTime? ROD_RELEASE_ACTUAL { get; set; }
        public string ROD_RELEASE_STATUS { get; set; }

        public DateTime? PRE_ASSESSOR_SUBMISSION_PROJECTED { get; set; }
        public DateTime? PRE_ASSESSOR_SUBMISSION_ACTUAL { get; set; }
        public string PRE_ASSESSOR_SUBMISSION_STATUS { get; set; }

        public DateTime? ASSESSOR_RELEASE_PROJECTED { get; set; }
        public DateTime? ASSESSOR_RELEASE_ACTUAL { get; set; }
        public string ASSESSOR_RELEASE_STATUS { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        public string UserAccount { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Access Type")]
        public int AccessType { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
