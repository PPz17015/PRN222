using BussinessObject;
using DataAccessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : IAccountRepository
    {
        public SystemAccount? Login(string email, string password)
            => new AccountDAO().Login(email, password);

        public SystemAccount? GetById(int id)
            => new AccountDAO().GetById(id);

        public SystemAccount? GetByEmail(string email)
            => new AccountDAO().GetByEmail(email);

        public List<SystemAccount> GetAll()
            => new AccountDAO().GetAll();

        public void Add(SystemAccount account)
            => new AccountDAO().Add(account);

        public void Update(SystemAccount account)
            => new AccountDAO().Update(account);

        public void Delete(int id)
            => new AccountDAO().Delete(id);
    }
}
