namespace ContactBookApp.Model.DTOs
{
    public class PaginationDTO
    {
        public int TotalUsers { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
