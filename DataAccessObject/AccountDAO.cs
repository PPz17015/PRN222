using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class AccountDAO
    {
        public SystemAccount? Login(string email, string password)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.SystemAccounts
                    .FirstOrDefault(acc => acc.AccountEmail == email && acc.AccountPassword == password);
            }
        }

        public SystemAccount? GetById(int id)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.SystemAccounts.FirstOrDefault(acc => acc.AccountId == id);
            }
        }

        public SystemAccount? GetByEmail(string email)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.SystemAccounts.FirstOrDefault(acc => acc.AccountEmail == email);
            }
        }

        public List<SystemAccount> GetAll()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.SystemAccounts.ToList();
            }
        }

        public void Add(SystemAccount account)
        {
            using (var context = new FunewsManagementContext())
            {
                context.SystemAccounts.Add(account);
                context.SaveChanges();
            }
        }

        public void Update(SystemAccount account)
        {
            using (var context = new FunewsManagementContext())
            {
                context.SystemAccounts.Update(account);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new FunewsManagementContext())
            {
                var acc = context.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
                if (acc != null)
                {
                    context.SystemAccounts.Remove(acc);
                    context.SaveChanges();
                }
            }
        }
    }
}
