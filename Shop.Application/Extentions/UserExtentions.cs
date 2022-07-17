using Shop.Domain.Models.Account;

namespace Shop.Application.Extentions
{
    public static class UserExtentions
    {
        public static string GetUserName(this User user)
        {
            //if (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName)) 
            //{
            //    return $"{user.FirstName}{user.LastName}";
            //}

            //return user.PhoneNumber;

            ////chon ma aval namefamily ro gereftim balayi b dard nemikhore vali mitoonim nagirim va az codeye bala estefade konim

            return $"{user.FirstName}{user.LastName}";
        }
    }
}