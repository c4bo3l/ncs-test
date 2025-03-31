export default interface GetEmployeeDto {
  id: string;
  name: string;
  email: string;
  phone: string;
  gender: string;
  days_worked: number;
  cafeId?: string;
  cafe?: string;
}
