using Microsoft.AspNetCore.Http;
using OpenLibraryNET;
using OpenLibraryNET.Data;
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
            String aboutBookString = "";
            if (work != null)
            {
                if (work.Data != null)
                {
                    if (work.Data.Title != null)
                    {
                        aboutBookString += $"<b>{work.Data.Title}</b>\n" ;
                    }
                    if (work.Data.Description != null)
                    {
                        aboutBookString += $"<b>Описание</b>: {work.Data.Description}\n";
                    }
                }
                

            }

            return aboutBookString;
        }

        public static String CreateBookHead(OLWork work)
        {
            String bookHead = String.Empty;
            if (work != null)
            {
                if (work.Data != null)
                {
                    if (work.Data.Title != null)
                    {
                        bookHead += $"{work.Data.Title} | ";
                    }
                    if (work.Data.ExtensionData != null)
                    {
                        if (work.Data.ExtensionData.ContainsKey("first_publish_year"))
                        {
                            bookHead += $"{work.Data.ExtensionData["first_publish_year"]} | ";
                        }
                        else if (work.Data.ExtensionData.ContainsKey("first_publish_date"))
                        {
                            bookHead += $"{work.Data.ExtensionData["first_publish_date"]} | ";
                        }
                    }
                    if (work.Data.Subjects.First() != null)
                    {
                        bookHead += $"{work.Data.Subjects[0]}";
                    }
                }
            }
            return bookHead;
        }
    }
}
