using System;
using System.Transactions;

namespace Utilities
{
    public class Transaction : IDisposable
    {
        private readonly TransactionScope _innerTrans;

        public Transaction()
        { 
            _innerTrans = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Complete()
        {
            _innerTrans.Complete();
        }

        public void Dispose()
        {
            _innerTrans.Dispose();
        }
    }
}
