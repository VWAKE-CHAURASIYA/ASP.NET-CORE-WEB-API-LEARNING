using MyRestAPI.DTO;

namespace MyRestAPI.IServices
{
    public interface IEmployeeServices
    {
        Task<Tuple<int, List<EmpDto>>> GetAllEmployee();
        Task<Tuple<int, string>> CreateEmployees(EmpDto dto);

        Task<Tuple<int, string>> UpdateEmployees(EmpDto dto);
        Task<Tuple<int, string>> DeleteEmployee(Guid id);

        Task<Tuple<int, EmpDto ?>> GetElementById(Guid id);
    }
}
