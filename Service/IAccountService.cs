using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAccountService
    {
        SystemAccount? Login(string email, string password);
        SystemAccount? GetById(int id);
        SystemAccount? GetByEmail(string email);
        List<SystemAccount> GetAll();
        bool Add(SystemAccount account);
        bool Update(SystemAccount account);
        bool Delete(int id);
        bool IsEmailExists(string email);
    }
}
