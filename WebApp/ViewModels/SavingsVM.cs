using Data.Models;
using System;

namespace Data.ViewModels
{
    public class SavingsVM
    {
        public long Id { get; set; }

        public decimal? Goal { get; set; }

        public decimal? Current { get; set; }

        public long? UserId { get; set; }

        public Guid Guid { get; set; }

        public DateTime? Date { get; set; }

        public string? UserName { get; set; }

        public SavingsVM() { }

        public SavingsVM(long id, decimal? goal, decimal? current, long? userId, Guid guid, string? userName = null)
        {
            Id = id;
            Goal = goal;
            Current = current;
            UserId = userId;
            Guid = guid;
            Date = DateTime.Now;
            UserName = userName;
        }

        public static SavingsVM FromSaving(Saving saving)
        {
            return new SavingsVM
            {
                Id = saving.Idsavings,
                Goal = saving.Goal,
                Current = saving.Current,
                UserId = saving.UserId,
                Guid = saving.Guid,
                Date = saving.Date,
                UserName = saving.User?.Jmbag
            };
        }
    }
}