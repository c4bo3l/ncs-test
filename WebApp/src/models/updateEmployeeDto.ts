import EmployeeBaseCRURequest from "./employeeBaseCruRequest";

export default interface UpdateEmployeeDto extends EmployeeBaseCRURequest {
  id: string;
}
