using Microsoft.AspNetCore.Http;
using OpenLibraryNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksSearchTelegramBot.res
{
    internal static class StringsGeneration
    {
        public static String GetWelcomeMessageWithUsername(String userName)
        {
            return $"Здравствуйте {userName}, это бот для поиска книг";
        }
        
        public static String CreateAboutBookMessage(OLWork work, OLAuthor author)
        {
            return $"123";
        }
    }
}
