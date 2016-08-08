using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProdigiousJuanOsorioTest.Source.DataLayer;
using System.Text;

namespace ProdigiousJuanOsorioTest.Source.BusinessLayer
{
    public class EmployeeManager
    {
        public Employee GetEmployee(string email, string password)
        {
            string passwordHash = GetHashFromStringSha256(password);
            
            using(ProdigiousDB db = new ProdigiousDB())
            {
                return db.Employees
                    .FirstOrDefault(e => e.Email == email && e.PasswordHash == passwordHash);
            }
        }               

        static string GetHashFromStringSha256(string stringValue)
        {
            //http://stackoverflow.com/questions/12416249/hashing-a-string-with-sha256
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(stringValue), 0, Encoding.UTF8.GetByteCount(stringValue));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        public Employee GetEmployeeByEmail(string email)
        {
            Employee emp = null;

            using(ProdigiousDB db = new ProdigiousDB())
            {
                emp = db.Employees.FirstOrDefault(e => e.Email == email);
            }

            return emp;
        }
    }
}