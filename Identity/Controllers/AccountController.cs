using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    // Class ============= || Account - Controller ||==========================================
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        // Managers
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        //private readonly DataProtectorTokenProvider<ApplicationUser> _dataProtectorTokenProvider;

        // || Constructor || ===================================================================
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DataProtectorTokenProvider<ApplicationUser> dataProtectorTokenProvider = null)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            //_dataProtectorTokenProvider = dataProtectorTokenProvider;
        }






        // Register ========================================================================== 
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string firstname, string lastname, string email, string age, string phonenumber, string password, bool rememberMe)
        {
            if(!string.IsNullOrWhiteSpace(email) && password != null)
            {    
                //var user = new ApplicationUser { UserName = email, Email = email };  // Create the User
                var user = new ApplicationUser(); // Create the User
                var setResult = user.SetUserData(firstname, lastname, email, age, phonenumber, password); 
                
                if (setResult.Succeeded) // If all validated
                {
                        var result = await userManager.CreateAsync(user); // Save the  User
                        if (result.Succeeded)  // If All Ok
                        {
                            await signInManager.SignInAsync(user, isPersistent: rememberMe);  // Sign In the User "Session with persistent cookie"
                            return StatusCode(201, "[\n \"Registration Successfull\" \n]");
                        }
                        return StatusCode(409, result.Errors.Select(e => e.Description));  // Create User - Errors
                }
                return StatusCode(409, setResult.Errors.Select(e => e.Description));  // Validation Errors
            }
            return StatusCode(400, "Email or/and password cant be empty!");  // Validation Errors

        }










        // Log In ===================================================================================
        [HttpPost]
        [Route("LogIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(string email, string password, bool rememberMe)
        {
            if (!string.IsNullOrWhiteSpace(email) && password != null)
            {
                var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);
                if (result.Succeeded) // If Login Ok
                {
                    return StatusCode(200, result);
                    //return new JsonResult(result);
                }
                return StatusCode(401, "[\n \"Invalid Log In\" \n]");  // If Erors return errors 
            }
            return StatusCode(401, "[\n \"Email or Password cant be empty\" \n]");
        }








        // Log Out ==================================================================================
        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return StatusCode(200, "[\n \"Successfully Logged Out\" \n]");
        }






        //// Get User  ==================================================================================
        //[HttpGet]
        //[Route("GetUserData")]
        ////[AllowAnonymous]
        //public async Task<IActionResult> GetUserData()
        //{
        //    // Get Current User
        //    var user = await userManager.GetUserAsync(HttpContext.User); // Get User
        //    GetUserDataViewModel userData = new GetUserDataViewModel();  // View Mdoel

        //    if (user != null) // If Logged in
        //    {
        //        userData.Email = user.Email;
        //        userData.Firstname = user.FirstName;
        //        userData.Lastname = user.LastName;
        //        userData.Age = user.Age;
        //        userData.Phonenumber = user.PhoneNumber;

        //        return new JsonResult(userData); // Return User Data
        //    }
        //    return new JsonResult(userData); // Empty User Data
        //}








        // Get User  ==================================================================================
        [HttpGet]
        [Route("GetUserData")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetUserData()
        {
            // Get Current User
            var user = await userManager.GetUserAsync(HttpContext.User); // Get User
                
            if (user != null) // If Logged in
            {
                object userData = new
                {
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Age = user.Age,
                    Phonenumber = user.PhoneNumber
                };
                return new JsonResult(userData); // Return User Data
            }
            return StatusCode(403, "Not Logged In"); // Empty User Data
        }







        // Get User Email - Used to check if user is logged in ============================================= 
        [HttpGet]
        [Route("GetUserEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserEmail()
        {
            // Get Current User
            var user = await userManager.GetUserAsync(HttpContext.User);
            GetUserEmailViewModel userEmail = new GetUserEmailViewModel(); // ViewModel


            if (user != null) // If User logged in
            {   
                return StatusCode(200, user.Email.ToString());
            }
            return StatusCode(403, "[\n Not Logged In \n]");
        }

            
           



        // Update User ==================================================================================
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(string firstName, string lastName, string email, string age, string phonenumber, string password)
        {
            // Get Current User
            var user = await userManager.GetUserAsync(HttpContext.User);


            // Set User data with validation
            var validationResult = user.SetUserData(firstName, lastName, email, age, phonenumber, password);


            // Update
            if (validationResult.Succeeded) // Update if validation passed
            {
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)// If OK
                {
                    return StatusCode(200, "[\n \"User Data Updated Successfully\" \n]");
                }
                else  // If Update Error
                {
                    return StatusCode(409, result.Errors.Select(e => e.Description));
                }
            }
            else // If validation Error
            {
                return StatusCode(409, validationResult.Errors.Select(e => e.Description));
            }
        }







        //// Update User ==================================================================================
        //[HttpPost]
        //[Route("UpdateUser")]
        //public async Task<IActionResult> UpdateUser(string email, string firstName, string lastName, int age, string password, string tel)
        //{
        //    // Get Current User
        //    var user = await userManager.GetUserAsync(HttpContext.User);


        //    // Update only if values are not null
        //    if (firstName != null) { user.FirstName = firstName; }
        //    if (lastName != null) { user.LastName = lastName; }


        //    // Email
        //    if (Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))  // Email Regex
        //    {
        //        user.Email = email;
        //        user.UserName = email;
        //        user.NormalizedUserName = email.ToUpper();
        //        user.NormalizedEmail = email.ToUpper();
        //    }
        //    else
        //    {
        //        return StatusCode(422, "Email is not valid"); // Email Error
        //    }



        //    if (tel != null) { user.PhoneNumber = tel; }
        //    if (age != 0) { user.Age = age; }



        //    // Password
        //    if (password != null)
        //    {
        //        var passwordHasher = new PasswordHasher<ApplicationUser>();
        //        var newHashedPassword = passwordHasher.HashPassword(user, password);
        //        user.PasswordHash = newHashedPassword;
        //    }




        //    // Update
        //    var result = await userManager.UpdateAsync(user);





        //    if (result.Succeeded)// If OK
        //    {
        //        return StatusCode(200, "User Data Updated Successfully");
        //    }
        //    else  // If Error
        //    {
        //        return new JsonResult(result.Errors);
        //    }
        //}


















        //// Update Password ==================================================================================
        //[HttpPost]
        //[Route("UpdatePassword")]
        //public async Task<IActionResult> UpdatePassword(string newPassword)
        //{
        //    // Get Current User
        //    var user = await userManager.GetUserAsync(HttpContext.User);



        //    // Change Password
        //    if (!await userManager.CheckPasswordAsync(user, newPassword))
        //    {
        //        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //        await userManager.ResetPasswordAsync(user, token, newPassword);
        //        return StatusCode(200, "Password Updated Successfully");
        //    }
        //    else // If password is the same as the old
        //    {
        //        return StatusCode(409, "Please enter different password from the old");
        //    }
        //}








        // Delete LogIn ==================================================================================
        [HttpPost]
        [Route("DeleteLogIn")]
        public async Task<IActionResult> DeleteLogIn(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                 // Get Current User
                 var user = await userManager.GetUserAsync(HttpContext.User);
                 
                 if (user.Email == email)
                 {
                     // Delete the User
                     var result = await userManager.DeleteAsync(user);
                 
                 
                     if (result.Succeeded)// If Success
                     {
                         return StatusCode(200, "[\n \"User Deleted Successfully\" \n]");
                     }
                     else// If Error
                     {
                         return new JsonResult(result.Errors.Select(e => e.Description));
                     }
                 }
                 else// If email not same as the users email
                 {
                     return StatusCode(422, "[\n \"The Email is not correct - Please enter your account email to confirm deletion of the account\" \n]");
                 }
            }
            return StatusCode(403, "[\n \"Email cant be Empty\" \n]");
        }


    }
}
