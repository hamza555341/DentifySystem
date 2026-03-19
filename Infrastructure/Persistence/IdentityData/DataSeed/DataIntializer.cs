using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence.IdentityData.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.IdentityData.DataSeed
{
    public class DataIntializer : IDataIntializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DataIntializer> _logger;

        public DataIntializer(UserManager<ApplicationUser> userManager 
              ,RoleManager<IdentityRole> roleManager ,ILogger<DataIntializer> logger)
            
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }



        public async Task IntializeDataAsync()
        {
          
            try
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if(!await _roleManager.RoleExistsAsync("Student"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Student"));
                }
               
                if (!await _roleManager.RoleExistsAsync("Patient"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));
                }



                if (!_userManager.Users.Any())
                {
                    var adminUser = new ApplicationUser()
                    {
                        DisplayName = "Hamza Oraby",
                        Email = "Hamza44@gmail.com",
                        UserName = "HamzaOraby",
                        PhoneNumber = "01092772908"
                    };


                    await _userManager.CreateAsync(adminUser,"P@ssw0rd");
                    await _userManager.AddToRoleAsync(adminUser, "Admin");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while Seeding data {ex.Message}");
            }



        }
    }
}
