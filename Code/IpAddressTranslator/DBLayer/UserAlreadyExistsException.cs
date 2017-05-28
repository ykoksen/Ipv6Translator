using System;
using Utilities.Exceptions;

namespace DBLayer
{
	public class UserAlreadyExistsException : StandardException
	{
		public UserAlreadyExistsException()
		{
		}

		public UserAlreadyExistsException(string message) : base(message)
		{
		}

		public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}