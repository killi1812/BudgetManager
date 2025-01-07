namespace WebApp.ViewModels;
    public class SavingsVM
    {
        public long Id { get; set; }

        public decimal? Goal { get; set; }

        public decimal? Current { get; set; }

        public long? UserId { get; set; }

        public string Guid { get; set; }

        public DateTime? Date { get; set; }

        public string? UserName { get; set; }
}