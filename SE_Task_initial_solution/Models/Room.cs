namespace SE_Task_initial_solution.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int BranchId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public bool IsAvailable { get; set; }
    }
}
