namespace TeenWise.WebSite.Models.AccessControl
{ 
    using System;
    using System.Web.Security;
    using TeenWise.WebSite.Models.Storage;

    public class UserMembershipProvider : MembershipProvider
    {
        internal const string BaseSecret = "Zaplify";

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            RegisteredUser user = Storage.Users.GetUser(username);
            if (user != null)
            {
                return AsMembershipUser(user);
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            MembershipUser registeredUser = GetUser(username, false);
            return (registeredUser != null);
            //return ((registeredUser != null) && IsValidAccessCode(username, password));
        }

        static bool IsValidAccessCode(string username, string password)
        {
            string[] parts = username.Split('@');
            if (parts.Length == 2)
            {
                string validCode = BaseSecret + parts[0].Length.ToString() + parts[1].Length.ToString();
                return (password.Equals(validCode, StringComparison.Ordinal) 
                    || password.Equals(BaseSecret, StringComparison.Ordinal));
            }
            return false;
        }

        static MembershipUser AsMembershipUser(RegisteredUser user)
        {   
            MembershipUser member = new MembershipUser(
                typeof(UserMembershipProvider).Name,    // provider
                user.EmailAddress,                      // username
                null,                                   // user key
                user.EmailAddress,                      // email
                null,                                   // password question
                null,                                   // comment
                true,                                   // isApproved
                false,                                  // isLockedOut
                user.RegisteredAt,                      // registeredDate
                DateTime.Now,                           // lastLoginDate
                DateTime.Now,                           // lastActivityDate
                DateTime.Now,                           // lastPasswordChangeDate
                DateTime.Now);                          // lastLockoutDate
            return member;
        }
    }
}