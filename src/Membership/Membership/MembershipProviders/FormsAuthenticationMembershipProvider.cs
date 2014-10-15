namespace X3Platform.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.IO;
    using System.Collections.Specialized;

    /// <summary></summary>
    public class FormsAuthenticationMembershipProvider : MembershipProvider
    {
        /// <summary></summary>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        /// <summary></summary>
        public override string ApplicationName
        {
            get { return "/"; }
            set { }
        }

        /// <summary></summary>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        /// <summary></summary>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return true;
        }

        /// <summary></summary>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            IDictionary<string, IAccountInfo> users = MembershipStorage.Instance.Accounts;

            if (users.ContainsKey(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;

                return null;
            }

            // users.Add(username, password);

            // this.WriteAllUsers(users);

            status = MembershipCreateStatus.Success;

            MembershipUser user = new MembershipUser(this.Name, username, username, email, passwordQuestion, "", isApproved, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            return user;
        }

        /// <summary></summary>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            IDictionary<string, IAccountInfo> users = MembershipStorage.Instance.Accounts;

            if (users.ContainsKey(username))
            {
                users.Remove(username);
                // this.WriteAllUsers(users);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary></summary>
        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        /// <summary></summary>
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        /// <summary></summary>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        /// <summary></summary>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection result = new MembershipUserCollection();

            IDictionary<string, IAccountInfo> users = MembershipStorage.Instance.Accounts;
            foreach (String username in users.Keys)
            {
                if (username.StartsWith(usernameToMatch))
                {
                    result.Add(this.GetUser(usernameToMatch, false));
                }
            }

            totalRecords = users.Count;
            return result;
        }

        /// <summary></summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection result = new MembershipUserCollection();

            IDictionary<string, IAccountInfo> users = MembershipStorage.Instance.Accounts;

            foreach (String username in users.Keys)
            {
                result.Add(this.GetUser(username, false));
            }

            totalRecords = users.Count;
            return result;
        }

        /// <summary></summary>
        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        /// <summary></summary>
        public override string GetPassword(string username, string answer)
        {
            return string.Empty;
        }

        /// <summary></summary>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            IAccountInfo account = MembershipManagement.Instance.AccountService.FindOneByLoginName(username);

            if (account != null)
            {
                MembershipUser result = new MembershipUser(this.Name, username, username, "", "", "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary></summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return this.GetUser(providerUserKey.ToString(), userIsOnline);
        }

        /// <summary></summary>
        public override string GetUserNameByEmail(string email)
        {
            return "";
        }
        
        /// <summary></summary>
        public override int MaxInvalidPasswordAttempts
        {
            get { return 999; }
        }

        /// <summary></summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        /// <summary></summary>
        public override int MinRequiredPasswordLength
        {
            get { return 1; }
        }

        /// <summary></summary>
        public override int PasswordAttemptWindow
        {
            get { return 999; }
        }

        /// <summary></summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        /// <summary></summary>
        public override string PasswordStrengthRegularExpression
        {
            get { return ""; }
        }

        /// <summary></summary>
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        /// <summary></summary>
        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        /// <summary></summary>
        public override string ResetPassword(string username, string answer)
        {
            return "";
        }

        /// <summary></summary>
        public override bool UnlockUser(string userName)
        {
            return true;
        }

        /// <summary></summary>
        public override void UpdateUser(MembershipUser user)
        {

        }

        /// <summary></summary>
        public override bool ValidateUser(string username, string password)
        {
            int index = username.IndexOf('\\');

            username = username.Substring(index + 1, username.Length - index - 1);

            IAccountInfo account = MembershipManagement.Instance.AccountService.LoginCheck(username, password);

            return account == null ? false : true;
        }
    }
}
