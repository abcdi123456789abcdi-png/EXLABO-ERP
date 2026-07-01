# 1. إنشاء المشروع
dotnet new mvc -n EXLABO.ERP
cd EXLABO.ERP

# 2. تثبيت الحزم
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Azure.AI.OpenAI
dotnet add package QuestPDF
dotnet add package ClosedXML

# 3. إنشاء المجلدات
mkdir -p wwwroot/images wwwroot/css
mkdir -p Core/Entities Core/DTO Core/Services
mkdir -p Infrastructure/Database

# 4. نسخ جميع الملفات أعلاه إلى مواقعها

# 5. تحديث appsettings.json بمعلومات OpenAI و SMTP

# 6. تشغيل المشروع
dotnet run