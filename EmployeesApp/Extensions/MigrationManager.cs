﻿using EmployeesApp.Models;
using Microsoft.EntityFrameworkCore;
namespace EmployeesApp.Extensions
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<EmployeeContext>())
                {
                    try
                    {
                        if (appContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                        {
                            appContext.Database.Migrate();
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }

            return webApp;
        }
    }
}
