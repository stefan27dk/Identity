using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Identity.Models
{
        public class ApplicationUser : IdentityUser
        {
            // ==== [Firstname] ================================================================================  
            public string FirstName { get; private set; }



            // ==== [Lastname] =================================================================================  
            public string LastName { get; private set; }



            // ==== [Age] ======================================================================================  
            public int Age { get; private set; }








            public IdentityResult SetUserData(string firstName, string lastName, string email, string age, string phonenumber, string password)
            {


                // If all empty
                if (firstName == null && lastName == null && email == null && age == null && phonenumber == null && password == null)
                {
                    return IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "Nothing to Update", Description = "All fields are Null - there is nothing to update" } });
                }



                // Password
                if (password != null)
                {
                    var result = SetPassword(password);
                    if (result != IdentityResult.Success)
                    {
                        return result;
                    }
                }




                // Email
                if (email != null)
                {
                    var result = SetEmail(email);
                    if (result != IdentityResult.Success)
                    {
                        return result;
                    }
                }




                // PhoneNumber
                if (phonenumber != null)
                {
                    var result = SetPhoneNumber(phonenumber);
                    if (result != IdentityResult.Success)
                    {
                        return result;
                    }
                }




                // FirstName
                if (firstName != null)
                {
                    var result = SetFirstName(firstName);
                    if (result != IdentityResult.Success)
                    {
                        return result;
                    }
                }





                // LastName
                if (lastName != null)
                {
                    var result = SetLastName(lastName);
                    if (result != IdentityResult.Success)
                    {
                        return result;
                    }
                }





                // Age
                if (age != null)
                {
                    var result = SetAge(age);
                    if (result != IdentityResult.Success)
                    {
                        return result;
                    }
                }

                return IdentityResult.Success;
            }








            // ################# [Setter Methods] ###################################################################################################

            // ==== [Set Email] =================================================================================
            public IdentityResult SetEmail(string email)
            {
                if (EmailIsValid(email))
                {
                    this.Email = email;
                    this.UserName = email;
                    this.NormalizedEmail = email.ToUpper();
                    this.NormalizedUserName = email.ToUpper();
                    return IdentityResult.Success;
                }

                return IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "Set Email Error", Description = "Email is not Valid" } });
            }






            // ==== [Set FirstName] =================================================================================
            public IdentityResult SetFirstName(string firstname)
            {
                if (FirstNameIsValid(firstname))
                {
                    this.FirstName = firstname;
                    return IdentityResult.Success;
                }

                return IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "Set Firstname Error", Description = "Firstname is not Valid - Min 2 Letters and Max 20 letters" } });
            }







            // ==== [Set LastName] =================================================================================
            public IdentityResult SetLastName(string lastname)
            {
                if (LastNameIsValid(lastname))
                {
                    this.LastName = lastname;
                    return IdentityResult.Success;
                }

                return IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "Set Lastname Error", Description = "Lastname is not Valid - Min 2 Letters and Max 20 letters" } });
            }







            // ==== [Set Age] =================================================================================
            public IdentityResult SetAge(string age)
            {
                if (AgeIsValid(age))
                {
                    this.Age = int.Parse(age);
                    return IdentityResult.Success;
                }

                return IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "Set Age Error", Description = "Not a valid Age  Min 0 and Max 200" } });
            }







            // ==== [Set PhoneNumber] =================================================================================
            public IdentityResult SetPhoneNumber(string phonenumber)
            {
                if (PhoneNumberIsValid(phonenumber))
                {
                    this.PhoneNumber = phonenumber;
                    return IdentityResult.Success;
                }

                return IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "Set Phonenumber Error", Description = "Not valid phonenumber - Min / Max 8 digits" } });
            }






            // ==== [Set Password] =================================================================================
            public IdentityResult SetPassword(string password)
            {
                if (PasswordIsValid(password))
                {
                    // Hash Password
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    var newHashedPassword = passwordHasher.HashPassword(this, password);

                    // Set Password
                    this.PasswordHash = newHashedPassword;
                    return IdentityResult.Success;
                }

                return IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "Set Password Error", Description = "Not a valid Password - Password must be at least 6 digits, letters or special chars" } });
            }




















            // ==== ################### [Validation Methods] ###################################################################################################

            // ==== [EMAIL Validation] ============================================================================  
            private bool EmailIsValid(string email)
            {
                if (Regex.IsMatch(email, @"^[a-z0-9]+(\.[a-z0-9]+)?@+[a-z0-9]+\.[a-z]{2,3}$"))
                {
                    return true;
                }
                return false;
            }






            // ==== [FirstName Validation] ============================================================================  
            private bool FirstNameIsValid(string firstname)
            {
                if (firstname.Length > 1 && firstname.Length <= 20 && Regex.IsMatch(firstname, @"^[a-zA-ZæøåÆØÅ]+$"))
                {
                    return true;
                }
                return false;
            }






            // ==== [LastName Validation] ============================================================================  
            private bool LastNameIsValid(string lastname)
            {
                if (lastname.Length > 0 && lastname.Length <= 20 && Regex.IsMatch(lastname, @"^[a-zA-ZæøåÆØÅ]+$"))
                {
                    return true;
                }
                return false;
            }






            // ==== [Age Validation] ============================================================================  
            private bool AgeIsValid(string age)
            {
                int theAge;
                if (int.TryParse(age, out theAge) && (theAge >= 0 && theAge <= 200))
                {
                    return true;
                }
                return false;
            }







            // ==== [PhoneNumber Validation] ============================================================================  
            private bool PhoneNumberIsValid(string phonenumber)
            {
                if (phonenumber.Length == 8 && int.TryParse(phonenumber, out int output))
                {
                    return true;
                }
                return false;
            }









            // ==== [Password Validation] ============================================================================  
            private bool PasswordIsValid(string password)
            {
                if (password.Length >= 6)
                {
                    return true;
                }
                return false;
            }

        }
}
