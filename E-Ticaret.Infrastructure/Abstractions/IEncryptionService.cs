using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Ticaret.Infrastructure.Abstractions
{
    public  interface IEncryptionService
    {

        string EncryptString(string plainText);
        string DecryptString(string cipherText);

    
        string ComputeHash(string plainText);
    }
}
