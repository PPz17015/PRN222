using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAccountRepository
    {
        SystemAccount? Login(string email, string password);
        SystemAccount? GetById(int id);
        SystemAccount? GetByEmail(string email);
        List<SystemAccount> GetAll();
        void Add(SystemAccount account);
        void Update(SystemAccount account);
        void Delete(int id);
    }
}
