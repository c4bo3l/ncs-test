export default interface EmployeeBaseCRURequest {
  name: string;
  email: string;
  phone: string;
  gender: string;
	cafeId?: string;
	startDate?: Date;
}