using System;
using System.Collections.Generic;
using System.Data.Entity;
namespace Models
{
    public class DatabaseContext : DbContext
    {
        static DatabaseContext()
        {
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
    //    public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<Anomaly> Anomalies { get; set; }

        public System.Data.Entity.DbSet<Models.Status> Status { get; set; }

        public System.Data.Entity.DbSet<Models.ReportType> ReportTypes { get; set; }

        public System.Data.Entity.DbSet<Models.Report> Reports { get; set; }

        public System.Data.Entity.DbSet<Models.WbsRequirment> WbsRequirments { get; set; }

        public System.Data.Entity.DbSet<Models.WbsDocument> WbsDocuments { get; set; }

        public System.Data.Entity.DbSet<Models.WbsUserType> WbsUserTypes { get; set; }

        public System.Data.Entity.DbSet<Models.AnomalyHse> AnomalyHses { get; set; }

        public System.Data.Entity.DbSet<Models.AnomalyLevel> AnomalyLevels { get; set; }

        public System.Data.Entity.DbSet<Models.AnomalyResult> AnomalyResults { get; set; }

        public System.Data.Entity.DbSet<Models.RequirmentType> RequirmentTypes { get; set; }

        public System.Data.Entity.DbSet<Models.Requirment> Requirments { get; set; }

        public System.Data.Entity.DbSet<Models.ContractRquirment> ContractRquirments { get; set; }
        public System.Data.Entity.DbSet<Models.RequirmentDetail> RequirmentDetails { get; set; }

        public DbSet<Act> Acts { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<RiskCalculator> RiskCalculators { get; set; }
        public DbSet<RiskControlingWork> RiskControlingWorks { get; set; }
        public DbSet<RiskIntensity> RiskIntensities { get; set; }
        public DbSet<RiskProbability> RiskProbabilities { get; set; }
        public DbSet<RiskStatus> RiskStatuses { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<UserRisk> UserRisks { get; set; }
        public DbSet<UserStage> UserStages { get; set; }
        public DbSet<TempTable> TempTables { get; set; }
        public DbSet<TempTable_Act> TempTable_Acts { get; set; }
        public DbSet<TempTable_Stage> TempTable_Stages { get; set; }
        public DbSet<TempTable_Risk> TempTable_Risks { get; set; }
        public DbSet<TempTable_Control> TempTable_Controls { get; set; }

        public System.Data.Entity.DbSet<Models.RiskMatris> RiskMatris { get; set; }

        public DbSet<CompanyStatusReport> CompanyStatusReports { get; set; }
        public DbSet<Accident> Accidents { get; set; }

        public DbSet<AccidentEmployeeType> AccidentEmployeeTypes { get; set; }
        public DbSet<AccidentInjury> AccidentInjuries { get; set; }
        public DbSet<AccidentPart> AccidentParts { get; set; }
        public DbSet<AccidentReasonAction> AccidentReasonActions { get; set; }
        public DbSet<AccidentReasonCondition> AccidentReasonConditions { get; set; }
        public DbSet<AccidentResult> AccidentResults { get; set; }
        public DbSet<AccidentType> AccidentTypes { get; set; }
        public DbSet<AccidentInjuryRelAccident> AccidentInjuryRelAccidents { get; set; }
        public DbSet<AccidentPartRelAccident> AccidentPartRelAccidents { get; set; }
        public DbSet<AccidentReasonActionRelAccident> AccidentReasonActionRelAccidents { get; set; }
        public DbSet<AccidentReasonConditionRelAccident> AccidentReasonConditionRelAccidents { get; set; }
        public DbSet<AccidentResultRelAccident> AccidentResultRelAccidents { get; set; }
        public DbSet<AccidentTypeRelAccident> AccidentTypeRelAccidents { get; set; }
        public DbSet<AccidentReport> AccidentReports { get; set; }
        public DbSet<AccidentReportRelAccident> AccidentReportRelAccidents { get; set; }
        public DbSet<HseDocument> HseDocuments { get; set; }
        public DbSet<HseDocumentType> HseDocumentTypes { get; set; }
        public DbSet<CompanyHumanResource> CompanyHumanResources { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<PermitType> PermitTypes { get; set; }
        public DbSet<Enviroment> Enviroments { get; set; }
        public DbSet<EnviromentType> EnviromentTypes { get; set; }
        public DbSet<Crisis> Crisises { get; set; }
        public DbSet<CrisisType> CrisisTypes { get; set; }
        public DbSet<PermitStatus> PermitStatuses { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<RelationType> RelationTypes { get; set; }
        public System.Data.Entity.DbSet<Models.UserJobRate> UserJobRates { get; set; }

        public DbSet<Safety> Safeties { get; set; }
        public DbSet<SafetyFileType> SafetyFileTypes { get; set; }
        public DbSet<SafetyType> SafetyTypes { get; set; }
    }
}
