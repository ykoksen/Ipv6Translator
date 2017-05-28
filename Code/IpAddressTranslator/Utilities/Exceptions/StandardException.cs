using System;

namespace Utilities.Exceptions
{
  public abstract class StandardException : Exception
  {
	  protected StandardException()
	  { }

	  protected StandardException(string message)
      : base(message)
    { }

	  protected StandardException(string message, Exception innerException)
      : base(message, innerException)
    { }
  }
}
