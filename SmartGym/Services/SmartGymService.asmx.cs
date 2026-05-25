using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using SmartGym.BLL;
using SmartGym.DAL;
using SmartGym.Models;
using SmartGym.Utilities;

namespace SmartGym.Services
{
    /// <summary>
    /// ASP.NET XML Web Service exposing SmartGym operations to remote callers.
    /// Demonstrates SOAP-style endpoints in .asmx form.
    /// </summary>
    [WebService(Namespace = "http://smartgym.local/services/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SmartGymService : WebService
    {
        private readonly MemberService _memberService = new MemberService();
        private readonly BookingService _bookingService = new BookingService();
        private readonly AccountDAL _accountDal = new AccountDAL();
        private readonly ClassDAL _classDal = new ClassDAL();
        private readonly TransactionDAL _txDal = new TransactionDAL();

        // --- HelloWorld smoke test ------------------------------------------
        [WebMethod(Description = "Returns a confirmation that the service is reachable.")]
        public string Ping()
        {
            return "SmartGym Web Service is alive at " + DateTime.Now.ToString("u");
        }

        // --- Add member -----------------------------------------------------
        [WebMethod(Description = "Registers a new member and returns the new MemberId.")]
        public int AddMember(string fullName, string email, string password,
                             string phone, string securityQuestion, string securityAnswer)
        {
            try
            {
                return _memberService.Register(fullName, email, password, phone,
                                               securityQuestion, securityAnswer);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("AddMember WS error: " + ex.Message);
                throw;
            }
        }

        // --- Update account (top up) ----------------------------------------
        [WebMethod(Description = "Tops up an account by the supplied amount.")]
        public string UpdateAccount(int accountId, decimal topUpAmount)
        {
            try
            {
                return _bookingService.TopUp(accountId, topUpAmount);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("UpdateAccount WS error: " + ex.Message);
                throw;
            }
        }

        // --- Process a booking ----------------------------------------------
        [WebMethod(Description = "Books a class for an account.")]
        public string ProcessBooking(int accountId, int classId)
        {
            try
            {
                return _bookingService.BookClass(accountId, classId);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("ProcessBooking WS error: " + ex.Message);
                throw;
            }
        }

        // --- Retrieve transactions ------------------------------------------
        [WebMethod(Description = "Returns the transaction history for a member as a DataSet.")]
        public DataSet GetTransactions(int memberId)
        {
            return _txDal.GetTransactionsDataSet(memberId);
        }

        // --- Retrieve classes -----------------------------------------------
        [WebMethod(Description = "Returns the full list of fitness classes.")]
        public List<FitnessClass> GetClasses()
        {
            return _classDal.GetAllClasses();
        }

        // --- Log booking activity (appends to file) -------------------------
        [WebMethod(Description = "Writes a booking activity entry to the file log.")]
        public void LogBookingActivity(string memberName, string className,
                                       decimal creditsDeducted, decimal remainingBalance)
        {
            FileLogger.LogBooking(memberName, className, creditsDeducted, remainingBalance);
        }

        // --- Get accounts for a member --------------------------------------
        [WebMethod(Description = "Returns membership accounts owned by a member.")]
        public List<MembershipAccount> GetAccounts(int memberId)
        {
            return _accountDal.GetAccountsForMember(memberId);
        }
    }
}
