namespace MsgBoard.Models.ViewModel.Admin
{
    public class AdminIndexViewModel
    {
        public int RowId { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Mail { get; set; }

        public string Pic { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDel { get; set; }
    }
}