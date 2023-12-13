using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using GST_Web.EmailSender;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.Email_Activity.Implemenatation;
using Repository_Logic.Email_Activity.Interface;
using Repository_Logic.ExportExcelSheet.Implemantation;
using Repository_Logic.ExportExcelSheet.Interface;

using Repository_Logic.FellowshipRepository.Implemantation;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.FileUploads.Implementation;
using Repository_Logic.FileUploads.Interface;
using Repository_Logic.GlobalFunction.Implementation;
using Repository_Logic.GlobalFunction.Interface;
using Repository_Logic.GstBill.Implementation;
using Repository_Logic.GstBill.Interface;
using Repository_Logic.ReturnFile.Implementation;
using Repository_Logic.ReturnFile.Interface;
using Repository_Logic.TaskAllocation.Implementation;
using Repository_Logic.TaskAllocation.Interface;
using Repository_Logic.UserOtherDatails.implementation;
using Repository_Logic.UserOtherDatails.Interface;
using Repository_Logic.ViewFilledGSt.Interface;
using Repository_Logic.ViewFilledGSt.Implemenation;
using DinkToPdf.Contracts;
using DinkToPdf;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ExcelSheetUploadRepository.Implementation;
using Repository_Logic.RegistorLogsRepository.Implementation;
using Repository_Logic.RegistorLogsRepository.Interface;
using Repository_Logic.LoginLogsDataRepository.Interface;
using Repository_Logic.LoginLogsDataRepository.Implementation;
using Repository_Logic.DeleteLogsRepository.Interface;
using Repository_Logic.DeleteLogsRepository.Implementation;
using Repository_Logic.MessageChatRepository;
using The_GST_1.ChatHubd;
using Repository_Logic.QueryRepository.Implementation;
using Repository_Logic.QueryRepository.Interface;
using Repository_Logic.ReturnFilesRepository.Interface;
using Repository_Logic.ReturnFilesRepository.Implementation;
using Repository_Logic.RecoverUser.Interface;
using Repository_Logic.RecoverUser.Implementation;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GStConnectionstring") ?? throw new InvalidOperationException("Connection string 'ApplicationDb_ContextConnection' not found.");

builder.Services.AddDbContext<Application_Db_Context>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IExtraDetails, ExtraDetails>();
builder.Services.AddScoped<IExportExcelSheet, ExportExcelSheet>();
builder.Services.AddScoped<IEmailActivity, EmailActivity>();
builder.Services.AddScoped<ITaskAllocation, TaskAllocation>();
builder.Services.AddScoped<IReturnFile, ReturnFile>();
builder.Services.AddScoped<IFellowshipRepository, FellowshipRepository>();
builder.Services.AddScoped<IGlobalFunctionRepository, GlobalFunctionRepository>();
builder.Services.AddScoped<IGSTBills, GSTBills>();
builder.Services.AddScoped<IViewGSTFilledGst, ViewFilledGst>();
builder.Services.AddScoped<IRegisterLogs, RegisterLogs>();
builder.Services.AddScoped<IDeleteLogs, DeleteLog>();
//New Update Database and Code
builder.Services.AddScoped<IExcelSheetUpload,ExcelSheetUpload>();
builder.Services.AddScoped<IReturnFilesRepository, ReturnFilesRepository>();

//Logs Repository
builder.Services.AddScoped<IRegisterLogs, RegisterLogs>();
builder.Services.AddScoped<ILoginLogs, LoginLogsImplementation>();
//Chating With users.
builder.Services.AddScoped<IMessage, Message>();

//Html to pdf File Convert
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<Application_Db_Context>();
// Add services to the container.
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IQuery, QueryImplementation>();
builder.Services.AddScoped<IRecoverUser, recoverUser>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
/*
builder.Services.AddControllersWithViews().AddRazorPagesOptions(options => {
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
});*/

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Rolegard", policy =>
          policy.RequireRole("CA", "Fellowship", "User"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
 void ConfigureServices(IServiceCollection services)
{
    // Other service registrations

    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Privacy}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "CA", "Fellowship", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string email = "ca@gmail.com";
    string password = "Admin@123";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new Application_User();
        user.UserName = email;
        user.FirstName="Akshay";
        user.LastName= "Jagtap";
        user.Email = email;
        user.EmailConfirmed = true;
        
        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "CA");
    }

}
app.MapHub<ChatHub>("/chatHub"); // Add this line to map SignalR hub

app.Run();

