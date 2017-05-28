using System;
using System.Runtime.Remoting.Messaging;
using DBLayer;
using Utilities;
using System.Security;
using System.Threading.Tasks;

namespace BusinessLayer
{
	public enum ActionPermission
	{
		CreateName
	}

	public class UserHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="permission"></param>
		/// <param name="throwException"></param>
		/// <returns></returns>
		/// <exception cref="SecurityException">if throwexception is true and permission not existing</exception>
		public static bool HasPermission(ActionPermission permission, bool throwException = true)
		{
			if (CurrentUser != null)
			{
				return true;
			}
			else if (throwException)
			{
				throw new SecurityException("User does not have permission to perform this action");
			}

			return false;
		}

		public static User CurrentUser
		{
			get => CallContext.LogicalGetData("currentUser") as User;
			set => CallContext.LogicalSetData("currentUser", value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="hashedPassword"></param>
		/// <param name="passwordTime"></param>
		/// <returns></returns>
		/// <exception cref="SecurityException">Incorrect password</exception>
		public static async Task<User> Logon(string username, string hashedPassword, DateTime passwordTime)
		{
			var back = await UserDb.GetUser(username);

			if (back.LastLoginTime >= passwordTime)
			{
				throw new SecurityException("Password time is before latest login");
			}

			if (!PasswordCheck(hashedPassword, passwordTime, back))
			{
				throw new SecurityException("Password is incorrect for user: " + username);
			}

			back.LastLoginTime = passwordTime;
			await UserDb.UpdateLoginTime(back);

			CurrentUser = back;

			return back;
		}

		private static bool PasswordCheck(string hashedPassword, DateTime passwordTime, User back)
		{
			return hashedPassword == GenerateHashedPassword(back.Password, passwordTime);
		}

		public static string GenerateHashedPassword(string prehashed, DateTime hashedTime)
		{
			string hashedPasswordWithTime = prehashed + hashedTime.Year + hashedTime.Month + hashedTime.Date + hashedTime.Hour + hashedTime.Minute + hashedTime.Second + hashedTime.Millisecond;
			return Md5Hashing.GetMd5Hash(hashedPasswordWithTime);
		}
	}
}
