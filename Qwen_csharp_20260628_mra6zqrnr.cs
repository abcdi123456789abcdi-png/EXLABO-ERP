using Microsoft.AspNetCore.Mvc;
using EXLABO.Infrastructure.Database;
using System;

namespace EXLABO.Web.Controllers
{
    public class BackupController : Controller
    {
        private readonly EXLABOContext db;
        public BackupController(EXLABOContext context) => db = context;
        public IActionResult Create()
        {
            try { db.Database.ExecuteSqlRaw(@"BACKUP DATABASE EXLABO TO DISK = 'C:\EXLABO_Backup.bak' WITH FORMAT;"); return Content("✅ Backup Success!"); }
            catch (Exception ex) { return Content($"❌ Error: {ex.Message}"); }
        }
    }
}