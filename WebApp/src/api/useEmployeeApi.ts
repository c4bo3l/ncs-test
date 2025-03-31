import moment from "moment";
import useAxios from "../hooks/useAxios";
import CreateEmployeeDto from "../models/createEmployeeDto";
import DeleteEmployeeDto from "../models/deleteEmployeeDto";
import GetEmployeeDto from "../models/getEmployeeDto";
import UpdateEmployeeDto from "../models/updateEmployeeDto";

const useEmployeeApi = () => {
  const { create: createAxios } = useAxios();
  
  const getEmployees = async (cafe?: string) => {
    const ax = await createAxios();
    const response = await ax.get<GetEmployeeDto[]>('/api/employees', {
      params: {
        cafe
      }
    });
    return response.data;
  };

  const createEmployee = async (dto: CreateEmployeeDto) => {
    const ax = await createAxios();
    const response = await ax.post<GetEmployeeDto>('/api/employee', {
      ...dto,
      ...dto.startDate && { startDate: moment(dto.startDate).format("YYYY-MM-DD") }
    });
    return response.data;
  };

  const updateEmployee = async (dto: UpdateEmployeeDto) => {
    const ax = await createAxios();
    const response = await ax.put<GetEmployeeDto>('/api/employee', {
      ...dto,
      ...dto.startDate && { startDate: moment(dto.startDate).format("YYYY-MM-DD") }
    });
    return response.data;
  };

  const deleteEmployee = async (dto: DeleteEmployeeDto) => {
    const ax = await createAxios();
    await ax.delete('/api/Employee', {
      data: dto
    });
  };

  return {
    getEmployees,
    createEmployee,
    updateEmployee,
    deleteEmployee
  };
};

export default useEmployeeApi;
