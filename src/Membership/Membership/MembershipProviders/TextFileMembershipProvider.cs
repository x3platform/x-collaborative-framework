namespace X3Platform.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.IO;

    /// <summary></summary>
    public class TextFileMembershipProvider : MembershipProvider
    {
        private String _sFilePath = "";

        /// <summary></summary>
        public String FilePath
        {
            get { return _sFilePath; }
        }

        private IDictionary<String, String> LoadAllUsers()
        {
            if (String.IsNullOrEmpty(this.FilePath))
            {
                throw new InvalidOperationException("FilePath is not set.");
            }

            Dictionary<String, String> result = new Dictionary<String, String>();

            using (StreamReader reader = new StreamReader(this.FilePath))
            {
                while (true)
                {
                    String sLine = reader.ReadLine();
                    if (sLine == null)
                    {
                        break;
                    }
                    if (sLine.Trim().Length == 0)
                    {
                        continue;
                    }
                    String[] line = sLine.Split(':');
                    result.Add(line[0], line[1]);
                }
            }

            return result;
        }

        private void WriteAllUsers(IDictionary<String, String> users)
        {
            if (String.IsNullOrEmpty(this.FilePath))
            {
                throw new InvalidOperationException("FilePath is not set.");
            }

            using (StreamWriter writer = new StreamWriter(this.FilePath, false))
            {
                foreach (String userId in users.Keys)
                {
                    writer.WriteLine(userId + ":" + users[userId]);
                }
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            _sFilePath = config["filePath"];
        }

        /// <summary></summary>
        public override string ApplicationName
        {
            get
            {
                return "/";
            }
            set
            {

            }
        }

        /// <summary></summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        /// <summary></summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPasswordQuestion"></param>
        /// <param name="newPasswordAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return true;
        }

        /// <summary></summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            IDictionary<String, String> users = this.LoadAllUsers();
            if (users.ContainsKey(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            users.Add(username, password);
            this.WriteAllUsers(users);

            status = MembershipCreateStatus.Success;

            MembershipUser user = new MembershipUser(this.Name, username, username, email, passwordQuestion, "", isApproved, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            return user;
        }

        /// <summary></summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            IDictionary<String, String> users = this.LoadAllUsers();
            if (users.ContainsKey(username))
            {
                users.Remove(username);
                this.WriteAllUsers(users);
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
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        /// <summary></summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection result = new MembershipUserCollection();

            IDictionary<String, String> users = this.LoadAllUsers();
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

            IDictionary<String, String> users = this.LoadAllUsers();
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
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string username, string answer)
        {
            return "";
        }

        /// <summary></summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            IDictionary<String, String> users = this.LoadAllUsers();
            if (users.ContainsKey(username))
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
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string ResetPassword(string username, string answer)
        {
            return "";
        }

        /// <summary></summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override bool UnlockUser(string userName)
        {
            return true;
        }

        /// <summary></summary>
        /// <param name="user"></param>
        public override void UpdateUser(MembershipUser user)
        {

        }

        /// <summary></summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            IDictionary<String, String> users = this.LoadAllUsers();
            if (!users.ContainsKey(username))
            {
                return false;
            }
            if (users[username] != password)
            {
                return false;
            }

            return true;
        }
    }
}
