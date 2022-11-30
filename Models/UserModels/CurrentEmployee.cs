namespace Project_C.Models.UserModels
{
    public class CurrentEmployee
    {
        public static Employee currentEmployee { get; set; }

        public static bool IsLoggedIn()=> CurrentEmployee.currentEmployee != null;
        public static bool IsAdmin() => currentEmployee.IsAdmin;
        
    }
}
