using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Utilities.Caching;

namespace DBLayer
{
	public class UserDb
	{
		/// <summary>
		/// Gets a user that is not deleted
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		[ItemCanBeNull]
		[NotNull]
		public static async Task<User> GetUser([NotNull] string username)
		{
			NullItem<User> back = Cache.GlobalCache.Get<User>(username);
			if (back != null)
				return back.Value;

			using (AddressTranslatorModelContainer container = new AddressTranslatorModelContainer())
			{
				User userBack = await (container.UserSet.Cast<User>().Where(user => user.Username == username && !user.Deleted)).FirstOrDefaultAsync();


				Cache.GlobalCache.Add(username, new NullItem<User>(userBack), true);

				return userBack;
			}
		}

		public static async Task UpdateLoginTime([NotNull]User user)
		{
			using (AddressTranslatorModelContainer container = new AddressTranslatorModelContainer())
			{
				container.Entry(user).State = EntityState.Modified;
				await container.SaveChangesAsync();

				Cache.GlobalCache.Put(user.Username, user);
			}
		}

		public static async Task CreateUser([NotNull] User newUser)
		{
			var user = await GetUser(newUser.Username);
			if (user != null)
			{
				throw new UserAlreadyExistsException("A user with that username already exists");
			}

			using (AddressTranslatorModelContainer container = new AddressTranslatorModelContainer())
			{
				container.Entry(newUser).State = EntityState.Added;
				await container.SaveChangesAsync();
			}
		}

		public static async Task UpdateUser([NotNull] User userUpdate)
		{
			using (AddressTranslatorModelContainer container = new AddressTranslatorModelContainer())
			{
				string orgUserName = await (from User user in container.UserSet
																		where user.Id == userUpdate.Id
																		select user.Username).FirstOrDefaultAsync();

				if (orgUserName != null)
					Cache.GlobalCache.Remove(orgUserName);

				container.Entry(userUpdate).State = EntityState.Modified;
				await container.SaveChangesAsync();

				Cache.GlobalCache.Put(userUpdate.Username, userUpdate);
			}
		}
	}
}
