using BussinessObject;
using Repository;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository repository = new AccountRepository();

        public SystemAccount? Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            return repository.Login(email.Trim(), password);
        }

        public SystemAccount? GetById(int id)
        {
            if (id <= 0) return null;
            return repository.GetById(id);
        }

        public SystemAccount? GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return repository.GetByEmail(email.Trim());
        }

        public List<SystemAccount> GetAll()
            => repository.GetAll();

        public bool Add(SystemAccount account)
        {
            if (account == null) return false;
            if (string.IsNullOrWhiteSpace(account.AccountName) ||
                string.IsNullOrWhiteSpace(account.AccountEmail) ||
                string.IsNullOrWhiteSpace(account.AccountPassword)) return false;

            // Kiểm tra email đã tồn tại chưa
            if (IsEmailExists(account.AccountEmail)) return false;

            // Kiểm tra role hợp lệ (1: Staff, 2: Lecturer)
            if (account.AccountRole < 1 || account.AccountRole > 2) return false;

            try
            {
                repository.Add(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(SystemAccount account)
        {
            if (account == null || account.AccountId <= 0) return false;
            if (string.IsNullOrWhiteSpace(account.AccountName) ||
                string.IsNullOrWhiteSpace(account.AccountEmail) ||
                string.IsNullOrWhiteSpace(account.AccountPassword)) return false;

            // Kiểm tra role hợp lệ (1: Staff, 2: Lecturer)
            if (account.AccountRole < 1 || account.AccountRole > 2) return false;

            try
            {
                repository.Update(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            if (id <= 0) return false;

            try
            {
                repository.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsEmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return repository.GetByEmail(email.Trim()) != null;
        }
    }
}
