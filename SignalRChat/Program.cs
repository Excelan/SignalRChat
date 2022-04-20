using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Areas.Identity.Data;
using SignalRChat.BackgroundTasks;
using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SignalRChatIdentityDbContextConnection");

builder.Services.AddDbContext<ChatIdentityDbContext>(options => {
	options.UseSqlite(connectionString);
	SQLitePCL.Batteries.Init();
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ChatIdentityDbContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddAuthorization(options => {
	options.FallbackPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
});

builder.Services.AddHostedService<ConfigurationNotifierService>();
builder.Services.AddTransient<INotificationSender, SignalRNotificationSender>();
builder.Services.AddSingleton<ISystemMessageSender, SignalRSystemMessageSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");

app.Run();
