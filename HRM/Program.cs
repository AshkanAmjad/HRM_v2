using IOC.Dependencies;
using HRM.Extensions;
using Data.Extensions;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Add Services
builder.Services.AddMappingService();
builder.Services.RegisterServices();
builder.Services.ValidationServices();
builder.Services.AddApplicationService(builder, builder.Configuration);
#endregion

#region Static Files


var wwwrootOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    OnPrepareResponse = context =>
    {
        context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
        context.Context.Response.Headers.Append("Expires", "-1");
    }
};

var province_provinceAvatarsOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Areas/Province/Documents/Province/Avatar/Thumb")),
    RequestPath = "/Province/Avatars/Province",
    OnPrepareResponse = context =>
    {
        context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
        context.Context.Response.Headers.Append("Expires", "-1");

    }
};

var county_countyAvatarsOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Areas/County/Documents/County/Avatar/Thumb")),
    RequestPath = "/County/Avatars/County",
    OnPrepareResponse = context =>
    {
        context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
        context.Context.Response.Headers.Append("Expires", "-1");

    }
};

//var province_countyAvatarsOptions = new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Areas/Province/Documents/County/Avatar/Thumb")),
//    RequestPath = "/Province/Avatars/County",
//    OnPrepareResponse = context =>
//    {
//        context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
//        context.Context.Response.Headers.Append("Expires", "-1");
//    }
//};

//var province_districtAvatarsOptions = new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Areas/Province/Documents/District/Avatar/Thumb")),
//    RequestPath = "/Province/Avatars/District",
//    OnPrepareResponse = context =>
//    {
//        context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
//        context.Context.Response.Headers.Append("Expires", "-1");
//    }
//};
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles(wwwrootOptions);
app.UseStaticFiles(province_provinceAvatarsOptions);
app.UseStaticFiles(county_countyAvatarsOptions);

//app.UseStaticFiles(province_countyAvatarsOptions);
//app.UseStaticFiles(province_districtAvatarsOptions);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Main}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
