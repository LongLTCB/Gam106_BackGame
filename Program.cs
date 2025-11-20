using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configure Entity Framework Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

var mailSettings = builder.Configuration.GetSection("EmailSettings")
.Get<WebApplication1.Models.EmailSettings>();
var emailMessage = new MimeKit.MimeMessage();
emailMessage.From.Add(new MimeKit.MailboxAddress(mailSettings.SenderName,
 mailSettings.SenderEmail));
emailMessage.To.Add(new MimeKit.MailboxAddress("Recipient Name", "tungtt64@fpt.edu.vn"));
emailMessage.Subject = "hello1234";
emailMessage.Body = new MimeKit.TextPart("plain") { Text = "hello from c# 10:02" };

using (var client = new MailKit.Net.Smtp.SmtpClient())
{
    client.Connect(mailSettings.SmtpServer, mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
    client.Authenticate(mailSettings.UserName, mailSettings.Password);
    client.Send(emailMessage);
    client.Disconnect(true);
}

app.Run();
