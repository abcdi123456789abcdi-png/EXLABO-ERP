using EXLABO.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EXLABO.Infrastructure.Database
{
    public static class SeedData
    {
        public static void Initialize(EXLABOContext db)
        {
            db.Database.EnsureCreated();

            if (!db.Companies.Any())
            {
                db.Companies.AddRange(
                    new Company { CompanyName = "Sysmex" },
                    new Company { CompanyName = "Siemens" },
                    new Company { CompanyName = "Randox" }
                );
                db.SaveChanges();
            }

            if (!db.Departments.Any())
            {
                db.Departments.AddRange(
                    new Department { DepartmentName = "Hematology" },
                    new Department { DepartmentName = "Coagulation" },
                    new Department { DepartmentName = "Clinical Chemistry" },
                    new Department { DepartmentName = "Hormones" },
                    new Department { DepartmentName = "Blood Gases" },
                    new Department { DepartmentName = "Quality Control" }
                );
                db.SaveChanges();
            }

            if (!db.Analyzers.Any())
            {
                var sysmex = db.Companies.First(c => c.CompanyName == "Sysmex");
                var siemens = db.Companies.First(c => c.CompanyName == "Siemens");
                var coag = db.Departments.First(d => d.DepartmentName == "Coagulation");
                var heme = db.Departments.First(d => d.DepartmentName == "Hematology");
                var chem = db.Departments.First(d => d.DepartmentName == "Clinical Chemistry");

                db.Analyzers.AddRange(
                    new Analyzer { AnalyzerName = "CN-3000", CompanyID = sysmex.CompanyID, DepartmentID = coag.DepartmentID },
                    new Analyzer { AnalyzerName = "CS-1600", CompanyID = sysmex.CompanyID, DepartmentID = coag.DepartmentID },
                    new Analyzer { AnalyzerName = "CA 101", CompanyID = sysmex.CompanyID, DepartmentID = coag.DepartmentID },
                    new Analyzer { AnalyzerName = "CA 104", CompanyID = sysmex.CompanyID, DepartmentID = coag.DepartmentID },
                    new Analyzer { AnalyzerName = "ADVIA 360", CompanyID = siemens.CompanyID, DepartmentID = heme.DepartmentID },
                    new Analyzer { AnalyzerName = "ADVIA 560", CompanyID = siemens.CompanyID, DepartmentID = heme.DepartmentID },
                    new Analyzer { AnalyzerName = "ADVIA 2120i", CompanyID = siemens.CompanyID, DepartmentID = heme.DepartmentID },
                    new Analyzer { AnalyzerName = "Atellica HEMA 580", CompanyID = siemens.CompanyID, DepartmentID = heme.DepartmentID },
                    new Analyzer { AnalyzerName = "Atellica HEMA 570", CompanyID = siemens.CompanyID, DepartmentID = heme.DepartmentID },
                    new Analyzer { AnalyzerName = "Atellica HEMA 530", CompanyID = siemens.CompanyID, DepartmentID = heme.DepartmentID },
                    new Analyzer { AnalyzerName = "Atellica HEMA 520", CompanyID = siemens.CompanyID, DepartmentID = heme.DepartmentID },
                    new Analyzer { AnalyzerName = "Dimension EXL", CompanyID = siemens.CompanyID, DepartmentID = chem.DepartmentID },
                    new Analyzer { AnalyzerName = "Atellica Solution", CompanyID = siemens.CompanyID, DepartmentID = chem.DepartmentID },
                    new Analyzer { AnalyzerName = "Atellica CI", CompanyID = siemens.CompanyID, DepartmentID = chem.DepartmentID }
                );
                db.SaveChanges();
            }

            if (!db.RandoxPrograms.Any()) SeedRandoxPrograms(db);
            if (!db.Customers.Any()) SeedCustomers(db);
            if (!db.Items.Any()) SeedItemsAndLots(db);
        }

        private static void SeedRandoxPrograms(EXLABOContext db)
        {
            db.RandoxPrograms.AddRange(
                new RandoxProgram { ProgramName = "General Clinical Chemistry", ProgramCode = "RQ9112", Category = "Clinical Chemistry" },
                new RandoxProgram { ProgramName = "Lipid Programme", ProgramCode = "RQ9126", Category = "Clinical Chemistry" },
                new RandoxProgram { ProgramName = "Cardiac Programme", ProgramCode = "RQ9127", Category = "Clinical Chemistry" },
                new RandoxProgram { ProgramName = "Immunoassay Programme", ProgramCode = "RQ9125", Category = "Immunoassay" },
                new RandoxProgram { ProgramName = "Haematology Programme", ProgramCode = "RQ9118", Category = "Hematology" },
                new RandoxProgram { ProgramName = "Coagulation Programme", ProgramCode = "RQ9135", Category = "Hematology" },
                new RandoxProgram { ProgramName = "HbA1c Programme", ProgramCode = "RQ9129", Category = "Immunoassay" },
                new RandoxProgram { ProgramName = "Specific Proteins Programme", ProgramCode = "RQ9114", Category = "Specific Proteins" }
            );
            db.SaveChanges();
        }

        private static void SeedCustomers(EXLABOContext db)
        {
            var customers = new List<string>
            {
                "Modern Aqaba Hospital", "Ibn Al-Nafis Hospital", "Centra lab", "Al-Matalka lab",
                "Al-Hanan Hospital", "Arabi lab -central", "PCL (Perfect Choice Lab)", "RIGHTLAB / SHMESANI",
                "Scope Lab", "CML", "Jah Lab Center", "Mega lab-Wadi Abdoun", "Mega lab-Central",
                "Mega lab vitro – Al-Khaldi", "Mega Lab Aqaba", "Med Lab Fuhays", "Med lab Dabouq",
                "Med lab Al-Yasmeen", "Med lab Aqaba", "Med lab BAQAA", "Med lab Madaba 1",
                "Med lab Academy C", "Med lab Academy U", "Med lab Nazzal", "Med lab Abdon",
                "Med lab Al-Zarqa", "Med lab Dabouq 2", "Med lab Madaba 2", "Med lab Al-Zarqa new",
                "Med lab Mafraq", "HEMOLAB JUBEHA", "HEMOLAB TABRBOR", "HEMOLAB Al-Madinah",
                "VITALLAB 5TH", "VITALLAB SHMESANI", "ALQAWASMI SHMESANI", "My Lab Al-Khaldi",
                "Bio Lab Marj Al-Hamam", "Bio Lab Al-Dahia", "Bio Lab Al-Salt", "Bio Lab Karadour Abdoun",
                "Bio Lab Dabouq", "Bio Lab Al-Mugableen", "Specialty Eye Hospital", "Bio Lab Khalda",
                "Al-Iman Government Hospital / Ajloun", "Tafila Government Hospital / Tafila",
                "Istiklal Hospital", "Uni Lab", "Al-Luzmila Hospital", "Smart Lab", "IPRC",
                "Al-Arabi Lab", "Bio Lab 5th", "Bio Lab Abd-Al-Hadi Hosp.", "Mega Lab",
                "DRMS - Marka", "DRMS – Al-Zarqa", "DRMS - Ajloun", "DRMS – Al-Aqaba",
                "DRMS - STAT", "DRMS – Al-Tafela", "DRMS - Irbid Aydoun", "KAUH", "Right Lab",
                "DRMS – Al-Latroon1", "DRMS – Al-Latroon 2", "Al-Basheer Hospital", "DRMS - Central",
                "R.lab AL KHALIDI"
            };

            foreach (var name in customers.Distinct())
            {
                if (!db.Customers.Any(c => c.CustomerName == name))
                {
                    db.Customers.Add(new Customer { CustomerName = name, Aliases = name });
                }
            }
            db.SaveChanges();
        }

        private static void SeedItemsAndLots(EXLABOContext db)
        {
            var randox = db.Companies.First(c => c.CompanyName == "Randox");
            var siemens = db.Companies.First(c => c.CompanyName == "Siemens");
            var sysmex = db.Companies.First(c => c.CompanyName == "Sysmex");
            var chem = db.Departments.First(d => d.DepartmentName == "Clinical Chemistry");
            var heme = db.Departments.First(d => d.DepartmentName == "Hematology");
            var coag = db.Departments.First(d => d.DepartmentName == "Coagulation");

            var itemsList = new List<(string Code, string Name, string Type, int CompanyID, int DeptID)>
            {
                ("CEA-001", "CEA Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("FT4-002", "FT4 Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("PRL-003", "PRL Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("VITD-004", "Vitamin D Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("TSH-005", "TSH Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("B12-006", "Vitamin B12 Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("FER-007", "Ferritin Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("FPSA-008", "Free PSA Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("TPSA-009", "Total PSA Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("HBA1C-010", "HbA1c Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("CRP-011", "CRP Latex Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("BUN-012", "BUN Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("URIC-013", "Uric Acid Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("ALT-014", "ALT Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("GGT-015", "GGT Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("ALB-016", "Albumin Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("TBIL-017", "Total Bilirubin Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("HDL-018", "HDL Cholesterol Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("LDLP-019", "LDL Cholesterol Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("AMY-020", "Amylase Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("MG-021", "Magnesium Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("PO4-022", "Phosphorus Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("DTT-023", "DTT B12 Reagent", "Reagent", randox.CompanyID, chem.DepartmentID),
                ("PTT-024", "PTT Reagent", "Reagent", sysmex.CompanyID, coag.DepartmentID),
                ("DIL-025", "CBC Diluent", "Reagent", siemens.CompanyID, heme.DepartmentID),
                ("DIFF-026", "CBC Diff Reagent", "Reagent", siemens.CompanyID, heme.DepartmentID),
                ("LYSE-027", "CBC Lyse Reagent", "Reagent", siemens.CompanyID, heme.DepartmentID),
                ("ACUSERA-N", "Acusera Normal Control", "Control", randox.CompanyID, chem.DepartmentID),
                ("ACUSERA-P", "Acusera Pathological Control", "Control", randox.CompanyID, chem.DepartmentID)
            };

            foreach (var (code, name, type, companyId, deptId) in itemsList)
            {
                var item = new Item
                {
                    ItemCode = code,
                    ItemName = name,
                    ItemType = type,
                    CompanyID = companyId,
                    DepartmentID = deptId,
                    Unit = "Kit",
                    MinimumStock = 50,
                    Category = type
                };
                db.Items.Add(item);
                db.SaveChanges();

                // إنشاء Lot بـ 200 قطعة تنتهي نهاية 2027
                db.Lots.Add(new Lot
                {
                    ItemID = item.ItemID,
                    LotNumber = $"LOT-2026-{code}",
                    Quantity = 200,
                    ExpiryDate = new DateTime(2027, 12, 31),
                    ManufactureDate = DateTime.Now.AddMonths(-6),
                    StorageCondition = "2-8°C"
                });
            }
            db.SaveChanges();
        }
    }
}